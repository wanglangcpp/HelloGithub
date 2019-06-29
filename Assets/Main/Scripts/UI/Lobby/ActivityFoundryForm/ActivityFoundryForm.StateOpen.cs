using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        /// <summary>
        /// 初始状态。
        /// </summary>
        private class StateOpen : StateBase
        {
            protected override void OnEnter(IFsm<ActivityFoundryForm> fsm)
            {
                base.OnEnter(fsm);
                var owner = fsm.Owner;
                var srcData = owner.m_SrcData;
                if (!srcData.HasTeam)
                {
                    owner.CloseCDMaskDoor(false);
                    ChangeState<StateNoTeam>(fsm);
                }
                else
                {
                    CheckStaticDisplay(owner);
                    ChangeState<StateHasTeam>(fsm);
                }
            }

            private void CheckStaticDisplay(ActivityFoundryForm owner)
            {
                if (owner.CanPerformFoundry)
                {
                    owner.OpenCDMaskDoor(false);
                }
                else
                {
                    owner.CloseCDMaskDoor(false);
                }
            }
        }
    }
}
