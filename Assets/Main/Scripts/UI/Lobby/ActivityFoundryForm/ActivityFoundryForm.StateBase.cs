using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        /// <summary>
        /// 状态基类
        /// </summary>
        private abstract class StateBase : FsmState<ActivityFoundryForm>
        {
            protected IFsm<ActivityFoundryForm> m_CachedFsm = null;

            protected override void OnEnter(IFsm<ActivityFoundryForm> fsm)
            {
                //Log.Warning("[ActivityFoundryForm] OnEnter: " + GetType().Name);
                base.OnEnter(fsm);
                m_CachedFsm = fsm;
            }

            protected override void OnLeave(IFsm<ActivityFoundryForm> fsm, bool isShutdown)
            {
                m_CachedFsm = null;
                base.OnLeave(fsm, isShutdown);
                //Log.Warning("[ActivityFoundryForm] OnLeave: " + GetType().Name);
            }

            public virtual void OnClickCreateTeam(IFsm<ActivityFoundryForm> fsm)
            {

            }

            public virtual void OnClickInvite(IFsm<ActivityFoundryForm> fsm)
            {

            }

            public virtual void OnClickMatch(IFsm<ActivityFoundryForm> fsm, int level)
            {

            }

            public virtual void OnClickRequests(IFsm<ActivityFoundryForm> fsm)
            {

            }

            public virtual void OnClickFoundry(IFsm<ActivityFoundryForm> fsm)
            {

            }

            public virtual void OnClickSendLink(IFsm<ActivityFoundryForm> fsm)
            {

            }

            public virtual void OnClickClaimReward(IFsm<ActivityFoundryForm> fsm)
            {

            }

            public virtual void OnClickLeave(IFsm<ActivityFoundryForm> fsm)
            {

            }

            public virtual void OnKickButton(IFsm<ActivityFoundryForm> fsm, int playerIndex)
            {

            }

            public virtual void OnClickCompletingAnimMask(IFsm<ActivityFoundryForm> fsm)
            {

            }
        }
    }
}
