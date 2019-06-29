using GameFramework.Event;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        /// <summary>
        /// 单次抽奖状态。
        /// </summary>
        private class StateBuyAll : StateBase
        {
            protected override void OnEnter(IFsm<ChanceDetailForm> fsm)
            {
                base.OnEnter(fsm);
                GameEntry.Event.Subscribe(EventId.NetworkCustomError, OnNetworkCustomError);

                if (CheckCurrency())
                {
                    GameEntry.UI.OpenUIForm(UIFormId.CostConfirmDialog, new CostConfirmDialogDisplayData
                    {
                        Mode = CostConfirmDialogType.Other,
                        PreMessage = GameEntry.Localization.GetString(m_Form.SpendCurrencyNoticeKey, m_Form.BuyAllCost.ToString()),
                        UseCurrencyType = m_Form.m_CurrentChanceType == ChanceType.Coin ? CurrencyType.Coin : CurrencyType.Money,
                        CurrencyCount = m_Form.BuyAllCost,
                        OnClickConfirm = delegate (object o) { GameEntry.LobbyLogic.OpenAllChances(m_Form.m_CurrentChanceType); },
                        OnClickCancel = delegate (object o) { ChangeState<StateNormal>(fsm); },
                    });
                }
                else
                {
                    ChangeState<StateNormal>(fsm);
                }
            }

            protected override void OnLeave(IFsm<ChanceDetailForm> fsm, bool isShutdown)
            {
                GameEntry.Event.Unsubscribe(EventId.NetworkCustomError, OnNetworkCustomError);
                base.OnLeave(fsm, isShutdown);
            }

            public override void OnChanceDataChanged(ChanceDataChangedEventArgs ne)
            {
                GameEntry.UI.OpenUIForm(UIFormId.ChanceReceiveForm, new ChanceReceiveDisplayData { ReceiveGoodsData = ne.ReceiveGoodsData, OnComplete = AfterReceiveItems });
            }

            private void OnNetworkCustomError(object sender, GameEventArgs e)
            {
                var ne = e as NetworkCustomErrorEventArgs;

                if (!s_BuyFailureErrorCodes.Contains(ne.ServerErrorCode))
                {
                    return;
                }

                ChangeState<StateNormal>(m_Form.m_Fsm);
            }

            private void AfterReceiveItems(object userData = null)
            {
                ChangeState<StateRequestData>(m_Form.m_Fsm);
            }

            private bool CheckCurrency()
            {
                return UIUtility.CheckCurrency(m_Form.CurrencyType, m_Form.BuyAllCost);
            }
        }
    }
}
