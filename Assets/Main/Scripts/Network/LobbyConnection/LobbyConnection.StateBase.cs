using GameFramework;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class LobbyConnection
    {
        private abstract class StateBase : FsmState<LobbyConnection>
        {
            protected override void OnEnter(IFsm<LobbyConnection> fsm)
            {
                base.OnEnter(fsm);
                Log.Info(string.Format("Lobby Connection: {0} OnEnter.", GetType().Name));
            }

            protected override void OnLeave(IFsm<LobbyConnection> fsm, bool isShutdown)
            {
                //      Log.Info(string.Format("Lobby Connection: {0} OnLeave.", GetType().Name));
            }

            public virtual void Connect(IFsm<LobbyConnection> fsm)
            {

            }

            public virtual void SignIn(IFsm<LobbyConnection> fsm)
            {

            }

            public virtual void OnNetworkConnected(IFsm<LobbyConnection> fsm, object sender, UnityGameFramework.Runtime.NetworkConnectedEventArgs ne)
            {
            }

            public virtual void OnNetworkClosed(IFsm<LobbyConnection> fsm, object sender, UnityGameFramework.Runtime.NetworkClosedEventArgs ne)
            {
                if (GetType().Name != "ConnectState")
                {
                    ChangeState<WaitForConnectState>(fsm);
                    GameEntry.Waiting.ClearWaitingOfType(WaitingType.Network);
                }
            }
        }
    }
}
