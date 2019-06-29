using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class Character : TargetableObject
    {
        [SerializeField]
        private CharacterData m_CharacterData = null;

        [SerializeField]
        private CharacterTransformToUpdate m_TransformToUpdate = null;

        private HashSet<int> m_EffectEntityIds = new HashSet<int>();
        private DRAnimation m_AnimationDataRow = null;
        private DRAnimationCrossFade m_AnimationCrossFadeDataRow = null;
        private CharacterMotionStateCategory? m_LastStateCategory = null;

        public new CharacterData Data
        {
            get
            {
                return m_CharacterData;
            }
        }

        public new CharacterMotion Motion
        {
            get
            {
                return base.Motion as CharacterMotion;
            }
            set
            {
                base.Motion = value;
            }
        }

        public NavMeshAgent NavAgent
        {
            get;
            private set;
        }

        /// <summary>
        /// 死亡后实体保留的时间。死亡状态持续这个时间后，会发出通知。
        /// </summary>
        public abstract float DeadKeepTime { get; }

        /// <summary>
        /// 死亡后实体保留时间是否已到。
        /// </summary>
        public override bool DeadKeepTimeIsReached
        {
            get
            {
                if (!IsDead)
                {
                    return false;
                }

                return Motion.DeadKeepTimeIsReached;
            }
        }

        public override float ModelHeight
        {
            get
            {
                IDataTable<DRCharacter> dt = GameEntry.DataTable.GetDataTable<DRCharacter>();
                DRCharacter dataRow = dt.GetDataRow(m_CharacterData.CharacterId);
                if (dataRow == null)
                {
                    Log.Warning("Can not load character '{0}' from data table.", m_CharacterData.CharacterId.ToString());
                    return 0f;
                }

                return dataRow.ColliderHeight;
            }
        }

        public override bool IsEntering
        {
            get
            {
                if (Motion == null)
                {
                    return false;
                }

                return Motion.Entering;
            }
        }

        public abstract int SteadyBuffId
        {
            get;
        }

        internal CharacterTransformToUpdate TransformToUpdate
        {
            get
            {
                return m_TransformToUpdate;
            }
        }

        public virtual int GetSkillLevel(int skillIndex)
        {
            return 1;
        }

        public override void UpdateTransform(PBTransformInfo pb)
        {
            CachedTransform.localEulerAngles = new Vector3(0f, pb.Rotation, 0f);

            var newPosition = AIUtility.SamplePosition(new Vector2(pb.PositionX, pb.PositionY));
            //if (Vector3.Distance(CachedTransform.localPosition, newPosition) <= GameEntry.RoomLogic.Config.PositionSyncThDist)
            //{
            //    m_TransformToUpdate = new CharacterTransformToUpdate(newPosition, pb.Rotation);
            //    return;
            //}

            // Direct update.
            m_TransformToUpdate = null;
            CachedTransform.localPosition = newPosition;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_CharacterData = userData as CharacterData;
            if (m_CharacterData == null)
            {
                Log.Error("Character data is invalid.");
                return;
            }

            m_CharacterData.OnShow();

            Name = m_CharacterData.Name;

            CachedTransform.localPosition = AIUtility.SamplePosition(m_CharacterData.Position);
            CachedTransform.localRotation = Quaternion.Euler(new Vector3(0f, m_CharacterData.Rotation, 0f));
            CachedTransform.localScale = Vector3.one * m_CharacterData.Scale;

            IDataTable<DRCharacter> dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
            DRCharacter drCharacter = dtCharacter.GetDataRow(m_CharacterData.CharacterId);
            if (drCharacter == null)
            {
                Log.Warning("Can not load character '{0}' from data table.", m_CharacterData.CharacterId.ToString());
                return;
            }

            IDataTable<DRAnimation> dtAnimation = GameEntry.DataTable.GetDataTable<DRAnimation>();
            m_AnimationDataRow = dtAnimation.GetDataRow(m_CharacterData.CharacterId);
            if (m_AnimationDataRow == null)
            {
                Log.Warning("Character animation '{0}' not found.", Data.CharacterId.ToString());
                return;
            }

            IDataTable<DRAnimationCrossFade> dtAnimationCrossFade = GameEntry.DataTable.GetDataTable<DRAnimationCrossFade>();
            m_AnimationCrossFadeDataRow = dtAnimationCrossFade.GetDataRow(m_CharacterData.CharacterId);
            if (m_AnimationCrossFadeDataRow == null)
            {
                Log.Warning("Character AnimationCrossFade '{0}' not found.", Data.CharacterId.ToString());
                return;
            }

            NavAgent = gameObject.GetOrAddComponent<NavMeshAgent>();
            NavAgent.enabled = true;
            NavAgent.radius = drCharacter.ColliderRadius;
            NavAgent.speed = Data.Speed;
            NavAgent.height = drCharacter.ColliderHeight;
            NavAgent.acceleration = 10000f;
            NavAgent.angularSpeed = 0f;
            NavAgent.stoppingDistance = 0f;
            NavAgent.autoRepath = true;
            NavAgent.autoBraking = true;
            NavAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            NavAgent.avoidancePriority = 50;

            Motion = gameObject.AddComponent<CharacterMotion>();

            ImpactCollider.center = new Vector3(drCharacter.ImpactCenterX, drCharacter.ImpactCenterY, drCharacter.ImpactCenterZ);
            ImpactCollider.radius = drCharacter.ImpactRadius;
            ImpactCollider.height = drCharacter.ImpactHeight;

            GameEntry.Event.Subscribe(EventId.CharacterPropertyChange, OnCharacterPropertyChange);

            GameEntry.SceneLogic.BaseInstanceLogic.ContactChecker.Register(NavAgent);

            m_LastStateCategory = null;
            Motion.OnMovingUpdate += OnMovingUpdate;
            Motion.OnMovingEnd += OnMovingEnd;
            Motion.OnPerformSkillStart += OnPerformSkillStart;
            Motion.OnPerformSkillEnd += OnPerformSkillEnd;
            Motion.OnSkillRushUpdate += OnSkillRushUpdate;
            Motion.OnStateChanged += OnStateChanged;

            if (Data.Steady.IsSteadying && Data.BuffPool.GetById(SteadyBuffId) == null)
            {
                var buffPool = (Data.BuffPool as OnlineBuffPool);
                if (buffPool != null)
                {
                    buffPool.AddOfflineBuff(SteadyBuffId, Data, BaseBuffPool.GetNextSerialId(), Time.time);
                }
                else
                {
                    AddBuff(SteadyBuffId, Data, BaseBuffPool.GetNextSerialId(), null);
                }
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            Data.Steady.UpdateSteady(elapseSeconds);
            Data.UpdateBuff();
        }

        protected override void OnHide(object userData)
        {
            if (this == null || gameObject == null)
            {
                return;
            }

            m_CharacterData.OnHide();

            m_TransformToUpdate = null;

            if (Motion != null)
            {
                Motion.OnMovingUpdate -= OnMovingUpdate;
                Motion.OnMovingEnd -= OnMovingEnd;
                Motion.OnPerformSkillStart -= OnPerformSkillStart;
                Motion.OnPerformSkillEnd -= OnPerformSkillEnd;
                Motion.OnSkillRushUpdate -= OnSkillRushUpdate;
                Motion.OnStateChanged -= OnStateChanged;

                Motion.Shutdown();
                Destroy(Motion);
                Motion = null;
            }

            NavAgent.enabled = false;

            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.CharacterPropertyChange, OnCharacterPropertyChange);
                GameEntry.SceneLogic.BaseInstanceLogic.ContactChecker.Unregister(NavAgent);
            }

            base.OnHide(userData);
        }

        protected virtual void OnStateChanged()
        {
            if (Motion.IsInAir && (!m_LastStateCategory.HasValue || m_LastStateCategory.Value != CharacterMotionStateCategory.Air))
            {
                RemoveBuffEffectsForAirStates();
            }
            else if (Motion.IsOnGround && (!m_LastStateCategory.HasValue || m_LastStateCategory.Value != CharacterMotionStateCategory.Ground))
            {
                AddBuffEffectsForGroundStates();
            }

            m_LastStateCategory = Motion.IsInAir ? CharacterMotionStateCategory.Air : CharacterMotionStateCategory.Ground;
        }

        private void RemoveBuffEffectsForAirStates()
        {
            var buffDatas = Data.BuffPool.Buffs;
            var toRemove = new List<BuffData>();
            for (int i = 0; i < buffDatas.Length; ++i)
            {
                if (buffDatas[i].GoToAir == GoToAirBuffActionType.HideEffect)
                {
                    toRemove.Add(buffDatas[i]);
                }
            }

            RemoveBuffEffects(toRemove);
        }

        private void AddBuffEffectsForGroundStates()
        {
            var buffDatas = Data.BuffPool.Buffs;
            for (int i = 0; i < buffDatas.Length; ++i)
            {
                if (buffDatas[i].GoToAir == GoToAirBuffActionType.HideEffect)
                {
                    AddBuffEffects(buffDatas[i]);
                }
            }
        }

        private void OnCharacterPropertyChange(object sender, GameEventArgs eventArgs)
        {
            var e = eventArgs as CharacterPropertyChangeEventArgs;

            if (e.CharacterEntityId != Id)
            {
                return;
            }

            NavAgent.speed = Data.Speed;
        }

        /// <summary>
        /// 调整角色转向
        /// </summary>
        /// <param name="direction">面向方向</param>
        /// <returns>是否需要转向</returns>
        public virtual bool CheckFaceTo(Vector3 direction)
        {
            return false;
        }

        /// <summary>
        /// 是否无视状态伤害（全部）。
        /// </summary>
        public bool HasStateHarmFreeBuff
        {
            get
            {
                for (int i = 0; i < Data.BuffPool.Buffs.Length; ++i)
                {
                    var buff = Data.BuffPool.Buffs[i];
                    if (buff.BuffType == BuffType.StateHarmFree || buff.BuffType == BuffType.StateHarmFreeAdvanced || buff.BuffType == BuffType.StateAndNumHarmFree)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 是否无视状态伤害的位移部分的 Buff。
        /// </summary>
        public bool HasDisplacementHarmFreeBuff
        {
            get
            {
                for (int i = 0; i < Data.BuffPool.Buffs.Length; ++i)
                {
                    var buff = Data.BuffPool.Buffs[i];
                    if (buff.BuffType == BuffType.DisplacementHarmFree)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 是否无视状态伤害（高级）。
        /// </summary>
        public bool HasStateHarmFreeAdvancedBuff
        {
            get
            {
                for (int i = 0; i < Data.BuffPool.Buffs.Length; ++i)
                {
                    var buff = Data.BuffPool.Buffs[i];
                    if (buff.BuffType == BuffType.StateHarmFreeAdvanced || buff.BuffType == BuffType.StateAndNumHarmFree)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 是否无视浮空状态而转换成普通受击状态。
        /// </summary>
        public bool HasImmuneFloatImpactBuff
        {
            get
            {
                for (int i = 0; i < Data.BuffPool.Buffs.Length; ++i)
                {
                    var buff = Data.BuffPool.Buffs[i];
                    if (buff.BuffType == BuffType.ImmuneFloatImpact)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 是否无敌。
        /// </summary>
        public bool HasStateAndNumHarmFreeBuff
        {
            get
            {
                for (int i = 0; i < Data.BuffPool.Buffs.Length; ++i)
                {
                    var buff = Data.BuffPool.Buffs[i];
                    if (buff.BuffType == BuffType.StateAndNumHarmFree)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void AddBuffEffects(BuffData added)
        {
            if (Motion.IsInAir && added.GoToAir == GoToAirBuffActionType.HideEffect)
            {
                return;
            }

            int buffEffectId = 0;
            int needRemoveDefaultId = -1;
            if (!CheckHasSameBuff(added))
            {
                buffEffectId = added.DefaultEffectId;
            }
            else
            {
                buffEffectId = added.BlendEffectId;
                needRemoveDefaultId = added.DefaultEffectId;
            }

            if (buffEffectId <= 0 || CheckHasSameBuffEffect(buffEffectId))
            {
                return;
            }

            if (needRemoveDefaultId > 0)
            {
                List<BuffData> removeBuffs = new List<BuffData>();
                for (int i = 0; i < Data.BuffPool.Buffs.Length; i++)
                {
                    if (Data.BuffPool.Buffs[i].BuffId == needRemoveDefaultId && Data.BuffPool.Buffs[i].BuffEffectEntityIds != null)
                    {
                        removeBuffs.Add(Data.BuffPool.Buffs[i]);
                    }
                }
                RemoveBuffEffects(removeBuffs);
            }


            DRBuffCharacterEffect buffEffectData = GameEntry.DataTable.GetDataTable<DRBuffCharacterEffect>().GetDataRow(buffEffectId);
            if (buffEffectData == null)
            {
                Log.Error("Wrong BuffCharacterEffect ID.");
                return;
            }

            UnityGameFramework.Runtime.Entity buffOwnerEntity = null;
            buffOwnerEntity = GameEntry.Entity.GetEntity(Id);

            if (buffOwnerEntity == null)
            {
                return;
            }

            added.CurrentBuffEffectId = buffEffectId;
            for (int j = 0; j < Constant.MaxCharacterBuffEffectCount; j++)
            {
                if (buffEffectData.EffectPath[j] != null && buffEffectData.EffectPath[j] != string.Empty)
                {
                    added.BuffEffectEntityIds[j] = GameEntry.Entity.GetSerialId();
                    GameEntry.Entity.ShowEffect(new EffectData(added.BuffEffectEntityIds[j], buffEffectData.AttachPointPath[j], buffEffectData.EffectPath[j], Id));
                }
                else
                {
                    added.BuffEffectEntityIds[j] = 0;
                }
            }

        }

        public void RemoveBuffEffects(IList<BuffData> removed)
        {
            for (int i = 0; i < removed.Count; i++)
            {
                if (removed[i].BuffEffectEntityIds == null)
                {
                    continue;
                }

                for (int j = 0; j < Constant.MaxCharacterBuffEffectCount; j++)
                {
                    if (removed[i].BuffEffectEntityIds[j] == 0)
                    {
                        continue;
                    }

                    int effectEntityId = removed[i].BuffEffectEntityIds[j];
                    removed[i].BuffEffectEntityIds[j] = 0;
                    removed[i].CurrentBuffEffectId = 0;

                    if (effectEntityId != 0 && GameEntry.Entity.HasEntity(effectEntityId) || GameEntry.Entity.IsLoadingEntity(effectEntityId))
                    {
                        GameEntry.Entity.HideEntity(effectEntityId);
                    }
                }
            }
        }

        private bool CheckHasSameBuff(BuffData added)
        {
            for (int i = 0; i < Data.BuffPool.Buffs.Length; i++)
            {
                if (Data.BuffPool.Buffs[i].BuffId == added.BuffId && Data.BuffPool.Buffs[i].SerialId != added.SerialId)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckHasSameBuffEffect(int effectId)
        {
            for (int i = 0; i < Data.BuffPool.Buffs.Length; i++)
            {
                var buff = Data.BuffPool.Buffs[i];
                if (buff.CurrentBuffEffectId != effectId || buff.BuffEffectEntityIds == null)
                {
                    continue;
                }

                for (int j = 0; j < buff.BuffEffectEntityIds.Length; ++j)
                {
                    int value = buff.BuffEffectEntityIds[j];
                    if (value != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddEffect(int effectEntityId)
        {
            m_EffectEntityIds.Add(effectEntityId);
        }

        public void HideAndRemoveEffect(int effectEntityId)
        {
            HideAndRemoveEffectInternal(effectEntityId, true);
        }

        public void HideAndRemoveAllEffect()
        {
            foreach (int id in m_EffectEntityIds)
            {
                HideAndRemoveEffectInternal(id, false);
            }

            m_EffectEntityIds.Clear();
        }

        public float GetAnimationLength(string animationAliasName)
        {
            if (string.IsNullOrEmpty(animationAliasName))
            {
                Log.Warning("Animation alias name is invalid.");
                return 0f;
            }

            string animationName = m_AnimationDataRow.GetAnimationName(animationAliasName);
            if (string.IsNullOrEmpty(animationName))
            {
                return 0f;
            }

            AnimationState animationState = CachedAnimation[animationName];
            if (animationState == null)
            {
                Log.Warning("Can not find animation '{0}' for character '{1}'.", animationName, Data.CharacterId.ToString());
                return 0f;
            }

            return animationState.length;
        }

        public virtual float PlayAnimation(string animationAliasName, bool needRewind = false, bool dontCrossFade = false, bool queued = false)
        {
            if (string.IsNullOrEmpty(animationAliasName))
            {
                Log.Warning("Animation alias name is invalid.");
                return 0f;
            }

            string animationName = m_AnimationDataRow.GetAnimationName(animationAliasName);
            if (string.IsNullOrEmpty(animationName))
            {
                return 0f;
            }

            AnimationState animationState = CachedAnimation[animationName];
            if (animationState == null)
            {
                Log.Debug("Can not find animation '{0}' for character '{1}'.", animationName, Data.CharacterId.ToString());
                return 0f;
            }

            if (needRewind)
            {
                CachedAnimation.Rewind(animationName);
            }

            if (dontCrossFade)
            {
                if (queued)
                {
                    CachedAnimation.PlayQueued(animationName);
                }
                else
                {
                    CachedAnimation.Play(animationName);
                }
            }
            else
            {
                float crossFadeTime = m_AnimationCrossFadeDataRow.GetAnimationCrossFade(animationAliasName);
                if (crossFadeTime > 0)
                {
                    if (queued)
                    {
                        CachedAnimation.CrossFadeQueued(animationName, crossFadeTime);
                    }
                    else
                    {
                        CachedAnimation.CrossFade(animationName, crossFadeTime);
                    }
                }
                else
                {
                    if (queued)
                    {
                        CachedAnimation.PlayQueued(animationName);
                    }
                    else
                    {
                        CachedAnimation.Play(animationName);
                    }
                }
            }

            return animationState.length;
        }

        protected virtual void OnMovingUpdate()
        {
            //CachedTransform.LookAt2D(Motion.MoveTargetPosition.ToVector2());
            CachedTransform.LookAt2D((CachedTransform.position + NavAgent.velocity).ToVector2());
        }

        protected virtual void OnMovingEnd()
        {

        }

        protected virtual void OnPerformSkillStart(int skillId, int skillIndex, bool isInCombo, bool isContinualTap)
        {

        }

        protected virtual void OnPerformSkillEnd(int skillId, SkillEndReasonType reason)
        {

        }

        protected virtual void OnSkillRushUpdate(int skillId, bool justEntered)
        {

        }

        private void HideAndRemoveEffectInternal(int effectEntityId, bool removeFromEffectEntityIds)
        {
            if (GameEntry.Entity.HasEntity(effectEntityId) || GameEntry.Entity.IsLoadingEntity(effectEntityId))
            {
                GameEntry.Entity.HideEntity(effectEntityId);
            }

            if (removeFromEffectEntityIds)
            {
                m_EffectEntityIds.Remove(effectEntityId);
            }
        }

        [Serializable]
        public class CharacterTransformToUpdate
        {
            public Vector3 Position { get; private set; }

            // 虽保留朝向信息，但在同步时需要谨慎使用，以防出现奇怪的表现。
            public float Rotation { get; private set; }

            public CharacterTransformToUpdate(Vector3 position, float rotation)
            {
                Position = position;
                Rotation = rotation;
            }
        }
    }
}
