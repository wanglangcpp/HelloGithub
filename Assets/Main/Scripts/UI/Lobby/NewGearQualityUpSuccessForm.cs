using UnityEngine;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备升阶成功界面。
    /// </summary>
    public class NewGearQualityUpSuccessForm : NGUIForm
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
        private class GearItem
        {
            [SerializeField]
            public UISprite Icon = null;

            [SerializeField]
            public UISprite QualityBg = null;

            [SerializeField]
            public UILabel NameLabel = null;
        }

        [SerializeField]
        private GearItem m_LastGearView = null;

        [SerializeField]
        private GearItem m_CurGearView = null;

        [SerializeField]
        private AttributeItem[] m_AttributeItems = null;

        private NewGearQualityUpDisplayData m_Data = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Data = userData as NewGearQualityUpDisplayData;

            SetNewGearView(m_LastGearView, m_Data.LastQuality, m_Data.LastQualityLevel);
            SetNewGearView(m_CurGearView, m_Data.BaseNewGearData.Quality, m_Data.BaseNewGearData.QualityLevel);
            for (int i = 0; i < m_AttributeItems.Length; i++)
            {
                SetAttributeVisible(m_AttributeItems[i], i < m_Data.LastAttributeType.Length);
                if (i >= m_Data.LastAttributeType.Length)
                {
                    continue;
                }
                SetAtrributeItem(m_AttributeItems[i], m_Data.LastAttributeType[i], m_Data.NowAttributeType[i], m_Data.LastAttribute[i]);
            }

            if (m_Data.OnOpenFinished != null)
            {
                m_Data.OnOpenFinished(null);
            }
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);

            if (m_Data != null && m_Data.OnCloseAction != null)
            {
                m_Data.OnCloseAction.Invoke(null);
            }
        }

        private void SetNewGearView(GearItem item, QualityType quality, int qualityLevel)
        {
            if (m_Data == null)
            {
                return;
            }
            item.QualityBg.spriteName = Constant.Quality.ItemBorderSpriteNames[(int)quality];
            var drTable = GameEntry.DataTable.GetDataTable<DRNewGear>().GetDataRow(m_Data.BaseNewGearData.Type);
            item.Icon.LoadAsync(drTable.IconId);
            DRNewGear newGearDataRow = GameEntry.DataTable.GetDataTable<DRNewGear>().GetDataRow(m_Data.BaseNewGearData.Type);
            if (newGearDataRow != null)
            {
                item.NameLabel.color = Constant.Quality.Colors[(int)quality];
                if (qualityLevel > 0)
                {
                    string name = GameEntry.Localization.GetString(newGearDataRow.Name);
                    item.NameLabel.text = GameEntry.Localization.GetString("UI_TEXT_GEAR_NAME_QUALITY_IMPROVEMENT", name, qualityLevel);
                }
                else
                {
                    item.NameLabel.text = GameEntry.Localization.GetString(newGearDataRow.Name);
                }
            }
        }

        private void SetAttributeVisible(AttributeItem item, bool visible)
        {
            item.AttributeLastName.gameObject.SetActive(visible);
            item.AttributeNowName.gameObject.SetActive(visible);
            item.AttributeNow.gameObject.SetActive(visible);
            item.AttributeLast.gameObject.SetActive(visible);
            item.AttributePlus.gameObject.SetActive(visible);
        }

        private void SetAtrributeItem(AttributeItem item, AttributeType lastType, AttributeType nowType, float lastValue)
        {
            item.AttributeNowName.text = GameEntry.Localization.GetString(Constant.AttributeName.AttributeNameDics[(int)nowType]);
            item.AttributeLastName.text = GameEntry.Localization.GetString(Constant.AttributeName.AttributeNameDics[(int)lastType]);
            item.AttributeNow.text = UIUtility.GetAttributeValueStr(nowType, m_Data.BaseNewGearData.GetFloatAttribute(nowType));
            item.AttributeLast.text = UIUtility.GetAttributeValueStr(lastType, lastValue);
            string plasValue = lastType == nowType ? UIUtility.GetAttributeValueStr(lastType, m_Data.BaseNewGearData.GetFloatAttribute(nowType) - lastValue) : 0.ToString();
            item.AttributePlus.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", plasValue);
        }
    }
}
