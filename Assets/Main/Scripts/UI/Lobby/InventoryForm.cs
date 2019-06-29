using GameFramework;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InventoryForm : NGUIForm
    {
        public enum InventoryType
        {
            Normal,
            GearDress,
            GearChange,
            GearUpgradeSelect,
            DevelopmentStrengthenSelect,
            GearComposeSelect,
            GearComposeItemSelect,
            GearElevationSelect,
            SoulDress,
            EpigraphDress,
            EpigraphChange,
        }

        private enum TabType
        {
            Gear = 0,
            Soul = 1,
            Epigraph = 2,
            Material = 3,
            Item = 4,
            HeroStone = 5,
        }

        [SerializeField]
        private ScrollViewCache m_InventoryItems = null;

        [SerializeField]
        private GameObject m_DetailPanel = null;

        [SerializeField]
        private DatailPanel m_DetailPanelData = null;

        [SerializeField]
        private GameObject m_DetailAttributeItemTemplate = null;

        [SerializeField]
        private GameObject m_ChangeButton = null;

        [SerializeField]
        private GameObject m_DresseButton = null;

        [SerializeField]
        private UIButton m_ConfirmBtn = null;

        [SerializeField]
        private UIButton m_ClearBtn = null;

        [SerializeField]
        private UIButton m_FilterBtn = null;

        [SerializeField]
        private UILabel m_EmptyLabel = null;

        [SerializeField]
        private Collider[] m_TabColliderList = null;

        [SerializeField]
        private UIToggle[] m_TabToggleList = null;

        [SerializeField]
        private UILabel[] m_TabTextList = null;

        [SerializeField]
        private GameObject m_AllTitle = null;

        [SerializeField]
        private GameObject m_RecommendationTitle = null;

        [SerializeField]
        private Color m_ToggleDisabledColor = Color.white;

        [SerializeField]
        private Color m_ToggleNormalColor = Color.white;

        [SerializeField]
        private UILabel m_SystemUnopened = null;

        [SerializeField]
        private GameObject m_DecalLine = null;

        [SerializeField]
        private GameObject m_ButtonList = null;

        private object m_Data = null;
        private UIToggle m_SelectedItem = null;
        private InventoryType m_InventoryType = InventoryType.Normal;
        private int m_HeroType = 0;
        private bool m_CanSwitchTabs = true;
        private bool m_FirstTimeOnResume = true;
        private ScrollViewCache m_CurrentScrollViewCache = null;
        private TabType m_CurrentTab;
        private Dictionary<TabType, GameFrameworkFunc<int>> m_RefreshFuncs = new Dictionary<TabType, GameFrameworkFunc<int>>();

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            InitScenario(userData);
            StartCoroutine(InitInventoryDataCo());
            m_FirstTimeOnResume = true;
        }

        protected override void OnPause()
        {
            base.OnPause();
            m_CanSwitchTabs = true;
            if (GameEntry.IsAvailable)
            {              
                GameEntry.Event.Unsubscribe(EventId.ItemDataChanged, OnItemDataChanged);
                GameEntry.Event.Unsubscribe(EventId.SellGoods, SellGoodsSuccess);
                GameEntry.Event.Unsubscribe(EventId.ChangeGear, OperationFinishedSuccess);
                GameEntry.Event.Unsubscribe(EventId.DressSoulSuccess, OperationFinishedSuccess);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();           
            GameEntry.Event.Subscribe(EventId.ItemDataChanged, OnItemDataChanged);
            GameEntry.Event.Subscribe(EventId.SellGoods, SellGoodsSuccess);
            GameEntry.Event.Subscribe(EventId.ChangeGear, OperationFinishedSuccess);
            GameEntry.Event.Subscribe(EventId.DressSoulSuccess, OperationFinishedSuccess);

            if (m_FirstTimeOnResume)
            {
                m_FirstTimeOnResume = false;
            }
            else
            {
                ClearSelection();
                m_RefreshFuncs[m_CurrentTab]();

            }

            RefreshList(true);
        }

        protected override void OnClose(object userData)
        {
            m_CurrentScrollViewCache.RecycleAllItems();
            m_CurrentScrollViewCache = null;
            m_RefreshFuncs.Clear();
            base.OnClose(userData);
        }

        private IEnumerator InitInventoryDataCo()
        {
            TabType tabType = TabType.Gear;
            switch (m_InventoryType)
            {
                case InventoryType.GearDress:
                case InventoryType.GearChange:
                    InitTab(true, TabType.Gear);
                    m_RefreshFuncs.Add(TabType.Gear, RefreshGearOprationList);
                    tabType = TabType.Gear;
                    break;
                case InventoryType.GearUpgradeSelect:
                    InitTab(false, TabType.Gear);
                    m_RefreshFuncs.Add(TabType.Gear, RefreshGearList);
                    tabType = TabType.Gear;
                    break;
                case InventoryType.DevelopmentStrengthenSelect:
                    InitTab(false, TabType.Gear, TabType.Epigraph, TabType.Soul);
                    m_RefreshFuncs.Add(TabType.Gear, RefreshGearList);
                    m_RefreshFuncs.Add(TabType.Epigraph, RefreshEpigraphList);
                    m_RefreshFuncs.Add(TabType.Soul, RefreshSoulList);
                    tabType = TabType.Gear;
                    break;
                case InventoryType.GearComposeSelect:
                    InitTab(false, TabType.Gear);
                    m_RefreshFuncs.Add(TabType.Gear, RefreshComposeGearList);
                    tabType = TabType.Gear;
                    break;
                case InventoryType.GearComposeItemSelect:
                    InitTab(false, TabType.Item);
                    m_RefreshFuncs.Add(TabType.Item, RefreshComposeGearItemList);
                    tabType = TabType.Item;
                    break;
                case InventoryType.GearElevationSelect:
                    InitTab(false, TabType.Gear);
                    m_RefreshFuncs.Add(TabType.Gear, RefreshElevationGearItemList);
                    tabType = TabType.Gear;
                    break;
                case InventoryType.SoulDress:
                    InitTab(false, TabType.Soul);
                    m_RefreshFuncs.Add(TabType.Soul, RefreshDressSoulList);
                    tabType = TabType.Soul;
                    break;
                case InventoryType.EpigraphDress:
                case InventoryType.EpigraphChange:
                    InitTab(false, TabType.Epigraph);
                    m_RefreshFuncs.Add(TabType.Epigraph, RefreshEpigraphList);
                    tabType = TabType.Epigraph;
                    break;
                case InventoryType.Normal:
                default:
                    InitTab(false, TabType.Epigraph, TabType.Gear, TabType.HeroStone, TabType.Item, TabType.Material, TabType.Soul);
                    m_RefreshFuncs.Add(TabType.Gear, RefreshGearList);
                    m_RefreshFuncs.Add(TabType.Soul, RefreshSoulList);
                    m_RefreshFuncs.Add(TabType.Epigraph, RefreshEpigraphList);
                    m_RefreshFuncs.Add(TabType.Material, RefreshMaterialList);
                    m_RefreshFuncs.Add(TabType.Item, RefreshItemList);
                    m_RefreshFuncs.Add(TabType.HeroStone, RefreshHeroPieceList);
                    tabType = TabType.Gear;
                    break;
            }

            SelectTab(tabType, false);
            yield return null;
            RefreshList(true);
        }

        private void RefreshList(bool needResetPos = false)
        {
            ClearSelection();
            int dataCount = m_RefreshFuncs[m_CurrentTab]();
            m_CurrentScrollViewCache.RecycleItemsAtAndAfter(dataCount);

            if (dataCount < m_CurrentScrollViewCache.GetMaxRowCountInPanel() || needResetPos)
            {
                m_CurrentScrollViewCache.ResetPosition();
            }
            else
            {
                m_CurrentScrollViewCache.InvalidateBounds();
            }

            m_DetailPanel.SetActive(false);
            if (m_CurrentTab == TabType.Epigraph)
            {
                m_SystemUnopened.gameObject.SetActive(true);
                m_EmptyLabel.gameObject.SetActive(false);
            }
            else
            {
                m_SystemUnopened.gameObject.SetActive(false);
                m_EmptyLabel.gameObject.SetActive(dataCount == 0);
            }
            m_DecalLine.SetActive(dataCount > 0);
            m_ButtonList.SetActive(dataCount > 0);
            m_FilterBtn.gameObject.SetActive(m_CurrentTab == TabType.Gear);
            m_IsFilterSetAll = true;
            m_GearCountLabel.text = m_GearCountDisLabel.text = GameEntry.Localization.GetString("UI_TEXT_SLASH",
                GameEntry.Data.Gears.Data.Count, 200/* GameEntry.ServerConfig.GetInt(Constant.ServerConfig.InventorySlotsPerTab) */);
            if (dataCount > 0)
            {
                m_CurrentScrollViewCache.GetItem(0).GetComponent<UIToggle>().value = true;
                m_CurrentScrollViewCache.GetItem(0).OnInventoryItemChanged(true);
            }
        }

        private void InitTab(bool useRecommendItems, params TabType[] types)
        {
            for (int i = 0; i < m_TabToggleList.Length; i++)
            {
                if (!m_TabToggleList[i].gameObject.activeSelf)
                {
                    continue;
                }

                bool hasSameType = false;
                for (int j = 0; j < types.Length; j++)
                {
                    if (i == (int)types[j])
                    {
                        hasSameType = true;
                        m_TabToggleList[i].Set(false, false);
                        m_TabColliderList[i].enabled = true;
                        m_TabTextList[i].color = m_ToggleNormalColor;
                        break;
                    }
                }

                if (!hasSameType)
                {
                    m_TabToggleList[i].Set(false, false);
                    m_TabColliderList[i].enabled = false;
                    m_TabTextList[i].color = m_ToggleDisabledColor;
                }
            }

            if (useRecommendItems)
            {
                m_InventoryItems.SetActive(false);
                m_RecommendGears.SetActive(true);
                m_CurrentScrollViewCache = m_RecommendGears;
            }
            else
            {
                m_InventoryItems.SetActive(true);
                m_RecommendGears.SetActive(false);
                m_CurrentScrollViewCache = m_InventoryItems;
            }
        }

        private void SelectTab(TabType tabType, bool notify)
        {
            m_CurrentTab = tabType;
            m_TabToggleList[(int)tabType].Set(true, notify);
        }

        // Called via reflection by NGUI.
        public void OnTab(UIIntKey typeKey, bool selected)
        {
            if (selected)
            {
                m_CurrentTab = (TabType)typeKey.Key;
            }

            if (m_CanSwitchTabs && selected)
            {
                RefreshList(true);
            }
        }

        private void ClearSelection()
        {
            if (m_SelectedItem != null)
            {
                m_SelectedItem.value = false;
                m_SelectedItem = null;
            }
        }

        private void InitScenario(object userData)
        {
            var myDataDict = userData as InventoryDisplayData;

            m_InventoryType = InventoryType.Normal;
            m_HeroType = 0;
            //m_ChangeGearId = 0;
            m_ChangeGearPosition = 0;
            m_EpigraphIndex = -1;

            if (myDataDict == null)
            {
                m_ClearBtn.gameObject.SetActive(false);
                m_ConfirmBtn.gameObject.SetActive(false);
                return;
            }

            m_InventoryType = myDataDict.InventoryType;
            m_CanSwitchTabs = myDataDict.NeedToggle;
            m_ComposeQuality = myDataDict.ComposeQuality;
            m_GearType = myDataDict.GearType;
            m_MultiSelectedGears = myDataDict.MultiSelectedGears;
            m_HeroType = myDataDict.HeroType;

            if (m_InventoryType == InventoryType.GearElevationSelect || m_InventoryType == InventoryType.GearUpgradeSelect ||
                m_InventoryType == InventoryType.DevelopmentStrengthenSelect || m_InventoryType == InventoryType.GearComposeSelect ||
                m_InventoryType == InventoryType.GearComposeItemSelect)
            {
                m_ClearBtn.gameObject.SetActive(true);
                m_ConfirmBtn.gameObject.SetActive(true);
                m_ConfirmBtn.isEnabled = true;
            }
            else
            {
                m_ClearBtn.gameObject.SetActive(false);
                m_ConfirmBtn.gameObject.SetActive(false);
            }

            if (m_InventoryType == InventoryType.EpigraphChange || m_InventoryType == InventoryType.EpigraphDress)
            {
                m_EpigraphIndex = myDataDict.EpigraphIndex;
            }

            if (m_InventoryType != InventoryType.GearChange && m_InventoryType != InventoryType.GearDress)
            {
                return;
            }

            m_ChangeGearPosition = myDataDict.ChangeGearPosition;

            if (m_InventoryType == InventoryType.GearChange)
            {
                //m_ChangeGearId = myDataDict.ChangeGearId;
            }
        }

        public void OnClickUpgradeButton()
        {

        }

        public void OnClickStrengthenButton()
        {

        }

        public void OnClickSaleButton()
        {
            ItemData data = m_Data as ItemData;
            if (data == null)
            {
                return;
            }

            DRItem drItem = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(data.Type);
            if (drItem == null)
            {
                Log.Warning("Can not find item {0}.", data.Type.ToString());
                return;
            }
            string name = GameEntry.Localization.GetString(drItem.Name);
            string str = GameEntry.Localization.GetString("UI_TEXT_SOLD_TO_COINS", name, drItem.Price * data.Count);
            if (drItem.Quality >= (int)QualityType.Purple)
            {
                str += GameEntry.Localization.GetString("UI_TEXT_NOTICE_HIGH_QUALITY", name, GameEntry.Localization.GetString("UI_TEXT_QUALITY_" + ((QualityType)drItem.Quality).ToString().ToUpper()));
            }

            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Message = str,
                Mode = 2,
                OnClickConfirm = o => { OnClickSaleConfirm(); },
                ConfirmText = GameEntry.Localization.GetString("UI_BUTTON_CONFIRM"),
                CancelText = GameEntry.Localization.GetString("UI_BUTTON_CANCEL"),
            });
        }

        public void OnClickClearBtn()
        {
            if (m_InventoryType == InventoryType.GearComposeSelect)
            {
                m_MultiSelectedGears.Clear();
            }
            else if (m_InventoryType == InventoryType.GearElevationSelect)
            {
                m_MultiSelectedGears.Clear();
            }
            else
            {
                m_Data = null;
            }

            RefreshList();
        }

        private void SellGoodsSuccess(object sender, GameEventArgs e)
        {
            RefreshList();
        }

        private void OperationFinishedSuccess(object sender, GameEventArgs e)
        {
            if (isActiveAndEnabled)
            {
                CloseSelf();
            }
        }

        public void OnClickToHeroButton()
        {

        }

        public void SetSelectedItem(InventoryItem item)
        {
            m_SelectedItem = item != null ? item.GetComponent<UIToggle>() : null;
            if (m_SelectedItem != null)
            {
                m_DetailPanel.SetActive(true);
            }
        }

        public void OnClickConfirmBtn()
        {
            switch (m_InventoryType)
            {
                case InventoryType.GearUpgradeSelect:
                case InventoryType.DevelopmentStrengthenSelect:
                case InventoryType.GearComposeSelect:
                case InventoryType.GearComposeItemSelect:
                case InventoryType.GearElevationSelect:
                default:
                    break;
            }

            CloseSelf(true);
        }

        public void OnClickFilterButton()
        {
            switch (m_CurrentTab)
            {
                case TabType.Gear:
                    ShowFilterGearPanel();
                    break;
                default:
                    break;
            }
        }

        private GearInfoAttributeItem GetOrCreateAttributeItem(int index)
        {
            GearInfoAttributeItem script;
            GameObject go;

            if (index >= m_CachedAttributeItems.Count)
            {
                go = NGUITools.AddChild(m_DetailPanelData.m_DetailListView.gameObject, m_DetailAttributeItemTemplate);
                script = go.GetComponent<GearInfoAttributeItem>();
                m_CachedAttributeItems.Add(script);
            }
            else
            {
                script = m_CachedAttributeItems[index];
                go = script.gameObject;
                go.SetActive(true);
            }

            return script;
        }

        private void SetAttribute(GearInfoAttributeItem script, string name, string value)
        {
            script.Name = name;
            script.Value = value;
        }

        private void AddAttribute(string key, string value, int index)
        {
            GearInfoAttributeItem script = GetOrCreateAttributeItem(index);
            SetAttribute(script, GameEntry.Localization.GetString(key), value);
        }

        private void AddAttributePercentage(string key, float value, int index)
        {
            AddAttribute(key, GameEntry.Localization.GetString("UI_TEXT_PERCENTNUMBER", value), index);
        }

        private void HideAttributes()
        {
            for (int i = 0; i < m_CachedAttributeItems.Count; ++i)
            {
                m_CachedAttributeItems[i].gameObject.SetActive(false);
            }

            m_DetailPanelData.m_DetailListView.Reposition();
            m_DetailPanelData.m_DetailScrollView.ResetPosition();
        }

        private InventoryItem GetOrCreateInventoryItem(int index)
        {
            var ret = m_InventoryItems.GetOrCreateItem(index, OnCreateInventoryItem);
            ret.gameObject.name = string.Format("Inventory Item {0:D3}", index);
            return ret;
        }

        private void OnCreateInventoryItem(InventoryItem inventoryItem)
        {
            UIUtility.RefreshToggleGroup(inventoryItem.GetComponent<UIToggle>(), ToggleGroupBaseValue);
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<InventoryItem>
        {

        }
    }
}
