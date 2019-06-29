using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureSelectServer : ProcedureBase
    {
        private bool m_GoBackToLogin = false;
        private bool m_SelectServerOK = false;
        private ServerListData m_ServerDatas = null;

        private UIForm m_SelectServerForm = null;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(EventId.ResponseConnectLobbyServer, OnResponseConnectLobbyServer);

            m_GoBackToLogin = false;
            m_SelectServerOK = false;

            m_ServerDatas = GameEntry.Data.GetTempData<ServerListData>(Constant.TempData.ServerList);

            GameEntry.UI.OpenUIForm(UIFormId.SelectServer);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            m_ServerDatas = null;

            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
                GameEntry.Event.Unsubscribe(EventId.ResponseConnectLobbyServer, OnResponseConnectLobbyServer);
            }

            if (GameEntry.IsAvailable && GameEntry.UI != null && m_SelectServerForm != null)
            {
                GameEntry.UI.CloseUIForm(m_SelectServerForm);
                m_SelectServerForm = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_GoBackToLogin)
            {
                ChangeState<ProcedureLogin>(procedureOwner);
                return;
            }

            if (m_SelectServerOK)
            {
                ChangeState<ProcedureUpdateTexts>(procedureOwner);
                return;
            }
        }

        public ServerData GetLastServer()
        {
            int lastServerId = GameEntry.Data.Account.LastServerId;
            if (lastServerId == 0)
            {
                return null;
            }

            if (m_ServerDatas == null)
            {
                return null;
            }

            return m_ServerDatas.GetServerData(lastServerId);
        }

        public ServerData[] GetServerDataByTab(int tabId)
        {
            if (m_ServerDatas == null)
            {
                return new ServerData[] { };
            }

            return m_ServerDatas.GetServerDataByArea(tabId);
        }

        public ServerData[] GetServerDataByFlag(ServerFlag flag)
        {
            if (m_ServerDatas == null)
            {
                return new ServerData[] { };
            }

            return m_ServerDatas.GetServerDataByFlag(flag);
        }

        public int GetServerCount
        {
            get { return m_ServerDatas.ServerList.Count; }
        }

        public void ConnectServer(int serverId)
        {
            ServerData serverData = m_ServerDatas.GetServerData(serverId);
            if (serverData == null)
            {
                Log.Warning("Server Id '{0}' is invalid.", serverId.ToString());
                return;
            }

            GameEntry.Data.Account.ServerData = serverData;
            GameEntry.Event.Fire(this, new RequestConnectLobbyServerEventArgs());
        }

        public void GoBackToLogin()
        {
            m_GoBackToLogin = true;
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs ne = e as UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
            if (ne.UIForm.Logic is SelectServerForm)
            {
                m_SelectServerForm = ne.UIForm;
            }
        }

        private void OnResponseConnectLobbyServer(object sender, GameEventArgs e)
        {
            m_SelectServerOK = true;
            GameEntry.TaskComponent.LogInTime = GameEntry.Time.LobbyServerUtcTime;
        }
    }
}
