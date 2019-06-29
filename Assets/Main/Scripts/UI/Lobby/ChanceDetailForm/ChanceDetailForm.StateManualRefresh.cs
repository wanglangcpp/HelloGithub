using GameFramework.Event;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 手动刷新状态。
        /// </summary>
        private class StateManualRefresh : StateBase
        {
            private bool m_DataObtained = false;

            public override void OnChanceDataChanged(ChanceDataChangedEventArgs e)
            {
                m_DataObtained = true;
            }

            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                GameEntry.Event.Subscribe(EventId.NetworkCustomError, OnNetworkCustomError);
                m_DataObtained = false;
                if (!CheckCurrency())
                {
                    ChangeState<StateNormal>(fsm);
                }
                else
                {
                    GameEntry.LobbyLogic.RefreshChanceInfo(m_Form.m_CurrentChanceType);
                }
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                m_DataObtained = false;
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.NetworkCustomError, OnNetworkCustomError);
                }
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ChanceDetailForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (m_DataObtained)
                {
                    ChangeState<StateOldContentsFadingOut>(fsm);
                }
            }

            private bool CheckCurrency()
            {
                return UIUtility.CheckCurrency(m_Form.CurrencyType, m_Form.CostForManualRefresh);
            }

            private void OnNetworkCustomError(object sender, GameEventArgs e)
            {
                ChangeState<StateNormal>(m_Form.m_Fsm);
            }
        }
    }
}
