using UnityEngine;
using System.Collections;
using GameFramework;
using GameFramework.Event;
using System;

namespace Genesis.GameClient
{
    public class SkillBadgeStageAdvanceSubForm : NGUISubForm
    {
        [SerializeField]
        private UISprite m_LeftSkillBadgeIcon = null;

        [SerializeField]
        private UISprite m_LeftBadgeBorder = null;

        [SerializeField]
        private UILabel m_LeftSkillBadgeName = null;

        [SerializeField]
        private UILabel m_LeftSkillBadgeNum = null;

        [SerializeField]
        private UISprite m_RightSkillBadgeIcon = null;

        [SerializeField]
        private UISprite m_RightBadgeBorder = null;

        [SerializeField]
        private UILabel m_RightSkillBadgeName = null;

        [SerializeField]
        private UILabel m_RightSkillBadgeNum = null;

        [SerializeField]
        private UILabel m_AdvanceNumToOneText = null;

        [SerializeField]
        private UILabel m_CanAdvanceNum = null;

        [SerializeField]
        private UISlider m_Slider = null;

        [SerializeField]
        private UIButton m_StageAdvanceBtn = null;

        [SerializeField]
        private UILabel m_AdvanceCostCoinText = null;

        [SerializeField]
        private UILabel m_InsufficientNum = null;

        [SerializeField]
        private UISprite m_ProgressBg = null;

        [SerializeField]
        private Collider m_SliderBgCollider = null;

        [SerializeField]
        private UIButton m_SliderThumbButton = null;

        /// <summary>
        /// 当前徽章编号。
        /// </summary>
        private int m_SourceBadgeId = 0;

        /// <summary>
        /// 进阶所需数量。
        /// </summary>
        private float m_LevelUpBadgeCount = 0;

        /// <summary>
        /// 是否是从英雄界面打开的。
        /// </summary>
        private bool m_IsOpenThisFromHeroPanel = false;

        /// <summary>
        /// 当前徽章总数量。
        /// </summary>
        private float m_SourceBadgeCount = 0;

        /// <summary>
        /// 进阶花费金币数量。
        /// </summary>
        private int m_LevelUpCoin = 0;

        /// <summary>
        /// 进阶徽章编号。
        /// </summary>
        private int m_LevelUpBadgeId = 0;

        /// <summary>
        /// 能进阶的徽章最大数量。
        /// </summary>
        private float m_CanLevelUpBadgeGrossNum = 0;

        /// <summary>
        /// 确定进阶徽章的数量。
        /// </summary>
        private int m_ConfirmAdvanceNum = 0;

        private int m_HeroId = 0;
        private int m_SkillIndex = 0;
        private int m_BadgeSlotCategory = 0;
        private int m_GenericBadgeSlotIndex = 0;

        public void InitSkillBadgeStageAdvance()
        {
            gameObject.SetActive(false);
        }

        public void RefreshData(int skillBadgeId, bool isOpenThisFromHeroPanel, int heroId, int skillIndex, int badgeSlotCategory, int genericBadgeSlotIndex)
        {
            m_IsOpenThisFromHeroPanel = isOpenThisFromHeroPanel;
            m_SourceBadgeId = skillBadgeId;
            m_HeroId = heroId;
            m_SkillIndex = skillIndex;
            m_BadgeSlotCategory = badgeSlotCategory;
            m_GenericBadgeSlotIndex = genericBadgeSlotIndex;
            CalculateAdvanceCount();
            LeftAndRightShow();
            OnCanNotAdvance();
        }

        public void RefreshData(int skillBadgeId, bool isOpenThisFromHeroPanel)
        {
            m_IsOpenThisFromHeroPanel = isOpenThisFromHeroPanel;
            m_SourceBadgeId = skillBadgeId;
            m_BadgeSlotCategory = (int)GeneralItemUtility.GetSkillBadgeCateogry(skillBadgeId);
            CalculateAdvanceCount();
            LeftAndRightShow();
            OnCanNotAdvance();
        }

        private void CalculateAdvanceCount()
        {
            var drBadge = GeneralItemUtility.GetSkillBadgeDataRow(m_SourceBadgeId);
            m_LevelUpBadgeCount = drBadge.LevelUpCount;
            m_LevelUpCoin = drBadge.LevelUpCoin;
            m_LevelUpBadgeId = drBadge.LevelUpBadgeId;
            m_SourceBadgeCount = GeneralItemUtility.GetGeneralItemCount(m_SourceBadgeId);
            if (m_IsOpenThisFromHeroPanel)
            {
                m_SourceBadgeCount += 1;
            }
            m_CanLevelUpBadgeGrossNum = m_SourceBadgeCount / m_LevelUpBadgeCount;
            m_Slider.numberOfSteps = Mathf.FloorToInt(m_CanLevelUpBadgeGrossNum);
            m_SliderBgCollider.enabled = Mathf.FloorToInt(m_CanLevelUpBadgeGrossNum) > 1;
            m_SliderThumbButton.isEnabled = Mathf.FloorToInt(m_CanLevelUpBadgeGrossNum) > 1;
        }

