using GameFramework.Event;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        private partial class StateHasTeam
        {
            /// <summary>
            /// 可锻造的状态。
            /// </summary>
            private class StateFoundry : StateBase
            {
                private int cachedLevel = -1;

                protected override void OnEnter(IFsm<StateHasTeam> fsm)
                {
                    base.OnEnter(fsm);

                    if (m_OuterFsm.Owner.m_CDMaskDoorStatus != ActivityFoundryForm.CDMaskDoorStatus.Open)
                    {
                        m_OuterFsm.Owner.OpenCDMaskDoor(true);
                    }

                    cachedLevel = m_OuterFsm.Owner.m_SrcData.Progress.CurrentLevel;
                }

                public override void OnPerformedFoundry(object sender, GameEventArgs e)
                {
                    base.OnPerformedFoundry(sender, e);

                    m_OuterFsm.Owner.m_EffectsController.ShowEffect(EffectFireClick);

                    if (m_OuterFsm.Owner.RewardLevel >= 0)
                    {
                        ChangeState<StateReward>(m_CachedFsm);
                    }
                    else if (m_OuterFsm.Owner.IsComplete)
                    {
                        ChangeState<StateCompleting>(m_CachedFsm);
                    }
                    else if (m_OuterFsm.Owner.m_SrcData.Progress.CurrentLevel > cachedLevel)
                    {
                        ChangeState<StateLevelUp>(m_CachedFsm);
                    }
                    else if (m_OuterFsm.Owner.IsCoolingDown)
                    {
                        ChangeState<StateCD>(m_CachedFsm);
                    }
                }
            }
        }
    }
}
