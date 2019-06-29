using GameFramework;
using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// Dummy class intended as an instance logic placeholder.
    /// </summary>

    [Serializable]
    public class NonInstanceLogic : BaseInstanceLogic
    {
        private bool m_MainUIIsReady = false;
        private bool m_MyHeroCharFirstTimeReady = false;
        private BaseNameBoard m_MyNameBoard = null;

        public override InstanceLogicType Type
        {
            get
            {
                return InstanceLogicType.NonInstance;
            }
        }

        public override void Init(int instanceOrSceneId, object userData)
        {
            base.Init(instanceOrSceneId, userData);
            m_SceneId = instanceOrSceneId;
            m_SceneRegionIds = new int[0];
            m_MyHeroesData = new PlayerHeroesData();
            m_Me = new Me(GameEntry.Data.Player.Id, m_MyHeroesData, m_CampTargetableObjects, this);
            GameEntry.Event.Subscribe(EventId.HeroTeamDataChanged, OnHeroTeamChanged);
            GameEntry.Event.Subscribe(EventId.RefreshNearbyPlayers, OnRefreshNearbyPlayersData);
        }

        public override void Update(float elapseTime, float realElapseTime)
        {
            base.Update(elapseTime, realElapseTime);

        }

        public override void Shutdown()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            if (IsLobby && m_Me != null && m_Me.GetCurrentHeroCharacter() != null)
            {
                GameEntry.Data.NearbyPlayers.ClearAllData();
                GameEntry.Data.Player.LoginPostion = m_Me.GetCurrentHeroCharacter().transform.position.ToVector2();
                GameEntry.LobbyLogic.UpdateNearbyPlayersPosition();
            }
            GameEntry.Event.Unsubscribe(EventId.HeroTeamDataChanged, OnHeroTeamChanged);
            GameEntry.Event.Unsubscribe(EventId.RefreshNearbyPlayers, OnRefreshNearbyPlayersData);

            if (IsLobby)
            {
                m_MyNameBoard = null;
            }

            m_Me.Shutdown();
            m_Me = null;

            base.Shutdown();
        }

        private PlayerHeroesData m_MyHeroesData = null;

        public override PlayerHeroesData MyHeroesData
        {
            get
            {
                return m_MyHeroesData;
            }
        }

        public override bool AnyOfMyHeroesHasDied
        {
            get
            {
                return false;
            }
        }

        public override bool IsSwitchingHero
        {
            get
            {
                return false;
            }
        }

        public override bool CanShowGuideIndicator
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException("CanShowGuideIndicator");
            }
        }

        public override void RequestSwitchHero(int index, bool ignoreCD = false)
        {
            throw new NotSupportedException("SwitchHero");
        }

        public override bool PrepareAndShowMeHero()
        {
            CreateCameraController();
            if (IsLobby)
            {
                Vector3 vec = Vector3.zero;
                if (!AIUtility.TrySamplePosition(GameEntry.Data.Player.LoginPostion, false, out vec))
                {
                    //m_MyHeroesData.CurrentHeroData.Position = GameEntry.Data.NearbyPlayers.GenerateRandomPosition();
                    //m_MyHeroesData.CurrentHeroData.Position = new Vector3(58.7f, 2.83f, 36f).ToVector2();
                    m_MyHeroesData.CurrentHeroData.Position = Vector2.zero;
                }
                else
                {
                    m_MyHeroesData.CurrentHeroData.Position = GameEntry.Data.Player.LoginPostion;
                }
            }
            GameEntry.Entity.ShowLobbyMeHero(m_MyHeroesData.CurrentHeroData);
            return true;
        }

        protected override void DoStartInstance()
        {
            IsRunning = true;
            GameEntry.Input.MeHeroCharacter = m_MeHeroCharacter;
            GameEntry.Input.JoystickActivated = true;
            GameEntry.Input.SkillActivated = false;
        }

        protected override void TryAutoSwitchHero()
        {
            throw new NotSupportedException("TryAutoSwitchHero");
        }

        protected override void OnInstanceSuccess(InstanceSuccessReason reason, GameFrameworkAction onComplete)
        {
            throw new NotSupportedException("OnInstanceSuccess");
        }

        protected override void OnInstanceFailure(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
        {
            throw new NotSupportedException("OnInstanceFailure");
        }

        protected override void OnReviveHeroes(object o, GameEventArgs e)
        {
            throw new NotSupportedException("OnReviveHeroes");
        }

        protected override AbstractInstancePreparer CreateInstancePreparer()
        {
            return new Preparer();
        }

        protected override AbstractInstanceWaiter CreateInstanceWaiter()
        {
            return new DefaultInstanceWaiter();
        }

        protected override AbstractInstanceRunner CreateInstanceRunner()
        {
            return new Runner();
        }

        protected override void OnShowEntitySuccess(object o, GameEventArgs e)
        {
            base.OnShowEntitySuccess(o, e);
            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;

            MeHeroCharacter meHeroCharacter = ne.Entity.Logic as MeHeroCharacter;
            if (meHeroCharacter != null)
            {
                m_MyHeroCharFirstTimeReady = true;

                GameEntry.Input.MeHeroCharacter = meHeroCharacter;
                GameEntry.Input.JoystickActivated = true;

                meHeroCharacter.NavAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                if (m_MyNameBoard != null)
                {
                    GameEntry.Impact.DestroyNameBoard(m_MyNameBoard);
                    m_MyNameBoard = null;
                }

                m_MyNameBoard = GameEntry.Impact.CreateNameBoard(meHeroCharacter, NameBoardMode.NameOnly);
                m_MyNameBoard.SetName(GameEntry.Data.Player.Name);
                m_MyNameBoard.SetNameColor(m_MyNameBoard.MyNameColor);
                m_MyNameBoard.RefreshPosition();
                return;
            }

            HeroCharacter heroCharacter = ne.Entity.Logic as HeroCharacter;
            if (heroCharacter != null && IsLobby)
            {
                NearbyPlayerData data = GameEntry.Data.NearbyPlayers.GetData(heroCharacter.Id);
                heroCharacter.NavAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                BaseNameBoard nameBoard = GameEntry.Impact.CreateNameBoard(heroCharacter, NameBoardMode.NameOnly);
                nameBoard.SetName(data.Player.Name);
                nameBoard.SetNameColor(nameBoard.OtherNameColor);
                nameBoard.RefreshPosition();
                return;
            }
        }

        private void OnRefreshNearbyPlayersData(object o, GameEventArgs e)
        {
            GameEntry.LobbyLogic.ShowNearbyPlayers();
        }

        private void OnHeroTeamChanged(object o, GameEventArgs e)
        {
            if (!(GameEntry.Procedure.CurrentProcedure is ProcedureMain))
            {
                return;
            }
            if (m_MeHeroCharacter==null||m_MeHeroCharacter.CachedTransform==null)
            {
                return;
            }
            GameEntry.Input.MeHeroCharacter = null;
            GameEntry.Input.JoystickActivated = false;
            GameEntry.Input.SkillActivated = false;

            m_MyHeroesData = new PlayerHeroesData();
            m_MyHeroesData.Add(GetHeroData(GameEntry.Entity.GetSerialId(), GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).MainHeroType,
                m_MeHeroCharacter.CachedTransform.localPosition.ToVector2(),
                m_MeHeroCharacter.CachedTransform.localRotation.eulerAngles.y));
            m_MyHeroesData.SwitchHero(0);
            CameraController.ResetTarget();
            GameEntry.Entity.HideEntity(m_MeHeroCharacter.Id);
            GameEntry.Entity.ShowLobbyMeHero(m_MyHeroesData.CurrentHeroData);
        }

        protected override AbstractInstanceSuccess CreateSuccessResult()
        {
            throw new NotSupportedException("CreateSuccessResult");
        }

        protected override AbstractInstanceFailure CreateFailureResult(bool shouldOpenUI)
        {
            throw new NotSupportedException("CreateFailureResult");
        }

        private void SendLeaveInstanceRequest()
        {
            throw new NotSupportedException("SendLeaveInstanceRequest");
        }

        private class Preparer : AbstractInstancePreparer
        {
            private bool m_HasFireInstanceReadyToStart = false;
            private Transform m_SpawnPoint = null;

            private NonInstanceLogic InstanceLogic
            {
                get
                {
                    return m_InstanceLogic as NonInstanceLogic;
                }
            }

            public override void OnLoadBehaviorSuccess(LoadBehaviorSuccessEventArgs e)
            {

            }

            public override void OnLoadSceneSuccess(UnityGameFramework.Runtime.LoadSceneSuccessEventArgs e)
            {
                UnityGameFramework.Runtime.LoadSceneSuccessEventArgs ne = e as UnityGameFramework.Runtime.LoadSceneSuccessEventArgs;
                if (ne.SceneName.Contains("_"))
                {
                    return;
                }

                RefreshSpawnPoint();

                var instanceLogic = m_InstanceLogic as NonInstanceLogic;
                var heroData = instanceLogic.GetHeroData(GameEntry.Entity.GetSerialId(), GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).MainHeroType,
                    (GameEntry.OfflineMode.OfflineModeEnabled || !instanceLogic.IsLobby) ? m_SpawnPoint.position.ToVector2() : m_SpawnPoint.position.ToVector2(),
                    m_SpawnPoint.rotation.eulerAngles.y);
                instanceLogic.m_MyHeroesData.Add(heroData);
                instanceLogic.m_MyHeroesData.SwitchHero(0);
                instanceLogic.PrepareAndShowMeHero();
                GameEntry.SceneLogic.OpenMainUI();
            }

            public override void OnOpenUIFormSuccess(UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs e)
            {
                var ne = e as UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
                var instanceLogic = m_InstanceLogic as NonInstanceLogic;

                if (ne.UIForm.Logic is MainForm)
                {
                    instanceLogic.m_MainUIIsReady = true;
                    CheckFireInstanceReadyToStart();
                }
            }

            public override void StartInstance()
            {
                var instanceLogic = m_InstanceLogic as NonInstanceLogic;
                if (!GameEntry.OfflineMode.OfflineModeEnabled && instanceLogic.IsLobby)
                {
                    GameEntry.Data.NearbyPlayers.InitCurHeroTypes();
                    GameEntry.LobbyLogic.RefreshNearbyPlayers();
                }

                if (instanceLogic.IsLobby)
                {
                    GameEntry.LobbyLogic.ShowAllLobbyNpcs();
                }
                FireShouldGoToWaiting();
            }

            public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(elapseSeconds, realElapseSeconds);
                CheckFireInstanceReadyToStart();
            }

            private void CheckFireInstanceReadyToStart()
            {
                if (m_HasFireInstanceReadyToStart)
                {
                    return;
                }

                if (InstanceLogic.m_MainUIIsReady && InstanceLogic.m_MyHeroCharFirstTimeReady && SceneRegion.LoadingSceneRegionCount <= 0)
                {
                    m_HasFireInstanceReadyToStart = true;
                    GameEntry.Event.Fire(this, new InstanceReadyToStartEventArgs());
                }
            }

            private void RefreshSpawnPoint()
            {
                SpawnPoint spawnPoint = GameObject.FindObjectOfType<SpawnPoint>();
                if (spawnPoint == null)
                {
                    GameObject gameObject = new GameObject("Spawn Point");
                    gameObject.AddComponent<SpawnPoint>();
                    m_SpawnPoint = gameObject.transform;
                }
                else
                {
                    m_SpawnPoint = spawnPoint.transform;
                }
            }
        }

        private class Runner : AbstractInstanceRunner
        {
            private const float UpdateNearbyPlayerPosSec = 0.5f;
            private float m_UpdateNearbyPlayerPosSec = 0;
            private const float RefreshNearbyPlayerSec = 120f;
            private float m_RefreshNearbyPlayerSec = 0;

            private NonInstanceLogic InstanceLogic
            {
                get
                {
                    return m_InstanceLogic as NonInstanceLogic;
                }
            }

            public override void OnDraw(InstanceDrawReason reason, GameFrameworkAction onComplete)
            {
                throw new NotSupportedException();
            }

            public override void OnLose(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
            {
                throw new NotSupportedException();
            }

            public override void OnWin(InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                throw new NotSupportedException();
            }

            public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(elapseSeconds, realElapseSeconds);
                if (InstanceLogic.IsLobby)
                {
                    UpdateNearbyPlayers(elapseSeconds, realElapseSeconds);
                    RefreshNearbyPlayers(elapseSeconds, realElapseSeconds);
                }
            }

            private void RefreshNearbyPlayers(float elapseSeconds, float realElapseSeconds)
            {
                m_RefreshNearbyPlayerSec += elapseSeconds;
                if (m_RefreshNearbyPlayerSec < RefreshNearbyPlayerSec)
                {
                    return;
                }
                m_RefreshNearbyPlayerSec = 0;
                GameEntry.LobbyLogic.RefreshNearbyPlayers();
            }

            private void UpdateNearbyPlayers(float elapseSeconds, float realElapseSeconds)
            {
                m_UpdateNearbyPlayerPosSec += elapseSeconds;
                if (m_UpdateNearbyPlayerPosSec < UpdateNearbyPlayerPosSec)
                {
                    return;
                }
                m_UpdateNearbyPlayerPosSec = 0;

                var removeList = GameEntry.Data.NearbyPlayers.RemoveList;
                if (removeList.Count > 0)
                {
                    for (int i = 0; i < removeList.Count; i++)
                    {
                        if (GameEntry.Entity.GetEntity(removeList[i]) != null)
                        {
                            GameEntry.Entity.HideEntity(removeList[i]);
                        }
                    }
                    removeList.Clear();
                }

                var players = GameEntry.Data.NearbyPlayers.Data;
                for (int i = 0; i < players.Count; i++)
                {
                    var entity = GameEntry.Entity.GetGameEntity(players[i].Player.Id);
                    if (entity == null)
                    {
                        continue;
                    }

                    var player = players[i];

                    if (player.RandomMovement == null)
                    {
                        player.RandomMovement = GameEntry.Data.NearbyPlayers.InitRandomMovement(entity.CachedTransform.localPosition.ToVector2());
                    }

                    if (!player.RandomMovement.IsStartMove)
                    {
                        GameEntry.LobbyLogic.MoveNearbyPlayer(player);
                        continue;
                    }

                    if (player.RandomMovement.IsArriveTargetPos)
                    {
                        TimeSpan ts = GameEntry.Time.LobbyServerUtcTime.Subtract(player.RandomMovement.ArrivalTime);
                        if (ts.TotalSeconds >= player.RandomMovement.StayTime)
                        {
                            player.RandomMovement = GameEntry.Data.NearbyPlayers.GenerateRandomMovement();
                            continue;
                        }
                    }
                    else
                    {
                        GameEntry.LobbyLogic.RefreshNearbyPlayerMoveState(player);
                    }
                }
            }
        }
    }
}
