using UnityEngine;
using System.Collections;
using GameFramework;
using System.Collections.Generic;
using System;

namespace Genesis.GameClient
{
    public class GenericSkillBadgeSlotItem : MonoBehaviour
    {
        /// <summary>
        /// 通用技能徽章位索引。
        /// </summary>
        public int m_GenericBadgeSlotIndex = 0;

        /// <summary>
        /// 技能徽章种类。
        /// </summary>
        public int m_SkillBadgeSlotCategory = 0;

        /// <summary>
        /// 通用技能徽章位颜色。
        /// </summary>
        [SerializeField]
        private int m_GenericSkillBadgeSlotColor = 0;

        public int GenericSkillBadgeSlotColor
        {
            get { return m_GenericSkillBadgeSlotColor; }
            set { m_GenericSkillBadgeSlotColor = value; }
        }

        [SerializeField]
        private SkillBadgeSlotState m_SkillBadgeSlotState;

        public SkillBadgeSlotState SkillBadgeSlotState
        {
            get { return m_SkillBadgeSlotState; }
            set { m_SkillBadgeSlotState = value; }
        }

        [SerializeField]
        private int m_GenericBadgeUnlockContitionId = 0;

        public int GenericBadgeUnlockContitionId
        {
            get { return m_GenericBadgeUnlockContitionId; }
            set { m_GenericBadgeUnlockContitionId = value; }
        }

        [SerializeField]
        private UISprite m_BadgeIcon = null;

        [SerializeField]
        private UISprite m_LcokIcon = null;

        [SerializeField]
        private UISprite m_SkillDetailBg = null;

        [SerializeField]
        private UILabel m_BadgeDesText = null;

        //[SerializeField]
        //public UIButton m_SkillBadgeOpenBtn = null;

        //[SerializeField]
        //private UILabel m_CostNumText = null;

        [SerializeField]
        private UISprite m_BorderBadge = null;

        [SerializeField]
        private UISprite m_BadgeBorderFrame = null;

        [SerializeField]
        private UISprite m_BadgeBorderColor = null;

        [SerializeField]
        private UISprite m_PlusIcon = null;

        [SerializeField]
        private SkillBadgeUnopenedTipsSubForm m_SkillBadgeUnopenedTipsSubForm = null;

        [SerializeField]
        private SkillBadgeChoiceSubForm m_SkillBadgeChoiceSubForm = null;

        [SerializeField]
        private Color m_BadgeConditionGreen;

        [SerializeField]
        private Color m_BadgeConditionRed;

        private bool m_IsOpenByMoneyOnly = false;
        private int m_HeroType = 0;
        private int m_SkillIndex = 0;
        private int m_SkillGroupId = 0;
        private int m_SkillBadgeId = 0;
        private int m_SkillElementId = 0;
        //private int m_SkillLevelCondition = 0;
        private int m_HeroStarLevelCondition = 0;
        //private Color m_SkillLevelTextColor;
        private Color m_HeroStarUpTextColor;
        private int m_DirectUnlockMoneyCost = 0;
        private GameFrameworkFunc<SkillBadgeReplaceSubForm> m_ShowSkillBadgeReplaceSubForm;
        private GameFrameworkFunc<SkillBadgeStageAdvanceSubForm> m_SkillBadgeStageAdvanceSubForm;

        public int DirectUnlockMoneyCost
        {
            get { return m_DirectUnlockMoneyCost; }
        }

