using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroInfoForm_Unpossessed
    {
        [SerializeField]
        private HeroInfoSkillInfoSubForm m_SkillInfo = null;

        [SerializeField]
        private UISprite m_SwitchSkillIcon = null;

        [SerializeField]
        private UISprite m_EvadeSkillIcon = null;

        [SerializeField]
        private List<UISprite> m_SkillIcons = new List<UISprite>();

        [SerializeField]
        private UISprite[] m_SkillCornerIcons = null;

        [SerializeField]
        private UISprite m_SwitchSkillCornerIcon = null;

        [SerializeField]
        private UIButton[] m_SkillButtons = null;

        [SerializeField]
        private GameObject[] m_SkillSelected = null;

        private HeroInfoSkillInfoSubForm m_UsingSkillInfo = null;

        private void RefreshSkillData()
        {
            ClearSkillState();

            if (m_UsingSkillInfo == null)
            {
                m_UsingSkillInfo = CreateSubForm<HeroInfoSkillInfoSubForm>("SkillInfo", gameObject, m_SkillInfo.gameObject, false);
            }

            m_UsingSkillInfo.InitSkillInfo();

            int skillGroupCountExceptJink = Constant.SkillGroupCount - 1;
            int i = 0;
            for (; i < skillGroupCountExceptJink; i++)
            {
                if (HeroData.SkillGroupIds[i] > 0)
                {
                    SetSkillIcon(HeroData.SkillGroupIds[i], m_SkillIcons[i], m_SkillCornerIcons[i], i, i);
                }
            }

            SetSkillIcon(HeroData.SwitchSkillGroupId, m_SwitchSkillIcon, m_SwitchSkillCornerIcon, Constant.SwitchSkillIndex, i++);
            SetSkillIcon(HeroData.SkillGroupIds[skillGroupCountExceptJink], m_EvadeSkillIcon, null, Constant.DodgeSkillIndex, i);
        }

        private void ShowSubSkillForm()
        {
            m_HeroRotationRegion.gameObject.SetActive(false);
            OpenSubForm(m_UsingSkillInfo);
        }

        private void SetSkillIcon(int skillGroupId, UISprite Icon, UISprite skillCornerIcon, int skillIndex, int index)
        {
            var dtSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();
            DRSkillGroup drSkillGroup = dtSkillGroup.GetDataRow(skillGroupId);
            if (drSkillGroup == null)
            {
                Log.Warning("Cannot find hero skill group '{0}'.", skillGroupId);
                return;
            }

            if (skillCornerIcon != null)
            {
                skillCornerIcon.gameObject.SetActive(drSkillGroup.SkillCategory > 0);
                if (drSkillGroup.SkillCategory == (int)SkillCategory.ContinualTapSkill)
                {
                    skillCornerIcon.spriteName = SkillCategory.ContinualTapSkill.ToString();
                }
                else if (drSkillGroup.SkillCategory == (int)SkillCategory.ChargeSkill)
                {
                    skillCornerIcon.spriteName = SkillCategory.ChargeSkill.ToString();
                }
                else
                {
                    skillCornerIcon.spriteName = SkillCategory.SwitchSkill.ToString();
                }
            }

            var dtSkill = GameEntry.DataTable.GetDataTable<DRSkill>();
            var skillId = drSkillGroup.SkillId;
            DRSkill drSkill = dtSkill.GetDataRow(skillId);
            if (drSkill == null)
            {
                Log.Warning("Cannot find hero skill by id '{0}'.", skillId.ToString());
                return;
            }

            Icon.LoadAsync(drSkill.IconId);

            var dtIcon = GameEntry.DataTable.GetDataTable<DRIcon>();
            DRIcon drIcon = dtIcon.GetDataRow(drSkill.IconId);
            if (drIcon == null)
            {
                Log.Warning("Icon ID '{0}' not found.", drSkill.IconId);
                return;
            }

            m_SkillButtons[index].normalSprite = drIcon.SpriteName;
        }

        public void OnClickComboSkill()
        {
            //SetSkillSelected(0);
            //ShowSubSkillForm();
            //m_UsingSkillInfo.ShowSkillInfo(HeroData, 0, 1);
        }

        public void OnClickSkill(int index)
        {
            //SetSkillSelected(index);
            //ShowSubSkillForm();
            //m_UsingSkillInfo.ShowSkillInfo(HeroData, index, 2);
        }

        public void OnClickSwitchSkill()
        {
            //SetSkillSelected(4);
            //ShowSubSkillForm();
            //m_UsingSkillInfo.ShowSkillInfo(HeroData, Constant.SwitchSkillIndex, 3);
        }

        public void OnClickEvadeSkill()
        {
            //SetSkillSelected(5);
            //ShowSubSkillForm();
            //m_UsingSkillInfo.ShowSkillInfo(HeroData, 4, 5);
        }

        public void SetSkillSelected(int index)
        {
            for (int i = 0; i < m_SkillSelected.Length; i++)
            {
                m_SkillSelected[i].SetActive(index == i);
            }
        }

        private void ClearSkillState()
        {
            for (int i = 0; i < m_SkillSelected.Length; i++)
            {
                m_SkillSelected[i].SetActive(false);
            }
        }

        public void OnClickWholeScreenButton()
        {
            ClearSkillState();
            m_HeroRotationRegion.gameObject.SetActive(true);
            if (m_UsingSkillInfo != null && m_UsingSkillInfo.IsAvailable)
            {
                CloseSubForm(m_UsingSkillInfo);
            }
        }
    }
}
