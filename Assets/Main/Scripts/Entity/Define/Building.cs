using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class Building : TargetableObject, ICanHaveTarget, ITargetPositionPool
    {
        [SerializeField]
        private BuildingData m_BuildingData = null;

        [SerializeField]
        private BulletShooter m_CachedBulletShooter = null;

        [SerializeField]
        private TargetPositionPool m_TargetPositionPool = null;

        public ITargetable Target
        {
            get;
            set;
        }

        public BehaviorTree Behavior
        {
            get;
            private set;
        }

        public bool HasTarget
        {
            get
            {
                return AIUtility.TargetCanBeSelected(Target);
            }
        }

        public new BuildingData Data
        {
            get
            {
                return m_BuildingData;
            }
        }

        public new BuildingMotion Motion
        {
            get
            {
                return base.Motion as BuildingMotion;
            }
            set
            {
                base.Motion = value;
            }
        }

        public override float ModelHeight
        {
            get
            {
                return 2f;
            }
        }

        public override bool NeedShowHPBarOnDamage
        {
            get
            {
                return true;
            }
        }

        public override bool DeadKeepTimeIsReached
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Transform ShooterTransform
        {
            get
            {
                if (m_CachedBulletShooter == null)
                {
                    return null;
                }

                return m_CachedBulletShooter.ShooterTransform;
            }
        }

        public Animation ShooterAnimation
        {
            get
            {
                if (m_CachedBulletShooter == null)
                {
                    return null;
                }

                return m_CachedBulletShooter.ShooterAnimation;
            }
        }

        public BuildingDropInfo DeadDropInfo
        {
            private set;
            get;
        }

        public int DeadDropCoinsEffectId
        {
            private set;
            get;
        }

        public bool ShowNameBoard
        {
            private set;
            get;
        }

        public HPBarDisplayRule HpBarRule
        {
            private set;
            get;
        }

        private List<string> DeathEffect { get; set; }

        private List<string> DeathEffectTargetPath { get; set; }

        private NavMeshObstacle[] m_NavMeshObstacles = null;

        public override void PlayAliasedAnimation(string animClipAlias)
        {
            Motion.PlayAliasedAnimation(animClipAlias);
        }

        internal void EnableNavMeshObstacles(bool isEnabled)
        {
            for (int i = 0; i < m_NavMeshObstacles.Length; ++i)
            {
                m_NavMeshObstacles[i].enabled = isEnabled;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_NavMeshObstacles = GetComponentsInChildren<NavMeshObstacle>(true);
            EnableNavMeshObstacles(false);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_BuildingData = userData as BuildingData;
            if (m_BuildingData == null)
            {
                Log.Error("Building data is invalid.");
                return;
            }

            var dtBuilding = GameEntry.DataTable.GetDataTable<DRBuilding>();

            DRBuilding drBuilding = dtBuilding.GetDataRow(Data.BuildingTypeId);
            if (drBuilding == null)
            {
                Log.Error("Building '{0}' not found.", Data.BuildingTypeId.ToString());
            }

            var dtBuildingModel = GameEntry.DataTable.GetDataTable<DRBuildingModel>();

            DRBuildingModel drBuildingModel = dtBuildingModel.GetDataRow(Data.BuildingModelId);
            if (drBuildingModel == null)
            {
                Log.Error("Building model '{0}' not found.", Data.BuildingModelId.ToString());
                return;
            }

            ImpactCollider.center = new Vector3(drBuildingModel.ImpactCenterX, drBuildingModel.ImpactCenterY, drBuildingModel.ImpactCenterZ);
            ImpactCollider.radius = drBuildingModel.ImpactRadius;
            ImpactCollider.height = drBuildingModel.ImpactHeight;
            ImpactCollider.enabled = drBuilding.AcceptImpact;
            DeadDropCoinsEffectId = drBuilding.DropCoinsEffectId;
            ShowNameBoard = drBuilding.ShowName;
            HpBarRule = drBuilding.HPBarDisplayRule;
            if (HpBarRule == HPBarDisplayRule.AlwaysDisplay)
            {
                GameEntry.Impact.CreateNameBoard(this, NameBoardMode.ShowBySelf);
            }

            GameEntry.Event.Subscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Subscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);

            InitTransformValues();
            InitShooter();
            InitMotion();
            InitBehavior(drBuilding);
            InitBuidingDrops(drBuilding);
            InitDeathEffectData(drBuilding);
            m_TargetPositionPool = new TargetPositionPool();
            Motion.OnStateChanged += OnStateChanged;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(object userData)
        {
            Motion.OnStateChanged -= OnStateChanged;
            m_TargetPositionPool = null;
            DeinitBehavior();
            DeinitMotion();
            DeinitShooter();

            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
                GameEntry.Event.Unsubscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);
                if (GameEntry.Event.Check(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish))
                {
                    GameEntry.Event.Unsubscribe(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish);
                }
            }

            base.OnHide(userData);
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

        private void InitDeathEffectData(DRBuilding drBuilding)
        {
            if (drBuilding == null)
            {
                return;
            }

            List<string> deathEffect = new List<string>();
            List<string> deathEffectTargetPath = new List<string>();

            var effects = drBuilding.GetDeathEffectInfos();

            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].ResourcePath != string.Empty)
                {
                    deathEffect.Add(effects[i].ResourcePath);
                    deathEffectTargetPath.Add(effects[i].TargetTransformPath);
                }
            }

            DeathEffect = deathEffect;
            DeathEffectTargetPath = deathEffectTargetPath;
        }

        private void InitBuidingDrops(DRBuilding drBuilding)
        {
            if (drBuilding == null)
            {
                return;
            }

            var buildingDropInfos = drBuilding.GetDropInfos();
            int sumWeight = 0;
            for (int i = 0; i < buildingDropInfos.Count; i++)
            {
                sumWeight += buildingDropInfos[i].Weight;
            }
            if (sumWeight == 0)
            {
                return;
            }
            int targetWeight = Random.Range(0, sumWeight);
            sumWeight = 0;
            for (int i = 0; i < buildingDropInfos.Count; i++)
            {
                int nextWeight = sumWeight + buildingDropInfos[i].Weight;
                if (targetWeight >= sumWeight && targetWeight < nextWeight)
                {
                    DeadDropInfo = buildingDropInfos[i];
                    return;
                }
                sumWeight = nextWeight;
            }
        }

        private void InitMotion()
        {
            Motion = gameObject.AddComponent<BuildingMotion>();
        }

        private void DeinitMotion()
        {
            if (Motion != null)
            {
                Motion.Shutdown();
                Destroy(Motion);
                Motion = null;
            }
        }

        private void InitTransformValues()
        {
            CachedTransform.localPosition = AIUtility.SamplePosition(m_BuildingData.Position);
            CachedTransform.localRotation = Quaternion.Euler(new Vector3(0f, m_BuildingData.Rotation, 0f));
            CachedTransform.localScale = new Vector3(m_BuildingData.Scale, m_BuildingData.Scale, m_BuildingData.Scale);
        }

        private void InitShooter()
        {
            m_CachedBulletShooter = GetComponent<BulletShooter>();
        }

        private void DeinitShooter()
        {
            if (m_CachedBulletShooter != null)
            {
                m_CachedBulletShooter = null;
            }
        }

        private void InitBehavior(DRBuilding dataRow)
        {
            Behavior = gameObject.AddComponent<BehaviorTree>();
            Behavior.StartWhenEnabled = false;
            Behavior.PauseWhenDisabled = true;
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

            Log.Debug("Load behavior '{0}' OK.", ne.BehaviorName);
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

        protected virtual void OnStateChanged()
        {
            if (IsDead)
            {
                if (Data.Category == NpcCategory.Boss)
                {
                    if (!GameEntry.Event.Check(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish))
                    {
                        GameEntry.Event.Subscribe(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish);
                    }

                    GameEntry.TimeScale.SetInstanceTimeScaleTask();
                    GameEntry.CameraPostEffect.EnableRadialBlur();
                }

                SendGetChestPacketIfNeeded();
                ShowDeathEffectIfNeeded();
            }
        }

        private void ShowDeathEffectIfNeeded()
        {
            if (DeathEffect == null && DeathEffect.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < DeathEffect.Count; i++)
            {
                int SerialId = GameEntry.Entity.GetSerialId();
                var effectData = new EffectData(SerialId, DeathEffectTargetPath[i], DeathEffect[i], Data.Id);
                effectData.CanAttachToDeadOwner = true;
                GameEntry.Entity.ShowEffect(effectData);
            }
        }

        private void OnTimeScaleTaskFinish(object sender, GameEventArgs e)
        {
            GameEntry.CameraPostEffect.DisableRadialBlur();
            GameEntry.Event.Unsubscribe(EventId.TimeScaleTaskFinish, OnTimeScaleTaskFinish);
        }

        private void SendGetChestPacketIfNeeded()
        {

        }
    }
}
