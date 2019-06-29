using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class LobbyConnection
    {
        private class WaitForConnectState : StateBase
        {
            public override void Connect(IFsm<LobbyConnection> fsm)
            {
                base.Connect(fsm);
                ChangeState<ConnectState>(fsm);
            }
        }
    }
}
