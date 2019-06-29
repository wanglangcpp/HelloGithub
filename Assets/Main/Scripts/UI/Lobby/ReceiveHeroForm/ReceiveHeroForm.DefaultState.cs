using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ReceiveHeroForm
    {
        private class DefaultState : StateBase
        {
            private float m_CurTime = 0.0f;

            protected override void OnEnter(IFsm<ReceiveHeroForm> fsm)
            {
                base.OnEnter(fsm);
                m_CurTime = 0.0f;
                ShowHeroInfo(fsm);
                fsm.Owner.m_Character.DefaultReceiveHero();
            }

            protected override void OnLeave(IFsm<ReceiveHeroForm> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ReceiveHeroForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                m_CurTime += elapseSeconds;
                //if (fsm.Owner.m_DefaultAnimTime <= m_CurTime)
                //{
                //    GoToNextStep(fsm);
                //}
            }

            public override void GoToNextStep(IFsm<ReceiveHeroForm> fsm)
            {
                ChangeState<EndState>(fsm);
            }

            public override void SkipAnimation(IFsm<ReceiveHeroForm> fsm)
            {
                GoToNextStep(fsm);
            }

            private void ShowHeroInfo(IFsm<ReceiveHeroForm> fsm)
            {
                fsm.Owner.m_HeroInfo.gameObject.SetActive(true);
                fsm.Owner.m_HeroInfo.InitReceiveHeroInfo(fsm.Owner.HeroData, fsm.Owner.HeroChipCount);
            }
        }
    }

}
