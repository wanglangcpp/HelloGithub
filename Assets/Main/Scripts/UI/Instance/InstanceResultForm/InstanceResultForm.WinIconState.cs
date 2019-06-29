using GameFramework.Fsm;
using System;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        private class WinIconState : StateBase
        {
            private const string WinIconAnimName = "InstanceWin";

            public override void GoToNextStep(IFsm<InstanceResultForm> fsm)
            {
                // Do nothing.
            }

            public override void SkipAnimation(IFsm<InstanceResultForm> fsm)
            {
                // Do nothing.
            }

            protected override void OnInit(IFsm<InstanceResultForm> fsm)
            {
                base.OnInit(fsm);
                fsm.Owner.m_RewardBgAnimation.gameObject.SetActive(false);
            }

            protected override void OnEnter(IFsm<InstanceResultForm> fsm)
            {
                base.OnEnter(fsm);

                fsm.Owner.m_WinIconAnimation[WinIconAnimName].speed = 1;
                fsm.Owner.m_WinIconAnimation[WinIconAnimName].time = 0;
                fsm.Owner.m_WinIconAnimation.Play();
                fsm.Owner.m_EffectsController.ShowEffect(WinIconEffectKey);

                fsm.Owner.m_RewardBgAnimation.gameObject.SetActive(true);
                fsm.Owner.m_RewardBgAnimation.Play();
            }

            protected override void OnUpdate(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (fsm.Owner.m_WinIconAnimation.IsPlaying(WinIconAnimName))
                {
                    return;
                }

                ChangeState(fsm, m_NextStateType);
            }

            public WinIconState(Type nextStateType) : base(nextStateType)
            {

            }
        }
    }
}
