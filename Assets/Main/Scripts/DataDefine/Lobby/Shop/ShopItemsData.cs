using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 商店中所有商品数据。
    /// </summary>
    [Serializable]
    public class ShopItemsData : GenericData<ShopItemData, PBShopItem>
    {
        public int TodayRefreshCount { get; set; }
    }
}
