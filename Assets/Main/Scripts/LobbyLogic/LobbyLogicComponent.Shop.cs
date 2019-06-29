using System;
using Random = UnityEngine.Random;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        /// <summary>
        /// 通用商店 -- 购买商品。
        /// </summary>
        /// <param name="data">商品项数据。</param>
        public void CommonShopBuy(int shopType, int index)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLPurchaseInShop msg = new CLPurchaseInShop();
                msg.ShopType = shopType;
                msg.ShopItemIndex = index;
                GameEntry.Network.Send(msg);
                return;
            }
        }

        /// <summary>
        /// 通用商店 -- 手动刷新。
        /// </summary>
        public void ShopManuallyRefresh(int shopType)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                var response = new CLRefreshShop();
                response.ShopType = shopType;
                GameEntry.Network.Send(response);
                return;
            }
        }
    }
}
