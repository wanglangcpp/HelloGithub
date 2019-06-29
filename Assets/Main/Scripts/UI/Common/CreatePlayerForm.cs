using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CreatePlayerForm : NGUIForm
    {
        [SerializeField]
        private GameObject[] m_Panel = null;

        [SerializeField]
        private UIInput m_PlayerName = null;

        [SerializeField]
        private Transform m_PlatformRoot = null;

        [SerializeField]
        private UILabel m_HeroNameText = null;

        [SerializeField]
        private UILabel m_HeroDescText = null;

        [SerializeField]
        private int[] m_CandidateHeroTypes = null;

        [SerializeField]
        private float m_HeroPosRadiusToPlatform = 2.1f;

        [SerializeField]
        private float m_HeroPlatformAngularSpeed = 360f;

        [SerializeField]
        private string m_SelectedHeroEffectResPath = "UI/effect_ui_createhero";

        private int m_SelectedHeroType = -1;

        private float m_DefaultPlatformRotation = 0f;
        private List<FakeCharacter> m_Characters = new List<FakeCharacter>();
        private SecondPageController m_SecondPageController = null;

        private int SelectedHeroIndex
        {
            get
            {
                int index = -1;
                for (int i = 0; i < m_Characters.Count; ++i)
                {
                    if (m_Characters[i] != null && m_Characters[i].Data.HeroId == m_SelectedHeroType)
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_DefaultPlatformRotation = m_PlatformRoot.localEulerAngles.y;
        }

        protected override void OnOpen(object userData)
        {
            GameEntry.Impact.IncreaseHidingNameBoardCount();
            GameEntry.Event.Subscribe(EventId.CheckNameComplete, OnCheckNameComplete);
            base.OnOpen(userData);

            //ShowFirstPage();
            ShowPage();
            OnClickRandomName();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_SecondPageController != null && m_SecondPageController.IsEnabled)
            {
                m_SecondPageController.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }
            GameEntry.Impact.DecreaseHidingNameBoardCount();
            GameEntry.Event.Unsubscribe(EventId.CheckNameComplete, OnCheckNameComplete);
            DeinitSecondPageController();
            m_PlatformRoot.localRotation = Quaternion.Euler(0f, m_DefaultPlatformRotation, 0f);
            m_SelectedHeroType = -1;
            base.OnClose(userData);
        }

        public void OnCreatePlayerClick()
        {
            if (m_SecondPageController != null && m_SecondPageController.IsEnabled)
            {
                m_SecondPageController.OnCreatePlayerClick();
            }
        }

        public void OnNextPageClient()
        {
            CLCheckDisplayName request = new CLCheckDisplayName();
            request.DisplayName = m_PlayerName.value;
            GameEntry.Network.Send(request);
        }

        public void OnClickEnterBtn()
        {
            CLCheckDisplayName request = new CLCheckDisplayName();
            request.DisplayName = m_PlayerName.value;
            GameEntry.Network.Send(request);
        }

        public void OnClickRandomName()
        {
            ProcedureCreatePlayer procedureCreatePlayer = GameEntry.Procedure.CurrentProcedure as ProcedureCreatePlayer;
            if (procedureCreatePlayer == null)
            {
                Log.Warning("Can not create player in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                return;
            }

            m_PlayerName.value = procedureCreatePlayer.GetRandomName();
        }

        public void OnClickBackBtn()
        {
            //if (m_SecondPageController != null && m_SecondPageController.IsEnabled)
            //{
            //    m_SecondPageController.OnClickBackBtn();
            //}
        }

        private void ClearAllCharacters()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            for (int i = 0; i < m_Characters.Count; ++i)
            {
                if (m_Characters[i] == null)
                {
                    continue;
                }

                GameEntry.Entity.HideEntity(m_Characters[i]);
            }

            m_Characters.Clear();
        }

        private void OnCheckNameComplete(object sender, GameEventArgs e)
        {
            //ShowSecondPage();
            //GameEntry.UIBackground.HideDefault();
            OnCreatePlayerClick();

            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.CreatPlayer, true);
            //SDKManager.Instance.helper.Record("CreatePlayer", m_PlayerName.value);
        }

        private void ShowFirstPage()
        {
            m_Panel[0].SetActive(true);
            m_Panel[1].SetActive(false);
            GameEntry.UIBackground.ShowDefault();
            DeinitSecondPageController();
        }

        private void ShowSecondPage()
        {
            m_Panel[0].SetActive(false);
            m_Panel[1].SetActive(true);
            GameEntry.UIBackground.HideDefault();
            InitSecondPageController();
        }

        private void ShowPage()
        {
            GameEntry.UIBackground.HideDefault();
            InitSecondPageController();
        }

        private void InitSecondPageController()
        {
            m_SecondPageController = new SecondPageController();
            m_SecondPageController.Init(this);
        }

        private void DeinitSecondPageController()
        {
            if (m_SecondPageController == null)
            {
                return;
            }

            m_SecondPageController.Shutdown();
            m_SecondPageController = null;
        }
    }
}
