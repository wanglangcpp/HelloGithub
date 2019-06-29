using GameFramework.Fsm;
using System;

namespace Genesis.GameClient
{
    public partial class InstanceResultForm
    {
        private class InitState : StateBase
        {
            public override void GoToNextStep(IFsm<InstanceResultForm> fsm)
            {
                // Do nothing
            }

            public override void SkipAnimation(IFsm<InstanceResultForm> fsm)
            {

            }

            protected override void OnEnter(IFsm<InstanceResultForm> fsm)
            {
                base.OnEnter(fsm);
                fsm.Owner.Reset();
            }

            protected override void OnUpdate(IFsm<InstanceResultForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (fsm.CurrentStateTime > .1f)
                {
                    ChangeState(fsm, m_NextStateType);
                }
            }

            public InitState(Type nextStateType) : base(nextStateType)
            {

            }
        }
    }
}
