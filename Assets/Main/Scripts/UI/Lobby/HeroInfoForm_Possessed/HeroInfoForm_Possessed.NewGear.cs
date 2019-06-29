using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroInfoForm_Possessed
    {
        [SerializeField]
        private AttributeItem[] m_NewGearAttributeItems = null;

        [SerializeField]
        private NewGearItem[] m_NewGearItems = null;

        [SerializeField]
        private UILabel m_NewGearLevelUpCoin = null;

        [SerializeField]
        private NewGearQualityMaterialItem[] m_NewGearQualityMaterials = null;

        [SerializeField]
        private UILabel m_NewGearName = null;

        [SerializeField]
        private UILabel m_NewGearStrenthenLevel = null;

        private int m_CurNewGearIndex = 0;

        private NewGearQualityUpDisplayData m_NewGearQualityUpData = new NewGearQualityUpDisplayData();

        private bool m_IsCanNewGearQualityUp = true;

        private bool m_IsGearQualityLevelUp = false;

        public bool CloseNewGear()
        {
            return true;
        }

        public bool RefreshNewGear()
        {
            m_CurNewGearIndex = 0;

            RrfreshNewGearItems();
            RefreshNewGearData();
            return true;
        }

        private void RrfreshNewGearItems()
        {
            var heroData = HeroData as LobbyHeroData;
            var newGear = heroData.NewGears;
            for (int i = 0; i < m_NewGearItems.Length; i++)
            {
                if (i < newGear.Data.Count)
                {
                    m_NewGearItems[i].RefreshNewGear(newGear.Data[i].Key, newGear.Data[i].Quality);
                    m_NewGearItems[i].IsSelect = m_CurNewGearIndex == i;
                    if (m_CurNewGearIndex == i && m_IsGearQualityLevelUp)
                    {
                        m_EffectsController.ShowEffect(string.Format("EffectGear{0}", i + 1));
                        m_IsGearQualityLevelUp = false;
                    }
                }
            }
        }

        private void RefreshNewGearData()
        {
            var heroData = HeroData as LobbyHeroData;
            var newGear = heroData.NewGears.Data[m_CurNewGearIndex];

            DRNewGear newGearDataRow = GameEntry.DataTable.GetDataTable<DRNewGear>().GetDataRow(newGear.Type);
            if (newGearDataRow != null)
            {
                string newGearName = GameEntry.Localization.GetString(newGearDataRow.Name);
                m_NewGearName.color = Constant.Quality.Colors[(int)newGear.Quality];
                if (newGear.QualityLevel > 0)
                {
                    m_NewGearName.text = GameEntry.Localization.GetString("UI_TEXT_GEAR_NAME_QUALITY_IMPROVEMENT", newGearName, newGear.QualityLevel);
                }
                else
                {
                    m_NewGearName.text = newGearName;
                }
            }
            DRNewGearQualityMaxLevel drMaxQualitylevel = GameEntry.DataTable.GetDataTable<DRNewGearQualityMaxLevel>().GetDataRow((int)newGear.Quality);
            if (drMaxQualitylevel != null)
            {
                m_NewGearStrenthenLevel.text = GameEntry.Localization.GetString("UI_TEXT_QUALITY_NUMBER", newGear.StrengthenLevel);
            }
            var attributeTypes = newGear.GetSortedStrenthenLevelAttributeTypes();
            SetNewGearQualityUpAttribute(newGear);

            for (int i = 0; i < m_NewGearAttributeItems.Length; i++)
            {
                m_NewGearAttributeItems[i].AttributeName.gameObject.SetActive(i < attributeTypes.Count);
                m_NewGearAttributeItems[i].AttributeNext.gameObject.SetActive(i < attributeTypes.Count);
                m_NewGearAttributeItems[i].AttributeNow.gameObject.SetActive(i < attributeTypes.Count);
                if (i >= attributeTypes.Count)
                {
                    continue;
                }
                float value = newGear.GetFloatAttribute(attributeTypes[i]);
                m_NewGearAttributeItems[i].AttributeName.text = GameEntry.Localization.GetString(Constant.AttributeName.AttributeNameDics[(int)attributeTypes[i]]);
                m_NewGearAttributeItems[i].AttributeNow.text = UIUtility.GetAttributeValueStr(attributeTypes[i], value);
                if (!newGear.IsTopStrengthenLevel)
                {
                    m_NewGearAttributeItems[i].AttributeNext.text = GameEntry.Localization.GetString("UI_TEXT_PLUS",
                        UIUtility.GetAttributeValueStr(attributeTypes[i], newGear.GetFloatAttribute(attributeTypes[i], NewGearAttrFlag.NextStrengthenLevel) - value));
                }
                else
                {
                    m_NewGearAttributeItems[i].AttributeNext.text = GameEntry.Localization.GetString("UI_TEXT_MAX");
                }
            }
            int requiredCoins = newGear.StrengthenLevelRequiredCoins;
            m_NewGearLevelUpCoin.text = requiredCoins.ToString();

            var needItemIds = newGear.QualityLevelUpItemIds;
            var needItemCounts = newGear.QualityLevelUpItemCounts;
            m_IsCanNewGearQualityUp = true;
            for (int i = 0; i < m_NewGearQualityMaterials.Length; i++)
            {
                if (i >= needItemIds.Count)
                {
                    m_NewGearQualityMaterials[i].SetVisib(false);
                    continue;
                }
                m_NewGearQualityMaterials[i].SetVisib(true);
                var itemData = GameEntry.Data.Items.GetData(needItemIds[i]);
                var dataTable = GameEntry.DataTable.GetDataTable<DRItem>();
                DRItem itemDataRow = dataTable.GetDataRow(needItemIds[i]);
                if (itemDataRow == null)
                {
                    Log.Warning("Cannot find Item type is '{0}'.", needItemIds[i]);
                    continue;
                }
                m_NewGearQualityMaterials[i].ItemView.InitItem(needItemIds[i], (QualityType)itemDataRow.Quality);
                int hasCount = itemData == null ? 0 : itemData.Count;

                if (hasCount >= needItemCounts[i])
                {
                    m_NewGearQualityMaterials[i].ItemView.SetColor(Color.white);
                    m_NewGearQualityMaterials[i].CountLabel.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", hasCount, needItemCounts[i]);
                }
                else
                {
                    m_IsCanNewGearQualityUp = false;
                    m_NewGearQualityMaterials[i].ItemView.SetColor(Color.grey);
                    m_NewGearQualityMaterials[i].CountLabel.text = GameEntry.Localization.GetString("UI_TEXT_SLASH_RED", hasCount, needItemCounts[i]);
                }
            }
        }

        private void SetNewGearQualityUpAttribute(NewGearData newGear)
        {
            var attributeTypes = newGear.GetSortedQualityUpAttributeTypes();
            m_NewGearQualityUpData.LastAttribute = new float[attributeTypes.Count];
            m_NewGearQualityUpData.LastAttributeType = new AttributeType[attributeTypes.Count];
            m_NewGearQualityUpData.NowAttribute = new float[attributeTypes.Count];
            m_NewGearQualityUpData.NowAttributeType = new AttributeType[attributeTypes.Count];
            m_NewGearQualityUpData.LastQuality = newGear.Quality;
            m_NewGearQualityUpData.LastQualityLevel = newGear.QualityLevel;
            for (int i = 0; i < attributeTypes.Count; i++)
            {
                float value = newGear.GetFloatAttribute(attributeTypes[i]);
                m_NewGearQualityUpData.LastAttribute[i] = value;
                m_NewGearQualityUpData.LastAttributeType[i] = attributeTypes[i];
                m_NewGearQualityUpData.NowAttribute[i] = newGear.GetFloatAttribute(attributeTypes[i], NewGearAttrFlag.NextTotalQualityLevel);
                m_NewGearQualityUpData.NowAttributeType[i] = attributeTypes[i];
            }
        }

        public void OnClickNewGearQualityUp()
        {
            var heroData = HeroData as LobbyHeroData;
            var newGear = heroData.NewGears.Data[m_CurNewGearIndex];
            if (newGear.IsTopQualityLevel)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_GEAR_QUALITY_MAX"));
                return;
            }

            if (!m_IsCanNewGearQualityUp)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_MATERIAL_NOT_ENOUGH"));
                return;
            }
            GameEntry.LobbyLogic.RequestNewGearQualityLevelUp(HeroData.Type, newGear.Type);
        }

        private void OnNewGearQualityUp(object sender, GameEventArgs e)
        {
            var heroData = HeroData as LobbyHeroData;

            m_IsGearQualityLevelUp = true;

            var newGear = heroData.NewGears.Data[m_CurNewGearIndex];
            m_NewGearQualityUpData.BaseNewGearData = newGear;
            //m_NewGearQualityUpData.OnOpenFinished = OnOpenNewGearQualityUpSuccessFinished;
            m_NewGearQualityUpData.OnCloseAction = OnOpenNewGearQualityUpSuccessFinished;
            GameEntry.UI.OpenUIForm(UIFormId.NewGearQualityUpSuccessForm, m_NewGearQualityUpData);
        }

        private void OnOpenNewGearQualityUpSuccessFinished(object obj)
        {
            RrfreshNewGearItems();
            RefreshNewGearData();
        }

        public void OnClickStrengthenNewGearLevel()
        {
            var heroData = HeroData as LobbyHeroData;
            var newGear = heroData.NewGears.Data[m_CurNewGearIndex];
            int requiredCoins = newGear.StrengthenLevelRequiredCoins;

            if (GameEntry.Data.Player.Coin < requiredCoins)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_GOLD_NOT_ENOUGH"));
                return;
            }

            if (newGear.StrengthenLevel >= heroData.Level)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_GEAR_LEVEL_LESS_THAN_HERO_LEVEL"));
                return;
            }

            GameEntry.LobbyLogic.RequestNewGearStrengthen(HeroData.Type, newGear.Type);
        }

        private void OnStrengthenNewGearLevel(object sender, GameEventArgs e)
        {
            GameEntry.UI.OpenUIForm(UIFormId.NewGearLevelUpFrom);
            RefreshNewGearData();
        }

        public void OnClickNewGear(int index)
        {
            if (m_CurNewGearIndex == index)
            {
                return;
            }

            m_CurNewGearIndex = index;
            for (int i = 0; i < m_NewGearItems.Length; i++)
            {
                m_NewGearItems[i].IsSelect = i == index;
            }
            RefreshNewGearData();
        }

        public void OnClickGearMaterialItem(int index)
        {
            // Empty.
        }
    }
}
