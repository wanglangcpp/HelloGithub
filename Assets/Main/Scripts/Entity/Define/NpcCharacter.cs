using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Genesis.GameClient
{
    public partial class NpcCharacter : Character, ICanHaveTarget, ITargetPositionPool
    {
        [SerializeField]
        private NpcCharacterData m_NpcCharacterData = null;

        [SerializeField]
        private TargetPositionPool m_TargetPositionPool = null;

        public new NpcCharacterData Data
        {
            get
            {
                return m_NpcCharacterData;
            }
        }

        public BehaviorTree Behavior
        {
            get;
            private set;
        }

        public ITargetable Target
        {
            get;
            set;
        }

        public override bool NeedShowHPBarOnDamage
        {
            get
            {
                return Data.Category != NpcCategory.Boss && !AlwaysShowHPBar;
            }
        }

        public bool HasTarget
        {
            get
            {
                return AIUtility.TargetCanBeSelected(Target);
            }
        }

        public override float DeadKeepTime
        {
            get
            {
                return CachedDRNpc.KeepTime;
            }
        }

        public float StoppingDistance
        {
            get
            {
                return CachedDRNpc.StoppingDistance;
            }
        }

        public override int SteadyBuffId
        {
            get
            {
                return CachedDRNpc.SteadyBuffId;
            }
        }

        public int DeadDropCoinsCount
        {
            get
            {
                return CachedDRNpc.DropCoinsCount;
            }
        }

        public int DeadDropCoinsEffectId
        {
            get
            {
                return CachedDRNpc.DropCoinsEffect;
            }
        }

        public string[] CallForTargetKeys
        {
            get;
            set;
        }

        public string BornEnteringAnimation
        {
            get
            {
                return CachedDRNpc.BornEnteringAnimationAlias;
            }
        }

        private float BornEffectDelay
        {
            get
            {
                return CachedDRNpc.BornEffectDelay;
            }
        }

        public bool AlwaysShowHPBar
        {
            get
            {
                return CachedDRNpc.ShowHPBar;
            }
        }

        private DRNpc m_CachedDRNpc;

        private DRNpc CachedDRNpc
        {
            get
            {
                if (m_CachedDRNpc == null)
                {
                    int npcId = Data.NpcId;
                    IDataTable<DRNpc> dtNpc = GameEntry.DataTable.GetDataTable<DRNpc>();
                    m_CachedDRNpc = dtNpc.GetDataRow(npcId);
                    if (m_CachedDRNpc == null)
                    {
                        Log.Warning("Cannot find NPC '{0}'.", npcId.ToString());
                    }
                }

                return m_CachedDRNpc;
            }
        }

        private float m_LastTurnOnHitTime = -1f;
        private float m_DeathSinkStartTime;
        private float m_DeathSinkSpeed;
        private string m_DeathEffect;
        private bool m_HasShownDeathEffect;
        private bool m_ShouldShowHud = true;

        private float m_DeathBlownAwayTime = 0f;
        private Vector3 m_DeathBlownAwayVelocity = Vector3.zero;
        private float m_CurBornEffectTime = 0.0f;
        private bool m_ShowBornEffect = false;

        protected override void OnShow(object userData)
        {
            m_NpcCharacterData = userData as NpcCharacterData;
            if (m_NpcCharacterData == null)
            {
                Log.Error("NPC data is invalid.");
                return;
            }

            base.OnShow(userData);

            m_PatrolData = new PatrolData();
            GameEntry.Event.Subscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Subscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);
            GameEntry.Event.Subscribe(EventId.ShouldShowHudValueChanged, OnShouldShowHudValueChanged);

            IDataTable<DRNpc> dtNpc = GameEntry.DataTable.GetDataTable<DRNpc>();
            DRNpc dataRow = dtNpc.GetDataRow(Data.NpcId);
            if (dataRow == null)
            {
                Log.Warning("Can not load NPC '{0}' from data table.", Data.CharacterId.ToString());
                return;
            }

            NavAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            NavAgent.stoppingDistance = dataRow.StoppingDistance;
            NavAgent.avoidancePriority = dataRow.AvoidancePriority;

            m_DeathSinkStartTime = dataRow.DeathSinkStartTime;
            m_DeathSinkSpeed = dataRow.DeathSinkSpeed;
            m_DeathEffect = dataRow.DeathEffect;

            ResetLastTurnOnHitTime();

            InitBehavior(dataRow);
            m_TargetPositionPool = new TargetPositionPool();

            if (HasOwner && Data.AttackOwnerTarget)
            {
                NpcCharacter npcCharacter = Owner as NpcCharacter;
                if (npcCharacter != null)
                {
                    Target = npcCharacter.Target;
                }
            }
            m_ShowBornEffect = false;
            m_CurBornEffectTime = 0;
            Motion.PlayEntering();
            ShowBornColorChanger();
            ShowBornEffect(0);

            if (AlwaysShowHPBar && Data.Category != NpcCategory.Boss)
            {
                GameEntry.Impact.CreateNameBoard(this, NameBoardMode.HPBarOnly);
            }
        }

        private void OnShouldShowHudValueChanged(object sender, GameEventArgs e)
        {
            m_ShouldShowHud = false;
            GameEntry.Impact.DestroyAllArrowPrompt();
        }

        private void ShowBornColorChanger()
        {
            if (CachedDRNpc.BornColorChangerId > 0)
            {
                StartColorChange(CachedDRNpc.BornColorChangerId, CachedDRNpc.BornColorChangerDuration);
            }
        }

        private void ShowBornEffect(float elapseSeconds)
        {
            if (m_ShowBornEffect)
            {
                return;
            }
            if (CachedDRNpc.BornEffect == null || CachedDRNpc.BornEffect == string.Empty)
            {
                m_ShowBornEffect = true;
                return;
            }
            m_CurBornEffectTime += elapseSeconds;
            if (m_CurBornEffectTime < BornEffectDelay)
            {
                return;
            }
            m_ShowBornEffect = true;
            var effectData = new EffectData(GameEntry.Entity.GetSerialId(), string.Empty, CachedDRNpc.BornEffect, 0);
            effectData.Position = CachedTransform.position.ToVector2();
            effectData.Rotation = CachedTransform.rotation.eulerAngles.y;
            GameEntry.Entity.ShowEffect(effectData);
        }

        protected override void OnHide(object userData)
        {
            if (!GameEntry.IsAvailable) return;

            ResetLastTurnOnHitTime();

            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);
            GameEntry.Event.Unsubscribe(EventId.ShouldShowHudValueChanged, OnShouldShowHudValueChanged);
            if (GameEntry.Event.Check(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish))
            {
                GameEntry.Event.Unsubscribe(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish);
            }

            DeinitBehavior();

            ClearTargetPositions();
            Target = null;
            m_TargetPositionPool = null;
            m_CachedDRNpc = null;
            ResetDeathDissolving();
            m_PatrolData = null;
            base.OnHide(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (CheckCreateArrowPrompt())
            {
                GameEntry.Impact.CreateArrowPrompt(this);
            }
            UpdateDeath(elapseSeconds);
            if (!m_ShowBornEffect)
            {
                ShowBornEffect(elapseSeconds);
            }
        }

        private bool CheckCreateArrowPrompt()
        {
            if (!m_ShouldShowHud || GameEntry.SceneLogic.BaseInstanceLogic.Type == InstanceLogicType.MimicMelee)
            {
                return false;
            }
            Vector3 viewVec = Camera.main.WorldToViewportPoint(transform.position);
            return !((viewVec.x < 1 && viewVec.x > 0 && viewVec.y < 1 && viewVec.y > 0) || IsDead) && CachedDRNpc.ShowArrowPrompt;
        }

        public override bool CheckFaceTo(Vector3 direction)
        {
            if (Motion.DontTurnOnHit)
            {
                return false;
            }

            var dataTable = GameEntry.DataTable.GetDataTable<DRNpc>();
            DRNpc dataRow = dataTable.GetDataRow(Data.NpcId);
            if (dataRow == null)
            {
                Log.Warning("Npc ID {0} is invalid.", Data.NpcId);
                return false;
            }

            if (!dataRow.WillTurnOnHit)
            {
                return false;
            }

            // Cool down is ongoing
            if (m_LastTurnOnHitTime > 0f && Time.unscaledTime - m_LastTurnOnHitTime < dataRow.TurnOnHitCD)
            {
                return false;
            }

            m_LastTurnOnHitTime = Time.unscaledTime;

            var newRotation = Quaternion.LookRotation(direction);
            newRotation.x = 0f;
            newRotation.z = 0f;
            CachedTransform.localRotation = newRotation;

            return true;
        }

        public bool ChangeCamp(CampType camp)
        {
            if (Data.Camp == camp)
            {
                return false;
            }

            if (!IsAvailable || IsDead)
            {
                return false;
            }

            Data.Camp = camp;

            if (Target != null && AIUtility.GetRelation(camp, Target.Camp) != RelationType.Hostile)
            {
                Target = null;
            }

            return true;
        }

        public void AddTargetPositions(IList<Vector3> positions)
        {
            m_TargetPositionPool.AddTargetPositions(positions);
        }

        public void ClearTargetPositions()
        {
            m_TargetPositionPool.ClearTargetPositions();
        }

        public Vector3 SelectTargetPosition()
        {
            return m_TargetPositionPool.SelectTargetPosition();
        }

        private void ResetLastTurnOnHitTime()
        {
            m_LastTurnOnHitTime = -1f;
        }

        private void InitBehavior(DRNpc dataRow)
        {
            Behavior = gameObject.AddComponent<BehaviorTree>();
            Behavior.StartWhenEnabled = false;
            Behavior.ExternalBehavior = null;
            GameEntry.Behavior.LoadBehavior(Behavior, dataRow.AIBehavior);
        }

        private void DeinitBehavior()
        {
            if (Behavior != null)
            {
                Behavior.DisableBehavior();
                GameEntry.Behavior.UnloadBehavior(Behavior.ExternalBehavior);
                Destroy(Behavior);
                Behavior = null;
            }
        }

        private void OnLoadBehaviorSuccess(object sender, GameEventArgs e)
        {
            LoadBehaviorSuccessEventArgs ne = e as LoadBehaviorSuccessEventArgs;
            if (ne.Behavior != Behavior)
            {
                return;
            }

            if (GameEntry.IsAvailable && !GameEntry.SceneLogic.BaseInstanceLogic.ShouldProhibitAI)
            {
                Behavior.EnableBehavior();
            }

            //Log.Debug("Load behavior '{0}' OK.", ne.BehaviorName);
        }

        private void OnLoadBehaviorFailure(object sender, GameEventArgs e)
        {
            LoadBehaviorFailureEventArgs ne = e as LoadBehaviorFailureEventArgs;
            if (ne.Behavior != Behavior)
            {
                return;
            }

            Log.Warning("Can not load behavior '{0}' from '{1}' with error message '{2}'.", ne.BehaviorName, ne.BehaviorAssetName, ne.ErrorMessage);
        }

        protected override void OnStateChanged()
        {
            base.OnStateChanged();

            if (IsDead)
            {
                if (m_NpcCharacterData.Category == NpcCategory.Boss)
                {
                    if (!GameEntry.Event.Check(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish))
                    {
                        GameEntry.Event.Subscribe(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish);
                    }

                    GameEntry.TimeScale.SetInstanceTimeScaleTask();
                    GameEntry.CameraPostEffect.EnableRadialBlur();
                }

                m_HasShownDeathEffect = false;

                PerformDeathBlownAwayIfNeeded();

                
                int id = Data.NpcId;
                if (GameEntry.TaskComponent.MonsterCount.ContainsKey(id))
                {
                    GameEntry.TaskComponent.MonsterCount[id]++;
                }
                else
                {
                    GameEntry.TaskComponent.MonsterCount.Add(id, 1);
                }
            }
        }

        private void OnTimeScaleTaskFinish(object sender, GameEventArgs e)
        {
            GameEntry.CameraPostEffect.DisableRadialBlur();
            GameEntry.Event.Unsubscribe(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish);
        }

        private void ShowDeathEffectIfNeeded()
        {
            if (string.IsNullOrEmpty(m_DeathEffect))
            {
                return;
            }

            var effectData = new EffectData(GameEntry.Entity.GetSerialId(), string.Empty, m_DeathEffect, 0);
            effectData.Position = CachedTransform.position.ToVector2();
            effectData.Rotation = CachedTransform.rotation.eulerAngles.y;
            GameEntry.Entity.ShowEffect(effectData);
        }

        private void UpdateDeath(float elapseSeconds)
        {
            if (!IsDead)
            {
                return;
            }

            UpdateDeathBlowAway(elapseSeconds);

            if (DeadKeepTime - Motion.CurrentStateTime < m_DeathSinkStartTime)
            {
                UpdateDeathDissolving();

                if (!m_HasShownDeathEffect)
                {
                    m_HasShownDeathEffect = true;
                    ShowDeathEffectIfNeeded();
                }
            }
        }

        private void UpdateDeathBlowAway(float elapseSeconds)
        {
            if (Motion.CurrentStateTime < m_DeathBlownAwayTime)
            {
                NavAgent.nextPosition = CachedTransform.localPosition + m_DeathBlownAwayVelocity * elapseSeconds;
            }
            else
            {
                NavAgent.enabled = false;
            }
        }

        private void PerformDeathBlownAwayIfNeeded()
        {
            if (Motion.DeadlyImpactSourceType != ImpactSourceType.Skill)
            {
                return;
            }

            if (CachedDRNpc.BlownAwayDuration <= 0f)
            {
                return;
            }

            if (Random.value > CachedDRNpc.BlownAwayRatio)
            {
                return;
            }

            Vector3 repulseDirection = CachedTransform.localPosition - Motion.ImpactSourceEntity.CachedTransform.localPosition;
            repulseDirection.y = 0f;
            repulseDirection = repulseDirection.normalized;
            m_DeathBlownAwayTime = CachedDRNpc.BlownAwayDuration;
            m_DeathBlownAwayVelocity = repulseDirection * (CachedDRNpc.BlownAwayMinDistance + (CachedDRNpc.BlownAwayMaxDistance - CachedDRNpc.BlownAwayMinDistance) * Random.value) / (m_DeathBlownAwayTime > 0f ? m_DeathBlownAwayTime : 0.001f);
        }

        private void UpdateDeathDissolving()
        {
            SetDissolvingAmount((Motion.CurrentStateTime - m_CachedDRNpc.KeepTime + m_DeathSinkStartTime) * m_DeathSinkSpeed);
        }

        private void ResetDeathDissolving()
        {
            SetDissolvingAmount(0f);
        }

        private void SetDissolvingAmount(float amount)
        {
            if (ColorChanger == null)
            {
                //Log.Warning("ColorChanger component not found on '{0}'.", Name);
                return;
            }

            amount = Mathf.Clamp01(amount);

            var renderers = ColorChanger.GetRenderers();
            for (int i = 0; i < renderers.Count; ++i)
            {
                var renderer = renderers[i];
                renderer.shadowCastingMode = amount > 0 ? ShadowCastingMode.Off : ShadowCastingMode.On;
                for (int j = 0; j < renderer.materials.Length; ++j)
                {
                    var material = renderer.materials[j];
                    material.SetFloat("_DissolveAmount", amount);
                }
            }
        }
    }
}
