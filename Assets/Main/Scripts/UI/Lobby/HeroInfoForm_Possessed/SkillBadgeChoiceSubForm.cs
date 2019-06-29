using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;

namespace Genesis.GameClient
{
    public class SkillBadgeChoiceSubForm : MonoBehaviour
    {
        private GameFrameworkFunc<SkillBadgeReplaceSubForm> m_ShowSkillBadgeReplaceSubForm;
        private GameFrameworkFunc<SkillBadgeStageAdvanceSubForm> m_SkillBadgeStageAdvanceSubForm;
        private int m_HeroType = 0;
        private int m_SkillIndex = 0;
        private int m_SkillGroupId = 0;
        private int m_SkillBadgeId = 0;
        private int m_SkillBadgeSlotCategory = 0;
        private int m_GenericBadgeSlotIndex = 0;
        private int m_SkillElementId = 0;
        private int m_GenericSkillBadgeSlotColor = 0;
        private int m_CLParameter = 0;

        public void RefreshData(int heroType, int skillIndex, int skillGroupId, int skillBadgeId, int skillBadgeSlotCategory, int genericBadgeSlotIndex, GameFrameworkFunc<SkillBadgeReplaceSubForm> showSkillBadgeReplaceSubForm, GameFrameworkFunc<SkillBadgeStageAdvanceSubForm> skillBadgeStageAdvanceSubForm, int skillElementId = -1, int genericSkillBadgeSlotColor = 0)
        {
            m_HeroType = heroType;
            m_SkillIndex = skillIndex;
            m_SkillGroupId = skillGroupId;
            m_SkillBadgeId = skillBadgeId;
            m_SkillBadgeSlotCategory = skillBadgeSlotCategory;
            m_GenericBadgeSlotIndex = genericBadgeSlotIndex;
            m_SkillElementId = skillElementId;
            m_GenericSkillBadgeSlotColor = genericSkillBadgeSlotColor;
            m_ShowSkillBadgeReplaceSubForm = showSkillBadgeReplaceSubForm;
            m_SkillBadgeStageAdvanceSubForm = skillBadgeStageAdvanceSubForm;
        }

        public void OnClickReplaceBtn()
        {
            if (m_SkillBadgeSlotCategory == (int)SkillBadgeSlotCategory.Specific)
            {
                var skillBadgeReplaceSubForm = m_ShowSkillBadgeReplaceSubForm();
                skillBadgeReplaceSubForm.RefreshData(m_HeroType, m_SkillIndex, m_SkillGroupId, m_SkillBadgeSlotCategory, m_CLParameter);
            }
            else
            {
                var skillBadgeReplaceSubForm = m_ShowSkillBadgeReplaceSubForm();
                skillBadgeReplaceSubForm.RefreshData(m_HeroType, m_SkillIndex, m_SkillGroupId, m_SkillBadgeSlotCategory, m_GenericBadgeSlotIndex, m_SkillElementId, m_GenericSkillBadgeSlotColor);
            }
            OnClickScreenBack();
        }

        public void OnClickUndressBtn()
        {
            if (m_SkillBadgeSlotCategory == (int)SkillBadgeSlotCategory.Specific)
            {
                GameEntry.LobbyLogic.RemoveSkillBadge(m_HeroType, m_SkillIndex, m_SkillBadgeSlotCategory, m_CLParameter);
            }
            else
            {
                GameEntry.LobbyLogic.RemoveSkillBadge(m_HeroType, m_SkillIndex, m_SkillBadgeSlotCategory, m_GenericBadgeSlotIndex);
            }
            OnClickScreenBack();
        }

        public void OnClickBadgePiecesBtn()
        {
            var drBadge = GeneralItemUtility.GetSkillBadgeDataRow(m_SkillBadgeId);
            if (drBadge.LevelUpBadgeId == -1)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_BADGE_LEVEL_MAX"));
                OnClickScreenBack();
            }
            else
            {
                var skillBadgeStageAdvanceSubForm = m_SkillBadgeStageAdvanceSubForm();
                skillBadgeStageAdvanceSubForm.RefreshData(m_SkillBadgeId, true, m_HeroType, m_SkillIndex, m_SkillBadgeSlotCategory, m_GenericBadgeSlotIndex);
                OnClickScreenBack();
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