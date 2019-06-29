using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class PlotDialogueForm : NGUIForm
    {
        [SerializeField]
        private float m_NextPlotTime = 20.0f;

        [SerializeField]
        private float m_NextWordTime = 10.0f;

        [SerializeField]
        private MoveLabel m_PlotContent = null;

        [SerializeField]
        private SideItem m_LeftSide = null;

        [SerializeField]
        private SideItem m_RightSide = null;

        private float m_CurTime = 0.0f;
        private float m_CurWordTime = 0.0f;
        private bool m_StartPlay = false;
        private bool m_StartWordPlay = false;
        private DRPlotDialogue m_PlotData = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.TimeScale.PauseGame();
            var myUserData = userData as PlotDialogDisplayData;
            if (myUserData == null)
            {
                Log.Error("PlotDialogDisplayData is invalid.");
                return;
            }

            if (myUserData.PlotReturn != null)
            {
                myUserData.PlotReturn();
            }

            StartPlot(myUserData.PlotDialogId);
        }

        protected override void OnClose(object userData)
        {
            GameEntry.TimeScale.ResumeGame();
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_StartPlay)
            {
                m_CurTime += realElapseSeconds;
                if (m_CurTime >= m_NextPlotTime)
                {
                    StartPlot(m_PlotData.NextId);
                }
            }
            else if (m_StartWordPlay)
            {
                m_CurWordTime += realElapseSeconds;
                if (m_CurWordTime >= m_NextWordTime)
                {
                    ShowNextWordLine();
                }
            }

#if UNITY_EDITOR
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))
            {
                OnClickWholeScreen();
            }
#endif
        }

        private void StartPlot(int plotId)
        {
            if (plotId == 0)
            {
                m_StartPlay = false;
                CloseSelf();
                return;
            }
            m_StartWordPlay = false;
            m_StartPlay = true;
            var dataTable = GameEntry.DataTable.GetDataTable<DRPlotDialogue>();
            m_PlotData = dataTable.GetDataRow(plotId);
            if (m_PlotData == null)
            {
                Log.Error("Cannot find Plot Dialogue '{0}'.", plotId);
                return;
            }

            m_CurTime = 0.0f;
            HideAllSide();
            bool isLeft = m_PlotData.IsPlayer ? true : m_PlotData.HeadLeft;
            var curSide = isLeft ? m_LeftSide : m_RightSide;
            curSide.SetVisible(true);
            //m_PlayerPortrait.LoadAsync(UIUtility.GetPlayerPortraitIconId());
            if (m_PlotData.IsPlayer)
            {
                curSide.SetPlayerName(GameEntry.Data.Player.Name);
            }
            else
            {
                curSide.SetNameValue(m_PlotData.RoleName);
            }
            curSide.SetNameIcon(m_PlotData.IsPlayer ? UIUtility.GetPlayerPortraitIconId() : m_PlotData.HeadIconId, m_PlotData.IsPlayer);
            m_PlotContent.ContentValue = GameEntry.Localization.GetString(m_PlotData.Content);
            if (!m_PlotContent.HasShowEndLine())
            {
                m_StartWordPlay = true;
                m_StartPlay = false;
                m_CurWordTime = 0;
            }
        }

        private void ShowNextWordLine()
        {
            if (m_PlotContent.HasShowEndLine())
            {
                m_PlotContent.ResumeLabel();
                StartPlot(m_PlotData.NextId);
                return;
            }

            m_CurWordTime = 0.0f;
            m_StartWordPlay = true;
            m_StartPlay = false;
            m_PlotContent.ShowNextLine();
        }

        private void HideAllSide()
        {
            m_LeftSide.SetVisible(false);
            m_RightSide.SetVisible(false);
        }

        public void OnClickSkipBt()
        {
            m_PlotContent.ResumeLabel();
            m_CurWordTime = 0;
            StartPlot(m_PlotData.NextId);
        }

        public void OnClickWholeScreen()
        {
            m_CurWordTime = 0;
            if (m_PlotContent.HasShowEndLine())
            {
                m_PlotContent.ResumeLabel();
                StartPlot(m_PlotData.NextId);
            }
            else
            {
                ShowNextWordLine();
            }
        }
    }
}
