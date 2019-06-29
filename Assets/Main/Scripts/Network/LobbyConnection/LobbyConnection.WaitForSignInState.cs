using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class LobbyConnection
    {
        private class WaitForSignInState : StateBase
        {
            protected override void OnEnter(IFsm<LobbyConnection> fsm)
            {
                base.OnEnter(fsm);

                if (fsm.Owner.m_IsReConnecting && !(GameEntry.Procedure.CurrentProcedure is ProcedureSelectServer)
                    && !(GameEntry.Procedure.CurrentProcedure is ProcedureUpdateTexts)
                    && !(GameEntry.Procedure.CurrentProcedure is ProcedureLoadTexts))
                {
                    ChangeState<SignInState>(fsm);
                }
            }

            public override void SignIn(IFsm<LobbyConnection> fsm)
            {
                base.SignIn(fsm);
                ChangeState<SignInState>(fsm);
            }
        }
    }
}
