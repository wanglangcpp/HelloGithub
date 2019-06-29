using UnityEngine;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄升阶成功界面。
    /// </summary>
    public class QualityUpSuccessForm : NGUIForm
    {
        [Serializable]
        private class AttributeItem
        {
            [SerializeField]
            public UILabel AttributeNow = null;

            [SerializeField]
            public UILabel AttributeLast = null;

            [SerializeField]
            public UILabel AttributePlus = null;

            [SerializeField]
            public UILabel AttributeNowName = null;

            [SerializeField]
            public UILabel AttributeLastName = null;
        }

        [Serializable]
        private class HeroItem
        {
            [SerializeField]
            public UISprite Icon = null;

            [SerializeField]
            public UISprite QualityBg = null;

            [SerializeField]
            public UISprite Element = null;

            [SerializeField]
            public UISprite[] QualityIcons = null;
        }

        [SerializeField]
        private HeroItem m_LastHeroView = null;

        [SerializeField]
        private HeroItem m_CurHeroView = null;

        [SerializeField]
        private AttributeItem[] m_AttributeItems = null;

        private QualityUpDisplayData m_Data = null;

        private string[] HeroQualityIconNameDics = new string[]
        {
            "",
            "icon_quality_silvery",
            "icon_quality_green",
            "icon_quality_blue",
            "icon_quality_purple",
            "icon_quality_orange",
        };

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Data = userData as QualityUpDisplayData;

            SetHeroView(m_LastHeroView, m_Data.LastQuality, m_Data.LastQualityLevel);
            SetHeroView(m_CurHeroView, m_Data.BaseHeroData.Quality, m_Data.BaseHeroData.QualityLevel);
            for (int i = 0; i < m_AttributeItems.Length; i++)
            {
                SetAttribute(i);
            }
        }

        private void SetHeroView(HeroItem item, QualityType quality, int qualityLevel)
        {
            if (m_Data == null)
            {
                return;
            }
            item.Element.spriteName = UIUtility.GetElementSpriteName(m_Data.BaseHeroData.ElementId);
            item.QualityBg.spriteName = Constant.Quality.HeroBorderSpriteNames[(int)quality];
            var dataTable = GameEntry.DataTable.GetDataTable<DRHeroQualityMaxLevel>();
            DRHeroQualityMaxLevel maxLevelRow = dataTable.GetDataRow((int)quality);
            for (int i = 0; i < item.QualityIcons.Length; i++)
            {
                item.QualityIcons[i].gameObject.SetActive(i < maxLevelRow.MaxLevel);
                item.QualityIcons[i].spriteName = HeroQualityIconNameDics[(int)quality];
                if (i < qualityLevel)
                {
                    item.QualityIcons[i].color = Color.white;
                }
                else
                {
                    item.QualityIcons[i].color = Color.grey;
                }
            }
            item.Icon.LoadAsync(m_Data.BaseHeroData.IconId);
        }

        private void SetAttribute(int index)
        {
            if (m_Data == null)
            {
                return;
            }

            switch (index)
            {
                case 0:
                    SetAtrributeItem(m_AttributeItems[0], AttributeType.MaxHP, m_Data.LastMaxHp, m_Data.BaseHeroData.MaxHP);
                    break;
                case 1:
                    SetAtrributeItem(m_AttributeItems[1], AttributeType.PhysicalAttack, m_Data.LastPhysicalAttack, m_Data.BaseHeroData.PhysicalAttack);
                    break;
                case 2:
                    SetAtrributeItem(m_AttributeItems[2], AttributeType.PhysicalDefense, m_Data.LastPhysicalDefense, m_Data.BaseHeroData.PhysicalDefense);
                    break;
                default:
                    return;
            }
        }

        private void SetAtrributeItem(AttributeItem item, AttributeType type, float lastValue, float nowValue)
        {
            item.AttributeNowName.text = item.AttributeLastName.text = GameEntry.Localization.GetString(Constant.AttributeName.AttributeNameDics[(int)type]);
            item.AttributeNow.text = UIUtility.GetAttributeValueStr(type, nowValue);
            item.AttributeLast.text = UIUtility.GetAttributeValueStr(type, lastValue);
            item.AttributePlus.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", (nowValue - lastValue).ToString());
        }
    }
}
