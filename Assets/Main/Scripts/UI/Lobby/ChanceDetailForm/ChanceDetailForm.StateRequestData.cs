using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 请求数据状态。用于自动刷新或手动抽完全部奖品。
        /// </summary>
        private class StateRequestData : StateBase
        {
            private bool m_DataObtained = false;

            public override void OnChanceDataChanged(ChanceDataChangedEventArgs e)
            {
                m_DataObtained = true;
            }

            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                m_DataObtained = false;
                GameEntry.LobbyLogic.RequestChanceInfo(m_Form.m_CurrentChanceType);
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                m_DataObtained = false;
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (m_DataObtained)
                {
                    ChangeState<StateOldContentsFadingOut>(fsm);
                }
            }
        }
    }
}
