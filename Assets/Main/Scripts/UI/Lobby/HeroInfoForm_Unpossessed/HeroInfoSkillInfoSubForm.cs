using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroInfoSkillInfoSubForm : NGUISubForm
    {
        [Serializable]
        private class SkillEffectItem
        {
            public UILabel m_SkillEffectDescription = null;

            public void SetActive(bool isVisible)
            {
                m_SkillEffectDescription.transform.parent.gameObject.SetActive(isVisible);
            }
        }

        [SerializeField]
        private UISprite m_SkillIcon = null;

        [SerializeField]
        private UILabel m_SkillName = null;

        [SerializeField]
        private UILabel m_SkillLevel = null;

        [SerializeField]
        private UILabel m_SkillType = null;

        [SerializeField]
        private UILabel m_SkillDescription = null;

        [SerializeField]
        private UITable m_SkillEffectTable = null;

        [SerializeField]
        private SkillEffectItem[] m_SkillEffects = null;

        [SerializeField]
        private SkillEffectItem[] m_NextSkillEffects = null;

        [SerializeField]
        private UIButton m_LevelUpButton = null;

        [SerializeField]
        private GameObject m_LevelUpPanel = null;

        [SerializeField]
        private GameObject m_LevelUpTitle = null;

        private BaseLobbyHeroData m_HeroData = null;
        private int m_SkillIndex = 0;
        private const string NormalDescriptionColor = "ACC0C7FF";

        internal bool IsShowing
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        protected internal override void OnOpen()
        {
            base.OnOpen();
            GameEntry.Event.Subscribe(EventId.LobbyHeroDataChanged, OnLobbyHeroDataChanged);
        }

        protected internal override void OnClose()
        {
            GameEntry.Event.Unsubscribe(EventId.LobbyHeroDataChanged, OnLobbyHeroDataChanged);
            base.OnClose();
        }

        public void InitSkillInfo()
        {
            gameObject.SetActive(false);
        }

        public void ShowSkillInfo(BaseLobbyHeroData heroData, int skillIndex, int skillType)
        {
            m_HeroData = heroData;
            m_SkillIndex = skillIndex;
            m_SkillType.text = GameEntry.Localization.GetString("SKILL_TYPE_" + skillType.ToString());

            RefreshPanel();
        }

        private void RefreshPanel()
        {
            if (m_HeroData == null)
            {
                return;
            }

            int heroType = m_HeroData.Type;
            int skillGroupId = m_SkillIndex == Constant.SwitchSkillIndex ? m_HeroData.SwitchSkillGroupId : m_HeroData.SkillGroupIds[m_SkillIndex];
            int skillLevel = m_HeroData.GetSkillLevel(m_SkillIndex);

            var dtSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();
            DRSkillGroup drSkillGroup = dtSkillGroup.GetDataRow(skillGroupId);
            if (drSkillGroup == null)
            {
                Log.Error("Cannot find hero skill group '{0}'.", skillGroupId.ToString());
                return;
            }

            var dtSkill = GameEntry.DataTable.GetDataTable<DRSkill>();
            var skillId = drSkillGroup.SkillId;
            DRSkill drSkill = dtSkill.GetDataRow(skillId);
            if (drSkill == null)
            {
                Log.Error("Cannot find hero skill by id '{0}'.", skillId.ToString());
                return;
            }

            m_SkillIcon.LoadAsync(drSkill.IconId);

            m_SkillName.text = GameEntry.Localization.GetString(drSkill.Name);
            m_SkillLevel.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", skillLevel.ToString());
            m_SkillDescription.text = GameEntry.Localization.GetString(drSkill.Description);

            string str = string.Empty;
            //for (int i = 0; i < drSkill.EffectDescription.Length; i++)
            //{
            //    str += GameEntry.StringReplacement.GetString(GameEntry.Localization.GetString(drSkill.EffectDescription[i]));
            //}
            str = ColorUtility.AddStringColorToString(NormalDescriptionColor, str);
            m_SkillEffects[0].m_SkillEffectDescription.text = str;

            if (!(m_HeroData is UnpossessedLobbyHeroData) && RefreshSkillLevelUp(skillId, heroType, m_SkillIndex, skillLevel))
            {
                m_LevelUpPanel.SetActive(true);
                m_LevelUpTitle.SetActive(true);

                var nextSkillId = drSkillGroup.SkillId;
                DRSkill drNextSkill = dtSkill.GetDataRow(nextSkillId);
                if (drNextSkill == null)
                {
                    Log.Error("Cannot find hero next skill by id '{0}'.", nextSkillId.ToString());
                    return;
                }

                str = string.Empty;
                //for (int i = 0; i < drSkill.EffectDescription.Length; i++)
                //{
                //    str += GameEntry.StringReplacement.GetString(GameEntry.Localization.GetString(drNextSkill.EffectDescription[i]));
                //}
                str = ColorUtility.AddStringColorToString(NormalDescriptionColor, str);
                m_NextSkillEffects[0].m_SkillEffectDescription.text = str;
            }
            else
            {
                m_LevelUpPanel.SetActive(false);
                m_LevelUpTitle.SetActive(false);

                for (int i = 0; i < m_NextSkillEffects.Length; i++)
                {
                    m_NextSkillEffects[i].SetActive(false);
                }
            }

            m_SkillEffectTable.Reposition();
            m_SkillEffectTable.repositionNow = true;

            m_SkillEffectTable.transform.parent.GetComponent<UIScrollView>().ResetPosition();
        }

        public void OnClickLevelUp()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SkillStrengthenForm, new SkillStrengthenDisplayData { SelectHeroType = m_HeroData.Type, SkillIndex = m_SkillIndex, });
        }

        private bool RefreshSkillLevelUp(int skillId, int heroType, int heroIndex, int skillLevel)
        {
            IDataTable<DRSkillLevelUp> dt = GameEntry.DataTable.GetDataTable<DRSkillLevelUp>();
            DRSkillLevelUp dr = dt.GetDataRow(skillId);
            if (dr == null)
            {
                Log.Warning("Can not upgrade skill '{0}' without config.", skillId.ToString());
                return false;
            }

            //if (dr.HeroType != heroType || dr.SkillIndex != heroIndex || dr.SkillLevel != skillLevel)
            //{
            //    Log.Warning("Can not upgrade skill '{0}' which config is wrong.", skillId.ToString());
            //    return false;
            //}

            bool canLevelUp = true;

            //var hero = GameEntry.Data.LobbyHeros.GetData(heroType);
            //canLevelUp &= (hero != null && hero.Level >= dr.RequiresHeroLevel && hero.StarLevel >= dr.RequiresHeroStarLevel);

            m_LevelUpButton.isEnabled = canLevelUp;

            return true;
        }

        private void OnLobbyHeroDataChanged(object sender, GameEventArgs e)
        {
            RefreshPanel();
        }
    }
}
