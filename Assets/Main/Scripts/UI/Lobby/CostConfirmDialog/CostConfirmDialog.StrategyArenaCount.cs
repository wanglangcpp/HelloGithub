using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CostConfirmDialog
    {
        private class StrategyArenaCount : StrategyBase
        {
            private int m_CostCoinCount = int.MaxValue;
            private bool m_ReachMaxCount = false;
            public override void RefreshData()
            {
                m_Form.m_ItemIcon.gameObject.SetActive(false);
                m_Form.m_PreMessage.gameObject.SetActive(true);
                var vipConfig = GameEntry.DataTable.GetDataTable<DRVip>();
                var drVip = vipConfig.GetDataRow(GameEntry.Data.Player.VipLevel);
                m_Form.m_PreMessage.text = GameEntry.Localization.GetString("UI_TEXT_OFFLINE_PVP_PURCHASES");
                m_Form.m_ButtonCurrencyIcon.spriteName = CoinIconName;
                var costConfig = GameEntry.DataTable.GetDataTable<DRArenaCost>();
                var drCost = costConfig.GetDataRow(GameEntry.Data.OfflineArena.TodayBoughtCount);
                m_CostCoinCount = drCost.PlayCostCoin;
                m_Form.m_ButtonCurrencyCount.text = drCost.PlayCostCoin.ToString("N0");
                if (drVip.BuyArenaCount > GameEntry.Data.OfflineArena.TodayBoughtCount)
                {
                    m_ReachMaxCount = false;
                    m_Form.m_PostMessage.text = GameEntry.Localization.GetString("UI_TEXT_ARENA_VIP_INFO", GameEntry.Data.Player.VipLevel, GameEntry.Data.OfflineArena.TodayBoughtCount, drVip.BuyArenaCount);
                }
                else
                {
                    m_ReachMaxCount = true;
                    m_Form.m_PostMessage.text = GameEntry.Localization.GetString("UI_TEXT_ARENA_VIP_MAX_BUY_COUNT");
                }
            }

            public override void OnClickBuyButton()
            {
                base.OnClickBuyButton();
                if (m_ReachMaxCount)
                {
                    UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_PROMOTION_VIP_BUY_MORE"));
                    m_Form.CloseSelf();
                    return;
                }
                if (UIUtility.CheckCurrency(CurrencyType.Coin, m_CostCoinCount))
                {
                    GameEntry.LobbyLogic.BuyAdditionalArenaCount();
                }
            }
        }
    }
}
