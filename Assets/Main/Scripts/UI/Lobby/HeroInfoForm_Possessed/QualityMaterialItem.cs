using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class QualityMaterialItem : MonoBehaviour
    {
        [SerializeField]
        private GeneralItemView m_ItemView = null;

        [SerializeField]
        private GameObject m_PlusIcon = null;

        [SerializeField]
        private GameObject m_LockedIcon = null;

        [SerializeField]
        private UIEffectsController m_EquipEffect = null;

        private int m_HeroType = 0;

        private int m_QualityItemSlotIndex = 0;

        private DRHeroQualityItem m_Item = null;

        private bool m_IsLoadItem = false;

        public bool IsOpenAndNotEquiped { get { return m_LockedIcon.activeSelf == false && m_PlusIcon.activeSelf; } }

        public void RefreshMeterialData(DRHeroQualityItem item, bool isLoadItem, bool isLocked, int heroType, int qualityItemSlotIndex)
        {
            m_IsLoadItem = isLoadItem;
            m_Item = item;
            m_HeroType = heroType;
            m_QualityItemSlotIndex = qualityItemSlotIndex;
            if (item == null)
            {
                m_ItemView.ItemIcon.LoadAsync(Constant.EmptyItemIconId);
                m_ItemView.Quality = QualityType.White;
            }
            else
            {
                m_ItemView.InitItem(item.Id, (QualityType)item.Quality);
            }
            m_ItemView.SetOnClickDelegate(OnClickItemButton);
            m_PlusIcon.SetActive(!isLoadItem && !isLocked);
            m_LockedIcon.SetActive(isLocked);
            gameObject.GetComponent<BoxCollider>().enabled = !isLocked;
            if (m_PlusIcon.activeSelf)
            {
                m_ItemView.ItemIcon.color = Color.grey;
            }
            else
            {
                m_ItemView.ItemIcon.color = Color.white;
            }
        }

        public bool ItemCanClick
        {
            set
            {
                if (value)
                {
                    m_ItemView.SetOnClickDelegate(OnClickItemButton);
                }
                else
                {
                    m_ItemView.SetOnClickDelegate(null);
                }
            }
        }

        public void ShowEquipEffect()
        {
            m_EquipEffect.Resume();
            m_EquipEffect.ShowEffect("EquipEffect");
        }

        private void OnClickItemButton()
        {
            GameEntry.UI.OpenItemInfoForm(new GeneralItemInfoDisplayData
            {
                TypeId = m_Item.Id,
                CanInlay = !m_IsLoadItem,
                OnInlay = AddQualityItemReturn,
            });
        }

        private void AddQualityItemReturn()
        {
            GameEntry.LobbyLogic.RequestUseHeroQualityItem(m_HeroType, m_QualityItemSlotIndex);
        }
    }
}
