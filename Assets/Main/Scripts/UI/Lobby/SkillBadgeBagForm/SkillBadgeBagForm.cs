using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class SkillBadgeBagForm : NGUIForm
    {
        private enum BadgeTabType
        {
            All = 0,
            Specific = 1,
            Bule = 2,
            Red = 3,
            Colorful = 4,
        }

        [SerializeField]
        private BadgeBagInfoSubForm m_BadgeBagInfoSubForm = null;

        [SerializeField]
        private SkillBadgeStageAdvanceSubForm m_SkillBadgeStageAdvanceSubForm = null;

        [SerializeField]
        private ScrollViewCache m_BadgeItems = null;

        [SerializeField]
        private UIGrid m_SkillBadgeGrid = null;

        [SerializeField]
        private UISprite m_BadgeItemGridBg = null;

        [SerializeField]
        private UILabel m_EmptyText = null;

        private SkillBadgeBagItem m_CurrentSelectedItem = null;

        private BadgeTabType m_CurrentTabType = BadgeTabType.All;

        private SkillBadgeStageAdvanceSubForm m_UsingSkillBadgeStageAdvance = null;

        private List<SkillBadgeDataForSort> m_SkillBadgeItems = new List<SkillBadgeDataForSort>();

        private List<SkillBadgeDataForSort> m_GenericBadgeItems = new List<SkillBadgeDataForSort>();

        private List<SkillBadgeDataForSort> m_SpecificBadgeItems = new List<SkillBadgeDataForSort>();

        private Dictionary<BadgeTabType, GameFrameworkAction> m_RefreshFuncs = new Dictionary<BadgeTabType, GameFrameworkAction>();

        protected override void OnOpen(object userData)
        {
            if (m_UsingSkillBadgeStageAdvance == null)
            {
                m_UsingSkillBadgeStageAdvance = CreateSubForm<SkillBadgeStageAdvanceSubForm>("BadgeAdvanceSubFormByBag", gameObject, m_SkillBadgeStageAdvanceSubForm.gameObject, false);
            }
            m_UsingSkillBadgeStageAdvance.InitSkillBadgeStageAdvance();

            GameEntry.Event.Subscribe(EventId.OnSkillBadgeBagDataChanged, OnSkillBadgeBagDataChanged);

            RefreshList();
            InitBadgeData();
            base.OnOpen(userData);
            m_RefreshFuncs[m_CurrentTabType]();
        }

        private void OnSkillBadgeBagDataChanged(object sender, GameEventArgs e)
        {
            m_SkillBadgeItems.Clear();
            m_GenericBadgeItems.Clear();
            m_SpecificBadgeItems.Clear();
            RefreshList();
            m_RefreshFuncs[m_CurrentTabType]();
            m_BadgeBagInfoSubForm.RefreshData(m_CurrentSelectedItem.BadgeId, m_CurrentSelectedItem.BadgeCount, ShowSkillBadgeStageAdvanceSubForm);
        }

        private void RefreshList()
        {
            List<ItemData> skillBadgeItems = GameEntry.Data.SkillBadgeItems.Data;

            for (int i = 0; i < skillBadgeItems.Count; i++)
            {
                m_SkillBadgeItems.Add(new SkillBadgeDataForSort { ItemData = skillBadgeItems[i] });
                if (GeneralItemUtility.GetSkillBadgeCateogry(skillBadgeItems[i].Type) == SkillBadgeSlotCategory.Specific)
                {
                    m_SpecificBadgeItems.Add(new SkillBadgeDataForSort { ItemData = skillBadgeItems[i] });
                }
                else
                {
                    m_GenericBadgeItems.Add(new SkillBadgeDataForSort { ItemData = skillBadgeItems[i] });
                }
            }
            m_SkillBadgeItems.Sort(CompareBadges);
        }

        private void InitBadgeData()
        {
            m_RefreshFuncs.Add(BadgeTabType.All, ShowAllBadge);
            m_RefreshFuncs.Add(BadgeTabType.Specific, ShowSpecificBadge);
            m_RefreshFuncs.Add(BadgeTabType.Bule, ShowBuleBadge);
            m_RefreshFuncs.Add(BadgeTabType.Red, ShowRedBadge);
            m_RefreshFuncs.Add(BadgeTabType.Colorful, ShowColorful);
        }

        private void ShowAllBadge()
        {
            if (m_SkillBadgeItems.Count == 0)
            {
                m_EmptyText.gameObject.SetActive(true);
                m_BadgeBagInfoSubForm.gameObject.SetActive(false);
                return;
            }

            AllBadgesSort();
            for (int i = 0; i < m_SkillBadgeItems.Count; i++)
            {
                SkillBadgeBagItem skillBadgeBagItem = GetOrCreateBadgeItem(i);
                skillBadgeBagItem.RefreshData(m_SkillBadgeItems[i].ItemData.Type, m_SkillBadgeItems[i].ItemData.Count, OnClickItem);
            }

            m_BadgeItems.RecycleItemsAtAndAfter(m_SkillBadgeItems.Count);
            m_BadgeItems[0].IsSelected = true;
            m_CurrentSelectedItem = m_BadgeItems[0];
            StartCoroutine(RepositionCo());
            m_EmptyText.gameObject.SetActive(false);
            m_BadgeBagInfoSubForm.gameObject.SetActive(true);
        }

        private IEnumerator RepositionCo()
        {
            yield return null;
            UIUtility.GridAutoAdaptScreen(m_BadgeItemGridBg.width, m_SkillBadgeGrid, true);
            m_BadgeItems.Reposition();
        }

        private void ShowSpecificBadge()
        {
            m_BadgeItems.RecycleAllItems();
            if (m_SpecificBadgeItems.Count == 0)
            {
                m_EmptyText.gameObject.SetActive(true);
                m_BadgeBagInfoSubForm.gameObject.SetActive(false);
                return;
            }

            TabBadgeSort(m_SpecificBadgeItems);
            for (int i = 0; i < m_SpecificBadgeItems.Count; i++)
            {
                SkillBadgeBagItem skillBadgeItem = m_BadgeItems.GetOrCreateItem(i);
                skillBadgeItem.gameObject.SetActive(true);
                skillBadgeItem.RefreshData(m_SpecificBadgeItems[i].ItemData.Type, m_SpecificBadgeItems[i].ItemData.Count, OnClickItem);
            }
            m_BadgeItems[0].IsSelected = true;
            m_CurrentSelectedItem = m_BadgeItems[0];
            StartCoroutine(RepositionCo());
            m_EmptyText.gameObject.SetActive(false);
            m_BadgeBagInfoSubForm.gameObject.SetActive(true);
        }

        private void ShowBuleBadge()
        {
            ShowItemByBadgeColor((int)GenericSkillBadgeColor.Blue);
        }

        private void ShowRedBadge()
        {
            ShowItemByBadgeColor((int)GenericSkillBadgeColor.Red);
        }

        private void ShowColorful()
        {
            ShowItemByBadgeColor((int)GenericSkillBadgeColor.Colorful);
        }

        public void OnToggleValueChanged(int keyValue, bool selected)
        {
            if (selected)
            {
                m_RefreshFuncs[(BadgeTabType)keyValue]();
                m_CurrentTabType = (BadgeTabType)keyValue;
                if (m_CurrentSelectedItem == null)
                {
                    return;
                }

                m_BadgeBagInfoSubForm.RefreshData(m_CurrentSelectedItem.BadgeId, m_CurrentSelectedItem.BadgeCount, ShowSkillBadgeStageAdvanceSubForm);
            }
        }

        private void ShowItemByBadgeColor(int colorId)
        {
            m_BadgeItems.RecycleAllItems();
            List<SkillBadgeDataForSort> skillBadgeDataForSorts = FiltrateGenericBadgeAndSort(colorId);
            if (skillBadgeDataForSorts.Count == 0)
            {
                m_EmptyText.gameObject.SetActive(true);
                m_BadgeBagInfoSubForm.gameObject.SetActive(false);
            }
            else
            {
                m_EmptyText.gameObject.SetActive(false);
                m_BadgeBagInfoSubForm.gameObject.SetActive(true);
                for (int i = 0; i < skillBadgeDataForSorts.Count; i++)
                {
                    SkillBadgeBagItem skillBadgeItem = m_BadgeItems.GetOrCreateItem(i);
                    skillBadgeItem.gameObject.SetActive(true);
                    skillBadgeItem.RefreshData(skillBadgeDataForSorts[i].ItemData.Type, skillBadgeDataForSorts[i].ItemData.Count, OnClickItem);
                }
                m_BadgeItems[0].IsSelected = true;
                m_CurrentSelectedItem = m_BadgeItems[0];
                StartCoroutine(RepositionCo());
            }
        }

        private void AllBadgesSort()
        {
            var dtGenericSkillBadge = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>();
            for (int i = 0; i < m_SkillBadgeItems.Count; i++)
            {
                if (GeneralItemUtility.GetSkillBadgeCateogry(m_SkillBadgeItems[i].ItemData.Type) == SkillBadgeSlotCategory.Specific)
                {
                    m_SkillBadgeItems[i].BadgeCategory = (int)SkillBadgeSlotCategory.Specific;
                }
                else
                {
                    var drGenericSkillBadge = dtGenericSkillBadge.GetDataRow(m_SkillBadgeItems[i].ItemData.Type);
                    m_SkillBadgeItems[i].BadgeCategory = (int)SkillBadgeSlotCategory.Generic;
                    m_SkillBadgeItems[i].Color = drGenericSkillBadge.ColorId;
                }
                var dtSpecificBadge = GeneralItemUtility.GetSkillBadgeDataRow(m_SkillBadgeItems[i].ItemData.Type);
                m_SkillBadgeItems[i].Level = dtSpecificBadge.Level;
            }
            m_SkillBadgeItems.Sort(CompareBadges);
        }

        private List<SkillBadgeDataForSort> FiltrateGenericBadgeAndSort(int colorId)
        {
            var dtGenericSkillBadge = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>();
            List<SkillBadgeDataForSort> skillBadges = new List<SkillBadgeDataForSort>();
            for (int i = 0; i < m_GenericBadgeItems.Count; i++)
            {
                var drGenericSkillBadge = dtGenericSkillBadge.GetDataRow(m_GenericBadgeItems[i].ItemData.Type);
                if (drGenericSkillBadge.ColorId == colorId)
                {
                    m_GenericBadgeItems[i].Level = drGenericSkillBadge.Level;
                    skillBadges.Add(m_GenericBadgeItems[i]);
                }
            }
            TabBadgeSort(skillBadges);
            return skillBadges;
        }

        private void TabBadgeSort(List<SkillBadgeDataForSort> badgeList)
        {
            if (badgeList != null)
            {
                for (int i = 0; i < badgeList.Count; i++)
                {
                    var dtSpecificBadge = GeneralItemUtility.GetSkillBadgeDataRow(badgeList[i].ItemData.Type);
                    badgeList[i].Level = dtSpecificBadge.Level;
                }
                badgeList.Sort(CompareBadges);
            }
        }

        private SkillBadgeBagItem GetOrCreateBadgeItem(int index)
        {
            var ret = m_BadgeItems.GetOrCreateItem(index);
            ret.gameObject.name = string.Format("Inventory Item {0:D3}", index);
            return ret;
        }

        public void OnClickItem(SkillBadgeBagItem skillBadgeBagItem)
        {
            m_CurrentSelectedItem.IsSelected = false;
            skillBadgeBagItem.IsSelected = true;
            m_CurrentSelectedItem = skillBadgeBagItem;
            m_BadgeBagInfoSubForm.RefreshData(m_CurrentSelectedItem.BadgeId, m_CurrentSelectedItem.BadgeCount, ShowSkillBadgeStageAdvanceSubForm);
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

        protected override void OnClose(object userData)
        {
            m_SkillBadgeItems.Clear();
            m_GenericBadgeItems.Clear();
            m_SpecificBadgeItems.Clear();
            m_RefreshFuncs.Clear();
            GameEntry.Event.Unsubscribe(EventId.OnSkillBadgeBagDataChanged, OnSkillBadgeBagDataChanged);
            base.OnClose(userData);
        }

        private int CompareBadges(SkillBadgeDataForSort x, SkillBadgeDataForSort y)
        {
            if (x.BadgeCategory != y.BadgeCategory)
            {
                return x.BadgeCategory.CompareTo(y.BadgeCategory);
            }

            if (x.Level != y.Level)
            {
                return y.Level.CompareTo(x.Level);
            }

            if (x.Color != y.Color)
            {
                return x.Color.CompareTo(y.Color);
            }

            return x.ItemData.Type.CompareTo(y.ItemData.Type);
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<SkillBadgeBagItem>
        {

        }

        private class SkillBadgeDataForSort
        {
            public ItemData ItemData;
            public int BadgeCategory;
            public int Level;
            public int Color;
        }
    }
}