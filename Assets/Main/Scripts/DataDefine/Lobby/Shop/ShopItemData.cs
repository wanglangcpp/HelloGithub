using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 商店中商品项数据。
    /// </summary>
    [Serializable]
    public class ShopItemData : IGenericData<ShopItemData, PBShopItem>
    {
        public int Key
        {
            get { return m_ItemInfo.Key; }
        }

        [SerializeField]
        private ItemData m_ItemInfo = new ItemData();

        public ItemData ItemInfo { get { return m_ItemInfo; } }

        [SerializeField]
        private CurrencyType m_CurrencyCategory;

        public CurrencyType CurrencyCategory { get { return m_CurrencyCategory; } }

        [SerializeField]
        private int m_CurrencyPrice;

        public int CurrencyPrice { get { return m_CurrencyPrice; } }

        [SerializeField]
        private bool m_CanBuy;

        public bool CanBuy { get { return m_CanBuy; } }

        public void UpdateData(PBShopItem pb)
        {
            m_ItemInfo.UpdateData(pb.ItemInfo);
            m_CanBuy = pb.CanBuy;
            m_CurrencyCategory = (CurrencyType)pb.CurrencyInfo.Type;
            m_CurrencyPrice = pb.CurrencyInfo.Price;
        }
    }

}
