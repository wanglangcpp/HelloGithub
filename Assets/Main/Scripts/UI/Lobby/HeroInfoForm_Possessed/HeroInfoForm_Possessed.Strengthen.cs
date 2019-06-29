using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroInfoForm_Possessed
    {
        [SerializeField]
        private UISprite m_QualityCornerIcon = null;

        [SerializeField]
        private UILabel m_QualityLevelLabel = null;

        [SerializeField]
        private GameObject m_StrengthenText = null;
        [SerializeField]
        private GameObject m_StrengthenEffect = null;

        [SerializeField]
        private UIButton m_QualityStrengthenButton = null;

        [SerializeField]
        private QualityMaterialItem[] m_QualityItem = null;

        [SerializeField]
        private Animation m_ItemsFadeOutAnim = null;

        [SerializeField]
        private Animation m_AfterItemsFadeOutAnim = null;

        [SerializeField]
        private TweenRotation m_AfterItemsFadeOutRotation = null;

        [SerializeField]
        private UISprite m_OldHeroIcon = null;

        [SerializeField]
        private UISprite m_NewHeroIcon = null;

        [SerializeField]
        private UISprite m_OldBorderIcon = null;

        [SerializeField]
        private UISprite m_NewBorderIcon = null;

        [SerializeField]
        private UILabel m_QualityMaxHpAddText = null;

        [SerializeField]
        private UILabel m_QualityAttackAddText = null;

        [SerializeField]
        private UILabel m_QualityDefenseAddText = null;

        private float m_QualityLevelMaxHpAttri = 0;
        private float m_QualityLevelAttackAttri = 0;
        private float m_QualityLevelDefenseAttri = 0;
        private bool m_CanHeroStarUp = false;
        private float m_ReqCount = 0;
        private string m_PieceName = string.Empty;
        private int m_StarLevelUpItemId = 0;
        private QualityUpDisplayData m_QualityUpSuccessData = new QualityUpDisplayData();
        private StrengthenDisplayData m_StrengthenDisplayData = new StrengthenDisplayData();
        private bool m_StartHeroStrengthenUpAnim = false;
        private bool m_StartAfterItemsFadeOutAnim = false;

        public bool CloseStrengthen()
        {
            m_StartHeroStrengthenUpAnim = false;
            RecoverAnimation();
            return true;
        }

        public bool RefreshStrengthen()
        {
            m_QualityLevelMaxHpAttri = 0;
            m_QualityLevelAttackAttri = 0;
            m_QualityLevelDefenseAttri = 0;
            m_AfterItemsFadeOutAnim.gameObject.SetActive(false);
            m_QualityStrengthenButton.gameObject.SetActive(true);
            m_StartHeroStrengthenUpAnim = false;
            QualityType quality = HeroData.Quality;
            var dataTable = GameEntry.DataTable.GetDataTable<DRHeroQualityMaxLevel>();
            DRHeroQualityMaxLevel maxLevelRow = dataTable.GetDataRow((int)quality);
            if (maxLevelRow == null)
            {
                Log.Warning("Cannot find Quality max level '{0}'.", quality);
                return false;
            }

            var heroData = HeroData as LobbyHeroData;
            var qualityItemStates = heroData.GetQualityItemSlotStates();
            DRHeroQualityLevel drQualityLevel = GameEntry.DataTable.GetDataTable<DRHeroQualityLevel>().GetDataRow(heroData.QualityLevelId);
            if (drQualityLevel == null)
            {
                Log.Warning("Cannot find DRHeroQualityLevel's  QualityItem. QualityLevelId is '{0}'.", heroData.NextQualityLevelId);
                return false;
            }

            m_OldHeroIcon.gameObject.SetActive(false);
            m_OldBorderIcon.gameObject.SetActive(false);
            m_NewHeroIcon.LoadAsync(HeroData.IconId);
            m_NewBorderIcon.spriteName = Constant.Quality.HeroBorderSpriteNames[(int)HeroData.Quality];

            var qualityItems = drQualityLevel.GetQualityItemIds();
            var qualityItemCounts = drQualityLevel.GetQualityItemCounts();
            bool equip = true;
            for (int i = 0; i < m_QualityItem.Length; i++)
            {
                if (qualityItems[i] > 0 && qualityItemCounts[i] > 0)
                {
                    bool isLoad = qualityItemStates[i];
                    DRHeroQualityItem drItem = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>().GetDataRow(qualityItems[i]);

                    if (m_QualityItem[i].IsOpenAndNotEquiped && isLoad)
                    {
                        m_QualityItem[i].ShowEquipEffect();
                    }
                    m_QualityItem[i].RefreshMeterialData(drItem, isLoad, false, HeroData.Type, i);
                    if (!isLoad)
                    {
                        equip = isLoad;
                    }
                    else
                    {
                        CalculateEquipedItemIncrement(drItem);
                    }
                }
                else
                {
                    m_QualityItem[i].RefreshMeterialData(null, false, true, HeroData.Type, i);
                }
            }
            m_QualityStrengthenButton.isEnabled = equip;
            m_StrengthenText.SetActive(equip);
            m_StrengthenEffect.SetActive(equip);
            //var animation = m_QualityStrengthenButton.GetComponent<Animation>();
            //if (equip)
            //{
            //    animation.Play();
            //}
            //else
            //{
            //    animation.Stop();
            //}
            m_QualityLevelLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", heroData.QualityLevel);
            m_QualityLevelLabel.color = Constant.Quality.QualityLevelColors[(int)heroData.Quality];
            m_QualityCornerIcon.spriteName = Constant.Quality.HeroQualityLevelBorderSpriteNames[(int)heroData.Quality];
            m_QualityCornerIcon.gameObject.SetActive((heroData.QualityLevel > 0) && (heroData is LobbyHeroData));
            QualityIncrement(drQualityLevel);
            return true;
        }

        private void QualityIncrement(DRHeroQualityLevel drQualityLevel)
        {
            var attrIds = drQualityLevel.GetAttrIds();
            var attrVals = drQualityLevel.GetAttrVals();
            for (int i = 0; i < attrIds.Count; i++)
            {
                if (attrIds[i] == (int)AttributeType.MaxHP)
                {
                    m_QualityLevelMaxHpAttri += attrVals[i];
                }
                else if (attrIds[i] == (int)AttributeType.PhysicalAttack)
                {
                    m_QualityLevelAttackAttri += attrVals[i];
                }
                else if (attrIds[i] == (int)AttributeType.PhysicalDefense)
                {
                    m_QualityLevelDefenseAttri += attrVals[i];
                }
            }
            m_QualityMaxHpAddText.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", m_QualityLevelMaxHpAttri);
            m_QualityAttackAddText.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", m_QualityLevelAttackAttri);
            m_QualityDefenseAddText.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", m_QualityLevelDefenseAttri);
        }

        private void CalculateEquipedItemIncrement(DRHeroQualityItem drItem)
        {
            var attrIds = drItem.GetAttrIds();
            var attrVals = drItem.GetAttrVals();
            for (int i = 0; i < attrIds.Count; i++)
            {
                if (attrIds[i] == (int)AttributeType.MaxHP)
                {
                    m_QualityLevelMaxHpAttri += attrVals[i];
                }
                else if (attrIds[i] == (int)AttributeType.PhysicalAttack)
                {
                    m_QualityLevelAttackAttri += attrVals[i];
                }
                else if (attrIds[i] == (int)AttributeType.PhysicalDefense)
                {
                    m_QualityLevelDefenseAttri += attrVals[i];
                }
            }
        }

        private void ShowUpgradeStarEffect()
        {
            if (m_StarLevelProgressBar.value >= 1)
            {
                if (m_EffectsController.EffectIsShowing("EffectStarUp") == false)
                    m_EffectsController.ShowEffect("EffectStarUp");
            }
            else
            {
                m_EffectsController.DestroyEffect("EffectStarUp");
            }
        }

        public void OnClickQualityStrengthen()
        {
            if (HeroData.NextQualityLevelId <= 0)
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_HERO_MAX_TOTAL_QUALITY_LEVEL"));
                return;
            }
            m_QualityUpSuccessData.LastMaxHp = HeroData.MaxHP;
            m_QualityUpSuccessData.LastPhysicalAttack = HeroData.PhysicalAttack;
            m_QualityUpSuccessData.LastPhysicalDefense = HeroData.PhysicalDefense;
            m_QualityUpSuccessData.LastQuality = HeroData.Quality;
            m_QualityUpSuccessData.LastQualityLevel = HeroData.QualityLevel;
            m_QualityUpSuccessData.BaseHeroData = HeroData;
            GameEntry.LobbyLogic.RequestIncreaseHeroQuality(HeroData.Type);
        }

        private void OnIncreaseHeroQualityLevel(object sender, GameEventArgs e)
        {
            for (int i = 0; i < m_QualityItem.Length; i++)
            {
                m_QualityItem[i].ItemCanClick = false;
            }
            m_StrengthenText.SetActive(false);
            m_StartHeroStrengthenUpAnim = true;
            m_StartAfterItemsFadeOutAnim = false;
            m_ItemsFadeOutAnim.Rewind();
            m_ItemsFadeOutAnim.Play();
            m_EffectsController.ShowEffect("EffectOrderToUpgrade");

        }

        private void RecoverAnimation()
        {
            m_ItemsFadeOutAnim.Rewind();
            m_ItemsFadeOutAnim.clip.SampleAnimation(m_ItemsFadeOutAnim.gameObject, 0);
            RefreshStrengthen();
        }

        private void PlayStrengthenAnimFinished()
        {
            RecoverAnimation();
            GameEntry.UI.OpenUIForm(UIFormId.QualityUpSuccessForm, m_QualityUpSuccessData);
        }

        private void OnHeroQualityItemDataChange(object sender, GameEventArgs e)
        {
            RefreshStrengthen();
        }

        private void UpdateHeroStrengthenAnim()
        {
            if (!m_StartHeroStrengthenUpAnim)
            {
                return;
            }

            if (m_ItemsFadeOutAnim.isPlaying)
            {
                return;
            }

            if (!m_ItemsFadeOutAnim.isPlaying && !m_StartAfterItemsFadeOutAnim)
            {
                m_StartAfterItemsFadeOutAnim = true;
                m_QualityStrengthenButton.gameObject.SetActive(false);
                m_AfterItemsFadeOutAnim.gameObject.SetActive(true);
                m_AfterItemsFadeOutAnim.Rewind();
                m_AfterItemsFadeOutAnim.Play();
                m_AfterItemsFadeOutRotation.ResetToBeginning();
                m_AfterItemsFadeOutRotation.PlayForward();
                m_OldHeroIcon.LoadAsync(HeroData.IconId);
                m_NewHeroIcon.LoadAsync(HeroData.IconId);
                m_OldBorderIcon.spriteName = Constant.Quality.HeroBorderSpriteNames[(int)m_QualityUpSuccessData.LastQuality];
                m_NewBorderIcon.spriteName = Constant.Quality.HeroBorderSpriteNames[(int)HeroData.Quality];
                m_QualityLevelLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", HeroData.QualityLevel);
                m_QualityLevelLabel.color = Constant.Quality.QualityLevelColors[(int)HeroData.Quality];
                m_QualityCornerIcon.spriteName = Constant.Quality.HeroQualityLevelBorderSpriteNames[(int)HeroData.Quality];
                m_QualityCornerIcon.gameObject.SetActive((HeroData.QualityLevel > 0) && (HeroData is LobbyHeroData));
                return;
            }

            if (!m_AfterItemsFadeOutAnim.isPlaying)
            {
                m_StartAfterItemsFadeOutAnim = false;
                PlayStrengthenAnimFinished();
            }
        }
    }
}
