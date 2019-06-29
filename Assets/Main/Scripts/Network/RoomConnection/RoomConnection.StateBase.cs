using GameFramework;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class RoomConnection
    {
        private abstract class StateBase : FsmState<RoomConnection>
        {
            protected override void OnEnter(IFsm<RoomConnection> fsm)
            {
                base.OnEnter(fsm);
                Log.Info(string.Format("Room Connection: {0} OnEnter.", GetType().Name));
            }

            public virtual void Connect(IFsm<RoomConnection> fsm)
            {
                ChangeState<WaitForConnectState>(fsm);
            }

            public virtual void SignIn(IFsm<RoomConnection> fsm)
            {

            }

            public virtual void OnNetworkConnected(IFsm<RoomConnection> fsm, object sender, UnityGameFramework.Runtime.NetworkConnectedEventArgs ne)
            {

            }

            public virtual void OnNetworkClosed(IFsm<RoomConnection> fsm, object sender, UnityGameFramework.Runtime.NetworkClosedEventArgs ne)
            {
            }
        }
    }
}
