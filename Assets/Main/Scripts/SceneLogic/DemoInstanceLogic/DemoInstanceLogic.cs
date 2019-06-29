using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 展示副本逻辑。
    /// </summary>
    public partial class DemoInstanceLogic : BaseSinglePlayerInstanceLogic
    {
        public override InstanceLogicType Type
        {
            get
            {
                return InstanceLogicType.Demo;
            }
        }

        private DRDemoInstance m_DRInstance;

        protected override DRInstance DRInstance
        {
            get
            {
                return m_DRInstance;
            }
        }

        public override void Init(int instanceOrSceneId, object userData)
        {
            base.Init(instanceOrSceneId, userData);
            m_DRInstance = GetInstanceDataRow<DRDemoInstance>(m_InstanceId);
            InitInstanceDataBefore(m_DRInstance);
        }

        public override void RequestStartInstance()
        {
            InitNpcDataTable(m_DRInstance);
            base.RequestStartInstance();
        }

        protected override void OnInstanceSuccess(InstanceSuccessReason reason, GameFrameworkAction onComplete)
        {
            GameEntry.Tutorial.HideMask();
            base.OnInstanceSuccess(reason, onComplete);
            GoToCreatePlayer();
        }

        protected override void PrepareMyHeroesData(PlayerHeroesData heroesData, Vector2 spawnPosition, float spawnRotation)
        {
            var demoHeroes = GameEntry.Tutorial.Config.GetDemoHeroes();
            for (int i = 0; i < demoHeroes.Count; ++i)
            {
                var demoHero = demoHeroes[i];
                LobbyHeroData lobbyHeroData = (LobbyHeroData)demoHero;
                var heroData = GetHeroData(GameEntry.Entity.GetSerialId(), lobbyHeroData, true, CampType.Player, spawnPosition, spawnRotation);
                heroData.MinHP = m_DRInstance.HeroMinHP;
                heroData.MaxHP = demoHero.MaxHP;
                heroData.HP = demoHero.Hp;
                heroData.PhysicalAttack = demoHero.PhysicalAttack;
                heroData.PhysicalDefense = demoHero.PhysicalDefense;
                heroData.CriticalHitProb = demoHero.CriticalHitProb;
                heroData.CriticalHitRate = demoHero.CriticalHitRate;
                heroData.Speed = demoHero.Speed;
                heroData.IsDead = false;
                heroesData.Add(heroData);
            }
        }

        private void GoToCreatePlayer()
        {
            var mainProcedure = GameEntry.Procedure.CurrentProcedure as ProcedureMain;

            if (mainProcedure == null)
            {
                return;
            }
            ShouldShowHud = false;
            mainProcedure.GoToCreatePlayer();
        }

        protected override void PrepareSuccessData()
        {
            // Empty.
        }
    }
}
