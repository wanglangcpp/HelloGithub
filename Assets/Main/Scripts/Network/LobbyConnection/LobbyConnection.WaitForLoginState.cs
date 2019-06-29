using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class LobbyConnection
    {
        private class WaitForLoginState : StateBase
        {
            protected override void OnEnter(IFsm<LobbyConnection> fsm)
            {
                base.OnEnter(fsm);
                ChangeState<LoginState>(fsm);
            }
        }
    }
}
