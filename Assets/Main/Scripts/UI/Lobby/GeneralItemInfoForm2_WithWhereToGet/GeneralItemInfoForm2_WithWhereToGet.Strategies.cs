using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class GeneralItemInfoForm2_WithWhereToGet
    {
        /// <summary>
        /// 界面策略基类。
        /// </summary>
        private abstract class StrategyBase
        {

            public Action OnMixItemShowEffect;

            protected GeneralItemInfoForm2_WithWhereToGet m_Form;

            protected GeneralItemInfoDisplayData CachedDisplayData { get { return m_Form.m_CachedDisplayData; } }

            protected int m_CurrentItemId = 0;

            public StrategyBase InitForm(GeneralItemInfoForm2_WithWhereToGet form)
            {
                m_Form = form;
                return this;
            }

            public virtual void InitData()
            {
                m_CurrentItemId = CachedDisplayData.TypeId;
            }

            public virtual void InitLeftSection()
            {
                m_Form.m_MainItemView.InitItem(CachedDisplayData.TypeId, MainItemQuality);
                m_Form.m_MainItemView.SetOnClickDelegate(null);
                m_Form.m_MainItemNameLbl.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemName(CachedDisplayData.TypeId));
                m_Form.m_MainItemAttributesRoot.SetActive(false);
                m_Form.m_MainItemDescLbl.text = string.Empty;
                UpdateMainItemCount();
                m_Form.m_InlayBtn.gameObject.SetActive(false);
            }

            protected void UpdateMainItemCount()
            {
                m_Form.m_MainItemCountLbl.text = GameEntry.Localization.GetString("UI_TEXT_ITEM_HAVE_COUNT", ItemCount);
            }

            public virtual void InitRightSection()
            {
                m_Form.m_WhereToGetBg.gameObject.SetActive(false);
                m_Form.m_SynthPathRoot.SetActive(false);
                m_Form.m_SynthTokenSprite.gameObject.SetActive(false);
                m_Form.m_SynthCostLbl.text = string.Empty;
                m_Form.m_SynthBtn.gameObject.SetActive(false);
                m_Form.m_SynthGraphRoot.SetActive(false);
            }

            public virtual void Shutdown()
            {
                RecycleWhereToGetItems();
            }

            public virtual void OnClickSynthBtn()
            {
                throw new NotSupportedException(string.Format("You cannot synthesize hero quality items in strategy '{0}'.", m_Form.m_Strategy));
            }

            public virtual void OnClickInlayBtn()
            {
                throw new NotSupportedException(string.Format("You cannot inlay hero quality items in strategy '{0}'.", m_Form.m_Strategy));
            }

            public QualityType MainItemQuality { get { return (QualityType)(GeneralItemUtility.GetGeneralItemQuality(CachedDisplayData.TypeId)); } }

            public abstract int ItemCount { get; }

            protected abstract WhereToGetBgSize WhereToGetBgSize { get; }

            protected void RefreshWhereToGetItems()
            {
                m_Form.m_WhereToGetBg.cachedTransform.SetLocalPositionY(WhereToGetBgSize.Y);
                m_Form.m_WhereToGetBg.height = Mathf.RoundToInt(WhereToGetBgSize.Height);

                var itemWhereToGetDT = GameEntry.DataTable.GetDataTable<DRGeneralItemWhereToGet>();
                DRGeneralItemWhereToGet dr = itemWhereToGetDT.GetDataRow(m_CurrentItemId);
                if (dr == null)
                {
                    RecycleWhereToGetItems();
                    return;
                }

                var whereToGetIds = dr.GetWhereToGetIds();
                for (int i = 0; i < whereToGetIds.Count; ++i)
                {
                    if (whereToGetIds[i] <= 0)
                    {
                        continue;
                    }

                    var script = m_Form.m_WhereToGetScrollViewCache.GetOrCreateItem(i);
                    var whereToGetLogic = GameEntry.WhereToGet.GetLogic(whereToGetIds[i]);
                    whereToGetLogic.MixedItemId = CachedDisplayData.TypeId;
                    whereToGetLogic.NeedItemId = m_CurrentItemId;

                    script.RefreshData(whereToGetLogic);

                    if (whereToGetLogic.Type != WhereToGetType.Text)
                    {
                        script.SetPostClickDelegate(delegate (object o) { m_Form.CloseSelf(); }, false);
                    }
                    else
                    {
                        script.SetPostClickDelegate(null, false);
                    }

                    m_Form.m_WhereToGetScrollViewCache.ResetPosition();
                }
            }

            protected void RecycleWhereToGetItems()
            {
                m_Form.m_WhereToGetScrollViewCache.RecycleAllItems();
            }
        }

        /// <summary>
        /// 界面默认策略。
        /// </summary>
        private class StrategyDefault : StrategyBase
        {
            private DRItem m_CachedMainItemDR = null;

            private DRItem CachedMainItemDR
            {
                get
                {
                    if (m_CachedMainItemDR == null)
                    {
                        m_CachedMainItemDR = GameEntry.DataTable.GetDataTable<DRItem>()[CachedDisplayData.TypeId];
                    }

                    return m_CachedMainItemDR;
                }
            }

            public override void InitLeftSection()
            {
                base.InitLeftSection();
                m_Form.m_MainItemDescLbl.text = GameEntry.Localization.GetString(GeneralItemUtility.GetGeneralItemDescription(CachedDisplayData.TypeId));
            }

            public override void InitRightSection()
            {
                base.InitRightSection();
                m_Form.m_WhereToGetBg.gameObject.SetActive(true);
                RefreshWhereToGetItems();
            }

            public override int ItemCount
            {
                get
                {
                    var itemData = GameEntry.Data.Items.GetData(CachedDisplayData.TypeId);
                    return itemData == null ? 0 : itemData.Count;
                }
            }

            protected override WhereToGetBgSize WhereToGetBgSize
            {
                get
                {
                    return m_Form.m_WhereToGetBgSizeDefault;
                }
            }
        }

        /// <summary>
        /// 界面策略之英雄升品道具。
        /// </summary>
        private class StrategyHeroQualityItem : StrategyBase
        {
            private DRHeroQualityItem m_CachedMainItemDR = null;
            private List<SynthPathViewItem> m_CachedSynthPathViewItems = new List<SynthPathViewItem>();
            private int m_CurrentSynthPathLen = 0;

            public override void InitData()
            {
                base.InitData();
                GameEntry.Event.Subscribe(EventId.HeroQualityItemDataChange, OnHeroQualityItemDataChange);
            }

            public override void Shutdown()
            {
                RecycleSynthPathViewItems(0);
                GameEntry.Event.Unsubscribe(EventId.HeroQualityItemDataChange, OnHeroQualityItemDataChange);
                base.Shutdown();
            }

            private void OnHeroQualityItemDataChange(object sender, GameEventArgs e)
            {
                UpdateMainItemCount();
                UpdateSynthGraphOrWhereToGetView();
            }

            private DRHeroQualityItem CachedMainItemDR
            {
                get
                {
                    if (m_CachedMainItemDR == null)
                    {
                        m_CachedMainItemDR = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>()[CachedDisplayData.TypeId];
                    }

                    return m_CachedMainItemDR;
                }
            }

            public override void InitLeftSection()
            {
                base.InitLeftSection();
                m_Form.m_MainItemAttributesRoot.SetActive(true);

                var attrIds = CachedMainItemDR.GetAttrIds();
                var attrVals = CachedMainItemDR.GetAttrVals();

                var attrViewItems = m_Form.m_MainItemAttrViewItems;
                for (int i = 0; i < attrIds.Count && i < attrViewItems.Length; ++i)
                {
                    attrViewItems[i].Root.gameObject.SetActive(true);
                    attrViewItems[i].NameLabel.text = GameEntry.Localization.GetString(Constant.AttributeName.AttributeNameDics[attrIds[i]]);
                    attrViewItems[i].ValueLabel.text = UIUtility.GetAttributeValueStr((AttributeType)(attrIds[i]), attrVals[i]);
                }

                for (int i = attrIds.Count; i < attrViewItems.Length; ++i)
                {
                    attrViewItems[i].Root.gameObject.SetActive(false);
                }

                var item = GameEntry.Data.HeroQualityItems.GetData(CachedDisplayData.TypeId);
                m_Form.m_InlayBtn.gameObject.SetActive(CachedDisplayData.CanInlay && item != null && item.Count > 0);
            }

            public override void InitRightSection()
            {
                base.InitRightSection();
                m_Form.m_SynthPathRoot.SetActive(true);
                InitSynthPath();
                UpdateSynthGraphOrWhereToGetView();
            }

            public override int ItemCount
            {
                get
                {
                    var itemData = GameEntry.Data.HeroQualityItems.GetData(CachedDisplayData.TypeId);
                    return itemData == null ? 0 : itemData.Count;
                }
            }

            public override void OnClickSynthBtn()
            {
                var dr = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>()[m_CurrentItemId];
                if (!UIUtility.CheckCurrency(CurrencyType.Coin, dr.SynthCoinCost))
                {
                    return;
                }

                var synthItemIds = dr.GetSynthItemIds();
                var synthItemCounts = dr.GetSynthItemCounts();
                for (int i = 0; i < synthItemIds.Count; ++i)
                {
                    var itemId = synthItemIds[i];
                    var expectCount = synthItemCounts[i];
                    var ownedCount = GeneralItemUtility.GetGeneralItemCount(itemId);

                    if (ownedCount < expectCount)
                    {
                        UIUtility.ShowOkayButtonDialog(GameEntry.Localization.GetString("UI_TEXT_GEARSTRENGTHEN_NOTICE_MATERIALNOTENOUGH"));
                        return;
                    }
                }

                GameEntry.LobbyLogic.RequestSynthesizeHeroQualityItem(m_CurrentItemId);

                if (OnMixItemShowEffect != null)
                    OnMixItemShowEffect.Invoke();
            }

            public override void OnClickInlayBtn()
            {
                if (ItemCount <= 0)
                {
                    UIUtility.ShowOkayButtonDialog(GameEntry.Localization.GetString("UI_TEXT_GEARSTRENGTHEN_NOTICE_MATERIALNOTENOUGH"));
                    return;
                }

                if (CachedDisplayData.OnInlay != null)
                {
                    CachedDisplayData.OnInlay();
                }

                m_Form.CloseSelf();
            }

            protected override WhereToGetBgSize WhereToGetBgSize
            {
                get
                {
                    return m_Form.m_WhereToGetBgSizeHeroQualityItem;
                }
            }

            private void InitSynthPath()
            {
                AppendSynthPath(CachedDisplayData.TypeId);
            }

            private void AppendSynthPath(int itemId)
            {
                m_CurrentItemId = itemId;
                var script = GetOrCreateSynthPathViewItem(m_CurrentSynthPathLen++);
                script.InnerItemView.InitItem(m_CurrentItemId, (QualityType)GeneralItemUtility.GetGeneralItemQuality(m_CurrentItemId));

                int index = m_CurrentSynthPathLen - 1;
                script.InnerItemView.SetOnClickDelegate(delegate ()
                {
                    OnClickSynthPathItemView(index);
                });

                script.DownArrow.gameObject.SetActive(true);
                script.RightArrow.gameObject.SetActive(false);

                if (m_CurrentSynthPathLen > 1)
                {
                    var lastScript = m_CachedSynthPathViewItems[m_CurrentSynthPathLen - 2];
                    lastScript.DownArrow.gameObject.SetActive(false);
                    lastScript.RightArrow.gameObject.SetActive(true);
                }

                RecycleSynthPathViewItems(m_CurrentSynthPathLen);
                m_Form.m_SynthPathListView.Reposition();
            }

            private void OnClickSynthPathItemView(int index)
            {
                if (index < m_CurrentSynthPathLen - 1)
                {
                    RecycleSynthPathViewItems(index + 1);
                    var view = m_CachedSynthPathViewItems[index];
                    view.DownArrow.gameObject.SetActive(true);
                    view.RightArrow.gameObject.SetActive(false);
                    m_CurrentItemId = view.InnerItemView.GeneralItemId;
                    UpdateSynthGraphOrWhereToGetView();
                }
            }

            private void UpdateSynthGraphOrWhereToGetView()
            {
                var drHeroQualityItem = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>()[m_CurrentItemId];
                var synthItemIds = drHeroQualityItem.GetSynthItemIds();
                var synthItemCounts = drHeroQualityItem.GetSynthItemCounts();

                if (synthItemIds.Count <= 0)
                {
                    m_Form.m_WhereToGetBg.gameObject.SetActive(true);
                    m_Form.m_SynthGraphRoot.SetActive(false);
                    RefreshWhereToGetItems();
                    m_Form.m_SynthBtn.gameObject.SetActive(false);
                    m_Form.m_SynthCostLbl.text = string.Empty;
                    m_Form.m_SynthTokenSprite.gameObject.SetActive(false);
                    UpdateSynthPrice(0);
                }
                else
                {
                    m_Form.m_WhereToGetBg.gameObject.SetActive(false);
                    m_Form.m_SynthGraphRoot.SetActive(true);
                    InitSynthGraph(synthItemIds, synthItemCounts);
                    m_Form.m_SynthBtn.gameObject.SetActive(true);
                    UpdateSynthPrice(drHeroQualityItem.SynthCoinCost);
                }
                var item = GameEntry.Data.HeroQualityItems.GetData(CachedDisplayData.TypeId);
                m_Form.m_InlayBtn.gameObject.SetActive(CachedDisplayData.CanInlay && item != null && item.Count > 0);
            }

            private void UpdateSynthPrice(int price)
            {
                if (price > 0)
                {
                    m_Form.m_SynthCostLbl.text = price.ToString();
                    m_Form.m_SynthTokenSprite.gameObject.SetActive(true);
                }
                else
                {
                    m_Form.m_SynthCostLbl.text = string.Empty;
                    m_Form.m_SynthTokenSprite.gameObject.SetActive(false);
                }
            }

            private void InitSynthGraph(IList<int> synthItemIds, IList<int> synthItemCounts)
            {
                m_Form.m_SynthTargetItemView.InitItem(m_CurrentItemId, (QualityType)GeneralItemUtility.GetGeneralItemQuality(m_CurrentItemId));
                m_Form.m_SynthTargetItemView.SetOnClickDelegate(null);
                if (!CheckSynthItemIds(synthItemIds)) return;

                switch (synthItemIds.Count)
                {
                    case 1:
                        HideSynthItemViewGroup(0);
                        ShowSynthItemViewGroup(1, synthItemIds[0], synthItemCounts[0]);
                        HideSynthItemViewGroup(2);
                        break;
                    case 2:
                        ShowSynthItemViewGroup(0, synthItemIds[0], synthItemCounts[0]);
                        HideSynthItemViewGroup(1);
                        ShowSynthItemViewGroup(2, synthItemIds[1], synthItemCounts[1]);
                        break;
                    case 3:
                        ShowSynthItemViewGroup(0, synthItemIds[0], synthItemCounts[0]);
                        ShowSynthItemViewGroup(1, synthItemIds[1], synthItemCounts[1]);
                        ShowSynthItemViewGroup(2, synthItemIds[2], synthItemCounts[2]);
                        break;
                    default:
                        throw new System.NotSupportedException(string.Format("You cannot synthesize a hero quality item from '{0}' items.", synthItemIds.Count));
                }
            }

            private void HideSynthItemViewGroup(int index)
            {
                m_Form.m_SynthArrows[index].gameObject.SetActive(false);
                m_Form.m_SynthItemViews[index].gameObject.SetActive(false);
                m_Form.m_SynthItemViews[index].SetOnClickDelegate(null);
                m_Form.m_SynthItemCountLbls[index].text = string.Empty;
            }

            private void ShowSynthItemViewGroup(int index, int itemId, int itemExpectedCount)
            {
                m_Form.m_SynthArrows[index].gameObject.SetActive(true);
                m_Form.m_SynthItemViews[index].gameObject.SetActive(true);
                m_Form.m_SynthItemViews[index].InitItem(itemId, (QualityType)GeneralItemUtility.GetGeneralItemQuality(itemId));
                m_Form.m_SynthItemViews[index].SetOnClickDelegate(delegate ()
                {
                    AppendSynthPath(itemId);
                    UpdateSynthGraphOrWhereToGetView();
                });
                m_Form.m_SynthItemCountLbls[index].text = GetItemCountString(itemId, itemExpectedCount);
            }

            private string GetItemCountString(int itemId, int itemExpectedCount)
            {
                int count = GeneralItemUtility.GetGeneralItemCount(itemId);
                string formatKey = count < itemExpectedCount ? "UI_TEXT_SLASH_RED" : "UI_TEXT_SLASH";
                return GameEntry.Localization.GetString(formatKey, count, itemExpectedCount);
            }

            private SynthPathViewItem GetOrCreateSynthPathViewItem(int index)
            {
                return GetSynthPathViewItem(index) ?? CreateSynthPathViewItem();
            }

            private bool CheckSynthItemIds(IList<int> itemIds)
            {
                var dt = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>();
                for (int i = 0; i < itemIds.Count; ++i)
                {
                    if (!dt.HasDataRow(itemIds[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            public SynthPathViewItem GetSynthPathViewItem(int index)
            {
                if (index < m_CachedSynthPathViewItems.Count)
                {
                    var item = m_CachedSynthPathViewItems[index];
                    item.gameObject.SetActive(true);
                    return item;
                }

                return null;
            }

            private SynthPathViewItem CreateSynthPathViewItem()
            {
                SynthPathViewItem item = NGUITools.AddChild(m_Form.m_SynthPathListView.gameObject, m_Form.m_SynthPathViewItemTemplate)
                    .GetComponent<SynthPathViewItem>();
                m_CachedSynthPathViewItems.Add(item);
                item.gameObject.SetActive(true);
                return item;
            }

            private void RecycleSynthPathViewItems(int fromIndex)
            {
                m_CurrentSynthPathLen = Mathf.Clamp(fromIndex, 0, m_CurrentSynthPathLen);
                for (int i = fromIndex; i < m_CachedSynthPathViewItems.Count; ++i)
                {
                    m_CachedSynthPathViewItems[i].gameObject.SetActive(false);
                }
            }
        }

        private static StrategyBase GetStrategy(Scenario scenario, GeneralItemInfoForm2_WithWhereToGet form)
        {
            switch (scenario)
            {
                case Scenario.Default:
                    return new StrategyDefault().InitForm(form);
                case Scenario.HeroQualityItem:
                default:
                    return new StrategyHeroQualityItem().InitForm(form);
            }
        }
    }
}
