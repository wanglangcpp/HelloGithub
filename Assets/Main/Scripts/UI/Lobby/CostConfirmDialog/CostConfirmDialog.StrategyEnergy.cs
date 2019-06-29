

namespace Genesis.GameClient
{
    public partial class CostConfirmDialog
    {
        private class StrategyEnergy : StrategyBase
        {
            public override void RefreshData()
            {               
                m_Form.m_ItemIcon.gameObject.SetActive(true);
                m_Form.m_PreMessage.gameObject.SetActive(false);
                m_Form.m_ItemIcon.spriteName = EnergyName;
                m_Form.m_ButtonCurrencyIcon.spriteName = MoneyIconName;
                int maxEnergyCount = GameEntry.DataTable.GetDataTable<DRVip>().GetDataRow(GameEntry.Data.Player.VipLevel).ExchangeEnergyCount;
                int energyCount = GameEntry.Data.VipsData.GetData((int)VipPrivilegeType.ExchangeEnergy).UsedVipPrivilegeCount;
                var drExchange = GameEntry.DataTable.GetDataTable<DRExchange>().GetDataRow(energyCount + 1);
                if (drExchange == null)
                {
                    return;
                }
                m_Form.m_ItemCount.text = drExchange.Energy.ToString();
                m_Form.m_ButtonCurrencyCount.text = drExchange.EnergyCostMoney.ToString();
                int remainCount = maxEnergyCount - energyCount;
                if (remainCount > 0)
                {
                    m_Form.m_PostMessage.text = GameEntry.Localization.GetString("UI_TEXT_BUY_TODAY", remainCount, maxEnergyCount);
                }
                else
                {
                    m_Form.m_PostMessage.text = GameEntry.Localization.GetString("UI_TEXT_BUY_TODAY_COLOUR", remainCount, maxEnergyCount);
                }
            }

            public override void OnClickBuyButton()
            {
                base.OnClickBuyButton();
                int energyCount = GameEntry.Data.VipsData.GetData((int)VipPrivilegeType.ExchangeEnergy).UsedVipPrivilegeCount;
                var drExchange = GameEntry.DataTable.GetDataTable<DRExchange>().GetDataRow(energyCount + 1);
                if (drExchange == null)
                {
                    return;
                }
                if (!UIUtility.CheckCurrency(CurrencyType.Money, drExchange.EnergyCostMoney))
                {
                    m_Form.CloseSelf();
                    return;
                }
                GameEntry.LobbyLogic.ExchangeEnergy();
            }
        }
    }
}
