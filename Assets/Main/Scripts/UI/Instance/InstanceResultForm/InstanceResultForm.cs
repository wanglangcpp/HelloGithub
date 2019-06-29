using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using System;
using System.Reflection;


namespace Genesis.GameClient
{
    public partial class InstanceResultForm : NGUIForm
    {
        private const string WinIconEffectKey = "EffectWin";

        [SerializeField]
        private Transform[] m_SubPanels = null;

        [SerializeField]
        private float m_ExpSubPanelDuration = 3f;

        [SerializeField]
        private Transform m_RequestSubPanel = null;

        [SerializeField]
        private Transform m_ExpAndRewardSubPanel = null;

        [SerializeField]
        private Transform m_CongratulationsSubPanel = null;

        [SerializeField]
        private Transform m_StoryRewardsPanel = null;

        [SerializeField]
        private UIButton m_ReturnLobbyButton = null;

        [SerializeField]
        private UIButton m_ReturnChapterBtn = null;

        [SerializeField]
        private UISprite[] m_BigStars = null;

        [SerializeField]
        private Animation[] m_StarsMoveAnimations = null;

        [SerializeField]
        private UISprite[] m_StoryBigStars = null;

        [SerializeField]
        private Request[] m_Requests = null;

        [SerializeField]
        private Hero[] m_Heroes = null;

        [SerializeField]
        private UILabel m_CoinEarned = null;

        [SerializeField]
        private UILabel m_PlayerExpPlus = null;

        [SerializeField]
        private UILabel m_HeroLevelMax = null;

        [SerializeField]
        private UILabel m_MeridianEnergyCount = null;

        [SerializeField]
        private Transform m_CoinParent = null;

        [SerializeField]
        private GeneralItemView[] m_ItemEarned = null;

        [SerializeField]
        private GeneralItemView[] m_StoryItemEarned = null;

        [SerializeField]
        private Animation m_WinIconAnimation = null;

        [SerializeField]
        private Animation m_RewardBgAnimation = null;

        [SerializeField]
        private Animation m_MeridianEnergy = null;

        [SerializeField]
        private float m_BigStarsAnimationSpeed = 0;

        private IFsm<InstanceResultForm> m_Fsm;

        private InstanceResultData m_Data;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var resultData = userData as InstanceResultData;
            if (resultData == null)
            {
                CreateFsm_Simple();
                return;
            }

            m_Data = resultData;

            switch (m_Data.Type)
            {
                case InstanceLogicType.SinglePlayer:
                    CreateFsm_SinglePlayer();
                    break;
                case InstanceLogicType.CosmosCrack:
                    CreateFsm_CosmosCrack();
                    break;
                default:
                    Log.Error("Instance logic type {0} is not supported.", m_Data.Type);
                    return;
            }
        }

        private void CreateFsm_Simple()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm("Simple", this,
                new InitState(typeof(WinIconState)),
                new WinIconState(typeof(SimpleState)),
                new SimpleState());
            m_Fsm.Start<InitState>();
        }

        private void CreateFsm_SinglePlayer()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm("SinglePlayer", this,
                new InitState(typeof(WinIconState)),
                new WinIconState(typeof(RequestState)),
                new RequestState(typeof(ExpAndRewardState), m_RequestSubPanel, null),
                new ExpAndRewardState(null, m_ExpAndRewardSubPanel, m_RequestSubPanel));
            m_Fsm.Start<InitState>();
        }

        private void CreateFsm_CosmosCrack()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm("CosmosCrack", this,
                new InitState(typeof(WinIconState)),
                new WinIconState(typeof(RequestState)),
                new RequestState(typeof(RewardState), m_RequestSubPanel, null),
                new RewardState(null, m_ExpAndRewardSubPanel, m_RequestSubPanel));
            m_Fsm.Start<InitState>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;
            m_Data = null;
            base.OnClose(userData);
        }

        // Called by NGUI via reflection
        public void OnClickNextButton()
        {
            (m_Fsm.CurrentState as StateBase).GoToNextStep(m_Fsm);
        }

        public void OnClickWholeBg()
        {
            (m_Fsm.CurrentState as StateBase).SkipAnimation(m_Fsm);
        }

        public void OnClickNextInstance()
        {
            (m_Fsm.CurrentState as StateBase).GotoNextInstance(m_Fsm);
        }


        public void OnClickOpenInstance()
        {
            var tInstance = GameEntry.SceneLogic.BaseInstanceLogic as SinglePlayerInstanceLogic;
            //int type = tInstance.InstanceId
            //GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenInstanceSelectForm, true);
            if (tInstance == null)
                return;
            tInstance.OnLevelInstance();
            Abandon(false);
        }

        private void Abandon(bool autoHideLoading)
        {
            if (GameEntry.SceneLogic.BaseInstanceLogic.HasResult)
            {
                GameEntry.SceneLogic.GoBackToLobby(autoHideLoading);
                return;
            }

            GameEntry.SceneLogic.BaseInstanceLogic.SetInstanceFailure(InstanceFailureReason.AbandonedByUser, false,
                delegate () { GameEntry.SceneLogic.GoBackToLobby(autoHideLoading); });
        }

        private void Reset()
        {
            for (int i = 0; i < m_SubPanels.Length; ++i)
            {
                m_SubPanels[i].gameObject.SetActive(false);
            }


            for (int i = 0; i < m_BigStars.Length; i++)
            {
                m_BigStars[i].gameObject.SetActive(false);
            }

            m_ReturnLobbyButton.gameObject.SetActive(false);
            m_ReturnChapterBtn.gameObject.SetActive(false);
        }
    }
}
