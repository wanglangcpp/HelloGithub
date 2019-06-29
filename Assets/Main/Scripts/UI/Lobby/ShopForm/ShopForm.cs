using GameFramework;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 商店界面。
    /// </summary>
    public partial class ShopForm : NGUIForm
    {
        [SerializeField]
        private GameObject m_CommonShopItemTemplate = null;

        [SerializeField]
        private GameObject m_VipShopItemTemplate = null;

        [SerializeField]
        private UIButton m_RefreshButton = null;

        [SerializeField]
        private UILabel m_ManualRefreshCostNumberLabel = null;

        [SerializeField]
        private UILabel m_FreeRefreshCostNumberLabel = null;

        [SerializeField]
        private UISprite[] m_RefreshCurrencyIcon = null;

        [SerializeField]
        private UILabel m_AutoRefreshCountDown = null;

        [SerializeField]
        private UIGrid m_ShopItemGridView = null;

        [SerializeField]
        private UISprite m_ShopItemGridBg = null;

        [SerializeField]
        private UIToggle[] m_TabToggles = null;

        [SerializeField]
        private UISprite m_ShopBg = null;

        [SerializeField]
        private BorderShop[] m_BorderShops = null;

        private ShopScenario m_Scenario;
        private StrategyBase m_Strategy;
        private DateTime m_NextAutoRefreshTime;
        private List<ShopItem> m_CachedShopItems = new List<ShopItem>();
        private float m_CurUpdateTime = 0;
        private const float UpdateTime = 1;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ResetDisplay();
            ParseUserData(userData);
            InitTabs();
            InitStrategy();
            StartCoroutine(InitAnchorCo());
        }
        protected override void OnResume()
        {
            base.OnResume();
            RefreshShopItems(m_Scenario);
        }

        private IEnumerator InitAnchorCo()
        {
            m_ShopBg.gameObject.SetActive(false);
            yield return null;
            m_ShopBg.gameObject.SetActive(true);
            yield return null;
            m_ShopBg.ResetAndUpdateAnchors();
        }

        protected override void OnClose(object userData)
        {
            ResetDisplay();
            DeinitStrategy();
            base.OnClose(userData);
        }

        // Called by NGUI via reflection.
        public void OnClickRefreshButton()
        {
            var shopData = GameEntry.Data.ShopsData.GetShopData(m_Scenario);
            var refreshData = GameEntry.Data.ShopsData.GetShopRefreshData(m_Scenario);
            int moneyCost = (refreshData.FreeTimes - shopData.TodayRefreshCount) > 0 ? 0 : refreshData.CostCurrencyAmount;
            CheckMoneyAndPerformManualRefresh(moneyCost);
        }

        public void OnTab(int index, bool isSelected)
        {
            if (!isSelected)
            {
                return;
            }

            if (m_Scenario != (ShopScenario)index)
            {
                SwitchScenario((ShopScenario)index);
                RefreshBordeOnTab((ShopScenario)index);
            }
        }

        private void CheckMoneyAndPerformManualRefresh(int moneyCost)
        {
            var refreshData = GameEntry.Data.ShopsData.GetShopRefreshData(m_Scenario);
            if (!UIUtility.CheckCurrency(refreshData.CostCurrencyType, moneyCost))
            {
                return;
            }

            m_Strategy.PerformManualRefresh();
        }

        private void SubscribeEvents()
        {
            GameEntry.Event.Subscribe(EventId.ShopDataChanged, OnShopDataChanged);
            GameEntry.Event.Subscribe(EventId.PurchaseInShop, OnPurchaseComplete);
        }

        internal void Buy(int index)
        {
            m_Strategy.Buy(index);
        }

        private void UnsubscribeEvents()
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Event.Unsubscribe(EventId.ShopDataChanged, OnShopDataChanged);
            GameEntry.Event.Unsubscribe(EventId.PurchaseInShop, OnPurchaseComplete);
        }

        private void ResetDisplay()
        {
            ResetShopItems();
            m_AutoRefreshCountDown.text = string.Empty;
            m_RefreshButton.isEnabled = true;
            m_ManualRefreshCostNumberLabel.text = string.Empty;
        }

        private void ResetShopItems()
        {
            var shopItems = m_ShopItemGridView.GetChildList();
            for (int i = 0; i < shopItems.Count; ++i)
            {
                Destroy(shopItems[i].gameObject);
            }

            m_CachedShopItems.Clear();
        }

        private void ParseUserData(object userData)
        {
            var myUserData = userData as ShopDisplayData;

            if (myUserData == null)
            {
                m_Scenario = ShopScenario.Common;
                return;
            }

            m_Scenario = myUserData.Scenario;
        }

        private void InitTabs()
        {
            for (int i = 0; i < m_TabToggles.Length; ++i)
            {
                m_TabToggles[i].Set(((int)m_Scenario) == (i + 1), false);
            }
        }

        private void InitStrategy()
        {
            switch (m_Scenario)
            {
                case ShopScenario.Common:
                    m_Strategy = new StrategyCommon();
                    break;
                case ShopScenario.Vip:
                    m_Strategy = new StrategyVip();
                    break;
                case ShopScenario.OfflineArena:
                    m_Strategy = new OfflineArena();
                    break;
                default:
                    Log.Error("Scenario '{0}' is unsupported.", m_Scenario);
                    return;
            }

            m_Strategy.Init(this);
            SubscribeEvents();
        }

        private void DeinitStrategy()
        {
            if (m_Strategy == null)
            {
                return;
            }

            UnsubscribeEvents();
            m_Strategy.ShutDown();
            m_Strategy = null;
        }

        private void SwitchScenario(ShopScenario newScenario)
        {
            DeinitStrategy();
            ResetDisplay();
            m_Scenario = newScenario;
            InitStrategy();
        }

        private void OnShopDataChanged(object sender, GameEventArgs e)
        {
            RefreshLabel();
            m_Strategy.OnShopDataChanged(sender, e);
            RefreshNextAutoRefreshTime();
        }

        private void RefreshLabel()
        {
            var shopData = GameEntry.Data.ShopsData.GetShopData(m_Scenario);
            var refreshData = GameEntry.Data.ShopsData.GetShopRefreshData(m_Scenario);
            bool showIcon = true;

            if (shopData == null || refreshData == null)
            {
                return;
            }

            if ((refreshData.FreeTimes - shopData.TodayRefreshCount) > 0)
            {
                m_FreeRefreshCostNumberLabel.text = GameEntry.Localization.GetString("UI_TEXT_FREE_REFRESH", refreshData.FreeTimes - shopData.TodayRefreshCount, refreshData.FreeTimes);
                m_ManualRefreshCostNumberLabel.text = string.Empty;
                showIcon = false;
            }
            else
            {
                m_ManualRefreshCostNumberLabel.text = GameEntry.Localization.GetString("UI_TEXT_INTEGER", refreshData.CostCurrencyAmount);
                m_FreeRefreshCostNumberLabel.text = GameEntry.Localization.GetString("UI_BUTTON_REFRESH");
            }

            for (int i = 0; i < m_RefreshCurrencyIcon.Length; i++)
            {
                if (m_RefreshCurrencyIcon[i] != null)
                {
                    m_RefreshCurrencyIcon[i].gameObject.SetActive(showIcon && i == (int)refreshData.CostCurrencyType);
                }
            }
        }

        private void OnPurchaseComplete(object sender, GameEventArgs e)
        {
            PurchaseInShopEventArgs msg = e as PurchaseInShopEventArgs;
            m_Strategy.OnPurchaseComplete(sender, e);
            RefreshNextAutoRefreshTime();
            ShowReceivedGoods(msg.ItemViewData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_CurUpdateTime += elapseSeconds;
            if (m_CurUpdateTime >= UpdateTime)
            {
                m_CurUpdateTime = 0;
                RefreshNextAutoRefreshTime();
            }
        }

        private void RefreshNextAutoRefreshTime()
        {
            var refreshData = GameEntry.Data.ShopsData.GetShopRefreshData(m_Scenario);
            if (refreshData == null)
            {
                return;
            }
            m_NextAutoRefreshTime = TimeUtility.GetNearestUtcTime(refreshData.UtcTime);
            var timeSpan = m_NextAutoRefreshTime - GameEntry.Time.LobbyServerUtcTime;
            var remainingTimeText = GameEntry.Localization.GetString("UI_TEXT_FEFRESH_TIME", GameEntry.Localization.GetString("UI_TEXT_HOUR_MINUTE_SECOND", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
            m_AutoRefreshCountDown.text = GameEntry.Localization.GetString("UI_TEXT_NEXT_FREEGIVE", remainingTimeText);
        }

        private void RefreshShopItems(ShopScenario type)
        {
            StartCoroutine(RefreshShopItemsCo(type));
        }

        private IEnumerator RefreshShopItemsCo(ShopScenario type)
        {
            yield return null;

            if (GameEntry.Data.ShopsData.GetShopData(type) == null)
            {
                yield break;
            }
            var shopItemDatas = GameEntry.Data.ShopsData.GetShopData(type).Data;

            for (int i = 0; i < shopItemDatas.Count; ++i)
            {
                ShopItem script;
                if (i >= m_CachedShopItems.Count)
                {
                    var go = NGUITools.AddChild(m_ShopItemGridView.gameObject, m_Strategy.ShopItemTemplate);
                    go.name = string.Format("Shop Item {0}", i.ToString());
                    script = go.GetComponent<ShopItem>();
                    m_CachedShopItems.Add(script);
                }
                else
                {
                    script = m_CachedShopItems[i];
                    script.gameObject.SetActive(true);
                }

                script.RefreshData(this, shopItemDatas[i], i);
                script.SetBuyBtnAnchor(m_BorderShops[i].ShopBorde.gameObject);
            }

            for (int i = shopItemDatas.Count; i < m_CachedShopItems.Count; ++i)
            {
                m_CachedShopItems[i].gameObject.SetActive(false);
            }

            UIUtility.GridAutoAdaptScreen(m_ShopItemGridBg.width, m_ShopItemGridView, false);
            m_ShopItemGridView.Reposition();
        }

        private void ShowReceivedGoods(ReceivedGeneralItemsViewData receiveGoodsData)
        {
            GameEntry.RewardViewer.RequestShowRewards(receiveGoodsData, false);
        }

        private void RefreshBordeOnTab(ShopScenario type)
        {
            for (int i = 0; i < m_BorderShops.Length; i++)
            {
                m_BorderShops[i].Refresh(type);
            }
        }

        [Serializable]
        private class BorderShop
        {
            public UISprite ShopBorde = null;
            public GameObject BoothBorder = null;
            public GameObject BoothVipBorder = null;

            public void Refresh(ShopScenario type)
            {
                if (type == ShopScenario.Vip)
                {
                    ShopBorde.spriteName = "border_shop_vip";
                    BoothBorder.SetActive(false);
                    BoothVipBorder.SetActive(true);
                }
                else
                {
                    ShopBorde.spriteName = "border_shop";
                    BoothBorder.SetActive(true);
                    BoothVipBorder.SetActive(false);
                }
            }
        }
    }
}
