using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class InventoryForm
    {
        private int RefreshSoulList()
        {
            List<SoulDataWithHero> data = GeneralItemUtility.GetAllSoulData();
            if (m_InventoryType == InventoryType.SoulDress)
            {
                RemoveSelfSoul(data);
            }
            data.Sort(Comparer.SoulDataWithHeroComparer);
            m_EmptyLabel.gameObject.SetActive(data.Count == 0);

            for (int i = 0; i < data.Count; i++)
            {
                InventoryItem inventoryItem = GetOrCreateInventoryItem(i);
                inventoryItem.HeroType = data[i].HeroType;
                inventoryItem.SoulData = data[i].SoulData;
                inventoryItem.InventoryForm = this;
                inventoryItem.EnableMultiSelectIcon = false;
            }

            return data.Count;
        }

        private int RefreshDressSoulList()
        {
            return 0;
        }

        private void RemoveSelfSoul(List<SoulDataWithHero> data)
        {
            for (int i = 0; i < data.Count;)
            {
                if (data[i].HeroType != 0)
                {
                    data.Remove(data[i]);
                    continue;
                }
                i++;
            }
        }

        public void OnClickDressSoul()
        {
            if (m_InventoryType != InventoryType.SoulDress)
            {
                return;
            }
// 
//             SoulData data = m_Data as SoulData;
//             CLChangeSoul request = new CLChangeSoul() { HeroId = m_HeroType, PutOnSoulId = data.Key };
//             GameEntry.Network.Send(request);
        }

        public void SelectSoul(SoulData data)
        {
            if (data == null)
            {
                m_Data = null;
                m_DetailPanel.SetActive(false);
                return;
            }

            m_Data = data;
            SoulData soul = data as SoulData;
            m_DetailPanel.SetActive(true);
            int iconId = GeneralItemUtility.GetGeneralItemIconId(data.Type);
            m_DetailPanelData.m_DetailIcon.LoadAsync(iconId);
            m_DetailPanelData.m_DetailName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(data.Type));
            m_DetailPanelData.m_DetailName.color = ColorUtility.GetColorForQuality(data.Quality);
            m_DetailPanelData.m_DetailLevel.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailNumber.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailType.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailStarPanel.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailDescription.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailScrollView.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailUpgradeButton.gameObject.SetActive(false);
            m_DetailPanelData.m_SoulEpigraphStarPanel.gameObject.SetActive(true);
            m_DetailPanelData.m_DetailUseButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailSaleButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailToHeroButton.gameObject.SetActive(false);
            m_DetailPanelData.m_DetailExchangeButton.gameObject.SetActive(false);
            m_ChangeButton.SetActive(false);
            UIUtility.SetStarLevel(m_DetailPanelData.m_SoulEpigraphStars, soul.Quality);
            m_DresseButton.SetActive(m_InventoryType != InventoryType.DevelopmentStrengthenSelect && m_InventoryType == InventoryType.SoulDress);
            m_DetailPanelData.m_DetailStrengthenButton.gameObject.SetActive(m_InventoryType != InventoryType.DevelopmentStrengthenSelect && m_InventoryType != InventoryType.SoulDress);

            ShowSoulAttributes();
        }

        private void OnSoulDataChanged(object sender, GameEventArgs e)
        {
            if (m_CurrentTab == TabType.Soul)
            {
                RefreshList();
            }
        }

        private void ShowSoulAttributes()
        {
            var soul = m_Data as SoulData;
            int index = 0;

            AddAttribute(UIUtility.GetSoulAttributeNameKey(soul.EffectId), UIUtility.GetSoulEffectValueText(soul), index++);

            for (int i = index; i < m_CachedAttributeItems.Count; ++i)
            {
                m_CachedAttributeItems[i].gameObject.SetActive(false);
            }

            m_DetailPanelData.m_DetailListView.Reposition();
            m_DetailPanelData.m_DetailScrollView.ResetPosition();
        }

    }
}
