using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class RoomConnection
    {
        private class WaitForConnectState : StateBase
        {
            protected override void OnEnter(IFsm<RoomConnection> fsm)
            {
                base.OnEnter(fsm);
                ChangeState<ConnectState>(fsm);
            }

        }
    }
}
