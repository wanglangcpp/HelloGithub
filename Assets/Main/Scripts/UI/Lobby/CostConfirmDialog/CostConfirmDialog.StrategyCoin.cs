

namespace Genesis.GameClient
{
    public partial class CostConfirmDialog
    {
        private class StrategyCoin : StrategyBase
        {          

            public override void RefreshData()
            {
                m_Form.m_ItemIcon.gameObject.SetActive(true);
                m_Form.m_PreMessage.gameObject.SetActive(false);
                m_Form.m_ItemIcon.spriteName = CoinIconName;
                m_Form.m_ButtonCurrencyIcon.spriteName = MoneyIconName;
                int maxCoinCount = GameEntry.DataTable.GetDataTable<DRVip>().GetDataRow(GameEntry.Data.Player.VipLevel).ExchangeCoinCount;
                int coinCount = GameEntry.Data.VipsData.GetData((int)VipPrivilegeType.ExchangeCoin).UsedVipPrivilegeCount;
                var drExchange = GameEntry.DataTable.GetDataTable<DRExchange>().GetDataRow(coinCount + 1);
                if (drExchange == null)
                {
                    return;
                }
                m_Form.m_ItemCount.text = drExchange.Coin.ToString();
                m_Form.m_ButtonCurrencyCount.text = drExchange.CoinCostMoney.ToString();
                int remainCount = maxCoinCount - coinCount;
                if (remainCount > 0)
                {
                    m_Form.m_PostMessage.text = GameEntry.Localization.GetString("UI_TEXT_BUY_TODAY", remainCount, maxCoinCount);
                }
                else
                {
                    m_Form.m_PostMessage.text = GameEntry.Localization.GetString("UI_TEXT_BUY_TODAY_COLOUR", remainCount, maxCoinCount);
                }
            }

            public override void OnClickBuyButton()
            {
                base.OnClickBuyButton();
                int coinCount = GameEntry.Data.VipsData.GetData((int)VipPrivilegeType.ExchangeCoin).UsedVipPrivilegeCount;
                var drExchange = GameEntry.DataTable.GetDataTable<DRExchange>().GetDataRow(coinCount + 1);
                if (drExchange == null)
                {
                    return;
                }
                if (!UIUtility.CheckCurrency(CurrencyType.Money, drExchange.CoinCostMoney))
                {
                    m_Form.CloseSelf();
                    return;
                }               
                GameEntry.LobbyLogic.ExchangeCoin();
            }
        }
    }
}
