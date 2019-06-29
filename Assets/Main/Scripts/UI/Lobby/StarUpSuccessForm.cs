using UnityEngine;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄升级成功界面。
    /// </summary>
    public class StarUpSuccessForm : NGUIForm
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
            public UISprite[] Stars = null;
        }

        [SerializeField]
        private HeroItem m_LastHeroView = null;

        [SerializeField]
        private HeroItem m_CurHeroView = null;

        [SerializeField]
        private AttributeItem[] m_AttributeItems = null;

        private const string StarUpEffectName = "EffectStarUp";
        private StrengthenDisplayData m_Data = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var data = userData as StrengthenDisplayData;
            m_Data = data;
            SetHeroView(m_LastHeroView, data.BaseHeroData.StarLevel - 1, data.BaseHeroData);
            SetHeroView(m_CurHeroView, data.BaseHeroData.StarLevel, data.BaseHeroData);
            
            for (int i = 0; i < m_AttributeItems.Length; i++)
            {
                SetAttribute(i, data);
            }
        }

        protected override void OnPostOpen(object data)
        {
            base.OnPostOpen(data);
            if (m_Data == null)
            {
                return;
            }
            m_EffectsController.ShowEffect(StarUpEffectName + m_Data.BaseHeroData.StarLevel.ToString());
        }

        private void SetHeroView(HeroItem item, int starLevel, BaseLobbyHeroData data)
        {
            item.Element.spriteName = UIUtility.GetElementSpriteName(data.ElementId);
            item.QualityBg.spriteName = Constant.Quality.HeroBorderSpriteNames[(int)data.Quality];
            UIUtility.SetStarLevel(item.Stars, starLevel);
            item.Icon.LoadAsync(data.IconId);
        }

        private void SetAttribute(int index, StrengthenDisplayData userData)
        {
            if (userData == null || userData.BaseHeroData == null)
            {
                return;
            }

            var heroData = userData.BaseHeroData;
            switch (index)
            {
                case 0:
                    SetAtrributeItem(m_AttributeItems[0], AttributeType.MaxHP, userData.LastMaxHP, heroData.MaxHP);
                    break;
                case 1:
                    SetAtrributeItem(m_AttributeItems[1], AttributeType.PhysicalAttack, userData.LastPhysicalAttack, heroData.PhysicalAttack);
                    break;
                case 2:
                    SetAtrributeItem(m_AttributeItems[2], AttributeType.PhysicalDefense, userData.LastPhysicalDefense, heroData.PhysicalDefense);
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

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

    }
}