        public void RefreshData(int heroType, int skillIndex, int skillGroupId, int skillBadgeId, GameFrameworkFunc<SkillBadgeReplaceSubForm> showSkillBadgeReplaceSubForm, GameFrameworkFunc<SkillBadgeStageAdvanceSubForm> skillBadgeStageAdvanceSubForm)
        {
            m_HeroType = heroType;
            m_SkillIndex = skillIndex;
            m_SkillGroupId = skillGroupId;
            m_SkillBadgeId = skillBadgeId;
            m_ShowSkillBadgeReplaceSubForm = showSkillBadgeReplaceSubForm;
            m_SkillBadgeStageAdvanceSubForm = skillBadgeStageAdvanceSubForm;
            var dtSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();
            DRSkillGroup skillGroup = dtSkillGroup.GetDataRow(skillGroupId);
            if (skillGroup == null)
            {
                Log.Warning("cannot find skill group by '{0}'.", skillGroupId);
                return;
            }
            m_SkillElementId = skillGroup.ElementId;
            if (m_SkillBadgeSlotState == SkillBadgeSlotState.LockedState)
            {
                SkillBadgeSlotLockedData(heroType, skillIndex);
            }
            else if (m_SkillBadgeSlotState == SkillBadgeSlotState.EquipBadgeState)
            {
                SkillBadgeSlotEquipState(skillBadgeId);
            }

            if (m_GenericSkillBadgeSlotColor >= 0)
            {
                m_BadgeBorderColor.color = GameEntry.ClientConfig.GetGenericSkillBadgeSlotColor(m_GenericSkillBadgeSlotColor);
            }
            else
            {
                gameObject.SetActive(false);
            }
            OnSkillBadgeSlotStateChange();
        }

        /// <summary>
        /// 当为锁定状态时需要计算的数据。
        /// </summary>
        /// <param name="heroType"></param>
        /// <param name="skillIndex"></param>
        private void SkillBadgeSlotLockedData(int heroType, int skillIndex)
        {
            var lobbyHeros = GameEntry.Data.LobbyHeros.Data;
            BaseLobbyHeroData heroData = null;
            for (int i = 0; i < lobbyHeros.Count; i++)
            {
                if (lobbyHeros[i].Type == heroType)
                {
                    heroData = lobbyHeros[i];
                }
            }
            if (heroData == null)
            {
                Log.Error("SkillStrengthenItem2 cannot find Hero '{0}' .", heroType);
            }

            var dtSkillBadgeSlotUnlockCondition = GameEntry.DataTable.GetDataTable<DRSkillBadgeSlotUnlockCondition>();
            if (m_GenericBadgeUnlockContitionId == -1)
            {
                return;
            }
            DRSkillBadgeSlotUnlockCondition drSkillBadgeSlotUnlockCondition = dtSkillBadgeSlotUnlockCondition.GetDataRow(m_GenericBadgeUnlockContitionId);
            if (drSkillBadgeSlotUnlockCondition == null)
            {
                Log.Warning("cannot find skillBadgeSlotUnlockCondition by '{0}'.", m_GenericBadgeUnlockContitionId);
                return;
            }
            m_IsOpenByMoneyOnly = drSkillBadgeSlotUnlockCondition.MoneyOnly;
            if (!drSkillBadgeSlotUnlockCondition.MoneyOnly)
            {
                //int skillLevel = heroData.GetSkillLevel(skillIndex);
                int heroStarLevel = heroData.StarLevel;
                //m_SkillLevelCondition = drSkillBadgeSlotUnlockCondition.SkillLevel;
                m_HeroStarLevelCondition = drSkillBadgeSlotUnlockCondition.HeroStarLevel;
                //m_SkillLevelTextColor = skillLevel >= m_SkillLevelCondition ? m_BadgeConditionGreen : m_BadgeConditionRed;
                m_HeroStarUpTextColor = heroStarLevel >= m_HeroStarLevelCondition ? m_BadgeConditionGreen : m_BadgeConditionRed;
            }
            m_DirectUnlockMoneyCost = drSkillBadgeSlotUnlockCondition.DirectUnlockMoneyCost;
        }

