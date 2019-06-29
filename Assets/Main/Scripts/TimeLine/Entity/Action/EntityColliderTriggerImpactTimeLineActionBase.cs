using GameFramework.DataTable;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Genesis.GameClient
{
    public abstract partial class EntityColliderTriggerImpactTimeLineActionBase : EntityAbstractTimeLineAction
    {
        private EntityColliderTriggerImpactTimeLineActionDataAbstract m_Data = null;

        public EntityColliderTriggerImpactTimeLineActionBase(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityColliderTriggerImpactTimeLineActionDataAbstract;
        }

        public abstract int[] ImpactIds
        {
            get;
        }

        public abstract bool AffectFriendly
        {
            get;
        }

        public abstract bool AffectNeutral
        {
            get;
        }

        public abstract bool AffectHostile
        {
            get;
        }

        public abstract bool AcceptRepeatedImpact
        {
            get;
        }

        public abstract float? RepeatedImpactIntervalTime
        {
            get;
        }

        public abstract string HitEffectResourceName
        {
            get;
        }

        public abstract int HitEffectAttachPointId
        {
            get;
        }

        public abstract int[] HitSoundIds
        {
            get;
        }

        public abstract bool ShouldBroadcastSound
        {
            get;
        }

        public abstract float HitSoundMinInterval
        {
            get;
        }

        public abstract int[] BuffIds
        {
            get;
        }

        public abstract EntityColliderTriggerImpactTimeLineActionDataAbstract.BuffConditionalImpactGroup[] BuffConditionalImpactGroups
        {
            get;
        }

        public abstract int ColorChangeId
        {
            get;
        }

        public abstract float ColorChangeDuration
        {
            get;
        }

        public abstract string LineEffectResourceName
        {
            get;
        }

        public abstract string LineEffectMySideAttachPoint
        {
            get;
        }

        private ColliderTrigger m_ColliderTrigger = null;
        private ITimeLineInstance<Entity> m_CachedTimeLineInstance = null;
        private StrategyBase m_Strategy = null;
        private int m_SkillRecoverHPCount = 0;
        private int m_SkillLevel;
        private int m_SkillIndex;
        private SkillBadgesData m_SkillBadges;

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            m_SkillRecoverHPCount = 0;

            m_Strategy = CreateStrategy(m_Data.ItsStrategy);
            m_Strategy.Init(timeLineInstance, this);

            if (!string.IsNullOrEmpty(m_Data.LineEffectResourceName))
            {
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntityFailure, OnShowEntityFailure);
            }

            m_CachedTimeLineInstance = timeLineInstance;

            m_SkillLevel = GetSkillLevel(timeLineInstance);
            m_SkillIndex = GetSkillIndex(timeLineInstance);
            m_SkillBadges = GetSkillBadges(timeLineInstance, m_SkillIndex);

            m_Strategy.OnStart(timeLineInstance);
        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            m_Strategy.OnUpdate(timeLineInstance, elapseSeconds);
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {
            Clean(timeLineInstance);
        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {
            Clean(timeLineInstance);
        }

        protected ColliderTrigger CreateColliderTrigger(ITimeLineInstance<Entity> timeLineInstance, string colliderTriggerName)
        {
            ColliderTrigger colliderTrigger = GameEntry.Impact.CreateColliderTrigger(colliderTriggerName);
            colliderTrigger.CachedTransform.SetParent(timeLineInstance.Owner.CachedTransform);
            colliderTrigger.TimeLineInstance = timeLineInstance;
            colliderTrigger.TriggerEnter += OnTriggerEnter;
            colliderTrigger.TriggerExit += OnTriggerExit;
            m_ColliderTrigger = colliderTrigger;
            return colliderTrigger;
        }

        private void Clean(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (!string.IsNullOrEmpty(m_Data.LineEffectResourceName))
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntityFailure, OnShowEntityFailure);
            }

            HideAllLineEffects(timeLineInstance);
            if (m_ColliderTrigger != null)
            {
                m_ColliderTrigger.TriggerEnter -= OnTriggerEnter;
                m_ColliderTrigger.TriggerExit -= OnTriggerExit;
                GameEntry.Impact.DestroyColliderTrigger(m_ColliderTrigger);
                m_ColliderTrigger = null;
                m_CachedTimeLineInstance = null;
            }

            if (m_Strategy != null)
            {
                m_Strategy.Shutdown();
                m_Strategy = null;
            }

            m_SkillRecoverHPCount = 0;
        }

        private void OnTriggerEnter(ITimeLineInstance<Entity> timeLineInstance, Collider target)
        {
            if (target.gameObject.layer != Constant.Layer.TargetableObjectLayerId)
            {
                return;
            }

            ICampable campedOwner = timeLineInstance.Owner as ICampable;
            if (campedOwner == null)
            {
                return;
            }

            Entity entityTarget = target.GetComponent<Entity>();
            ITargetable targetableTarget = entityTarget as ITargetable;
            if (targetableTarget == null)
            {
                return;
            }

            m_Strategy.OnTriggerEnter(timeLineInstance, campedOwner, targetableTarget, entityTarget);
        }

        private void OnTriggerExit(ITimeLineInstance<Entity> timeLineInstance, Collider target)
        {
            if (target.gameObject.layer != Constant.Layer.TargetableObjectLayerId)
            {
                return;
            }

            ICampable campedOwner = timeLineInstance.Owner as ICampable;
            if (campedOwner == null)
            {
                return;
            }

            Entity entity = target.GetComponent<Entity>();
            ITargetable targetEntity = entity as ITargetable;
            if (targetEntity == null)
            {
                return;
            }

            m_Strategy.OnTriggerExit(timeLineInstance, entity);
        }

        private void DoImpact(ITimeLineInstance<Entity> timeLineInstance, ITargetable target, ImpactData impactData)
        {
            if (target.IsDead)
            {
                return;
            }

            ICampable campedOwner = timeLineInstance.Owner as ICampable;
            if (campedOwner == null)
            {
                return;
            }

            Entity targetEntity = target as Entity;
            if (targetEntity == null)
            {
                return;
            }

            if (!WillCauseImpact(campedOwner, target))
            {
                return;
            }

            if (!AcceptRepeatedImpact && impactData.ImpactedEntityIds.Contains(targetEntity.Id))
            {
                return;
            }

            impactData.ImpactedEntityIds.Add(targetEntity.Id);

            var impactIdWithBuffConditions = new List<KeyValuePair<int, BuffType>>();
            FillNormalImpactIds(impactIdWithBuffConditions);

            var targetTargetableObj = targetEntity as TargetableObject;
            CheckBuffsOnTargetForConditionalImpacts(targetTargetableObj, impactIdWithBuffConditions);

            var targetNpcCharacter = targetEntity as NpcCharacter;
            if (targetNpcCharacter != null && !targetNpcCharacter.HasTarget)
            {
                ITargetable targetableOwner = timeLineInstance.Owner as ITargetable;
                if (targetableOwner != null)
                {
                    targetNpcCharacter.Target = targetableOwner;
                }
            }

            if (targetTargetableObj == null)
            {
                return;
            }

            PlaySoundIfNeeded(timeLineInstance, targetTargetableObj, impactData);

            var auxData = new ImpactAuxData();
            auxData.SkillRecoverHPCount = m_SkillRecoverHPCount;
            if (GameEntry.SceneLogic.BaseInstanceLogic.CanAddBuffInSkillTimeLine(campedOwner, targetTargetableObj))
            {
                AddBuffToTarget(targetTargetableObj, timeLineInstance.Owner.Data, auxData);
            }

            StartColorChangeIfNeeded(targetTargetableObj);
            ShowHitEffectIfNeeded(targetTargetableObj);

            GameEntry.Impact.PerformImpacts(campedOwner, timeLineInstance.Owner.Data, target, targetEntity.Data, ImpactSourceType.Skill,
                impactIdWithBuffConditions, timeLineInstance.Id, m_SkillLevel, m_SkillIndex, m_SkillBadges, timeLineInstance.CurrentTime, auxData);
            m_SkillRecoverHPCount = auxData.SkillRecoverHPCount;

            timeLineInstance.FireEvent(this, (int)EntityTimeLineEvent.Impact, targetEntity);
        }

        private void FillNormalImpactIds(List<KeyValuePair<int, BuffType>> impactIdWithBuffConditions)
        {
            for (int i = 0; i < ImpactIds.Length; ++i)
            {
                impactIdWithBuffConditions.Add(new KeyValuePair<int, BuffType>(ImpactIds[i], BuffType.Undefined));
            }
        }

        private void CheckBuffsOnTargetForConditionalImpacts(TargetableObject targetTargetableObj, List<KeyValuePair<int, BuffType>> impactIds)
        {
            if (targetTargetableObj == null)
            {
                return;
            }

            for (int i = 0; i < BuffConditionalImpactGroups.Length; ++i)
            {
                var buffConditionalImpactGroup = BuffConditionalImpactGroups[i];
                if (buffConditionalImpactGroup.RequiredBuffType == null)
                {
                    AddBuffConditionedImpacts(impactIds, buffConditionalImpactGroup);
                    break;
                }

                bool buffTypeSatisfied = false;
                for (int j = 0; j < targetTargetableObj.Data.Buffs.Count; ++j)
                {
                    if (targetTargetableObj.Data.Buffs[j].BuffType == buffConditionalImpactGroup.RequiredBuffType)
                    {
                        buffTypeSatisfied = true;
                        break;
                    }
                }

                if (buffTypeSatisfied)
                {
                    AddBuffConditionedImpacts(impactIds, buffConditionalImpactGroup);
                    break;
                }
            }
        }

        private static void AddBuffConditionedImpacts(List<KeyValuePair<int, BuffType>> impactIds,
            EntityColliderTriggerImpactTimeLineActionDataAbstract.BuffConditionalImpactGroup buffConditionalImpactGroup)
        {
            for (int k = 0; k < buffConditionalImpactGroup.ImpactIds.Length; ++k)
            {
                impactIds.Add(new KeyValuePair<int, BuffType>(buffConditionalImpactGroup.ImpactIds[k],
                    buffConditionalImpactGroup.RequiredBuffType.HasValue ? buffConditionalImpactGroup.RequiredBuffType.Value : BuffType.Undefined));
            }
        }

        private void ShowHitEffectIfNeeded(Entity targetEntity)
        {
            if (string.IsNullOrEmpty(HitEffectResourceName))
            {
                return;
            }

            Character targetCharacter = targetEntity as Character;
            if (targetCharacter == null)
            {
                return;
            }

            IDataTable<DRCharacter> dt = GameEntry.DataTable.GetDataTable<DRCharacter>();
            string hitPointPath = dt[targetCharacter.Data.CharacterId].GetHitPoint(HitEffectAttachPointId);
            if (string.IsNullOrEmpty(hitPointPath))
            {
                return;
            }
            if (CheckCongenerEffect(targetEntity, hitPointPath))
            {
                return;
            }
            int serialId = GameEntry.Entity.GetSerialId();
            GameEntry.Entity.ShowEffect(new EffectData(serialId, hitPointPath, HitEffectResourceName, targetEntity.Id));
        }

        private bool CheckCongenerEffect(Entity targetEntity, string hitPointPath)
        {
            var effectTarget = targetEntity.CachedTransform.FindChild(hitPointPath);
            if (effectTarget == null)
            {
                return false;
            }
            var effectNames = HitEffectResourceName.Split('/');
            var effectItem = effectTarget.FindChild(effectNames[effectNames.Length - 1] + "(Clone)");
            if (effectItem == null)
            {
                return false;
            }
            var effect = effectTarget.GetComponentInChildren<ParticleSystem>();
            if (effect == null)
            {
                return false;
            }
            effect.time = 0;
            effect.Play();

            return true;
        }

        private void HideAllLineEffects(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (m_Strategy != null)
                m_Strategy.HideAllLineEffects(timeLineInstance);
        }

        private bool StartColorChangeIfNeeded(TargetableObject targetTargetableObj)
        {
            if (ColorChangeId <= 0)
            {
                return false;
            }

            targetTargetableObj.StartColorChange(ColorChangeId, ColorChangeDuration);
            return true;
        }

        private int PlaySoundIfNeeded(ITimeLineInstance<Entity> timeLineInstance, TargetableObject targetTargetableObj, ImpactData impactData)
        {
            if (Time.time < impactData.LastSoundTime + HitSoundMinInterval)
            {
                return 0;
            }

            int materialType = targetTargetableObj.Data.MaterialType;
            if (materialType < 0 || materialType > HitSoundIds.Length - 1)
            {
                return 0;
            }

            if (!ShouldBroadcastSound && !(targetTargetableObj is MeHeroCharacter) && !(timeLineInstance.Owner is MeHeroCharacter))
            {
                return 0;
            }

            int soundId = HitSoundIds[materialType];
            if (soundId <= 0)
            {
                return 0;
            }

            if (GameEntry.Sound.PlaySound(soundId, targetTargetableObj) < 0)
            {
                return 0;
            }

            impactData.LastSoundTime = Time.time;
            return soundId;
        }

        private void AddBuffToTarget(TargetableObject targetTargetableObj, EntityData ownerData, ImpactAuxData impactAuxData)
        {
            for (int i = 0; i < BuffIds.Length; ++i)
            {
                targetTargetableObj.AddBuff(BuffIds[i], ownerData, BaseBuffPool.GetNextSerialId(), m_SkillBadges);
            }

            impactAuxData.BuffIdsToAddToTarget = m_Data.BuffIds;
        }

        private bool WillCauseImpact(ICampable campedOwner, ITargetable target)
        {
            RelationType relation = AIUtility.GetRelation(campedOwner.Camp, target.Camp);
            if (relation == RelationType.Friendly && !AffectFriendly
                || relation == RelationType.Neutral && !AffectNeutral
                || relation == RelationType.Hostile && !AffectHostile)
            {
                return false;
            }

            return true;
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.ShowEntityFailureEventArgs;
            if (ne.EntityLogicType != typeof(Effect))
            {
                return;
            }

            var simpleLineED = ne.UserData as SimpleLineEffectData;
            if (simpleLineED == null || simpleLineED.OwnerId != m_CachedTimeLineInstance.Owner.Id)
            {
                return;
            }

            m_Strategy.OnShowLineEffectFailure(sender, simpleLineED);
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
            if (ne.EntityLogicType != typeof(Effect))
            {
                return;
            }

            var simpleLineED = ne.UserData as SimpleLineEffectData;
            if (simpleLineED == null || simpleLineED.OwnerId != m_CachedTimeLineInstance.Owner.Id)
            {
                return;
            }

            m_Strategy.OnShowLineEffectSuccess(sender, simpleLineED);
        }
    }
}
