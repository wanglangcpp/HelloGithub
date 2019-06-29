using GameFramework.DataTable;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class InventoryForm
    {
        [SerializeField]
        private ScrollViewCache m_RecommendGears = null;

        [SerializeField]
        private FilterGearSubForm m_FilterGearTemplate = null;

        [SerializeField]
        private UILabel m_GearCountLabel = null;

        [SerializeField]
        private UILabel m_GearCountDisLabel = null;

        private FilterGearSubForm m_UsingFilterGear = null;

        private readonly List<GearInfoAttributeItem> m_CachedAttributeItems = new List<GearInfoAttributeItem>();

        private int m_ChangeGearPosition = 0;

        private List<GearData> m_MultiSelectedGears = new List<GearData>();

        private int m_GearType = 0;

        private int m_ComposeQuality = 0;

        private bool m_IsFilterSetAll = true;

        private void ShowFilterGearPanel()
        {
            if (m_UsingFilterGear == null)
            {
                m_UsingFilterGear = CreateSubForm<FilterGearSubForm>("FilterGearSubForm", gameObject, m_FilterGearTemplate.gameObject, true);
            }
            else
            {
                OpenSubForm(m_UsingFilterGear);
            }

            switch (m_InventoryType)
            {
                case InventoryType.GearComposeSelect:
                    m_UsingFilterGear.RefreshFilter(GetAllGearsData(), RefreshComposeGearList, m_IsFilterSetAll);
                    break;
                case InventoryType.GearElevationSelect:
                    m_UsingFilterGear.RefreshFilter(GetAllGearsData(), RefreshElevationGearItemList, m_IsFilterSetAll);
                    break;
                case InventoryType.GearDress:
                case InventoryType.GearChange:
                    m_UsingFilterGear.RefreshFilter(GetAllGearsData(), RefreshGearOprationList, m_IsFilterSetAll, true);
                    break;
                case InventoryType.GearUpgradeSelect:
                case InventoryType.DevelopmentStrengthenSelect:
                case InventoryType.Normal:
                    m_UsingFilterGear.RefreshFilter(GetAllGearsData(), RefreshGearList, m_IsFilterSetAll);
                    break;
                default:
                    break;
            }

            m_IsFilterSetAll = false;
            m_CurrentScrollViewCache.ResetPosition();
        }

        private void CloseSubFilterGear()
        {
            if (m_UsingFilterGear != null && m_UsingFilterGear.isActiveAndEnabled)
            {
                CloseSubForm(m_UsingFilterGear);
            }
        }

        private int RefreshComposeGearList()
        {
            return RefreshComposeGearList(null);
        }

        private int RefreshComposeGearList(List<GearDataWithHero> filterGearsData)
        {
            CloseSubFilterGear();
            if (filterGearsData == null)
            {
                filterGearsData = GetAllGearsData();
            }
            else
            {
                m_CurrentScrollViewCache.RecycleItemsAtAndAfter(filterGearsData.Count);
            }
            filterGearsData.Sort(Comparer.GearDataWithHeroComparer);

            int index = 0;
            for (int i = 0; i < filterGearsData.Count; i++)
            {
                if (filterGearsData[i].GearData.Quality == Constant.GearQualityCount)
                {
                    continue;
                }

                InventoryItem inventoryItem = GetOrCreateInventoryItem(index++);
                inventoryItem.HeroType = 0;
                inventoryItem.GearData = filterGearsData[i].GearData;
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = true;
                inventoryItem.MultiSelectIndex = m_MultiSelectedGears == null ? 0 : m_MultiSelectedGears.FindIndex(g => g.Id == filterGearsData[i].GearData.Id);
            }

            m_EmptyLabel.gameObject.SetActive(index == 0);
            return index;
        }

        private int RefreshElevationGearItemList()
        {
            return RefreshElevationGearItemList(null);
        }

        private int RefreshElevationGearItemList(List<GearDataWithHero> filterGearsData)
        {
            CloseSubFilterGear();
            if (filterGearsData == null)
            {
                filterGearsData = GetAllGearsData();
            }
            else
            {
                m_CurrentScrollViewCache.RecycleItemsAtAndAfter(filterGearsData.Count);
            }
            filterGearsData.Sort(Comparer.GearDataWithHeroComparer);
            int index = 0;
            bool isHasItems = false;
            for (int i = 0; i < filterGearsData.Count; i++)
            {
                int gearType = filterGearsData[i].GearData.Position;
                int gearQuality = filterGearsData[i].GearData.Quality;
                if (gearQuality != m_ComposeQuality || m_GearType != gearType)
                {
                    continue;
                }

                isHasItems = true;
                InventoryItem inventoryItem = GetOrCreateInventoryItem(index++);
                inventoryItem.HeroType = 0;
                inventoryItem.GearData = filterGearsData[i].GearData;
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = true;
                inventoryItem.MultiSelectIndex = m_MultiSelectedGears.FindIndex(g => g.Id == filterGearsData[i].GearData.Id);
            }
            m_EmptyLabel.gameObject.SetActive(!isHasItems);

            return index;
        }

        private int RefreshComposeGearItemList()
        {
            int index = 0;
            IDataTable<DRItem> dt = GameEntry.DataTable.GetDataTable<DRItem>();
            List<ItemData> data = new List<ItemData>(GameEntry.Data.Items.Data);
            data.Sort(Comparer.ItemDataComparer);
            bool isHasItems = false;
            for (int i = 0; i < data.Count; i++)
            {
                int itemType = dt[data[i].Type].Type;
                if (itemType == (int)ItemType.DummyItem
                    || itemType == (int)ItemType.StrengthenItem
                    || itemType == (int)ItemType.HeroPieceItem)
                {
                    continue;
                }

                isHasItems = true;
                InventoryItem inventoryItem = GetOrCreateInventoryItem(index++);
                inventoryItem.HeroType = 0;
                inventoryItem.ItemData = data[i];
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = false;
            }
            m_EmptyLabel.gameObject.SetActive(!isHasItems);

            return index;
        }

        public void MultiSelectedGears(InventoryItem gearItem)
        {
            if (gearItem.MultiSelectIndex < 0)
            {
                int needCount = 0;
                if (InventoryType.GearElevationSelect == m_InventoryType)
                {
                    needCount = Constant.GearElevationMaterialCount;
                }
                else if (InventoryType.GearComposeSelect == m_InventoryType)
                {
                    needCount = Constant.GearComposeMaterialCount;
                }
                if (m_MultiSelectedGears.Count >= needCount)
                {
                    return;
                }
                if (m_MultiSelectedGears.Count > 0 && gearItem.GearData.Quality != m_MultiSelectedGears[0].Quality)
                {
                    if (InventoryType.GearComposeSelect == m_InventoryType)
                    {
                        UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_GEARFORMPIECES_NOT_SAME_QUALITY_NOTICE"));
                    }
                    return;
                }
                m_MultiSelectedGears.Add(gearItem.GearData);
                gearItem.MultiSelectIndex = m_MultiSelectedGears.Count - 1;
            }
            else
            {
                m_MultiSelectedGears.RemoveAt(gearItem.MultiSelectIndex);
                RefreshMultiSelectGears();
            }
        }

        public void RefreshMultiSelectGears()
        {
            var itemList = m_InventoryItems.GetChildList();
            for (int i = 0; i < itemList.Count; i++)
            {
                var gearItem = itemList[i].gameObject.GetComponent<InventoryItem>();
                gearItem.MultiSelectIndex = m_MultiSelectedGears.FindIndex(g => g.Id == gearItem.GearData.Id);
            }
        }

        public void SelectGear(GearData data)
        {
            if (data == null)
            {
                m_Data = null;
                m_DetailPanel.SetActive(false);
                return;
            }
            m_Data = data;
            m_DetailPanel.SetActive(true);
            int iconId = GeneralItemUtility.GetGeneralItemIconId(data.Type);
            m_DetailPanelData.m_DetailIcon.LoadAsync(iconId);
            m_DetailPanelData.m_DetailName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(data.Type));
            m_DetailPanelData.m_DetailName.color = ColorUtility.GetColorForQuality(data.Quality);
            m_DetailPanelData.m_DetailLevel.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailLevel.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", data.Level.ToString());
            m_DetailPanelData.m_DetailNumber.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailType.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailType.text = GameEntry.Localization.GetString(Constant.Gear.GearTypeNameDics[data.Position]);
            m_DetailPanelData.m_DetailStarPanel.gameObject.SetActive(true);
            m_DetailPanelData.m_SoulEpigraphStarPanel.gameObject.SetActive(false);
            for (int i = 0; i < m_DetailPanelData.m_DetailStars.Length; i++)
            {
                m_DetailPanelData.m_DetailStars[i].gameObject.SetActive(i < data.StrengthenLevel);
            }
            m_DetailPanelData.m_DetailDescription.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailScrollView.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailUseButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailSaleButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailToHeroButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailExchangeButton.gameObject.SetActive(false);

            m_DetailPanelData.m_DetailUpgradeButton.gameObject.SetActive(m_InventoryType == InventoryType.Normal);
            m_DetailPanelData.m_DetailStrengthenButton.gameObject.SetActive(m_InventoryType == InventoryType.Normal);

            m_ChangeButton.SetActive(m_InventoryType == InventoryType.GearChange);
            m_DresseButton.SetActive(m_InventoryType == InventoryType.GearDress);
            if (m_ConfirmBtn.gameObject.activeSelf && (m_InventoryType == InventoryType.GearUpgradeSelect || m_InventoryType == InventoryType.DevelopmentStrengthenSelect))
            {
                m_ConfirmBtn.isEnabled = true;
            }
            ShowGearAttributes();
        }

        public void OnClickDressGear()
        {

        }

        public void OnClickChangeGear()
        {

        }

        private int RefreshGearOprationList()
        {
            return RefreshGearOprationList(null);
        }

        private int RefreshGearOprationList(List<GearDataWithHero> filterGearsData)
        {
            CloseSubFilterGear();
            if (filterGearsData == null)
            {
                filterGearsData = GetAllGearsData();
            }
            else
            {
                m_CurrentScrollViewCache.RecycleItemsAtAndAfter(filterGearsData.Count);
            }
            filterGearsData.Sort(Comparer.GearDataWithHeroComparer);
            List<GearDataWithHero> recommendedData = new List<GearDataWithHero>();
            List<GearData> unrecommendGears = GameEntry.LobbyLogic.GetOneTypeOfGear(m_ChangeGearPosition);
            List<GearData> recommendedGear = GameEntry.LobbyLogic.GetRecommendedGear(unrecommendGears, m_HeroType);
            int i = 0;
            for (; i < recommendedGear.Count; i++)
            {
                recommendedData.Add(new GearDataWithHero(recommendedGear[i]));

                InventoryItem inventoryItem;
                inventoryItem = GetGearInventoryItem(i, true, false);
                inventoryItem.HeroType = recommendedData[i].HeroType;
                inventoryItem.GearData = recommendedData[i].GearData;
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = false;
            }

            int recommendCount = recommendedData.Count;
            m_RecommendationTitle.SetActive(recommendCount > 0);
            int totalCount = recommendCount + filterGearsData.Count;
            for (; i < totalCount; i++)
            {
                InventoryItem inventoryItem;
                inventoryItem = GetGearInventoryItem(i, false, i == recommendCount);
                inventoryItem.HeroType = filterGearsData[i - recommendCount].HeroType;
                inventoryItem.GearData = filterGearsData[i - recommendCount].GearData;
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = false;
            }
            if (m_SelectedItem != null)
            {
                m_SelectedItem.value = false;
                m_SelectedItem = null;
            }
            m_EmptyLabel.gameObject.SetActive((recommendCount + filterGearsData.Count) == 0);

            m_AllTitle.SetActive(totalCount > 0);
            return totalCount;
        }

        private int RefreshGearList()
        {
            return RefreshGearList(null);
        }

        private int RefreshGearList(List<GearDataWithHero> filterGearsData)
        {
            CloseSubFilterGear();
            if (filterGearsData == null)
            {
                filterGearsData = GetAllGearsData();
            }
            else
            {
                m_CurrentScrollViewCache.RecycleItemsAtAndAfter(filterGearsData.Count);
            }
            filterGearsData.Sort(Comparer.GearDataWithHeroComparer);
            m_EmptyLabel.gameObject.SetActive(filterGearsData.Count == 0);

            for (int i = 0; i < filterGearsData.Count; i++)
            {
                InventoryItem inventoryItem = GetOrCreateInventoryItem(i);
                inventoryItem.HeroType = filterGearsData[i].HeroType;
                inventoryItem.GearData = filterGearsData[i].GearData;
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = false;
            }

            return filterGearsData.Count;
        }

        private InventoryItem GetGearInventoryItem(int index, bool isRecommend, bool isInitAllTitle)
        {
            InventoryItem inventoryItem = GetOrCreateGearItem(index);

            if (isInitAllTitle)
            {
                m_AllTitle.name = (index + 1).ToString("D3") + "AllTitle";
            }

            inventoryItem.gameObject.SetActive(true);

            if (isRecommend)
            {
                inventoryItem.name = (index + 1).ToString("D3") + "inventoryItem";
            }
            else
            {
                inventoryItem.name = (index + 2).ToString("D3") + "inventoryItem";
            }
            return inventoryItem;
        }

        private void OnGearDataChanged(object sender, GameEventArgs e)
        {
            if (m_CurrentTab == TabType.Gear)
            {
                RefreshList();
            }
        }

        private void ShowGearAttributes()
        {
            var displayer = new GearAttributeDisplayer<GearInfoAttributeItem>(m_Data as GearData, GearAttributeNewValueMask.Default);
            displayer.GetItemDelegate += GetOrCreateAttributeItem;
            displayer.SetNameAndCurrentValueDelegate += SetAttribute;
            int attrCount = displayer.Run();

            for (int i = attrCount; i < m_CachedAttributeItems.Count; ++i)
            {
                m_CachedAttributeItems[i].gameObject.SetActive(false);
            }

            m_DetailPanelData.m_DetailListView.Reposition();
            m_DetailPanelData.m_DetailScrollView.ResetPosition();
        }

        private InventoryItem GetOrCreateGearItem(int index)
        {
            return m_RecommendGears.GetOrCreateItem(index, OnCreateInventoryItem);
        }

        private List<GearDataWithHero> GetAllGearsData()
        {
            List<GearDataWithHero> gearsData = new List<GearDataWithHero>();
            return gearsData;
        }
    }
}
