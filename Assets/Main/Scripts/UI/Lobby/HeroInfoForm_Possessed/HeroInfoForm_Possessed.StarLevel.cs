using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroInfoForm_Possessed
    {
        [SerializeField]
        private UIProgressBar m_StarLevelProgressBar = null;

        [SerializeField]
        private UILabel m_StarLevelValue = null;

        [SerializeField]
        private UIButton m_StarUpButton = null;

        [SerializeField]
        private UISprite m_HeroChipIcon = null;

        [SerializeField]
        private UIButton m_AchieveChipsButton = null;

        [SerializeField]
        private UISprite[] m_RightStars = null;

        [SerializeField]
        private UILabel m_Might = null;

        [SerializeField]
        private UILabel m_MaxHpAddText = null;

        [SerializeField]
        private UILabel m_AttackAddText = null;

        [SerializeField]
        private UILabel m_DefenseAddText = null;

        [SerializeField]
        private UILabel m_TeamToneUpText = null;

        private void RefreshStarLevelUp()
        {
            DRHero heroDataRow = GameEntry.DataTable.GetDataTable<DRHero>().GetDataRow(HeroData.Type);
            if (heroDataRow == null)
            {
                Log.Warning("Cannot find heroDataRow hero type is '{0}'.", HeroData.Type);
                return;
            }
            m_Might.text = GameEntry.Localization.GetString("UI_TEXT_HERO_GS_TEXT", HeroData.Might);
            m_StarLevelUpItemId = heroDataRow.StarLevelUpItemId;
            var item = GameEntry.Data.Items.GetData(m_StarLevelUpItemId);
            int type = item == null ? 0 : item.Key;
            DRItem itemDataRow = GameEntry.DataTable.GetDataTable<DRItem>().GetDataRow(type);
            if (itemDataRow != null)
            {
                m_PieceName = itemDataRow.Name;
            }
            m_HeroChipIcon.LoadAsync(GeneralItemUtility.GetGeneralItemIconId(m_StarLevelUpItemId));
            int starLevelUpItemCount = heroDataRow.StarLevelUpItemCounts[HeroData.StarLevel - 1];
            int count = item == null ? 0 : item.Count;
            m_MaxHpAddText.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", (heroDataRow.StarFactors[HeroData.StarLevel - 1] * 100) + "%");
            m_AttackAddText.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", (heroDataRow.StarFactors[HeroData.StarLevel - 1] * 100) + "%");
            m_DefenseAddText.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", (heroDataRow.StarFactors[HeroData.StarLevel - 1] * 100) + "%");
            m_TeamToneUpText.text = string.Empty;

            if (HeroData.StarLevel >= Constant.HeroStarLevelCount)
            {
                m_StarLevelProgressBar.value = 1;
                m_StarLevelValue.text = GameEntry.Localization.GetString("UI_TEXT_STARFULL_NOTICE");
                m_StarUpButton.gameObject.SetActive(false);
                m_AchieveChipsButton.gameObject.SetActive(false);
                m_Reminder[(int)TabType.StarLevel].SetActive(false);
            }
            else
            {
                m_StarLevelProgressBar.value = (float)count / (float)(starLevelUpItemCount <= 0 ? 1 : starLevelUpItemCount);
                m_StarLevelValue.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", count, starLevelUpItemCount);
                m_CanHeroStarUp = count >= starLevelUpItemCount;
                m_StarUpButton.gameObject.SetActive(m_CanHeroStarUp);
                m_AchieveChipsButton.gameObject.SetActive(!m_StarUpButton.gameObject.activeSelf);
                m_Reminder[(int)TabType.StarLevel].SetActive(m_CanHeroStarUp);
                //ShowUpgradeStarEffect();
            }
            m_ReqCount = starLevelUpItemCount;
        }

        public void OnClickStarLevelUpButton()
        {
            if (m_SwitchingHero)
            {
                return;
            }
            string name = GameEntry.Localization.GetString(m_PieceName);
            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Message = GameEntry.Localization.GetString("UI_TEXT_CONFIRM_STARLEVELUP", m_ReqCount, name),
                Mode = 2,
                OnClickConfirm = o => { OnConfirmStarLevel(); },
            });
        }

        public void OnClickAchieveChipsButton()
        {
            GameEntry.UI.OpenItemInfoForm(new GeneralItemInfoDisplayData
            {
                TypeId = m_StarLevelUpItemId,
                CanInlay = false,
            });
        }

        private void OnConfirmStarLevel()
        {
            m_StrengthenDisplayData.LastMaxHP = HeroData.MaxHP;
            m_StrengthenDisplayData.LastPhysicalAttack = HeroData.PhysicalAttack;
            m_StrengthenDisplayData.LastPhysicalDefense = HeroData.PhysicalDefense;
            m_StrengthenDisplayData.BaseHeroData = HeroData;

            CLHeroStarLevelUp request = new CLHeroStarLevelUp()
            {
                HeroType = HeroData.Type
            };

            GameEntry.Network.Send(request);
        }

        private void OnStarLevelUpSuccess(object sender, GameEventArgs e)
        {
            GameEntry.UI.OpenUIForm(UIFormId.StarUpSuccessForm, m_StrengthenDisplayData);
            RefreshStarLevelUp();
            RefreshHeroData();
        }

        //public void ShowExperienceItemEffect()
        //{
        //    for (int i = 0; i < m_ExperienceItems.Count; i++)
        //    {
        //        string effectName = string.Format("EffectExpSurround{0}", i + 1);
        //        if (m_ExperienceItems[i].CanUseItem)
        //        {
        //            if (!m_EffectsController.EffectIsShowing(effectName))
        //            {
        //                m_EffectsController.ShowEffect(effectName);
        //            }
        //        }
        //        else
        //        {
        //            m_EffectsController.DestroyEffect(effectName);
        //        }
        //    }
        //}

        //public void SetProgressBarThumbEffect(bool isShow)
        //{
        //    string effectName = "EffectProgressLight";
        //    if (isShow)
        //    {
        //        m_EffectsController.ShowEffect(effectName);
        //    }
        //    else
        //    {
        //        m_EffectsController.DestroyEffect(effectName);
        //        SetProgressBarEffect(m_ProgressController.ProgressBarIsFull);
        //    }
        //}

        //public void SetProgressBarEffect(bool isShow)
        //{
        //    string effectName = "EffectProgressMax";
        //    if (isShow)
        //    {
        //        m_EffectsController.ShowEffect(effectName);
        //    }
        //    else
        //    {
        //        m_EffectsController.DestroyEffect(effectName);
        //    }
        //}

        public bool CloseStarLevel()
        {
            return true;
        }

        public bool RefreshStarLevel()
        {
            if (HeroData == null)
            {
                return false;
            }
            RefreshStarLevelUp();
            return true;
        }

        private int CompareItems(DRItem a, DRItem b)
        {
            if (a.Quality == b.Quality)
            {
                return a.Id.CompareTo(b.Id);
            }
            return a.Quality.CompareTo(b.Quality);
        }

        private void OnUseItem(object sender, GameEventArgs e)
        {
            //ShowExperienceItemEffect();
        }
    }
}
