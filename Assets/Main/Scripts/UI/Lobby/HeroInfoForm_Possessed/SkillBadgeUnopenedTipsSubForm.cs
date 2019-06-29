using UnityEngine;
using System.Collections;
using GameFramework;

namespace Genesis.GameClient
{
    public class SkillBadgeUnopenedTipsSubForm : MonoBehaviour
    {
        //[SerializeField]
        //private UILabel m_SkillLevelText;

        //[SerializeField]
        //private UILabel m_SkillLevelNumText;

        [SerializeField]
        private UILabel m_HeroStarUpText = null;

        [SerializeField]
        private UILabel m_HeroStarUpNumText = null;

        [SerializeField]
        private UILabel m_OpenBadgeText = null;

        private int m_HeroType = 0;
        private int m_SelectSkillIndex = 0;
        private int m_DirectUnlockMoneyCost = 0;
        private int m_BadgeSlotCategory = 0;
        private int m_GenericBadgeSlotIndex = 0;

        public void OnRefreshData(int heroType, int selectSkillIndex, int directUnlockMoneyCost, int badgeSlotCategory, int genericBadgeSlotIndex, int heroStarLevelCondition, Color heroStarUpTextColor)
        {
            //m_SkillLevelText.color = skillLevelTextColor;
            //m_SkillLevelText.text = GameEntry.Localization.GetString("UI_TEXT_SKILL_LEVEL");
            //m_SkillLevelNumText.color = skillLevelTextColor;
            //m_SkillLevelNumText.text = skillLevelCondition.ToString();

            m_HeroType = heroType;
            m_SelectSkillIndex = selectSkillIndex;
            m_DirectUnlockMoneyCost = directUnlockMoneyCost;
            m_BadgeSlotCategory = badgeSlotCategory;
            m_GenericBadgeSlotIndex = genericBadgeSlotIndex;
            m_HeroStarUpText.color = heroStarUpTextColor;
            m_HeroStarUpText.text = GameEntry.Localization.GetString("UI_TEXT_HERO_STARUP");
            m_HeroStarUpNumText.color = heroStarUpTextColor;
            m_HeroStarUpNumText.text = heroStarLevelCondition.ToString();
            m_OpenBadgeText.text = GameEntry.Localization.GetString("UI_TEXT_CHAT_COST_DIAMOND_OPEN_BADGE", m_DirectUnlockMoneyCost);
        }

        public void OnClickSkillBadgeOpenButton()
        {
            var playerData = GameEntry.Data.Player;
            if (playerData.Money >= m_DirectUnlockMoneyCost)
            {
                GameEntry.LobbyLogic.ActivateSkillBadgeSlot(m_HeroType, m_SelectSkillIndex, m_BadgeSlotCategory, m_GenericBadgeSlotIndex);
                OnClickScreenBack();
            }
            else
            {
                UIUtility.CheckCurrency(CurrencyType.Money, m_DirectUnlockMoneyCost);
            }
        }

        public void OnClickScreenBack()
        {
            if (this != null)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}