using UnityEngine;
using System.Collections;
using GameFramework;


namespace Genesis.GameClient
{
    public class SkillStrengthenItem2 : MonoBehaviour
    {
        //[SerializeField]
        //private UILabel m_SkillLevelNumIsShow = null;

        //[SerializeField]
        //private UILabel m_SkillLevelNum = null;

        [SerializeField]
        private UISprite m_SkillIcon = null;

        [SerializeField]
        private UISprite m_SkillLockIcon = null;

        //[SerializeField]
        //private UILabel m_LockConditionText = null;

        //[SerializeField]
        //private UISprite[] m_BadgeSprites = null;

        [SerializeField]
        private UISprite m_SkillLockMask = null;

        [SerializeField]
        private UISprite m_SkillCategoryIcon = null;

        [SerializeField]
        private bool m_IsUnlock = true;

        [SerializeField]
        private Color m_IsUnlockColor;

        public bool IsLocked { get { return m_IsUnlock; } }

        public void RefreshData(int heroType, int skillIndex)
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
            var drSkill = GameEntry.DataTable.GetDataTable<DRSkill>();
            int skillId = skillGroup.SkillId;
            DRSkill skill = drSkill.GetDataRow(skillId);
            if (skill == null)
            {
                Log.Warning("cannot find skill by '{0}'.", skillId);
                return;
            }
            m_SkillIcon.LoadAsync(skill.IconId);
            //int skillLevelNum = heroData.GetSkillLevel(skillIndex);
            //var levelText = Constant.HeroSkillCanLevelUp[skillIndex] ? GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", skillLevelNum) : string.Empty;
            //m_SkillLevelNum.text = m_SkillLevelNumIsShow.text = levelText;
            //m_LockConditionText.text = GameEntry.Localization.GetString("UI_TEXT_SKILL_LEVEL_UNLOCK_CONDITION", skillGroup.SkillUnlockLevel);
            m_IsUnlock = heroData.Level >= skillGroup.SkillUnlockLevel ? true : false;

            if (m_SkillCategoryIcon != null)
            {
                m_SkillCategoryIcon.gameObject.SetActive(skillGroup.SkillCategory > (int)SkillCategory.Undefined);
                if (skillGroup.SkillCategory == (int)SkillCategory.ContinualTapSkill)
                {
                    m_SkillCategoryIcon.spriteName = SkillCategory.ContinualTapSkill.ToString();
                }
                else if (skillGroup.SkillCategory == (int)SkillCategory.ChargeSkill)
                {
                    m_SkillCategoryIcon.spriteName = SkillCategory.ChargeSkill.ToString();
                }
                else
                {
                    m_SkillCategoryIcon.spriteName = SkillCategory.SwitchSkill.ToString();
                }
            }

            OnSkillStateChange();
            var skillBadge = (heroData as LobbyHeroData).GetSkillBadge(skillIndex);
            if (skillBadge == null)
            {
                Log.Warning("cannot find skillBadge by '{0}'.", skillIndex);
                return;
            }
            //if (skillBadge.SpecificBadge.BadgeId > 0)
            //{
            //    m_BadgeSprites[0].gameObject.SetActive(true);
            //}
            //else
            //{
            //    m_BadgeSprites[0].gameObject.SetActive(false);
            //}
            //for (int i = 0; i < skillBadge.GenericBadges.Count; i++)
            //{
            //    if (skillBadge.GenericBadges[i].BadgeId > 0)
            //    {
            //        m_BadgeSprites[i + 1].gameObject.SetActive(true);
            //    }
            //    else
            //    {
            //        m_BadgeSprites[i + 1].gameObject.SetActive(false);
            //    }
            //}
        }

        private void OnSkillStateChange()
        {
            if (m_IsUnlock)
            {
                //m_SkillLevelNum.gameObject.SetActive(true);
                //m_SkillLevelNumIsShow.gameObject.SetActive(true);
                //m_LockConditionText.gameObject.SetActive(false);
                m_SkillLockIcon.gameObject.SetActive(false);
                m_SkillLockMask.gameObject.SetActive(false);
                if (m_SkillCategoryIcon != null)
                {
                    m_SkillCategoryIcon.color = Color.white;
                }
            }
            else
            {
                //m_SkillLevelNum.gameObject.SetActive(false);
                //m_SkillLevelNumIsShow.gameObject.SetActive(false);
                //m_LockConditionText.gameObject.SetActive(true);
                m_SkillLockIcon.gameObject.SetActive(true);
                m_SkillLockMask.gameObject.SetActive(true);
                if (m_SkillCategoryIcon != null)
                {
                    m_SkillCategoryIcon.color = m_IsUnlockColor;
                }
            }
        }
    }
}