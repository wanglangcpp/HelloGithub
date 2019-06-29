using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class InventoryForm
    {
        private int RefreshMaterialList()
        {
            int index = 0;
            IDataTable<DRItem> dt = GameEntry.DataTable.GetDataTable<DRItem>();
            List<ItemData> data = new List<ItemData>(GameEntry.Data.Items.Data);
            data.Sort(Comparer.ItemDataComparer);
            bool hasItems = false;
            for (int i = 0; i < data.Count; i++)
            {
                int itemType = dt[data[i].Type].Type;
                if (itemType != (int)ItemType.StrengthenItem)
                {
                    continue;
                }

                hasItems = true;
                InventoryItem inventoryItem = GetOrCreateInventoryItem(index++);
                inventoryItem.HeroType = 0;
                inventoryItem.MaterialData = data[i];
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = false;
            }
            m_EmptyLabel.gameObject.SetActive(!hasItems);
            return index;
        }

        public void SelectMaterial(ItemData data)
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
            m_DetailPanelData.m_DetailName.color = GeneralItemUtility.GetGeneralItemQualityColor(data.Type);
            m_DetailPanelData.m_DetailLevel.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailNumber.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailNumber.text = GameEntry.Localization.GetString("UI_TEXT_ITEM_NUMBER_HAD", data.Count);
            m_DetailPanelData.m_DetailType.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailStarPanel.gameObject.SetActive(false);
            m_DetailPanelData.m_SoulEpigraphStarPanel.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailDescription.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailDescription.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemDescription(data.Type));
            m_DetailPanelData.m_DetailScrollView.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailUpgradeButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailStrengthenButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailUseButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailSaleButton.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailToHeroButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailExchangeButton.gameObject.SetActive(false);
            m_ChangeButton.SetActive(false);
            m_DresseButton.SetActive(false);
            HideAttributes();
        }
    }
}
