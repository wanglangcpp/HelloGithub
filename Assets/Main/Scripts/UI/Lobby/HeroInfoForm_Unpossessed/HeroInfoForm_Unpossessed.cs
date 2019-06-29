using UnityEngine;
using System;
using System.Collections;

namespace Genesis.GameClient
{
    /// <summary>
    /// 未激活英雄详情界面。
    /// </summary>
    public partial class HeroInfoForm_Unpossessed : HeroInfoBaseForm
    {
        [SerializeField]
        private UILabel m_HeroName = null;

        [SerializeField]
        private UILabel m_Might = null;

        [SerializeField]
        private UISprite m_Element = null;

        [SerializeField]
        private UISprite[] m_Stars = null;

        [SerializeField]
        private UIProgressBar m_MaxHPProgress = null;

        [SerializeField]
        private UIProgressBar m_AttackProgress = null;

        [SerializeField]
        private UIProgressBar m_DefenseProgress = null;

        [SerializeField]
        private UITexture m_ModelTexture = null;

        private bool m_JustOpened = true;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_JustOpened = true;
            var userDataDict = userData as HeroInfoUnpossessedDisplayData;
            HeroData = userDataDict.UnpossessedHeroData;
            RefreshHero();
        }

        protected override void OnResume()
        {
            base.OnResume();
            ResumeCharacter();
            m_JustOpened = false;
        }

        private void ResumeCharacter()
        {
            if (m_Character == null || m_JustOpened)
            {
                return;
            }

            m_Character.CachedTransform.localRotation = Quaternion.identity;
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnClose(object userData)
        {
            m_JustOpened = false;
            GameEntry.DisplayModel.HideDisplayModel();
            base.OnClose(userData);
        }

        private void RefreshHero()
        {
            StartCoroutine(RefreshDataCo());
            RefreshBasicInfo();
            RefreshAttributeTendencies();
            RefreshHeroAttribute();
            RefreshHeroWhereToGo();
            RefreshSkillData();
            ShowFakeCharacter();
        }

        private IEnumerator RefreshDataCo()
        {
            m_SwitchingHero = true;
            while (m_Character == null || !m_Character.IsAvailable)
            {
                yield return null;
            }
            m_SwitchingHero = false;
        }

        private void RefreshAttributeTendencies()
        {
            var heroData = HeroData;
            m_MaxHPProgress.value = heroData.MaxHPFactorProgress <= 0 ? 0.01f : heroData.MaxHPFactorProgress;
            m_AttackProgress.value = heroData.PhysicalAttackFactorProgress <= 0 ? 0.01f : heroData.PhysicalAttackFactorProgress;
            m_DefenseProgress.value = heroData.PhysicalDefenseFactorProgress <= 0 ? 0.01f : heroData.PhysicalDefenseFactorProgress;
        }

        private void RefreshBasicInfo()
        {
            var heroData = HeroData;
            m_HeroName.text = heroData.Name;
            m_Element.spriteName = UIUtility.GetElementSpriteName(heroData.ElementId);
            m_Might.text = GameEntry.Localization.GetString("UI_TEXT_INTEGER", heroData.DefaultMight);
            int starLevel = heroData.StarLevel;
            m_Stars[0].transform.parent.parent.gameObject.SetActive(true);
            UIUtility.SetStarLevel(m_Stars, starLevel);
        }

        override protected void ShowFakeCharacter()
        {
            GameEntry.DisplayModel.DisplayModel(2, HeroData.Type, m_ModelTexture);
        }

        override protected void ClearFakeCharacter()
        {
            base.ClearFakeCharacter();
        }

        override public void OnClickHeroInteractor()
        {
            base.OnClickHeroInteractor();
            
        }
    }
}
