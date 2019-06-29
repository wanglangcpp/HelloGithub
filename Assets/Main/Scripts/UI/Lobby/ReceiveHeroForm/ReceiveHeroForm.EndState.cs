using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ReceiveHeroForm
    {
        private class EndState : StateBase
        {
            protected override void OnEnter(IFsm<ReceiveHeroForm> fsm)
            {
                base.OnEnter(fsm);
                HideAllUI(fsm);
                ClearFakeCharacter(fsm);
                fsm.Owner.ShowNextHero();
            }

            protected override void OnLeave(IFsm<ReceiveHeroForm> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            public override void GoToNextStep(IFsm<ReceiveHeroForm> fsm)
            {

            }

            public override void SkipAnimation(IFsm<ReceiveHeroForm> fsm)
            {

            }

            public override bool StartInit(IFsm<ReceiveHeroForm> fsm)
            {
                ChangeState<InitState>(fsm);
                return true;
            }

            private void ClearFakeCharacter(IFsm<ReceiveHeroForm> fsm)
            {
                if (fsm.Owner.m_Character != null && fsm.Owner.m_Character.IsAvailable)
                {
                    GameEntry.Entity.HideEntity(fsm.Owner.m_Character.Entity);
                    fsm.Owner.m_Character = null;
                }
            }

            private void HideAllUI(IFsm<ReceiveHeroForm> fsm)
            {
                fsm.Owner.m_HeroInfo.gameObject.SetActive(false);
                fsm.Owner.m_CongrContent.gameObject.SetActive(false);
            }
        }
    }
}
