using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedurePreDemoInstance : ProcedureMain
    {
        private bool m_HasFiredWillChangeSceneEvent = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_HasFiredWillChangeSceneEvent = false;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            m_HasFiredWillChangeSceneEvent = false;
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_HasFiredWillChangeSceneEvent)
            {
                m_HasFiredWillChangeSceneEvent = true;
                GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(
                    InstanceLogicType.Demo,
                    GameEntry.Tutorial.Config.DemoInstanceId,
                    true));
            }
        }
    }
}
