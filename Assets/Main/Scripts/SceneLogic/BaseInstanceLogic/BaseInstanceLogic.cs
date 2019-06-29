using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本逻辑基类。
    /// </summary>
    public abstract partial class BaseInstanceLogic
    {
        protected Me m_Me = null;

        [SerializeField]
        protected int m_InstanceId = 0;

        [SerializeField]
        protected int m_SceneId = 0;

        [SerializeField]
        protected int[] m_SceneRegionIds = null;

        [SerializeField]
        protected bool m_CanShowGuideIndicator = false;

        [SerializeField]
        protected Vector2 m_GuideIndicatorTarget = Vector2.zero;

        [SerializeField]
        protected MeHeroCharacter m_MeHeroCharacter = null;

        [SerializeField]
        protected List<HeroCharacter> m_HeroCharacters = new List<HeroCharacter>();

        [SerializeField]
        protected CameraController m_CameraController = null;

        [SerializeField]
        private bool m_ShouldProhibitAI = false;

        protected readonly IDictionary<CampType, List<ITargetable>> m_CampTargetableObjects = new Dictionary<CampType, List<ITargetable>>();

        protected readonly INavMeshAgentContactChecker m_ContactChecker = NavMeshAgentContactCheckerFactory.Create();

        private float m_InstanceStartTime = 0f;

        private bool m_ShouldShowHud = true;

        private IFsm<BaseInstanceLogic> m_Fsm = null;

        protected CampType m_MyCamp = CampType.Player;
        public CampType MyCamp { get { return m_MyCamp; } }

        public float InstanceStartTime
        {
            get
            {
                return m_InstanceStartTime;
            }

            protected set
            {
                m_InstanceStartTime = value;
                GameEntry.Event.Fire(this, new InstanceStartTimeChangedEventArgs());
            }
        }

        public bool ShouldShowHud
        {
            get
            {
                return m_ShouldShowHud;
            }
            protected set
            {
                m_ShouldShowHud = value;
                if (value == false)
                {
                    GameEntry.Event.Fire(this, new ShouldShowHudValueChangedEventArgs());
                }
            }
        }

        public bool IsRunning
        {
            get;
            protected set;
        }

        public bool IsLobby
        {
            get
            {
                return m_SceneId == Constant.LobbySceneId && Type == InstanceLogicType.NonInstance;
            }
        }

        /// <summary>
        /// 是否应该禁用 AI。
        /// </summary>
        public bool ShouldProhibitAI
        {
            get
            {
                return m_ShouldProhibitAI;
            }

            protected set
            {
                m_ShouldProhibitAI = value;
            }
        }

        public InstanceResultType InstanceResultType
        {
            get
            {
                if (m_Result == null)
                {
                    return InstanceResultType.Undefined;
                }

                return m_Result.Type;
            }
        }

        public INavMeshAgentContactChecker ContactChecker
        {
            get
            {
                return m_ContactChecker;
            }
        }

        public BaseInstanceLogic()
        {
            m_GuidePointSet = new GuidePointSetManager(this);

            foreach (CampType camp in Enum.GetValues(typeof(CampType)))
            {
                m_CampTargetableObjects.Add(camp, new List<ITargetable>());
            }
        }

        public MeHeroCharacter MeHeroCharacter
        {
            get
            {
                return m_MeHeroCharacter;
            }
        }

        public CameraController CameraController
        {
            get
            {
                return m_CameraController;
            }
        }

        public List<HeroCharacter> GetHeroCharacters()
        {
            return m_HeroCharacters;
        }

        /// <summary>
        /// 副本编号。
        /// </summary>
        public int InstanceId
        {
            get
            {
                return m_InstanceId;
            }
        }

        /// <summary>
        /// 场景编号。
        /// </summary>
        public int SceneId
        {
            get
            {
                return m_SceneId;
            }
        }

        public int[] GetSceneRegionIds()
        {
            return m_SceneRegionIds;
        }

        /// <summary>
        /// 获取位置同步速度。
        /// </summary>
        /// <param name="displacementToUpdate">需要更新的位移。</param>
        /// <param name="duration">时间。</param>
        /// <returns>位置同步速度。</returns>
        public virtual Vector3 GetPosSyncVelocity(Vector3 displacementToUpdate, float duration)
        {
            return Vector3.zero;
        }

        /// <summary>
        /// 是否应给玩家的英雄显示向导指示标志。开放给副本逻辑 AI 来控制。
        /// </summary>
        public virtual bool CanShowGuideIndicator
        {
            set
            {
                if (value == m_CanShowGuideIndicator)
                {
                    return;
                }

                m_CanShowGuideIndicator = value;
                GameEntry.Event.Fire(this, new CanShowGuideIndicatorChangedEventArgs());
            }
            get
            {
                return m_CanShowGuideIndicator;
            }
        }

        /// <summary>
        /// 玩家英雄向导指示的目的地。
        /// </summary>
        public Vector2 GuideIndicatorTarget
        {
            get
            {
                return m_GuideIndicatorTarget;
            }
            set
            {
                m_GuideIndicatorTarget = value;
            }
        }

        /// <summary>
        /// 玩家英雄数据。
        /// </summary>
        public abstract PlayerHeroesData MyHeroesData { get; }

        /// <summary>
        /// 副本逻辑类型。
        /// </summary>
        public abstract InstanceLogicType Type { get; }

        public virtual void Update(float elapseTime, float realElapseTime)
        {
            if (IsRunning)
            {
                UpdateTimer(elapseTime, realElapseTime);
            }
        }

        public virtual string GetRequest(int requestId)
        {
            return string.Empty;
        }

        /// <summary>
        /// 初始化副本逻辑。
        /// </summary>
        /// <param name="instanceOrSceneId">副本或场景编号。</param>
        /// <param name="userData">用户数据</param>
        public virtual void Init(int instanceOrSceneId, object userData)
        {
            InitFsm();

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntityFailure, OnShowEntityFailure);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.HideEntityComplete, OnHideEntityComplete);
            GameEntry.Event.Subscribe(EventId.DeadKeepTimeReached, OnDeadKeepTimeReached);
            GameEntry.Event.Subscribe(EventId.ShowWeaponsComplete, OnShowWeaponsComplete);
            GameEntry.Event.Subscribe(EventId.Revive, OnRevive);
        }

        /// <summary>
        /// 场景加载完成时的行为。
        /// </summary>
        public virtual void OnLoadSceneSuccess(GameEventArgs e)
        {
            (m_Fsm.CurrentState as StateBase).OnLoadSceneSuccess(m_Fsm, e as UnityGameFramework.Runtime.LoadSceneSuccessEventArgs);
        }

        /// <summary>
        /// 请求开始运行副本逻辑。
        /// </summary>
        public virtual void RequestStartInstance()
        {
            DoStartInstance();
            (m_Fsm.CurrentState as StateBase).StartInstance(m_Fsm);
        }

        /// <summary>
        /// 以胜利的结果进行副本结算。
        /// </summary>
        /// <param name="reason">胜利的原因。</param>
        /// <param name="onComplete">结算完成后的回调。</param>
        public bool SetInstanceSuccess(InstanceSuccessReason reason, GameFrameworkAction onComplete = null)
        {
            if (m_Result != null)
            {
                return false;
            }

            OnInstanceSuccess(reason, onComplete);
            return true;
        }

        /// <summary>
        /// 以失败的结果进行副本结算。
        /// </summary>
        /// <param name="reason">失败的原因。</param>
        /// <param name="shouldOpenUI">是否要打开界面。</param>
        /// <param name="onComplete">结算完成后的回调。</param>
        public bool SetInstanceFailure(InstanceFailureReason reason, bool shouldOpenUI = true, GameFrameworkAction onComplete = null)
        {
            if (m_Result != null)
            {
                return false;
            }

            OnInstanceFailure(reason, shouldOpenUI, onComplete);
            return true;
        }

        /// <summary>
        /// 以平局的结果进行副本结算。
        /// </summary>
        /// <param name="reason">平局的原因。</param>
        /// <param name="onComplete">结算完成后的回调。</param>
        public bool SetInstanceDraw(InstanceDrawReason reason, GameFrameworkAction onComplete = null)
        {
            if (m_Result != null)
            {
                return false;
            }

            OnInstanceDraw(reason, onComplete);
            return true;
        }

        /// <summary>
        /// 关闭副本逻辑。
        /// </summary>
        public virtual void Shutdown()
        {
            IsRunning = false;

            if (m_Result != null)
            {
                m_Result.ShutDown();
                m_Result = null;
            }

            DeinitFsm();

            if (GameEntry.IsAvailable)
            {
                GameEntry.TimeScale.ResetInstanceTimeScaleTask();

                if (GameEntry.Base.IsGamePaused)
                {
                    GameEntry.TimeScale.ResumeGame();
                }

                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntityFailure, OnShowEntityFailure);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.HideEntityComplete, OnHideEntityComplete);
                GameEntry.Event.Unsubscribe(EventId.DeadKeepTimeReached, OnDeadKeepTimeReached);
                GameEntry.Event.Unsubscribe(EventId.ShowWeaponsComplete, OnShowWeaponsComplete);
                GameEntry.Event.Unsubscribe(EventId.Revive, OnRevive);
            }

            ClearBehaviorTrees();

            if (m_MyPlayerBehavior != null)
            {
                DisableMyPlayerAI();
                UnloadBehaviorTrees();
            }

            DestroyCameraController();
            m_MeHeroCharacter = null;
            m_HeroCharacters.Clear();
            m_ContactChecker.Clear();
        }

        /// <summary>
        /// 玩家英雄是否死亡过。
        /// </summary>
        public virtual bool AnyOfMyHeroesHasDied
        {
            get
            {
                return m_Me.AnyHeroHasDied;
            }
        }

        /// <summary>
        /// 玩家是否正在切换英雄。
        /// </summary>
        public virtual bool IsSwitchingHero
        {
            get
            {
                return m_Me.IsSwitchingHero;
            }
        }

        /// <summary>
        /// 请求初始化和显示玩家英雄。
        /// </summary>
        /// <returns>成功与否。</returns>
        public virtual bool PrepareAndShowMeHero()
        {
            CreateCameraController();
            return m_Me.PrepareAndShowHero();
        }

        /// <summary>
        /// 请求切换英雄。
        /// </summary>
        /// <param name="index">英雄在战斗队列中的编号。</param>
        /// <param name="ignoreCD">是否忽略英雄冷却。</param>
        public abstract void RequestSwitchHero(int index, bool ignoreCD = false);

        /// <summary>
        /// 获取英雄是否在冷却。
        /// </summary>
        /// <param name="index">英雄在战斗队列中的编号。</param>
        /// <returns>英雄是否在冷却。</returns>
        public bool HeroIsCoolingDown(int index)
        {
            return m_Me.HeroIsCoolingDown(index);
        }

        public virtual void TryGoDie(TargetableObject targetableObject)
        {
            var character = targetableObject as Character;
            if (character != null)
            {
                character.Motion.PerformGoDie();
                targetableObject.IsDying = false;
                return;
            }

            var building = targetableObject as Building;
            if (building != null)
            {
                building.Motion.PerformGoDie();
                targetableObject.IsDying = false;
                return;
            }
            targetableObject.IsDying = false;
        }

        /// <summary>
        /// 自动战斗是否激活。
        /// </summary>
        public bool AutoFightIsEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// 自动战斗是否可以使用。
        /// </summary>
        public bool CanEnableAutoFight
        {
            get
            {
                if (m_MyPlayerBehavior.ExternalBehavior == null)
                {
                    return false;
                }

                if (m_MeHeroCharacter == null || !m_MeHeroCharacter.isActiveAndEnabled || m_MeHeroCharacter.Behavior.ExternalBehavior == null)
                {
                    return false;
                }

                return IsRunning;
            }
        }

        /// <summary>
        /// 激活自动战斗。
        /// </summary>
        public void EnableAutoFight()
        {
            if (AutoFightIsEnabled)
            {
                return;
            }

            if (m_MeHeroCharacter != null)
            {
                m_MeHeroCharacter.StopMove();
            }

            EnableMyPlayerAI();
            EnableMyHeroAI();

            AutoFightIsEnabled = true;

            GameEntry.Event.Fire(this, new AutoFightStateChangedEventArgs());
        }

        /// <summary>
        /// 解除自动战斗。
        /// </summary>
        public void DisableAutoFight()
        {
            if (!AutoFightIsEnabled)
            {
                return;
            }

            DisableMyHeroAI();
            DisableMyPlayerAI();

            AutoFightIsEnabled = false;

            GameEntry.Event.Fire(this, new AutoFightStateChangedEventArgs());
        }

        protected virtual void DoStartInstance()
        {
            IsRunning = true;
            EnableBahaviorTrees();
            GameEntry.Input.MeHeroCharacter = m_MeHeroCharacter;
            GameEntry.Input.JoystickActivated = true;
            GameEntry.Input.SkillActivated = true;
        }

        /// <summary>
        /// 请求尝试自动切换英雄。
        /// </summary>
        protected virtual void TryAutoSwitchHero()
        {
            m_Me.TryAutoSwitchHero();
        }

        /// <summary>
        /// 玩家的最后一个英雄死亡。
        /// </summary>
        protected virtual void OnAllHeroesDead()
        {
            DisableAutoFight();
        }

        /// <summary>
        /// 获取阵营中可作为目标的实体。
        /// </summary>
        /// <param name="camp">阵营。</param>
        /// <returns>实体数组。</returns>
        public ITargetable[] GetCampTargetableObjects(CampType camp)
        {
            return m_CampTargetableObjects[camp].ToArray();
        }

        /// <summary>
        /// 开启公共冷却。
        /// </summary>
        /// <param name="coolDownTime">冷却时间。</param>
        internal void StartCommonCoolDown(float coolDownTime)
        {
            m_Me.StartCommonCoolDown(coolDownTime);
        }

        /// <summary>
        /// 停止公共冷却。
        /// </summary>
        internal void StopCommonCoolDown()
        {
            m_Me.StopCommonCoolDown();
        }

        /// <summary>
        /// 快进公共冷却。
        /// </summary>
        /// <param name="amount">快进时间量。</param>
        internal void FastForwardCommonCoolDown(float amount)
        {
            m_Me.FastForwardCommonCoolDown(amount);
        }

        /// <summary>
        /// 当前玩家是否处于公共冷却状态。
        /// </summary>
        public bool IsDuringCommonCoolDown
        {
            get
            {
                return m_Me.IsDuringCommonCoolDown;
            }
        }

        public virtual void OnTargetableShow(TargetableObject targetable)
        {

        }

        public virtual void OnTargetableUpdate(TargetableObject targetable)
        {

        }

        public virtual void OnTargetableHide(TargetableObject targetable)
        {

        }

        public virtual bool CanAddBuffInSkillTimeLine(ICampable origin, TargetableObject target)
        {
            return true;
        }

        protected virtual void OnReviveHeroes(object o, GameEventArgs e)
        {
            m_Me.OnReviveHeroes();
        }

        private void OnRevive(object o, GameEventArgs e)
        {
            var ne = e as ReviveEventArgs;
            if (ne.HeroCharacter != null)
            {
                var camp = ne.HeroCharacter.Data.Camp;
                m_CampTargetableObjects[camp].Add(ne.HeroCharacter);
            }
        }

        protected virtual void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
            (m_Fsm.CurrentState as StateBase).OnOpenUIFormSuccess(m_Fsm, ne);
        }

        protected void CreateCameraController()
        {
            if (m_CameraController != null)
            {
                Log.Error("m_CameraController isn't null already.");
                return;
            }

            if (Camera.main == null)
            {
                Log.Error("Can not find main camera.");
                return;
            }

            m_CameraController = CameraController.Create(Camera.main.transform);
        }

        protected void DestroyCameraController()
        {
            if (m_CameraController == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(m_CameraController);
            m_CameraController = null;
        }

        protected HeroData GetHeroData(int entityId, int heroType, Vector2 position, float rotation)
        {
            LobbyHeroData lobbyHeroData = GameEntry.Data.LobbyHeros.GetData(heroType);
            if (lobbyHeroData == null)
            {
                Log.Warning("Lobby hero data '{0}' is not exist.", heroType.ToString());
                return null;
            }

            return GetHeroData(entityId, lobbyHeroData, true, CampType.Player, position, rotation);
        }

        protected virtual HeroData GetHeroData(int entityId, BaseLobbyHeroData rawHeroData, bool isMe, CampType camp, Vector2 position, float rotation)
        {
            int heroType = rawHeroData.Type;

            IDataTable<DRHero> dtHero = GameEntry.DataTable.GetDataTable<DRHero>();
            DRHero drHero = dtHero.GetDataRow(heroType);
            if (drHero == null)
            {
                Log.Warning("Can not find hero '{0}'.", heroType.ToString());
                return null;
            }

            IDataTable<DRCharacter> dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
            DRCharacter drCharacter = dtCharacter.GetDataRow(drHero.CharacterId);
            if (drCharacter == null)
            {
                Log.Warning("Can not find character '{0}'.", drHero.CharacterId.ToString());
                return null;
            }
            CharacterDataModifierType modifierType = (this is BasePvpInstanceLogic) ? CharacterDataModifierType.Online : CharacterDataModifierType.Offline;
            HeroData heroData = new HeroData(entityId, isMe, modifierType);
            PopulateHeroData(rawHeroData, camp, position, rotation, heroType, drHero, drCharacter, heroData);
            return heroData;
        }

        protected virtual void PopulateHeroData(BaseLobbyHeroData rawHeroData, CampType camp, Vector2 position, float rotation, int heroType, DRHero drHero, DRCharacter drCharacter, HeroData heroData)
        {
            heroData.Name = rawHeroData.Name;
            heroData.CharacterId = drHero.CharacterId;
            heroData.HeroId = heroType;
            heroData.Scale = drHero.Scale;
            heroData.Position = position;
            heroData.Rotation = rotation;
            heroData.Level = rawHeroData.Level;
            heroData.StarLevel = rawHeroData.StarLevel;
            heroData.WeaponSuiteId = rawHeroData.WeaponSuiteId;
            heroData.Camp = camp;
            heroData.Speed = rawHeroData.Speed;
            heroData.MaxHP = rawHeroData.MaxHP;
            heroData.HP = rawHeroData.HP;

            heroData.Steady.Steady = heroData.Steady.MaxSteady = rawHeroData.Steady;
            heroData.Steady.SteadyRecoverSpeed = rawHeroData.SteadyRecoverSpeed;
            heroData.PhysicalAttack = rawHeroData.PhysicalAttack;
            heroData.PhysicalDefense = rawHeroData.PhysicalDefense;
            heroData.MagicAttack = rawHeroData.MagicAttack;
            heroData.MagicDefense = rawHeroData.MagicDefense;
            heroData.OppPhysicalDfsReduceRate = rawHeroData.OppPhysicalDfsReduceRate;
            heroData.OppMagicDfsReduceRate = rawHeroData.OppMagicDfsReduceRate;
            heroData.PhysicalAtkHPAbsorbRate = rawHeroData.PhysicalAtkHPAbsorbRate;
            heroData.MagicAtkHPAbsorbRate = rawHeroData.MagicAtkHPAbsorbRate;
            heroData.PhysicalAtkReflectRate = rawHeroData.PhysicalAtkReflectRate;
            heroData.MagicAtkReflectRate = rawHeroData.MagicAtkReflectRate;
            heroData.DamageReductionRate = rawHeroData.DamageReductionRate;
            heroData.CriticalHitProb = rawHeroData.CriticalHitProb;
            heroData.CriticalHitRate = rawHeroData.CriticalHitRate;
            heroData.AntiCriticalHitProb = rawHeroData.AntiCriticalHitProb;
            heroData.DamageRandomRate = drHero.DamageRandomRate;
            heroData.AdditionalDamage = rawHeroData.AdditionalDamage;
            heroData.MaterialType = drCharacter.MaterialType;
            heroData.ElementId = drHero.ElementId;
            heroData.RecoverHP = rawHeroData.RecoverHP;
            heroData.ReducedSkillCoolDown = rawHeroData.ReducedSkillCoolDown;
            heroData.ReducedHeroSwitchCD = rawHeroData.ReducedHeroSwitchCD;
            heroData.SkillsBadges = rawHeroData.GetAllSkillBadges();

            for (int i = 0; i < Constant.TotalSkillGroupCount; i++)
            {
                heroData.SetSkillLevel(i, rawHeroData.GetSkillLevel(i));
            }

            heroData.DebutOnShow = false;
            heroData.SwitchSkillLevel = rawHeroData.GetSkillLevel(Constant.SwitchSkillIndex);
            heroData.SwitchSkillCD.Reset(drHero.CDWhenStart, false);
            heroData.ReplaceSkillsAndCacheDataRows();
            heroData.RefreshDodgeSkillEnergyData();
            heroData.InitSkillLevelLocks();
            heroData.IsDead = (heroData.HP <= 0);
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.ShowEntitySuccessEventArgs ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;

            if (ne.EntityLogicType == typeof(MeHeroCharacter))
            {
                MeHeroCharacter meHeroCharacter = ne.Entity.Logic as MeHeroCharacter;
                if (meHeroCharacter == null)
                {
                    Log.Warning("MeHeroCharacter is invalid.");
                    return;
                }

                if (meHeroCharacter.Data == MyHeroesData.CurrentHeroData)
                {
                    m_MeHeroCharacter = meHeroCharacter;
                    m_MeHeroCharacter.Data.SwitchSkillCD.SetReady();
                    CameraController.SetTarget(m_MeHeroCharacter.CachedTransform);
                    GameEntry.Input.MeHeroCharacter = meHeroCharacter;
                }

                return;
            }

            if (ne.EntityLogicType == typeof(HeroCharacter))
            {
                HeroCharacter heroCharacter = ne.Entity.Logic as HeroCharacter;
                if (heroCharacter == null)
                {
                    //Log.Warning("Hero character is invalid.");
                    return;
                }

                m_HeroCharacters.Add(heroCharacter);
                return;
            }
        }


        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.ShowEntityFailureEventArgs;

            var effectData = ne.UserData as EffectData;
            if (effectData != null)
            {
                OnShowEffectFailure(effectData);
            }

            Log.Warning("Can not show entity '{0}' in instance logic with error message '{1}'.", ne.EntityAssetName, ne.ErrorMessage);
        }

        private void OnHideEntityComplete(object sender, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.HideEntityCompleteEventArgs;
            OnHideManagedEffectComplete(ne.UserData as ManagedEffectUserData);
        }

        protected virtual void OnShowWeaponsComplete(object sender, GameEventArgs e)
        {

        }

        protected virtual void OnDeadKeepTimeReached(object sender, GameEventArgs e)
        {
            var ne = e as DeadKeepTimeReachedEventArgs;
            var meHeroCharacter = ne.Targetable as MeHeroCharacter;
            if (meHeroCharacter != null)
            {
                if (MyHeroesData.AnyIsAlive)
                {
                    TryAutoSwitchHero();
                }
                else
                {
                    OnAllHeroesDead();
                }
            }
        }

        protected void LoadMyPlayerAI(GameObject go)
        {
            m_MyPlayerBehavior = go.AddComponent<BehaviorTree>();
            m_MyPlayerBehavior.StartWhenEnabled = false;
            m_MyPlayerBehavior.PauseWhenDisabled = false;
            m_MyPlayerBehavior.ExternalBehavior = null;
            GameEntry.Behavior.LoadBehavior(m_MyPlayerBehavior, MyPlayerBehaviorName);
        }

        private void UpdateTimer(float elpaseSeconds, float realElapseSeconds)
        {
            if (m_InstanceTimer == null || m_Result != null)
            {
                return;
            }

            m_InstanceTimer.Update(elpaseSeconds, realElapseSeconds);
            if (m_InstanceTimer.LeftSeconds <= 0f)
            {
                OnInstanceTimeOut();
            }
        }

        protected virtual void OnInstanceTimeOut()
        {
            SetInstanceFailure(InstanceFailureReason.TimeOut);
        }

        protected virtual void OnInstanceSuccess(InstanceSuccessReason reason, GameFrameworkAction onComplete)
        {
            (m_Fsm.CurrentState as StateBase).OnInstanceSuccess(m_Fsm, reason, onComplete);
        }

        protected virtual void OnInstanceFailure(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
        {
            (m_Fsm.CurrentState as StateBase).OnInstanceFailure(m_Fsm, reason, shouldOpenUI, onComplete);
        }

        protected virtual void OnInstanceDraw(InstanceDrawReason reason, GameFrameworkAction onComplete)
        {
            (m_Fsm.CurrentState as StateBase).OnInstanceDraw(m_Fsm, reason, onComplete);
        }

        protected virtual void OnLoadBehaviorSuccess(object sender, GameEventArgs e)
        {
            var ne = e as LoadBehaviorSuccessEventArgs;
            (m_Fsm.CurrentState as StateBase).OnLoadBehaviorSuccess(m_Fsm, ne);
        }

        private void InitFsm()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm(GetType().Name, this,
                new StatePrepare(CreateInstancePreparer),
                new StateWaiting(CreateInstanceWaiter),
                new StateRunning(CreateInstanceRunner),
                new StateResult(GetResult));
            m_Fsm.Start<StatePrepare>();
        }

        private void DeinitFsm()
        {
            if (m_Fsm == null)
            {
                return;
            }

            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;
        }

        protected abstract AbstractInstancePreparer CreateInstancePreparer();

        protected abstract AbstractInstanceWaiter CreateInstanceWaiter();

        protected abstract AbstractInstanceRunner CreateInstanceRunner();

        protected AbstractInstanceResult GetResult()
        {
            return m_Result;
        }
    }
}
