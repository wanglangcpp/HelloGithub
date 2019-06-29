using UnityEngine;
using System.Collections;
using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class SpecificSkillBadgeSlotItem : MonoBehaviour
    {
        /// <summary>
        /// 通讯参数（通用徽章索引）。
        /// </summary>
        public int m_CLParameter = 0;

        /// <summary>
        /// 技能徽章位种类。
        /// </summary>
        [SerializeField]
        private int m_SkillBadgeSlotCategory;

        public int SkillBadgeSlotCategory
        {
            get { return m_SkillBadgeSlotCategory; }
            set { m_SkillBadgeSlotCategory = value; }
        }

        [SerializeField]
        private SkillBadgeSlotState m_SkillBadgeSlotState = SkillBadgeSlotState.LockedState;

        public SkillBadgeSlotState SkillBadgeSlotState
        {
            get { return m_SkillBadgeSlotState; }
            set { m_SkillBadgeSlotState = value; }
        }

        [SerializeField]
        private UISprite m_BadgeIcon = null;

        [SerializeField]
        private UISprite m_LcokIcon = null;

        [SerializeField]
        private UISprite m_SkillDetailBg = null;

        [SerializeField]
        private UILabel m_BadgeDesText = null;

        [SerializeField]
        private UIButton m_SkillBadgeOpenBtn = null;

        [SerializeField]
        private UILabel m_CostNumText = null;

        [SerializeField]
        private UISprite m_BorderBadge = null;

        [SerializeField]
        private UISprite m_BadgeBorderFrame = null;

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

        private int m_HeroType = 0;

        private int m_SkillIndex = 0;

        private int m_SkillGroupId = 0;

        private int m_SkillBadgeId = 0;

        //private int m_SkillLevelCondition = 0;

        private int m_HeroStarLevelCondition = 0;

        //private Color m_SkillLevelTextColor;

        private Color m_HeroStarUpTextColor;

        public int m_DirectUnlockMoneyCost = 0;

        private GameFrameworkFunc<SkillBadgeReplaceSubForm> m_ShowSkillBadgeReplaceSubForm;

        private GameFrameworkFunc<SkillBadgeStageAdvanceSubForm> m_SkillBadgeStageAdvanceSubForm;

        public void RefreshData(int heroType, int skillIndex, int skillGroupId, int skillBadgeId, GameFrameworkFunc<SkillBadgeReplaceSubForm> showSkillBadgeReplaceSubForm, GameFrameworkFunc<SkillBadgeStageAdvanceSubForm> skillBadgeStageAdvanceSubForm)
        {
            m_HeroType = heroType;
            m_SkillIndex = skillIndex;
            m_SkillGroupId = skillGroupId;
            m_SkillBadgeId = skillBadgeId;
            m_ShowSkillBadgeReplaceSubForm = showSkillBadgeReplaceSubForm;
            m_SkillBadgeStageAdvanceSubForm = skillBadgeStageAdvanceSubForm;
            if (m_SkillBadgeSlotState == SkillBadgeSlotState.LockedState)
            {
                SkillBadgeSlotLockedData(heroType, skillIndex);
            }
            if (m_SkillBadgeSlotState == SkillBadgeSlotState.EquipBadgeState)
            {
                SkillBadgeSlotEquipState(skillBadgeId);
            }
            OnSkillBadgeSlotStateChange();
        }

        /// <summary>
        /// 当技能徽章位是锁定状态时计算的数据
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
            int skillGroupId = 0;
            int skillGroupCountExceptJink = Constant.SkillGroupCount;
            if (skillIndex <= skillGroupCountExceptJink)
            {
                skillGroupId = heroData.SkillGroupIds[skillIndex];
            }
            else if (skillIndex == Constant.SwitchSkillIndex)
            {
                skillGroupId = heroData.SwitchSkillGroupId;
            }
            var drSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();
            DRSkillGroup skillGroup = drSkillGroup.GetDataRow(skillGroupId);
            if (skillGroup == null)
            {
                Log.Warning("cannot find skill group by '{0}'.", skillGroupId);
                return;
            }
            var dtSkillBadgeSlotUnlockCondition = GameEntry.DataTable.GetDataTable<DRSkillBadgeSlotUnlockCondition>();
            DRSkillBadgeSlotUnlockCondition drSkillBadgeSlotUnlockCondition = dtSkillBadgeSlotUnlockCondition.GetDataRow(skillGroup.SpecificBadgeUnlockConditionId);
            if (drSkillBadgeSlotUnlockCondition == null)
            {
                Log.Warning("cannot find skillBadgeSlotUnlockCondition by '{0}'.", skillGroup.SpecificBadgeUnlockConditionId);
                return;
            }
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

        private void SkillBadgeSlotEquipState(int skillBadgeId)
        {
            if (skillBadgeId > 0)
            {
                var dtSpecificSkillBadge = GameEntry.DataTable.GetDataTable<DRSpecificSkillBadge>();
                var specificSkillBadge = dtSpecificSkillBadge.GetDataRow(skillBadgeId);
                m_BadgeIcon.LoadAsync(GeneralItemUtility.GetSkillBadgeIconId(skillBadgeId));
                m_BadgeBorderFrame.spriteName = GeneralItemUtility.GetSkillBadgeBgSpriteName(GameClient.SkillBadgeSlotCategory.Specific, specificSkillBadge.Level);
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
                    m_SkillBadgeOpenBtn.gameObject.SetActive(true);
                    m_BorderBadge.gameObject.SetActive(true);
                    m_BadgeBorderFrame.gameObject.SetActive(false);
                    m_PlusIcon.gameObject.SetActive(false);
                    m_SkillDetailBg.gameObject.SetActive(false);
                    m_CostNumText.text = m_DirectUnlockMoneyCost.ToString();
                    break;
                case SkillBadgeSlotState.EnableState:
                    m_BadgeIcon.gameObject.SetActive(false);
                    m_BorderBadge.gameObject.SetActive(true);
                    m_BadgeBorderFrame.gameObject.SetActive(false);
                    m_PlusIcon.gameObject.SetActive(true);
                    m_LcokIcon.gameObject.SetActive(false);
                    m_SkillBadgeOpenBtn.gameObject.SetActive(false);
                    m_SkillDetailBg.gameObject.SetActive(false);
                    break;
                case SkillBadgeSlotState.EquipBadgeState:
                    m_BadgeIcon.gameObject.SetActive(true);
                    m_LcokIcon.gameObject.SetActive(false);
                    m_PlusIcon.gameObject.SetActive(false);
                    m_BorderBadge.gameObject.SetActive(false);
                    m_SkillBadgeOpenBtn.gameObject.SetActive(false);
                    m_BadgeBorderFrame.gameObject.SetActive(true);
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
                    m_SkillBadgeUnopenedTipsSubForm.gameObject.SetActive(true);
                    m_SkillBadgeUnopenedTipsSubForm.OnRefreshData(m_HeroType, m_SkillIndex, m_DirectUnlockMoneyCost, SkillBadgeSlotCategory, m_CLParameter, m_HeroStarLevelCondition, m_HeroStarUpTextColor);
                    break;
                case SkillBadgeSlotState.EnableState:
                    var skillBadgeReplaceSubForm = m_ShowSkillBadgeReplaceSubForm();
                    skillBadgeReplaceSubForm.RefreshData(m_HeroType, m_SkillIndex, m_SkillGroupId, m_SkillBadgeSlotCategory, m_CLParameter);
                    break;
                case SkillBadgeSlotState.EquipBadgeState:
                    m_SkillBadgeChoiceSubForm.gameObject.SetActive(true);
                    m_SkillBadgeChoiceSubForm.RefreshData(m_HeroType, m_SkillIndex, m_SkillGroupId, m_SkillBadgeId, m_SkillBadgeSlotCategory, m_CLParameter, m_ShowSkillBadgeReplaceSubForm, m_SkillBadgeStageAdvanceSubForm);
                    break;
                default:
                    break;
            }
        }
    }
}