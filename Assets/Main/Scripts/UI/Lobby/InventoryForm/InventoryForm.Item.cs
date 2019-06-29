using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class InventoryForm
    {
        public void OnClickUseButton()
        {
            ItemData data = m_Data as ItemData;
            if (data == null)
            {
                Log.Warning("Data is invalid when use.");
                return;
            }
        }

        public void OnClickSaleConfirm()
        {

        }

        public void OnClickExchangeButton()
        {
            ItemData data = m_Data as ItemData;
            if (data == null)
            {
                Log.Warning("Data is invalid when exchange.");
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.ExchangeBatchForm, new ExchangeBatchDisplayData { HeroPieceTypeId = data.Type, OwnedCount = data.Count });
        }

        public void SelectItem(ItemData data)
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
            DRItem drItem = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(data.Type);
            if (drItem == null)
            {
                Log.Warning("Can not find item {0}.", data.Type.ToString());
                return;
            }
            m_DetailPanelData.m_DetailUseButton.gameObject.SetActive(drItem.Type == (int)ItemType.HeroExpItem);
            if (m_InventoryType == InventoryType.GearComposeItemSelect || GeneralItemUtility.GetItemType(data.Type) == ItemType.HeroPieceItem)
            {
                m_DetailPanelData.m_DetailSaleButton.gameObject.SetActive(false);
            }
            else
            {
                m_DetailPanelData.m_DetailSaleButton.gameObject.SetActive(drItem.Price > 0);
            }
            m_DetailPanelData.m_DetailToHeroButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailExchangeButton.gameObject.SetActive(GeneralItemUtility.GetItemType(data.Type) == ItemType.HeroPieceItem);

            m_ChangeButton.SetActive(false);
            m_DresseButton.SetActive(false);
            if (m_ConfirmBtn.gameObject.activeSelf)
            {
                m_ConfirmBtn.isEnabled = true;
            }
            HideAttributes();
        }

        public void SelectHeroPiece(ItemData data)
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
            m_DetailPanelData.m_DetailSaleButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailToHeroButton.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailExchangeButton.gameObject.SetActive(true);
            m_ChangeButton.SetActive(false);
            m_DresseButton.SetActive(false);
            HideAttributes();
        }

        private int RefreshItemList()
        {
            int index = 0;
            IDataTable<DRItem> dt = GameEntry.DataTable.GetDataTable<DRItem>();
            List<ItemData> data = new List<ItemData>(GameEntry.Data.Items.Data);
            data.Sort(Comparer.ItemDataComparer);
            bool hasItems = false;
            for (int i = 0; i < data.Count; i++)
            {
                int itemType = dt[data[i].Type].Type;
                if (itemType == (int)ItemType.DummyItem
                    //|| itemType == (int)ItemType.StrengthenItem
                    //|| itemType == (int)ItemType.SoulPieceItem
                    //|| itemType == (int)ItemType.EpigraphPieceItem
                    //|| itemType == (int)ItemType.HeroPieceItem
                    )
                {
                    continue;
                }

                hasItems = true;
                InventoryItem inventoryItem = GetOrCreateInventoryItem(index++);
                inventoryItem.HeroType = 0;
                inventoryItem.ItemData = data[i];
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = false;
            }

            m_EmptyLabel.gameObject.SetActive(!hasItems);
            return index;
        }

        private int RefreshHeroPieceList()
        {
            int index = 0;
            IDataTable<DRItem> dt = GameEntry.DataTable.GetDataTable<DRItem>();
            List<ItemData> data = new List<ItemData>(GameEntry.Data.Items.Data);
            data.Sort(Comparer.ItemDataComparer);

            bool hasItems = false;
            for (int i = 0; i < data.Count; i++)
            {
                if (dt[data[i].Type].Type != (int)ItemType.HeroPieceItem)
                {
                    continue;
                }
                hasItems = true;
                InventoryItem inventoryItem = GetOrCreateInventoryItem(index++);
                inventoryItem.HeroType = 0;
                inventoryItem.HeroPieceData = data[i];
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = false;
            }

            m_EmptyLabel.gameObject.SetActive(!hasItems);

            return index;
        }

        private void OnItemDataChanged(object sender, GameEventArgs e)
        {
            if (m_CurrentTab == TabType.Material || m_CurrentTab == TabType.Item || m_CurrentTab == TabType.HeroStone)
            {
                RefreshList();
            }
        }
    }
}
