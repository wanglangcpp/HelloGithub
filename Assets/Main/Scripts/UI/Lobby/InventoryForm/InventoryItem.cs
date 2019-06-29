using UnityEngine;

namespace Genesis.GameClient
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_ItemIcon = null;

        [SerializeField]
        private UILabel m_ItemName = null;

        [SerializeField]
        private UILabel m_ItemNumber = null;

        [SerializeField]
        private UILabel m_ItemDescription = null;

        [SerializeField]
        private GameObject m_ItemLevelPanel = null;

        [SerializeField]
        private UILabel m_ItemLevel = null;

        [SerializeField]
        private GameObject m_ItemStarPanel = null;

        [SerializeField]
        private UISprite[] m_ItemStars = null;

        [SerializeField]
        private GameObject m_HeroPanel = null;

        [SerializeField]
        private GameObject m_MutiSelectIcon = null;

        [SerializeField]
        private UILabel m_MutiSelectIndex = null;

        [SerializeField]
        private UISprite m_HeroIcon = null;

        [SerializeField]
        private UISprite[] m_GearStars = null;

        [SerializeField]
        private GameObject m_GearStarPanel = null;

        private InventoryType m_InventoryType = InventoryType.Unknown;
        private int m_HeroType = 0;
        private object m_Data = null;
        private int m_Index = -1;

        public int HeroType
        {
            get
            {
                return m_HeroType;
            }
            set
            {
                m_HeroType = value;
                if (m_HeroType > 0)
                {
                    m_HeroPanel.SetActive(true);
                    m_HeroIcon.LoadAsync(GameEntry.DataTable.GetDataTable<DRHero>()[m_HeroType].IconId);
                }
                else
                {
                    m_HeroPanel.SetActive(false);
                }
            }
        }

        public GearData GearData
        {
            get
            {
                return m_Data as GearData;
            }
            set
            {
                m_InventoryType = InventoryType.Gear;
                m_Data = value;
                int iconId = GeneralItemUtility.GetGeneralItemIconId(value.Type);
                m_ItemIcon.LoadAsync(iconId);
                m_ItemName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(value.Type));
                m_ItemName.color = ColorUtility.GetColorForQuality(value.Quality);
                m_ItemNumber.gameObject.SetActive(false);
                m_ItemDescription.gameObject.SetActive(false);
                m_ItemLevelPanel.gameObject.SetActive(true);
                m_ItemLevel.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", value.Level.ToString());
                m_GearStarPanel.SetActive(true);
                m_ItemStarPanel.SetActive(false);
                for (int i = 0; i < m_ItemStars.Length; i++)
                {
                    m_GearStars[i].gameObject.SetActive(i < value.StrengthenLevel);
                }
            }
        }

        public SoulData SoulData
        {
            get
            {
                return m_Data as SoulData;
            }
            set
            {
                m_InventoryType = InventoryType.Soul;
                m_Data = value;
                int iconId = GeneralItemUtility.GetGeneralItemIconId(value.Type);
                m_ItemIcon.LoadAsync(iconId);
                m_ItemName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(value.Type));
                m_ItemName.color = ColorUtility.GetColorForQuality(value.Quality);
                m_ItemNumber.gameObject.SetActive(false);
                m_ItemDescription.gameObject.SetActive(false);
                m_ItemLevelPanel.gameObject.SetActive(false);
                m_ItemStarPanel.SetActive(true);
                m_GearStarPanel.SetActive(false);
                for (int i = 0; i < m_ItemStars.Length; i++)
                {
                    m_ItemStars[i].gameObject.SetActive(i < value.Quality);
                }
            }
        }

        public EpigraphData EpigraphData
        {
            get
            {
                return m_Data as EpigraphData;
            }
            set
            {
                m_InventoryType = InventoryType.Epigraph;
                m_Data = value;
                int iconId = GeneralItemUtility.GetGeneralItemIconId(value.Id);
                m_ItemIcon.LoadAsync(iconId);
                m_ItemName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(value.Id));
                m_ItemName.color = ColorUtility.GetColorForQuality(value.DTQuality);
                m_ItemNumber.gameObject.SetActive(false);
                m_ItemDescription.gameObject.SetActive(false);
                m_ItemLevelPanel.gameObject.SetActive(false);
                m_ItemStarPanel.SetActive(true);
                m_GearStarPanel.SetActive(false);
                for (int i = 0; i < m_ItemStars.Length; i++)
                {
                    m_ItemStars[i].gameObject.SetActive(i < value.Level);
                }
            }
        }

        public ItemData MaterialData
        {
            get
            {
                return m_Data as ItemData;
            }
            set
            {
                m_InventoryType = InventoryType.Material;
                m_Data = value;
                int iconId = GeneralItemUtility.GetGeneralItemIconId(value.Type);
                m_ItemIcon.LoadAsync(iconId);
                m_ItemName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(value.Type));
                m_ItemName.color = GeneralItemUtility.GetGeneralItemQualityColor(value.Type);
                m_ItemNumber.gameObject.SetActive(true);
                m_ItemNumber.text = GameEntry.Localization.GetString("UI_TEXT_ITEMNUMBER_WITHOUTPLUS", value.Count.ToString());
                m_ItemDescription.gameObject.SetActive(false);
                m_ItemLevelPanel.gameObject.SetActive(false);
                m_ItemStarPanel.SetActive(false);
                m_GearStarPanel.SetActive(false);
            }
        }

        public ItemData ItemData
        {
            get
            {
                return m_Data as ItemData;
            }
            set
            {
                m_InventoryType = InventoryType.Item;
                m_Data = value;
                int iconId = GeneralItemUtility.GetGeneralItemIconId(value.Type);
                m_ItemIcon.LoadAsync(iconId);
                m_ItemName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(value.Type));
                m_ItemName.color = GeneralItemUtility.GetGeneralItemQualityColor(value.Type);
                m_ItemNumber.gameObject.SetActive(true);
                m_ItemNumber.text = GameEntry.Localization.GetString("UI_TEXT_ITEMNUMBER_WITHOUTPLUS", value.Count.ToString()); ;
                m_ItemDescription.gameObject.SetActive(false);
                m_ItemLevelPanel.gameObject.SetActive(false);
                m_ItemStarPanel.SetActive(false);
                m_GearStarPanel.SetActive(false);
            }
        }

        public ItemData HeroPieceData
        {
            get
            {
                return m_Data as ItemData;
            }
            set
            {
                m_InventoryType = InventoryType.HeroPiece;
                m_Data = value;
                int iconId = GeneralItemUtility.GetGeneralItemIconId(value.Type);
                m_ItemIcon.LoadAsync(iconId);
                m_ItemName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(value.Type));
                m_ItemName.color = GeneralItemUtility.GetGeneralItemQualityColor(value.Type);
                m_ItemNumber.gameObject.SetActive(true);
                m_ItemNumber.text = GameEntry.Localization.GetString("UI_TEXT_ITEMNUMBER_WITHOUTPLUS", value.Count.ToString()); ;
                m_ItemDescription.gameObject.SetActive(false);
                m_ItemLevelPanel.gameObject.SetActive(false);
                m_ItemStarPanel.SetActive(false);
                m_GearStarPanel.SetActive(false);
            }
        }

        public bool EnableMultiSelectIcon
        {
            get
            {
                return m_MutiSelectIcon.activeSelf;
            }
            set
            {
                m_MutiSelectIcon.SetActive(value);
            }
        }

        public int MultiSelectIndex
        {
            get
            {
                return m_Index;
            }
            set
            {
                m_Index = value;
                if (value >= 0)
                {
                    m_MutiSelectIndex.text = (value + 1).ToString();
                }
                else
                {
                    m_MutiSelectIndex.text = "";
                }
            }
        }

        public InventoryForm InventoryForm
        {
            get;
            set;
        }

        public void OnInventoryItemChanged(bool selected)
        {
            InventoryForm.SetSelectedItem(selected ? this : null);
            switch (m_InventoryType)
            {
                case InventoryType.Gear:
                    InventoryForm.SelectGear(selected ? GearData : null);
                    break;
                case InventoryType.Soul:
                    InventoryForm.SelectSoul(selected ? SoulData : null);
                    break;
                case InventoryType.Epigraph:
                    InventoryForm.SelectEpigraph(selected ? EpigraphData : null);
                    break;
                case InventoryType.Material:
                    InventoryForm.SelectMaterial(selected ? MaterialData : null);
                    break;
                case InventoryType.Item:
                    InventoryForm.SelectItem(selected ? ItemData : null);
                    break;
                case InventoryType.HeroPiece:
                    InventoryForm.SelectHeroPiece(selected ? HeroPieceData : null);
                    break;
            }
        }

        public void OnClickMultiSelectIcon()
        {
            InventoryForm.MultiSelectedGears(this);
        }

        private enum InventoryType
        {
            Unknown,
            Gear,
            Soul,
            Epigraph,
            Material,
            Item,
            HeroPiece
        }
    }
}
