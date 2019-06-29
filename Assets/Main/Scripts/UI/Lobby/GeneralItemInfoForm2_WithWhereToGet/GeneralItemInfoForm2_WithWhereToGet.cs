using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 物品信息界面之详细版。
    /// </summary>
    /// <remarks>用于英雄升品道具和英雄碎片。</remarks>
    public partial class GeneralItemInfoForm2_WithWhereToGet : NGUIForm
    {
        #region Left section

        [SerializeField]
        private GeneralItemView m_MainItemView = null;

        [SerializeField]
        private UILabel m_MainItemNameLbl = null;

        [SerializeField]
        private UILabel m_MainItemCountLbl = null;

        [SerializeField]
        private GameObject m_MainItemAttributesRoot = null;

        [SerializeField]
        private AttributeViewItem[] m_MainItemAttrViewItems = null;

        [SerializeField]
        private UILabel m_MainItemDescLbl = null;

        [SerializeField]
        private UIButton m_InlayBtn = null;

        #endregion Left section

        #region Right section

        [SerializeField]
        private UISprite m_WhereToGetBg = null;

        [SerializeField]
        private GameObject m_SynthPathRoot = null;

        [SerializeField]
        private UIGrid m_SynthPathListView = null;

        [SerializeField]
        private WhereToGetScrollViewCache m_WhereToGetScrollViewCache = null;

        [SerializeField]
        private GameObject m_SynthGraphRoot = null;

        [SerializeField]
        private GeneralItemView m_SynthTargetItemView = null;

        [SerializeField]
        private UISprite[] m_SynthArrows = null;

        [SerializeField]
        private GeneralItemView[] m_SynthItemViews = null;

        [SerializeField]
        private UILabel[] m_SynthItemCountLbls = null;

        [SerializeField]
        private UISprite m_SynthTokenSprite = null;

        [SerializeField]
        private UILabel m_SynthCostLbl = null;

        [SerializeField]
        private UIButton m_SynthBtn = null;

        [SerializeField]
        private WhereToGetBgSize m_WhereToGetBgSizeDefault = null;

        [SerializeField]
        private WhereToGetBgSize m_WhereToGetBgSizeHeroQualityItem = null;

        #endregion Right section

        #region Prefabs

        [SerializeField]
        private GameObject m_SynthPathViewItemTemplate = null;

        #endregion Prefabs

        #region Inner classes

        [Serializable]
        private class AttributeViewItem
        {
            public Transform Root = null;
            public UILabel NameLabel = null;
            public UILabel ValueLabel = null;
        }

        [Serializable]
        private class WhereToGetBgSize
        {
            public float Y = 0f;
            public float Height = 0f;
        }

        [Serializable]
        private class WhereToGetScrollViewCache : UIScrollViewCache<WhereToGetDisplayItem>
        {
            // Empty.
        }

        private enum Scenario
        {
            Default,
            HeroQualityItem,
        }

        #endregion Inner classes

        private List<SynthPathViewItem> m_SynthPathViewItems = new List<SynthPathViewItem>();
        private GeneralItemInfoDisplayData m_CachedDisplayData = null;
        private Scenario m_Scenario;
        private StrategyBase m_Strategy;

        #region NGUIForm

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_SynthPathViewItems.Clear();
            m_CachedDisplayData = userData as GeneralItemInfoDisplayData;

            var typeId = m_CachedDisplayData.TypeId;
            var itemDT = GameEntry.DataTable.GetDataTable<DRItem>();

            DRItem itemDR = itemDT.GetDataRow(typeId);
            if (itemDR != null)
            {
                if (itemDR.Type == (int)ItemType.HeroPieceItem)
                {
                    
                    m_Scenario = Scenario.Default;
                }
                else if(itemDR.Type == (int)ItemType.HeroQualityItem)
                {
                    var heroQualityItemDT = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>();
                    DRHeroQualityItem heroQualityItemDR = heroQualityItemDT.GetDataRow(typeId);

                    if (typeId < Constant.GeneralItem.MinHeroQualityItemId ||
                        typeId > Constant.GeneralItem.MaxHeroQualityItemId ||
                        heroQualityItemDR == null)
                    {
                        Log.Error("Invalid hero quality item type ID '{0}'.", typeId.ToString());
                        return;
                    }

                    m_Scenario = Scenario.HeroQualityItem;
                }
                else
                {
                    Log.Error("Item type ID '{0}' in item data table but not a hero piece.");
                    return;
                }
            }
            else
            {
                Log.Error("Item type ID '{0}' not in item data table.");
                return;
            }

            m_Strategy = GetStrategy(m_Scenario, this);
            InitView();
            //GameEntry.NoviceGuide.CheckNoviceGuide(transform, UIFormId.GeneralItemInfoForm2_WithWhereToGet);
        }

        protected override void OnClose(object userData)
        {
            m_Strategy.Shutdown();
            m_Strategy = null;
            m_CachedDisplayData = null;
            m_SynthPathViewItems.Clear();
            base.OnClose(userData);
        }

        #endregion NGUIForm

        #region NGUI callbacks

        public void OnClickInlayBtn()
        {
            if (this != null && m_Strategy != null)
            {
                m_Strategy.OnClickInlayBtn();
            }
        }

        public void OnClickSynthBtn()
        {
            if (this != null && m_Strategy != null)
            {
                m_Strategy.OnMixItemShowEffect = OnMixItem;
                m_Strategy.OnClickSynthBtn();
            }
        }

        private void OnMixItem()
        {
            StartCoroutine(ShowMixEffect());
        }

        private IEnumerator ShowMixEffect()
        {
            m_EffectsController.ShowEffect("EffectItemLight1");

            yield return new WaitForSeconds(0.5f);

            m_EffectsController.ShowEffect("EffectItemLight2");
        }

        #endregion NGUI callbacks

        private void InitView()
        {
            m_Strategy.InitData();
            m_Strategy.InitLeftSection();
            m_Strategy.InitRightSection();
        }
    }
}