        /// <summary>
        /// 当为开启状态时需要计算的数据。
        /// </summary>
        private void SkillBadgeSlotEquipState(int skillBadgeId)
        {
            if (skillBadgeId > 0)
            {
                var dtGenericSkillBadge = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>();
                var genericSkillBadge = dtGenericSkillBadge.GetDataRow(skillBadgeId);
                if (genericSkillBadge == null)
                {
                    Log.Warning("cannot find genericSkillBadge by '{0}'.", skillBadgeId);
                    return;
                }
                m_BadgeIcon.LoadAsync(GeneralItemUtility.GetSkillBadgeIconId(skillBadgeId));
                m_BadgeBorderFrame.spriteName = GeneralItemUtility.GetSkillBadgeBgSpriteName(SkillBadgeSlotCategory.Generic, genericSkillBadge.Level);
                m_BadgeDesText.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemDescription(skillBadgeId));
            }
        }

        private void OnSkillBadgeSlotStateChange()
        {
            switch (m_SkillBadgeSlotState)
            {
                case SkillBadgeSlotState.LockedState:
                    m_BadgeIcon.gameObject.SetActive(false);
                    m_LcokIcon.gameObject.SetActive(true);
                    m_BorderBadge.gameObject.SetActive(true);
                    m_BadgeBorderFrame.gameObject.SetActive(false);
                    m_SkillDetailBg.gameObject.SetActive(false);
                    m_PlusIcon.gameObject.SetActive(false);
                    //m_CostNumText.text = m_DirectUnlockMoneyCost.ToString();
                    break;
                case SkillBadgeSlotState.EnableState:
                    m_BadgeIcon.gameObject.SetActive(false);
                    m_LcokIcon.gameObject.SetActive(false);
                    m_BorderBadge.gameObject.SetActive(true);
                    m_BadgeBorderFrame.gameObject.SetActive(false);
                    m_PlusIcon.gameObject.SetActive(true);
                    //m_SkillBadgeOpenBtn.gameObject.SetActive(false);
                    m_SkillDetailBg.gameObject.SetActive(false);
                    break;
                case SkillBadgeSlotState.EquipBadgeState:
                    m_BadgeIcon.gameObject.SetActive(true);
                    m_LcokIcon.gameObject.SetActive(false);
                    m_BorderBadge.gameObject.SetActive(false);
                    m_BadgeBorderFrame.gameObject.SetActive(true);
                    m_PlusIcon.gameObject.SetActive(false);
                    //m_SkillBadgeOpenBtn.gameObject.SetActive(false);
                    m_SkillDetailBg.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void OnClickItemSelf()
        {
            switch (m_SkillBadgeSlotState)
            {
                case SkillBadgeSlotState.LockedState:
                    if (m_IsOpenByMoneyOnly)
                    {
                        UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_ONLY_DIAMONDS_OPEN"));
                    }
                    else
                    {
                        m_SkillBadgeUnopenedTipsSubForm.gameObject.SetActive(true);
                        m_SkillBadgeUnopenedTipsSubForm.OnRefreshData(m_HeroType, m_SkillIndex, m_DirectUnlockMoneyCost, (int)SkillBadgeSlotCategory.Generic, m_GenericBadgeSlotIndex, m_HeroStarLevelCondition, m_HeroStarUpTextColor);
                    }
                    break;
                case SkillBadgeSlotState.EnableState:
                    var skillBadgeReplaceSubForm = m_ShowSkillBadgeReplaceSubForm();
                    skillBadgeReplaceSubForm.RefreshData(m_HeroType, m_SkillIndex, m_SkillGroupId, m_SkillBadgeSlotCategory, m_GenericBadgeSlotIndex, m_SkillElementId, m_GenericSkillBadgeSlotColor);
                    break;
                case SkillBadgeSlotState.EquipBadgeState:
                    m_SkillBadgeChoiceSubForm.gameObject.SetActive(true);
                    m_SkillBadgeChoiceSubForm.RefreshData(m_HeroType, m_SkillIndex, m_SkillGroupId, m_SkillBadgeId, m_SkillBadgeSlotCategory, m_GenericBadgeSlotIndex, m_ShowSkillBadgeReplaceSubForm, m_SkillBadgeStageAdvanceSubForm, m_SkillElementId, m_GenericSkillBadgeSlotColor);
                    break;
                default:
                    break;
            }
        }
    }
}