        private void LeftAndRightShow()
        {
            var drBadge = GeneralItemUtility.GetSkillBadgeDataRow(m_SourceBadgeId);
            string formatKey = m_SourceBadgeCount < m_LevelUpBadgeCount ? "UI_TEXT_SLASH_RED" : "UI_TEXT_SLASH";
            m_LeftSkillBadgeNum.text = GameEntry.Localization.GetString(formatKey, m_SourceBadgeCount, m_LevelUpBadgeCount);
            m_LeftSkillBadgeIcon.LoadAsync(GeneralItemUtility.GetSkillBadgeIconId(m_SourceBadgeId));
            m_LeftBadgeBorder.spriteName = GeneralItemUtility.GetSkillBadgeBgSpriteName((SkillBadgeSlotCategory)m_BadgeSlotCategory, drBadge.Level);
            m_LeftSkillBadgeName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(m_SourceBadgeId));

            var drLevelUpBadge = GeneralItemUtility.GetSkillBadgeDataRow(m_LevelUpBadgeId);
            m_RightSkillBadgeIcon.LoadAsync(GeneralItemUtility.GetSkillBadgeIconId(m_LevelUpBadgeId));
            m_RightBadgeBorder.spriteName = GeneralItemUtility.GetSkillBadgeBgSpriteName((SkillBadgeSlotCategory)m_BadgeSlotCategory, drLevelUpBadge.Level);
            m_RightSkillBadgeName.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(m_LevelUpBadgeId));

            m_AdvanceNumToOneText.text = GameEntry.Localization.GetString("UI_TEXT_THREE_IN_ONE", m_LevelUpBadgeCount);
        }

        private void OnCanNotAdvance()
        {
            if (m_CanLevelUpBadgeGrossNum < 1)
            {
                m_StageAdvanceBtn.isEnabled = false;
                m_ProgressBg.gameObject.SetActive(false);
                m_InsufficientNum.gameObject.SetActive(true);
                m_CanAdvanceNum.text = 0.ToString();
                m_RightSkillBadgeNum.text = 0.ToString();
                m_AdvanceCostCoinText.text = (m_ConfirmAdvanceNum * m_LevelUpCoin).ToString();
                m_InsufficientNum.text = GameEntry.Localization.GetString("UI_TEXT_INSUFFICIENT_NUMBER");
            }
            else
            {
                OnSliderValueChange();
                m_StageAdvanceBtn.isEnabled = true;
                m_ProgressBg.gameObject.SetActive(true);
                m_InsufficientNum.gameObject.SetActive(false);
            }
        }

        public void OnSliderValueChange()
        {
            float canLevelUpBadgeGrossNum = Mathf.Floor(m_CanLevelUpBadgeGrossNum);
            if (m_Slider.value >= 1f)
            {
                m_ConfirmAdvanceNum = Mathf.FloorToInt(canLevelUpBadgeGrossNum * m_Slider.value);
            }
            else
            {
                m_ConfirmAdvanceNum = Mathf.FloorToInt(canLevelUpBadgeGrossNum * m_Slider.value) + 1;
            }
            string formatKey = m_SourceBadgeCount < m_LevelUpBadgeCount ? "UI_TEXT_SLASH_RED" : "UI_TEXT_SLASH";
            m_LeftSkillBadgeNum.text = GameEntry.Localization.GetString(formatKey, m_SourceBadgeCount, m_LevelUpBadgeCount * m_ConfirmAdvanceNum);
            m_CanAdvanceNum.text = m_ConfirmAdvanceNum.ToString();
            m_RightSkillBadgeNum.text = m_ConfirmAdvanceNum.ToString();
            m_AdvanceCostCoinText.text = (m_ConfirmAdvanceNum * m_LevelUpCoin).ToString();
            m_StageAdvanceBtn.isEnabled = (m_ConfirmAdvanceNum * m_LevelUpCoin) < GameEntry.Data.Player.Coin;
        }

        public void OnClickAddBtn()
        {
            float canLevelUpBadgeGrossNum = Mathf.Floor(m_CanLevelUpBadgeGrossNum) - 1f;
            if (canLevelUpBadgeGrossNum >= 1)
            {
                if (m_Slider.value <= 1f)
                {
                    m_Slider.value += (1f / canLevelUpBadgeGrossNum);
                }
            }
        }

        public void OnClickMinusBtn()
        {
            float canLevelUpBadgeGrossNum = Mathf.Floor(m_CanLevelUpBadgeGrossNum) - 1f;
            if (canLevelUpBadgeGrossNum >= 1)
            {
                if (m_Slider.value >= 0)
                {
                    m_Slider.value -= (1f / canLevelUpBadgeGrossNum);
                }
            }
        }

        public void OnClickConfirmAdvanceBtn()
        {
            if (m_IsOpenThisFromHeroPanel)
            {
                GameEntry.LobbyLogic.LevelUpSkillBadge(true, m_SourceBadgeId, m_ConfirmAdvanceNum, m_HeroId, m_SkillIndex, m_BadgeSlotCategory, m_GenericBadgeSlotIndex);
            }
            else
            {
                GameEntry.LobbyLogic.LevelUpSkillBadge(false, m_SourceBadgeId, m_ConfirmAdvanceNum);
            }
        }

        protected internal override void OnOpen()
        {
            m_Slider.value = 0;
            GameEntry.Event.Subscribe(EventId.OnSkillBadgeDataChanged, OnSkillBadgeDataChanged);
            base.OnOpen();
        }

        protected internal override void OnClose()
        {
            GameEntry.Event.Unsubscribe(EventId.OnSkillBadgeDataChanged, OnSkillBadgeDataChanged);
            base.OnClose();
        }

        private void OnSkillBadgeDataChanged(object sender, GameEventArgs e)
        {
            m_Slider.value = 0;
            m_IsOpenThisFromHeroPanel = false;
            CalculateAdvanceCount();
            LeftAndRightShow();
            OnCanNotAdvance();
        }

    }
}