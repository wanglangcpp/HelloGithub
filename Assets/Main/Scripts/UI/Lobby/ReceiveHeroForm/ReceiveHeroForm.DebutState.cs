using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ReceiveHeroForm
    {
        private class DebutState : StateBase
        {
            private IFsm<ReceiveHeroForm> m_Fsm = null;

            protected override void OnEnter(IFsm<ReceiveHeroForm> fsm)
            {
                base.OnEnter(fsm);
                m_Fsm = fsm;
                fsm.Owner.m_Character.RegisterDebutReceiveHeroEndCallback(HeroDebutEnd);
            }

            protected override void OnLeave(IFsm<ReceiveHeroForm> fsm, bool isShutdown)
            {
                m_Fsm.Owner.m_Character.RegisterDebutReceiveHeroEndCallback(null);
                base.OnLeave(fsm, isShutdown);
            }

            public override void GoToNextStep(IFsm<ReceiveHeroForm> fsm)
            {
                ChangeState<DefaultState>(fsm);
            }

            public override void SkipAnimation(IFsm<ReceiveHeroForm> fsm)
            {
                // Empty.
            }

            private void HeroDebutEnd()
            {
                GoToNextStep(m_Fsm);
            }
        }
    }
}
