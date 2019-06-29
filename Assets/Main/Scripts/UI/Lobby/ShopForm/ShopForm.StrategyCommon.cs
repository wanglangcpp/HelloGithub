using System;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ShopForm
    {
        private class StrategyCommon : StrategyBase
        {
            public override GameObject ShopItemTemplate
            {
                get
                {
                    return m_Form.m_CommonShopItemTemplate;
                }
            }

            protected override ShopScenario ShopType
            {
                get
                {
                    return ShopScenario.Common;
                }
            }

            public override void OnShopDataChanged(object sender, GameEventArgs e)
            {
                m_Form.RefreshShopItems(ShopType);
            }

            public override void OnPurchaseComplete(object sender, GameEventArgs e)
            {
                m_Form.RefreshShopItems(ShopType);
            }

            public override void PerformManualRefresh()
            {
                GameEntry.LobbyLogic.ShopManuallyRefresh((int)ShopType);
            }

            public override void Buy(int index)
            {
                GameEntry.LobbyLogic.CommonShopBuy((int)ShopType, index);
            }
        }
    }
}
