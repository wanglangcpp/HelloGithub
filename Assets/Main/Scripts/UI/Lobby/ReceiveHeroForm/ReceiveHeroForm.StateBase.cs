using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ReceiveHeroForm
    {
        private abstract class StateBase : FsmState<ReceiveHeroForm>
        {
            protected override void OnEnter(IFsm<ReceiveHeroForm> fsm)
            {
                base.OnEnter(fsm);
            }

            protected override void OnLeave(IFsm<ReceiveHeroForm> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            public virtual bool StartInit(IFsm<ReceiveHeroForm> fsm)
            {
                return false;
            }

            public abstract void GoToNextStep(IFsm<ReceiveHeroForm> fsm);

            public abstract void SkipAnimation(IFsm<ReceiveHeroForm> fsm);
        }
    }
}
