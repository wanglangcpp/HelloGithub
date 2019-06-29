using GameFramework.Event;

namespace Genesis.GameClient
{
    public partial class InventoryForm
    {
        private int m_EpigraphIndex = 0;

        private int RefreshEpigraphList()
        {
            return 0;

            //             List<EpigraphData> data = new List<EpigraphData>(GameEntry.Data.Epigraphs.Data);
            //             data.Sort(Comparer.EpigraphDataComparer);
            //             m_EmptyLabel.gameObject.SetActive(data.Count == 0);
            //
            //             for (int i = 0; i < data.Count; i++)
            //             {
            //                 InventoryItem inventoryItem = GetOrCreateInventoryItem(i);
            //                 inventoryItem.HeroType = 0;
            //                 inventoryItem.EpigraphData = data[i];
            //                 inventoryItem.InventoryForm = this;
            //                 inventoryItem.EnableMultiSelectIcon = false;
            //             }
            //
            //             return data.Count;
        }

        public void OnClickDressEpigraph()
        {
            if (m_InventoryType != InventoryType.EpigraphDress)
            {
                return;
            }

            EpigraphData epigraphData = m_Data as EpigraphData;
            if (epigraphData == null)
            {
                return;
            }

//             CLChangeEpigraph msg = new CLChangeEpigraph();
//             msg.Index = m_EpigraphIndex;
//             msg.DressedEpigraph = new PBEpigraphInfo();
//             msg.DressedEpigraph.Level = epigraphData.Level;
//             msg.DressedEpigraph.Type = epigraphData.Id;
//             GameEntry.Network.Send(msg);
        }

        public void OnClickChangeEpigraph()
        {
            if (m_InventoryType != InventoryType.EpigraphChange)
            {
                return;
            }

            if (GameEntry.Data.EpigraphSlots.Data.Count <= m_EpigraphIndex)
            {
                return;
            }

            EpigraphData epigraphData = m_Data as EpigraphData;
            if (epigraphData == null)
            {
                return;
            }

//             CLChangeEpigraph msg = new CLChangeEpigraph();
//             msg.Index = m_EpigraphIndex;
//             msg.DressedEpigraph = new PBEpigraphInfo();
//             msg.DressedEpigraph.Level = epigraphData.Level;
//             msg.DressedEpigraph.Type = epigraphData.Id;
// 
//             msg.UndressedEpigraph = new PBEpigraphInfo();
//             msg.UndressedEpigraph.Level = GameEntry.Data.EpigraphSlots.Data[m_EpigraphIndex].Level;
//             msg.UndressedEpigraph.Type = GameEntry.Data.EpigraphSlots.Data[m_EpigraphIndex].Id;
// 
//             GameEntry.Network.Send(msg);
        }

        public void SelectEpigraph(EpigraphData data)
        {
            if (data == null)
            {
                m_Data = null;
                m_DetailPanel.SetActive(false);
                return;
            }

            m_Data = data;
            EpigraphData epigraph = data as EpigraphData;
            m_DetailPanel.SetActive(true);
            int iconId = GeneralItemUtility.GetGeneralItemIconId(data.Id);
            m_DetailPanelData.m_DetailIcon.LoadAsync(iconId);
            m_DetailPanelData.m_DetailName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(data.Id));
            m_DetailPanelData.m_DetailName.color = ColorUtility.GetColorForQuality(1);
            m_DetailPanelData.m_DetailLevel.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailNumber.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailType.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailStarPanel.gameObject.SetActive(false);
            m_DetailPanelData.m_SoulEpigraphStarPanel.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailDescription.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailScrollView.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailUpgradeButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailStrengthenButton.gameObject.SetActive(m_InventoryType != InventoryType.DevelopmentStrengthenSelect && m_InventoryType != InventoryType.EpigraphChange && m_InventoryType != InventoryType.EpigraphDress);
            m_DetailPanelData.m_DetailUseButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailSaleButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailToHeroButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailExchangeButton.gameObject.SetActive(false);
            m_ChangeButton.SetActive(m_InventoryType == InventoryType.EpigraphChange);
            m_DresseButton.SetActive(m_InventoryType == InventoryType.EpigraphDress);
            UIUtility.SetStarLevel(m_DetailPanelData.m_SoulEpigraphStars, epigraph.Level);
            ShowEpigraphAttributes();
        }

        private void OnEpigraphDataChanged(object sender, GameEventArgs e)
        {
            RefreshList();

            if (isActiveAndEnabled && (m_InventoryType == InventoryType.EpigraphChange || m_InventoryType == InventoryType.EpigraphDress))
            {
                CloseSelf();
            }
        }

        private void ShowEpigraphAttributes()
        {
            var epigraph = m_Data as EpigraphData;
            int index = 0;
            string value = string.Empty;
            switch ((AttributeType)epigraph.DTAttributeType)
            {
                case AttributeType.MaxHP:
                case AttributeType.PhysicalAttack:
                case AttributeType.MagicAttack:
                case AttributeType.PhysicalDefense:
                case AttributeType.MagicDefense:
                case AttributeType.AdditionalDamage:
                    value = epigraph.DTAttributeValue.ToString();
                    break;
                default:
                    value = GameEntry.Localization.GetString("UI_TEXT_PER_NUMBER", epigraph.DTAttributeValue * 100.0f);
                    break;
            }
            AddAttribute(Constant.AttributeName.AttributeNameDics[epigraph.DTAttributeType], value, index++);

            for (int i = index; i < m_CachedAttributeItems.Count; ++i)
            {
                m_CachedAttributeItems[i].gameObject.SetActive(false);
            }

            m_DetailPanelData.m_DetailListView.Reposition();
            m_DetailPanelData.m_DetailScrollView.ResetPosition();
        }
    }
}
