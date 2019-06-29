using GameFramework.Event;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        private partial class StateHasTeam
        {
            /// <summary>
            /// 状态基类。
            /// </summary>
            private class StateBase : FsmState<StateHasTeam>
            {
                protected IFsm<StateHasTeam> m_CachedFsm = null;
                protected IFsm<ActivityFoundryForm> m_OuterFsm = null;

                protected override void OnEnter(IFsm<StateHasTeam> fsm)
                {
                    //Log.Warning("StateHasTeam inner FSM OnEnter: {0}", GetType().Name);
                    base.OnEnter(fsm);
                    m_CachedFsm = fsm;
                    m_OuterFsm = m_CachedFsm.Owner.m_CachedFsm;
                }

                protected override void OnLeave(IFsm<StateHasTeam> fsm, bool isShutdown)
                {
                    m_CachedFsm = null;
                    m_OuterFsm = null;
                    base.OnLeave(fsm, isShutdown);
                    //Log.Warning("StateHasTeam inner FSM OnLeave: {0}", GetType().Name);
                }

                protected override void OnUpdate(IFsm<StateHasTeam> fsm, float elapseSeconds, float realElapseSeconds)
                {
                    base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                    m_OuterFsm.Owner.m_FoundryButton.isEnabled = (m_OuterFsm.Owner.m_CDMaskDoorStatus == ActivityFoundryForm.CDMaskDoorStatus.Open);
                }

                public virtual void OnPerformedFoundry(object sender, GameEventArgs e)
                {
                    m_OuterFsm.Owner.RefreshPlayers();
                    m_OuterFsm.Owner.RefreshProgress();
                }

                public virtual void OnRewardClaimed(object sender, GameEventArgs e)
                {

                }

                public virtual void OnClickCompletingAnimMask(IFsm<StateHasTeam> fsm)
                {

                }

                public void OnForceReset(IFsm<StateHasTeam> fsm)
                {
                    ChangeState<StateHasTeam.StateInit>(fsm);
                }
            }
        }
    }
}
