using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class SelectServerForm : NGUIForm
    {
        [SerializeField]
        private SelectServerItem m_SelectServerItemTemplate = null;

        [SerializeField]
        private GameObject m_LastServerPanel = null;

        [SerializeField]
        private GameObject m_SelectServerPanel = null;

        [SerializeField]
        private GameObject m_SelectServerScrollView = null;

        [SerializeField]
        private GameObject m_LastServer = null;

        [SerializeField]
        private UILabel m_LastServerNameInPanel = null;

        [SerializeField]
        private UIGrid m_ServerList = null;

        [SerializeField]
        private UILabel m_ServerConstituencyText = null;

        [SerializeField]
        private UILabel m_NetWorkText = null;

        [SerializeField]
        private UILabel m_ChangeServerText = null;

        [SerializeField]
        private GameObject m_HotIcon = null;

        [SerializeField]
        private GameObject m_FluentIcon = null;

        [SerializeField]
        private GameObject m_MaintenanceIcon = null;

        [SerializeField]
        private UserAgreementSubFrom m_UserAgreementSubFrom = null;

        [SerializeField]
        private NoticeSubFrom m_NoticeSubFrom = null;

        [SerializeField]
        private ScrollViewCache m_ServerTypesScrollView = null;

        private UserAgreementSubFrom m_UsingUserAgreement = null;

        private NoticeSubFrom m_UsingNotice = null;

        private List<SelectServerItem> m_ServerItems = new List<SelectServerItem>();

        private List<ServerAreaData> m_ServerTypes = new List<ServerAreaData>();

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CreateServerAreaTypeList();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            if (m_UsingUserAgreement == null)
            {
                m_UsingUserAgreement = CreateSubForm<UserAgreementSubFrom>("UserAgreementSubFrom", gameObject, m_UserAgreementSubFrom.gameObject, false);
            }
            m_UsingUserAgreement.InitUserAgreementPanel();

            if (m_UsingNotice == null)
            {
                m_UsingNotice = CreateSubForm<NoticeSubFrom>("NoticeSubFrom", gameObject, m_NoticeSubFrom.gameObject, false);
            }
            m_UsingNotice.InitNoticePanel();

            ProcedureSelectServer procedureSelectServer = GameEntry.Procedure.CurrentProcedure as ProcedureSelectServer;
            if (procedureSelectServer == null)
            {
                Log.Warning("Can not select server in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                return;
            }
            bool needShowAccountAgreement = GameEntry.Data.GetAndRemoveTempData<bool>(Constant.TempData.NeedShowAccountAgreement);
            if (needShowAccountAgreement)
            {
                ShowUserAgreementSubFrom();
            }
            ServerData lastServer = procedureSelectServer.GetLastServer();
            bool showLastServerPanel = (lastServer != null);
            if (lastServer == null)
            {
                lastServer = procedureSelectServer.GetServerDataByFlag(ServerFlag.Recommended)[0];
            }
            m_ChangeServerText.text = GameEntry.Localization.GetString("UI_BUTTON_CHANGESERVER");
            m_LastServerPanel.SetActive(true);
            m_SelectServerPanel.SetActive(false);
            m_LastServer.SetActive(showLastServerPanel);
            m_ServerTypes[m_ServerTypes.Count - 1].UiToggle.value = true;
            if (lastServer != null)
            {
                m_LastServer.GetComponent<SelectServerItem>().RefreshServerData(lastServer);
                m_LastServerNameInPanel.text = lastServer.Name;
                m_MaintenanceIcon.SetActive(lastServer.Flag == ServerFlag.Maintenance);
                if (lastServer.Load == ServerLoad.Full)
                {
                    m_NetWorkText.text = GameEntry.Localization.GetString("UI_BUTTON_SERVER_HOT");
                    m_HotIcon.SetActive(true);
                    m_FluentIcon.SetActive(false);
                }
                else if (lastServer.Load == ServerLoad.Good)
                {
                    m_NetWorkText.text = GameEntry.Localization.GetString("UI_BUTTON_SERVER_FLUENT");
                    m_HotIcon.SetActive(false);
                    m_FluentIcon.SetActive(true);
                }
                m_ServerConstituencyText.text = GameEntry.Localization.GetString("UI_SIGN_CONSTITUENCY", lastServer.Id);
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        private void CreateServerAreaTypeList()
        {
            ProcedureSelectServer procedureSelectServer = GameEntry.Procedure.CurrentProcedure as ProcedureSelectServer;
            if (procedureSelectServer == null)
            {
                Log.Warning("Can not select server in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                return;
            }

            int serverAreaCount = procedureSelectServer.GetServerCount / Constant.OneServerAreaContainMaxServerCount + 1;

            ServerAreaTypeItem recommendServer = m_ServerTypesScrollView.CreateItem();
            m_ServerTypesScrollView.Reposition();
            UIToggle recommenduiToggle = recommendServer.gameObject.GetComponent<UIToggle>();
            recommendServer.Refresh(OnTab);
            for (int i = serverAreaCount - 1; i >= 0; i--)
            {
                var serverItem = m_ServerTypesScrollView.CreateItem();
                m_ServerTypesScrollView.Reposition();
                UIToggle uiToggle = serverItem.gameObject.GetComponent<UIToggle>();
                m_ServerTypes.Add(new ServerAreaData { TabId = i, UiToggle = uiToggle, ServerAreaTypeItem = serverItem });
            }
            for (int i = serverAreaCount - 1; i >= 0; i--)
            {
                m_ServerTypes[i].ServerAreaTypeItem.Refresh(i + 1, OnTab);
            }
            m_ServerTypes.Add(new ServerAreaData { TabId = serverAreaCount, UiToggle = recommenduiToggle, ServerAreaTypeItem = recommendServer });
        }

        private void OnTab()
        {
            for (int i = 0; i < m_ServerTypes.Count; i++)
            {
                if (m_ServerTypes[i].UiToggle.value)
                {
                    if (i == m_ServerTypes.Count - 1)
                    {
                        ShowServerList();
                    }
                    else
                    {
                        ShowServerList(m_ServerTypes[i].TabId);
                    }
                    m_ServerList.Reposition();
                }
            }
        }

        public void OnClickLastServer()
        {
            ProcedureSelectServer procedureSelectServer = GameEntry.Procedure.CurrentProcedure as ProcedureSelectServer;
            if (procedureSelectServer == null)
            {
                Log.Warning("Can not select server in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                return;
            }

            //修改强制选择1服为真的最后一次选择
            ServerData lastServer = procedureSelectServer.GetLastServer();
            if (lastServer == null)
            {
                lastServer = procedureSelectServer.GetServerDataByFlag(ServerFlag.Recommended)[0];
            } 
            if (lastServer.CheckFlag(ServerFlag.Maintenance))
            {
                UIUtility.ShowOkayButtonDialog(GameEntry.Localization.GetString("UI_TEXT_SERVER_NOTE_MAINTENANCE"));
                return;
            }
            if(lastServer.isRestrict)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_STOP_REG") });
                return;
            }
            procedureSelectServer.ConnectServer(lastServer.Id);
        }

        public void OnClickSwitchAccount()
        {
            ProcedureSelectServer procedureSelectServer = GameEntry.Procedure.CurrentProcedure as ProcedureSelectServer;
            if (procedureSelectServer == null)
            {
                Log.Warning("Can not select server in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                return;
            }

            procedureSelectServer.GoBackToLogin();
        }

        public void OnClickSwitchServer()
        {
            m_LastServerPanel.SetActive(false);
            m_SelectServerPanel.SetActive(true);
        }

        public void OnClickScreen()
        {
            m_SelectServerPanel.SetActive(false);
            m_LastServerPanel.SetActive(true);
        }

        /// <summary>
        /// 根据Tab显示服务器列表。
        /// </summary>
        /// <param name="tabId"></param>
        private void ShowServerList(int tabId)
        {
            ProcedureSelectServer procedureSelectServer = GameEntry.Procedure.CurrentProcedure as ProcedureSelectServer;
            if (procedureSelectServer == null)
            {
                Log.Warning("Can not select server in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                return;
            }

            ServerData[] serverDatas = procedureSelectServer.GetServerDataByTab(tabId);
            ShowServerList(serverDatas);
        }

        /// <summary>
        /// 显示推荐服务器列表。
        /// </summary>
        private void ShowServerList()
        {
            ProcedureSelectServer procedureSelectServer = GameEntry.Procedure.CurrentProcedure as ProcedureSelectServer;
            if (procedureSelectServer == null)
            {
                Log.Warning("Can not select server in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                return;
            }
            ServerData[] serverDatas = procedureSelectServer.GetServerDataByFlag(ServerFlag.Recommended);
            ShowServerList(serverDatas);
        }

        private void ShowServerList(ServerData[] serverDatas)
        {
            for (int i = 0; i < serverDatas.Length; i++)
            {
                SelectServerItem selectServerItem = null;
                if (i < m_ServerItems.Count)
                {
                    selectServerItem = m_ServerItems[i];
                    m_ServerItems[i].gameObject.SetActive(true);
                }
                else
                {
                    GameObject go = NGUITools.AddChild(m_SelectServerScrollView, m_SelectServerItemTemplate.gameObject);
                    selectServerItem = go.GetComponent<SelectServerItem>();
                    m_ServerItems.Add(selectServerItem);
                    m_ServerItems[i].gameObject.SetActive(true);
                }

                selectServerItem.RefreshServerData(serverDatas[i]);
            }

            for (int i = serverDatas.Length; i < m_ServerItems.Count; i++)
            {
                m_ServerItems[i].gameObject.SetActive(false);
            }
        }

        public void ShowUserAgreementSubFrom()
        {
            if (m_UsingUserAgreement != null)
            {
                OpenSubForm(m_UsingUserAgreement);
            }
        }

        public void ShowmNoticeSubForm()
        {
            if (m_UsingNotice != null)
            {
                OpenSubForm(m_UsingNotice);
            }
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<ServerAreaTypeItem>
        {

        }

        private class ServerAreaData
        {
            public int TabId;
            public UIToggle UiToggle;
            public ServerAreaTypeItem ServerAreaTypeItem;
        }
    }
}
