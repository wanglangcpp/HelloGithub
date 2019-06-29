using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        private partial class StateHasTeam
        {
            /// <summary>
            /// 初始状态。
            /// </summary>
            private class StateInit : StateBase
            {
                protected override void OnEnter(IFsm<StateHasTeam> fsm)
                {
                    base.OnEnter(fsm);
                    var outerOwner = m_OuterFsm.Owner;
                    outerOwner.ResetHasTeamDisplay();

                    if (m_OuterFsm.Owner.IsComplete)
                    {
                        ChangeState<StateCompleted>(fsm);
                    }
                    else if (m_OuterFsm.Owner.RewardLevel >= 0)
                    {
                        ChangeState<StateReward>(fsm);
                    }
                    else if (m_OuterFsm.Owner.IsCoolingDown)
                    {
                        ChangeState<StateCD>(fsm);
                    }
                    else
                    {
                        ChangeState<StateFoundry>(fsm);
                    }
                }
            }
        }
    }
}
