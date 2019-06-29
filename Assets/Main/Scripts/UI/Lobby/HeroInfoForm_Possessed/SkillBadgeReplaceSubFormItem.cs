using UnityEngine;
using System.Collections;
using GameFramework;

namespace Genesis.GameClient
{
    public class SkillBadgeReplaceSubFormItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_BadgeEffecText = null;

        [SerializeField]
        private GeneralItemView m_GeneralItemView = null;

        private GameFrameworkAction<int> m_OnClickItem;

        private int m_SkillBadgeId = 0;

        public void RefreshData(int skillBadgeSlotCategory, int skillBadgeId, int badgeCount, GameFrameworkAction<int> skillBadgeReplaceSubForm)
        {
            m_OnClickItem = skillBadgeReplaceSubForm;
            m_SkillBadgeId = skillBadgeId;

            m_GeneralItemView.InitSkillBadge(skillBadgeId, badgeCount);
            m_BadgeEffecText.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemDescription(skillBadgeId));
        }

        public void OnClickSelf()
        {
            m_OnClickItem(m_SkillBadgeId);
        }
    }
}