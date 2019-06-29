using UnityEngine;
using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class HeroInfoForm_Possessed
    {
        [SerializeField]
        private SkillStrengthenItem2[] m_SkillItems = null;

        [SerializeField]
        private GenericSkillBadgeSlotItem[] m_GenericBadgeSlotItems = null;

        [SerializeField]
        private SpecificSkillBadgeSlotItem m_SpecificSkillBadgeSlotItem = null;

        [SerializeField]
        private UILabel m_SkillName = null;

        //[SerializeField]
        //private UILabel m_SkillLevel = null;

        [SerializeField]
        private UILabel m_SkillDescription = null;

        [SerializeField]
        private UIButton m_SkillLevelUpBtn = null;

        [SerializeField]
        private UILabel m_SkillLevelUpBtnText = null;

        [SerializeField]
        private UILabel m_SkillLevelUpCostNum = null;

        [SerializeField]
        private UISprite m_CoinIcon = null;

        [SerializeField]
        private UILabel m_HeroMaxText = null;

        [SerializeField]
        private UILabel m_SkillTypeText = null;

        [SerializeField]
        private UILabel m_SkillCDText = null;

        [SerializeField]
        private GameObject m_SkillBadgeGrid = null;

        [SerializeField]
        private GameObject m_BadgeBagBtn = null;

        [SerializeField]
        private GameObject m_SkillLockMask = null;

        [SerializeField]
        private GameObject m_GsPointBg = null;

        [SerializeField]
        private GameObject m_HeroInteractor = null;

        [SerializeField]
        private UIToggle[] m_Toggles = null;

        [SerializeField]
        private SkillBadgeReplaceSubForm m_SkillBadgeReplaceSubForm = null;

        [SerializeField]
        private SkillBadgeStageAdvanceSubForm m_SkillBadgeStageAdvanceSubForm = null;

        [SerializeField]
        private GameObject m_SKillTitleBg = null;

        [SerializeField]
        private UILabel m_UnlockText = null;

        private SkillBadgeReplaceSubForm m_UsingSkillBadgeReplace = null;

        private SkillBadgeStageAdvanceSubForm m_UsingSkillBadgeStageAdvance = null;

        private DRSkillGroup m_SkillGroup = null;

        private int m_SelectSkillIndex = 0;

        private int m_SkillGroupId = 0;

        private bool m_SkillCanUpgrade = false;

        public void ShowBadgeEffect()
        {
            for (int i = 0; i < m_GenericBadgeSlotItems.Length; i++)
            {
                string effectName = string.Format("EffectSkillBadgeCommon{0}", i + 1);
                if (m_GenericBadgeSlotItems[i].SkillBadgeSlotState == SkillBadgeSlotState.EnableState)
                {
                    if (m_EffectsController.EffectIsShowing(effectName) == false)
                        m_EffectsController.ShowEffect(effectName);
                }
                else
                {
                    m_EffectsController.DestroyEffect(effectName);
                }
            }
        }

        private void ShowEquipBadgeEffect(string effectName)
        {
            m_EffectsController.ShowEffect(effectName);
        }

        public bool CloseSkill()
        {
            m_HeroInteractor.SetActive(true);
            m_GsPointBg.SetActive(true);
            return true;
        }

        public bool RefreshSkill()
        {
            for (int i = 0; i < m_Toggles.Length - 1; i++)
            {
                m_Toggles[i].value = m_SelectSkillIndex == i;
            }
            m_Toggles[m_Toggles.Length - 1].value = m_SelectSkillIndex == Constant.SwitchSkillIndex;

            if (m_UsingSkillBadgeStageAdvance == null)
            {
                m_UsingSkillBadgeStageAdvance = CreateSubForm<SkillBadgeStageAdvanceSubForm>("SkillBadgeStageAdvanceSubForm", gameObject, m_SkillBadgeStageAdvanceSubForm.gameObject, false);
            }

            m_UsingSkillBadgeStageAdvance.InitSkillBadgeStageAdvance();

            OnRefreshData();
            m_HeroInteractor.SetActive(false);
            m_GsPointBg.SetActive(false);
            bool ret = true;
            return ret;
        }

        private void OnRefreshData()
        {
            if (m_UsingSkillBadgeReplace == null)
            {
                m_UsingSkillBadgeReplace = CreateSubForm<SkillBadgeReplaceSubForm>("SkillBadgeReplaceSubForm", gameObject, m_SkillBadgeReplaceSubForm.gameObject, false);
            }

            m_UsingSkillBadgeReplace.InitSkillBadgeReplace();

            for (int i = 0; i < Constant.SkillGroupCount; i++)
            {
                m_SkillItems[i].RefreshData(HeroData.Type, i);
            }
            m_SkillItems[Constant.SwitchSkillIndex].RefreshData(HeroData.Type, Constant.SwitchSkillIndex);
            RefreshSkillData(m_SelectSkillIndex);
        }

        private void RefreshSkillData(int skillIndex)
        {
            m_SkillGroupId = skillIndex == Constant.SwitchSkillIndex ? HeroData.SwitchSkillGroupId : HeroData.SkillGroupIds[skillIndex];
            var drSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();
            m_SkillGroup = drSkillGroup.GetDataRow(m_SkillGroupId);
            if (m_SkillGroup == null)
            {
                Log.Warning("cannot find skill group by '{0}'.", m_SkillGroupId);
                return;
            }
            var drSkill = GameEntry.DataTable.GetDataTable<DRSkill>();
            int skillId = m_SkillGroup.SkillId;
            DRSkill skill = drSkill.GetDataRow(skillId);
            if (skill == null)
            {
                Log.Warning("cannot find skill by '{0}'.", skillId);
                return;
            }
            m_SkillName.text = GameEntry.Localization.GetString(skill.Name);
            int skillLevel = HeroData.GetSkillLevel(skillIndex);
            //m_SkillLevel.text = Constant.HeroSkillCanLevelUp[skillIndex] ? GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", skillLevel) : string.Empty;

            string SkillUpgradeValueDescription = m_SkillGroup.SkillUpgradeValueDescription;
            if (skill.ParameterCount == 1)
            {
                m_SkillDescription.text = GameEntry.StringReplacement.GetString(GameEntry.Localization.GetString(SkillUpgradeValueDescription, skill.AttrBaseValues[0] + (skillLevel - 1) * skill.AttrDeltasPerLevel[0]));
            }
            else if (skill.ParameterCount == 2)
            {
                m_SkillDescription.text = GameEntry.StringReplacement.GetString(GameEntry.Localization.GetString(SkillUpgradeValueDescription, skill.AttrBaseValues[0] + (skillLevel - 1) * skill.AttrDeltasPerLevel[0], skill.AttrBaseValues[1] + (skillLevel - 1) * skill.AttrDeltasPerLevel[1]));
            }

            m_SkillLevelUpBtnText.text = GameEntry.Localization.GetString("UI_BUTTON_GEARSTRENGTHEN");
            var drSkillLevelUp = GameEntry.DataTable.GetDataTable<DRSkillLevelUp>();
            DRSkillLevelUp skillLevelUpCost = drSkillLevelUp.GetDataRow(skillLevel);
            if (skillLevelUpCost == null)
            {
                Log.Warning("cannot find skillLevelUpCost by '{0}'.", skillLevel);
                return;
            }
            var playerData = GameEntry.Data.Player;
            int skillMaxLevel = GameEntry.DataTable.GetDataTable<DRPlayer>().MaxIdDataRow.Id;
            if (playerData.Coin < skillLevelUpCost.CostCoinCount || skillLevel == skillMaxLevel)
            {
                m_SkillLevelUpBtn.isEnabled = false;
                m_SkillLevelUpCostNum.color = Color.red;
            }
            else
            {
                m_SkillLevelUpBtn.isEnabled = true;
                m_SkillLevelUpCostNum.color = Color.white;
            }

            //m_SkillLevelUpBtn.gameObject.SetActive(m_SkillItems[skillIndex].IsLocked && Constant.HeroSkillCanLevelUp[skillIndex]);
            m_SkillBadgeGrid.SetActive(m_SkillItems[skillIndex].IsLocked);
            m_BadgeBagBtn.SetActive(m_SkillItems[skillIndex].IsLocked);
            m_SkillLockMask.SetActive(!m_SkillItems[skillIndex].IsLocked);
            m_SKillTitleBg.SetActive(m_SkillItems[skillIndex].IsLocked);
            m_UnlockText.gameObject.SetActive(!m_SkillItems[skillIndex].IsLocked);
            m_UnlockText.text = GameEntry.Localization.GetString("UI_TEXT_SKILL_UNLOCKED", m_SkillGroup.SkillUnlockLevel);
            m_SkillLevelUpCostNum.text = skillLevelUpCost.CostCoinCount.ToString();
            m_HeroMaxText.text = GameEntry.Localization.GetString("UI_TEXT_SKILL_LEVEL_EXP_MAX");
            m_SkillCDText.text = GameEntry.Localization.GetString("UI_TEXT_SKLII_CD_TIME", skill.CoolDownTime);
            m_SkillTypeText.text = GameEntry.Localization.GetString(m_SkillGroup.SkillType);
            m_SkillCanUpgrade = skillLevel < HeroData.Level ? true : false;
            if (skillLevel == skillMaxLevel)
            {
                m_SkillLevelUpBtnText.gameObject.SetActive(false);
                m_SkillLevelUpCostNum.gameObject.SetActive(false);
                m_CoinIcon.gameObject.SetActive(false);
                m_HeroMaxText.gameObject.SetActive(true);
            }
            else
            {
                m_SkillLevelUpBtnText.gameObject.SetActive(true);
                m_SkillLevelUpCostNum.gameObject.SetActive(true);
                m_CoinIcon.gameObject.SetActive(true);
                m_HeroMaxText.gameObject.SetActive(false);
            }
            OnRefreshBadgeSlotData(skillIndex);
        }

        private void OnRefreshBadgeSlotData(int skillIndex)
        {
            var skillBadge = (HeroData as LobbyHeroData).GetSkillBadge(skillIndex);
            if (skillBadge == null)
            {
                Log.Warning("cannot find skillBadge by '{0}'.", skillIndex);
                return;
            }

            for (int i = 0; i < m_GenericBadgeSlotItems.Length; i++)
            {
                m_GenericBadgeSlotItems[i].GenericSkillBadgeSlotColor = m_SkillGroup.GenericSkillBadgeSlotColors[i];
                m_GenericBadgeSlotItems[i].GenericBadgeUnlockContitionId = m_SkillGroup.GenericBadgeUnlockContitionIds[i];
                m_GenericBadgeSlotItems[i].m_SkillBadgeSlotCategory = (int)SkillBadgeSlotCategory.Generic;
                m_GenericBadgeSlotItems[i].m_GenericBadgeSlotIndex = i;
                if (m_GenericBadgeSlotItems[i].GenericSkillBadgeSlotColor < 0)
                {
                    m_GenericBadgeSlotItems[i].enabled = false;
                    m_GenericBadgeSlotItems[i].gameObject.SetActive(false);
                }
                else
                {
                    m_GenericBadgeSlotItems[i].gameObject.SetActive(true);
                }

                var tempState = m_GenericBadgeSlotItems[i].SkillBadgeSlotState;

                if (skillBadge.GenericBadges[i].BadgeId < 0)
                {
                    m_GenericBadgeSlotItems[i].SkillBadgeSlotState = SkillBadgeSlotState.LockedState;
                }
                else if (skillBadge.GenericBadges[i].BadgeId == 0)
                {
                    m_GenericBadgeSlotItems[i].SkillBadgeSlotState = SkillBadgeSlotState.EnableState;
                }
                else
                {
                    m_GenericBadgeSlotItems[i].SkillBadgeSlotState = SkillBadgeSlotState.EquipBadgeState;
                }

                if (tempState == SkillBadgeSlotState.EnableState && m_GenericBadgeSlotItems[i].SkillBadgeSlotState == SkillBadgeSlotState.EquipBadgeState)
                {
                    ShowEquipBadgeEffect(string.Format("EffectBadgeCommonBoom{0}", i + 1));
                }

                m_GenericBadgeSlotItems[i].RefreshData(HeroData.Type, skillIndex, m_SkillGroupId, skillBadge.GenericBadges[i].BadgeId, ShowSkillBadgeReplaceSubForm, ShowSkillBadgeStageAdvanceSubForm);
            }

            //var tempSpecificState = m_SpecificSkillBadgeSlotItem.SkillBadgeSlotState;
            m_SpecificSkillBadgeSlotItem.SkillBadgeSlotCategory = (int)SkillBadgeSlotCategory.Specific;

            if (skillBadge.SpecificBadge.BadgeId < 0)
            {
                m_SpecificSkillBadgeSlotItem.SkillBadgeSlotState = SkillBadgeSlotState.LockedState;
            }
            else if (skillBadge.SpecificBadge.BadgeId == 0)
            {
                m_SpecificSkillBadgeSlotItem.SkillBadgeSlotState = SkillBadgeSlotState.EnableState;
            }
            else
            {
                m_SpecificSkillBadgeSlotItem.SkillBadgeSlotState = SkillBadgeSlotState.EquipBadgeState;
            }

            //if (tempSpecificState == SkillBadgeSlotState.EnableState && m_SpecificSkillBadgeSlotItem.SkillBadgeSlotState == SkillBadgeSlotState.EquipBadgeState)
            //{
            //    ShowEquipBadgeEffect("EffectBadgeCommonBoom0");
            //}

            m_SpecificSkillBadgeSlotItem.RefreshData(HeroData.Type, skillIndex, m_SkillGroupId, skillBadge.SpecificBadge.BadgeId, ShowSkillBadgeReplaceSubForm, ShowSkillBadgeStageAdvanceSubForm);

            //for (int i = 0; i < m_GenericBadgeSlotItems.Length; i++)
            //{
            //    m_GenericBadgeSlotItems[i].m_SkillBadgeOpenBtn.gameObject.SetActive(false);
            //}
            //for (int i = 0; i < m_GenericBadgeSlotItems.Length; i++)
            //{
            //    if (m_GenericBadgeSlotItems[i].SkillBadgeSlotState == SkillBadgeSlotState.LockedState)
            //    {
            //        m_GenericBadgeSlotItems[i].m_SkillBadgeOpenBtn.gameObject.SetActive(true);
            //        break;
            //    }
            //}
        }

        public void OnClickSkillLevelUpButton()
        {
            if (m_SkillCanUpgrade)
            {
                GameEntry.LobbyLogic.HeroSkillLevelUp(HeroData.Type, m_SelectSkillIndex);
            }
            else
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_SKILL_LEVEL_LESSTHAN_PLAYER_LEVEL"));
            }
        }

        public void OnClickSkillBadgeOpenButton(int directUnlockMoneyCost, int badgeSlotCategory, int genericBadgeSlotIndex)
        {
            var playerData = GameEntry.Data.Player;
            if (playerData.Money >= directUnlockMoneyCost)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Message = GameEntry.Localization.GetString("UI_TEXT_CHAT_COST_DIAMOND_OPEN_BADGE", directUnlockMoneyCost),
                    Mode = 2,
                    OnClickConfirm = o => { OnConfirmOpenBadgeSlot(badgeSlotCategory, genericBadgeSlotIndex); },
                });
            }
            else
            {
                UIUtility.CheckCurrency(CurrencyType.Money, directUnlockMoneyCost);
            }
        }

        private void OnConfirmOpenBadgeSlot(int badgeSlotCategory, int genericBadgeSlotIndex)
        {
            GameEntry.LobbyLogic.ActivateSkillBadgeSlot(HeroData.Type, m_SelectSkillIndex, badgeSlotCategory, genericBadgeSlotIndex);
        }

        public void OnClickSkillItem(int index, bool value)
        {
            if (!value)
            {
                return;
            }
            m_SelectSkillIndex = index;
            RefreshSkillData(index);

            ShowBadgeEffect();
        }

        public void OnClickOpenSkillBadgeBagBtn()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SkillBadgeBagForm);
        }

        private SkillBadgeReplaceSubForm ShowSkillBadgeReplaceSubForm()
        {
            if (m_UsingSkillBadgeReplace != null)
            {
                OpenSubForm(m_UsingSkillBadgeReplace);
                return m_UsingSkillBadgeReplace;
            }
            return null;
        }

        private SkillBadgeStageAdvanceSubForm ShowSkillBadgeStageAdvanceSubForm()
        {
            if (m_UsingSkillBadgeStageAdvance != null)
            {
                OpenSubForm(m_UsingSkillBadgeStageAdvance);
                return m_UsingSkillBadgeStageAdvance;
            }
            return null;
        }

        private void OnHeroSkillDataChanged(object sender, GameEventArgs e)
        {
            OnRefreshData();

            var d = e as HeroSkillDataChangedEventArgs;
            if (d != null)
            {
                if (d.ChangeType == HeroSkillDataChangedEventArgs.SkillChangeType.SkillLevelUp)
                {
                    m_EffectsController.ShowEffect(string.Format("EffectSkillItem{0}", m_SelectSkillIndex + 1));
                }
                ShowBadgeEffect();
            }
        }
    }
}
