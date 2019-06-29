using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class LobbyConnection
    {
        private readonly IFsm<LobbyConnection> m_Fsm;
        private bool m_IsConnecting = false;
        private bool m_IsReConnecting = false;

        public LobbyConnection()
        {
            m_Fsm = GameEntry.Fsm.CreateFsm(this,
                new WaitForConnectState(),
                new ConnectState(),
                new WaitForLoginState(),
                new LoginState(),
                new WaitForSignInState(),
                new SignInState(),
                new CheckingState());
            m_Fsm.Start<WaitForConnectState>();
        }

        public bool IsConnect
        {
            get
            {
                return m_IsConnecting;
            }
        }

        public void Shutdown()
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);
        }

        public void Connect()
        {
            m_IsConnecting = true;
            (m_Fsm.CurrentState as StateBase).Connect(m_Fsm);
        }

        public void SignIn()
        {
            (m_Fsm.CurrentState as StateBase).SignIn(m_Fsm);
        }

        public void OnNetworkConnected(object sender, UnityGameFramework.Runtime.NetworkConnectedEventArgs ne)
        {
            m_IsConnecting = false;
            (m_Fsm.CurrentState as StateBase).OnNetworkConnected(m_Fsm, sender, ne);
        }

        public void OnNetworkClosed(object sender, UnityGameFramework.Runtime.NetworkClosedEventArgs ne)
        {
            (m_Fsm.CurrentState as StateBase).OnNetworkClosed(m_Fsm, sender, ne);
            m_IsReConnecting = true;
        }
    }
}
