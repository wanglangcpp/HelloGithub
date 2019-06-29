using UnityEngine;
using System.Collections;
using GameFramework;

namespace Genesis.GameClient
{
    public class BadgeBagInfoSubForm : MonoBehaviour
    {
        [SerializeField]
        private GeneralItemView m_BadgeItemView = null;

        [SerializeField]
        private UILabel m_BadgeNameText = null;

        [SerializeField]
        private UILabel m_HaveNumText = null;

        [SerializeField]
        private UILabel m_BadgeNumText = null;

        [SerializeField]
        private UILabel m_BadgeDesText = null;

        [SerializeField]
        private UILabel m_BadgeEffectDesc = null;

        [SerializeField]
        private UIButton m_StageAdvanceBtn = null;

        private GameFrameworkFunc<SkillBadgeStageAdvanceSubForm> m_ShowSkillBadgeStageAdvanceSubForm;

        private int m_BadgeId = 0;

        public void RefreshData(int badgeId, int count, GameFrameworkFunc<SkillBadgeStageAdvanceSubForm> showSkillBadgeStageAdvanceSubForm)
        {
            m_BadgeId = badgeId;
            m_ShowSkillBadgeStageAdvanceSubForm = showSkillBadgeStageAdvanceSubForm;
            m_BadgeItemView.InitSkillBadge(badgeId);
            m_BadgeNumText.text = count.ToString();
            m_BadgeNameText.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(badgeId));
            m_BadgeDesText.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemDescription(badgeId));
            m_HaveNumText.text = GameEntry.Localization.GetString("UI_TEXT_BADGE_NUMBER_HAD");

            var drBadge = GeneralItemUtility.GetSkillBadgeDataRow(badgeId);
            string badgeEffectDesc = GameEntry.Localization.GetString(drBadge.EffectDesc, drBadge.LevelUpCount);
            m_BadgeEffectDesc.text = GameEntry.StringReplacement.GetString(badgeEffectDesc);
            m_StageAdvanceBtn.isEnabled = drBadge.LevelUpBadgeId != -1;
        }

        public void OnClickBadgeStageAdvance()
        {
            var showSkillBadgeStageAdvanceSubForm = m_ShowSkillBadgeStageAdvanceSubForm();
            showSkillBadgeStageAdvanceSubForm.RefreshData(m_BadgeId, false);
        }
    }
}