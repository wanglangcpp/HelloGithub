using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 所有商店中所有商品项数据。
    /// </summary>
    [Serializable]
    public class ShopData
    {
        [Serializable]
        public class RefreshShopInfo
        {
            [SerializeField]
            public TimeSpan UtcTime;

            [SerializeField]
            public int FreeTimes;

            [SerializeField]
            public CurrencyType CostCurrencyType;

            [SerializeField]
            public int CostCurrencyAmount;
        }

        [SerializeField]
        private Dictionary<ShopScenario, ShopItemsData> m_ShopsData = new Dictionary<ShopScenario, ShopItemsData>();

        [SerializeField]
        private Dictionary<ShopScenario, RefreshShopInfo> m_RefreshShopInfo = new Dictionary<ShopScenario, RefreshShopInfo>();

        public ShopItemsData GetShopData(ShopScenario key)
        {
            if (!m_ShopsData.ContainsKey(key))
            {
                return null;
            }
            return m_ShopsData[key];
        }

        public RefreshShopInfo GetShopRefreshData(ShopScenario key)
        {
            if (!m_RefreshShopInfo.ContainsKey(key))
            {
                return null;
            }
            return m_RefreshShopInfo[key];
        }

        private void InitRefreshShopInfo()
        {
            var allItem = GameEntry.DataTable.GetDataTable<DRShopRefresh>().GetAllDataRows();
            for (int i = 0; i < allItem.Length; i++)
            {
                RefreshShopInfo refreshShop = new RefreshShopInfo();
                refreshShop.UtcTime = TimeSpan.Parse(allItem[i].RefreshUtcTime);
                refreshShop.FreeTimes = allItem[i].RefreshFreeTimes;
                refreshShop.CostCurrencyType = (CurrencyType)allItem[i].RefreshCostCurrencyType;
                refreshShop.CostCurrencyAmount = allItem[i].RefreshCostCurrencyAmount;
                m_RefreshShopInfo.Add((ShopScenario)allItem[i].ShopType, refreshShop);
            }
        }

        public void UpdateData(PBShopInfo pb)
        {
            var shopItems = m_ShopsData[(ShopScenario)pb.ShopType];
            shopItems.ClearAndAddData(pb.ShopItems);
            shopItems.TodayRefreshCount = pb.TodayRefreshCount;
            if (m_RefreshShopInfo.Count == 0)
            {
                InitRefreshShopInfo();
            }
        }

        public void UpdateData(List<PBShopInfo> pb)
        {
            m_ShopsData.Clear();
            for (int i = 0; i < pb.Count; i++)
            {
                ShopItemsData shopData = new ShopItemsData();
                shopData.AddData(pb[i].ShopItems);
                shopData.TodayRefreshCount = pb[i].TodayRefreshCount;
                m_ShopsData.Add((ShopScenario)pb[i].ShopType, shopData);
            }

            if (m_RefreshShopInfo.Count == 0)
            {
                InitRefreshShopInfo();
            }
        }
    }
}
