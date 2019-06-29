using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public struct RankListPvpData
    {
        public int Ranking;
        public int Portrait;
        public int Level;
        public int Might;
        public string Name;
        public string Server;
        public string Grading;
    }
    /// <summary>
    /// 排行榜界面。
    /// </summary>
    public class RankListForm : NGUIForm
    {
        [SerializeField]
        private Tab m_MightTab = null;

        [SerializeField]
        private Tab m_OfflineArenaTab = null;

        [SerializeField]
        private Tab m_PvpMainTab = null;

        [SerializeField]
        private Tab m_PvpLocalServerTab = null;

        [SerializeField]
        private Tab m_PvpAllServerTab = null;

        [SerializeField]
        private UILabel m_MyScoreDesc = null;

        private Tab[] m_Tabs = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            var displayData = userData as RankListDisplayData;
            if (displayData == null)
            {
                InitTabs(RankListType.TotalMight);
                return;
            }
            InitTabs(displayData.Scenario);
        }

        protected override void OnClose(object userData)
        {
            DeinitTabs();
            base.OnClose(userData);
        }

        public void OnTab()
        {
            for (int i = 0; i < m_Tabs.Length - 1; ++i)
            {
                if (m_Tabs[i].TabToggle.value)
                {
                    if (!m_Tabs[i].Content.gameObject.activeSelf)
                    {
                        m_Tabs[i].Content.gameObject.SetActive(true);
                    }

                    if (!string.IsNullOrEmpty(m_Tabs[i].MyScoreDescKey))
                    {
                        m_MyScoreDesc.text = GameEntry.Localization.GetString(m_Tabs[i].MyScoreDescKey);
                    }
                }
                else
                {
                    if (m_Tabs[i].Content.gameObject.activeSelf)
                    {
                        m_Tabs[i].Content.gameObject.SetActive(false);
                    }
                }
                var curTitle = m_Tabs[i].Content.GetComponent<RankListFormBaseTabContent>().CurTitle;
                if (curTitle != null)
                {
                    curTitle.SetActive(m_Tabs[i].TabToggle.value);
                }
            }
        }

        private void InitTabs(RankListType scenario)
        {
            m_Tabs = new Tab[] { m_MightTab, m_OfflineArenaTab, m_PvpLocalServerTab, m_PvpAllServerTab, m_PvpMainTab/*,m_LevelTab */};

            for (int i = 0; i < m_Tabs.Length; ++i)
            {
                if ((int)scenario - 1 == i)
                {
                    if (scenario == RankListType.PvpMain)
                    {
                        m_Tabs[i].TabToggle.Set(true);
                    }
                    else
                    {
                        m_Tabs[i].TabToggle.Set(true);
                        m_Tabs[i].Content.gameObject.SetActive(true);
                    }
                }
                else
                {
                    m_Tabs[i].TabToggle.Set(false);
                    if (m_Tabs[i].Content.name == "Pvp Single Btn")
                    {
                        m_Tabs[i].Content.gameObject.SetActive(true);
                    }
                    else
                    {
                        m_Tabs[i].Content.gameObject.SetActive(false);
                    }
                }
            }

            //OnTab();
        }

        private void DeinitTabs()
        {
            // When this method is called, the toggles' GameObjects are inactive, so that their link in a group is broken.
            // Therefore, UIToggle.Set method is required to use here.
            for (int i = 0; i < m_Tabs.Length; ++i)
            {
                m_Tabs[i].TabToggle.Set(i == 0);
            }

            for (int i = 0; i < m_Tabs.Length; ++i)
            {
                m_Tabs[i].Content.gameObject.SetActive(false);
            }

            m_Tabs = null;
        }

        [Serializable]
        private class Tab
        {
            public UIToggle TabToggle = null;
            public Transform Content = null;
            public string MyScoreDescKey = null;
        }
    }
}
