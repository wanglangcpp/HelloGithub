using UnityEngine;
using System;
using System.Collections.Generic;
using GameFramework;

namespace Genesis.GameClient
{
    public class SkillBadgeReplaceSubForm : NGUISubForm
    {
        [SerializeField]
        private ScrollViewCache m_SkillBadgeReplaceSubFormItems = null;

        [SerializeField]
        private UIScrollView m_BadgesScrollView = null;

        [SerializeField]
        private UILabel m_WithoutBadgeText = null;

        [SerializeField]
        private UIButton m_AchieveBtn = null;

        [SerializeField]
        private UILabel m_AchieveBtnText = null;

        [SerializeField]
        private List<SkillBadgeDataForSort> m_SpecificSkillBadges = new List<SkillBadgeDataForSort>();

        [SerializeField]
        private List<SkillBadgeDataForSort> m_GenericSkillBadges = new List<SkillBadgeDataForSort>();

        [SerializeField]
        private int m_HeroId = 0;

        [SerializeField]
        private int m_SkillIndex = 0;

        [SerializeField]
        private int m_SkillBadgeSlotCategory = 0;

        [SerializeField]
        private int m_GenericBadgeSlotIndex = 0;

        private int CurrentBadgeId
        {
            get
            {
                var lobbyHeroData = GameEntry.Data.LobbyHeros.GetData(m_HeroId);
                if (lobbyHeroData == null)
                {
                    return -1;
                }

                var skillBadges = lobbyHeroData.GetSkillBadge(m_SkillIndex);

                switch (m_SkillBadgeSlotCategory)
                {
                    case (int)SkillBadgeSlotCategory.Specific:
                        return skillBadges.SpecificBadge.BadgeId;
                    case (int)SkillBadgeSlotCategory.Generic:
                    default:
                        return skillBadges.GenericBadges[m_GenericBadgeSlotIndex].BadgeId;
                }
            }
        }

        public void InitSkillBadgeReplace()
        {
            gameObject.SetActive(false);
        }

        public void RefreshData(int heroId, int skillIndex, int skillGroupId, int skillBadgeSlotCategory, int genericBadgeSlotIndex, int skillElementId = -1, int genericSkillBadgeSlotColor = 0)
        {
            var itemDatas = GameEntry.Data.SkillBadgeItems.Data;
            m_HeroId = heroId;
            m_SkillIndex = skillIndex;
            m_SkillBadgeSlotCategory = skillBadgeSlotCategory;
            m_GenericBadgeSlotIndex = genericBadgeSlotIndex;
            m_SpecificSkillBadges.Clear();
            m_GenericSkillBadges.Clear();
            for (int i = 0; i < itemDatas.Count; i++)
            {
                if (itemDatas[i].Type >= Constant.GeneralItem.MinSpecificSkillBadgeId && itemDatas[i].Type <= Constant.GeneralItem.MaxSpecificSkillBadgeId)
                {
                    m_SpecificSkillBadges.Add(new SkillBadgeDataForSort { ItemData = itemDatas[i] });
                }
                else
                {
                    m_GenericSkillBadges.Add(new SkillBadgeDataForSort { ItemData = itemDatas[i] });
                }
            }
            if (skillBadgeSlotCategory == (int)SkillBadgeSlotCategory.Specific)
            {
                var dtSpecificSkillBadge = GameEntry.DataTable.GetDataTable<DRSpecificSkillBadge>();
                for (int i = 0; i < m_SpecificSkillBadges.Count; i++)
                {
                    var drSpecificSkillBadge = dtSpecificSkillBadge.GetDataRow(m_SpecificSkillBadges[i].ItemData.Type);
                    if (drSpecificSkillBadge == null)
                    {
                        Log.Warning("cannot find drSpecificSkillBadge by '{0}'.", m_SpecificSkillBadges[i].ItemData.Type);
                        continue;
                    }

                    if (drSpecificSkillBadge.OriginalSkillGroupId != skillGroupId)
                    {
                        QuickRemoveBadge(m_SpecificSkillBadges, ref i);
                        continue;
                    }

                    m_SpecificSkillBadges[i].Level = drSpecificSkillBadge.Level;
                }
                m_SpecificSkillBadges.Sort(CompareBadges);
                CreateItems(m_SpecificSkillBadges, skillBadgeSlotCategory);
                OnItemsNull(m_SpecificSkillBadges.Count);
            }
            else
            {
                FiltrateGenericSkillBadge((GenericSkillBadgeSlotColor)genericSkillBadgeSlotColor, skillElementId);
                CreateItems(m_GenericSkillBadges, skillBadgeSlotCategory);
                OnItemsNull(m_GenericSkillBadges.Count);
            }
        }

        private void OnItemsNull(int itemsCount)
        {
            if (itemsCount == 0)
            {
                m_WithoutBadgeText.text = GameEntry.Localization.GetString("UI_TEXT_WIHTOUT_PADGE");
                m_AchieveBtnText.text = GameEntry.Localization.GetString("UI_BUTTON_ACHIEVE");
                m_WithoutBadgeText.gameObject.SetActive(true);
                m_AchieveBtn.gameObject.SetActive(true);
            }
            else
            {
                m_WithoutBadgeText.gameObject.SetActive(false);
                m_AchieveBtn.gameObject.SetActive(false);
            }
        }

        private void FiltrateGenericSkillBadge(GenericSkillBadgeSlotColor genericSkillBadgeSlotColor, int skillElementId)
        {
            var dtGenericSkillBadge = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>();
            for (int i = 0; i < m_GenericSkillBadges.Count; i++)
            {
                var drGenericSkillBadge = dtGenericSkillBadge.GetDataRow(m_GenericSkillBadges[i].ItemData.Type);
                if (drGenericSkillBadge == null)
                {
                    Log.Warning("Cannot find drSpecificSkillBadge by '{0}'.", m_GenericSkillBadges[i].ItemData.Type);
                    continue;
                }

                if (!CheckBadgeAndSlotColorMatch((GenericSkillBadgeColor)drGenericSkillBadge.ColorId, genericSkillBadgeSlotColor))
                {
                    QuickRemoveBadge(m_GenericSkillBadges, ref i);
                    continue;
                }

                if (drGenericSkillBadge.ColorId == (int)GenericSkillBadgeColor.Colorful)
                {
                    if (drGenericSkillBadge.ElementId != (int)HeroElementType.All && skillElementId != drGenericSkillBadge.ElementId)
                    {
                        QuickRemoveBadge(m_GenericSkillBadges, ref i);
                        continue;
                    }
                }
                else
                {
                    if ((m_SkillIndex == Constant.DodgeSkillIndex) != drGenericSkillBadge.IsDodge)
                    {
                        QuickRemoveBadge(m_GenericSkillBadges, ref i);
                        continue;
                    }
                }

                m_GenericSkillBadges[i].Level = drGenericSkillBadge.Level;
                m_GenericSkillBadges[i].Color = drGenericSkillBadge.ColorId;
            }

            m_GenericSkillBadges.Sort(CompareBadges);
        }

        private int CompareBadges(SkillBadgeDataForSort x, SkillBadgeDataForSort y)
        {
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

        private static void QuickRemoveBadge(IList<SkillBadgeDataForSort> badgeList, ref int index)
        {
            if (index != badgeList.Count - 1)
            {
                var tmp = badgeList[index];
                badgeList[index] = badgeList[badgeList.Count - 1];
                badgeList[badgeList.Count - 1] = tmp;
            }

            badgeList.RemoveAt(badgeList.Count - 1);
            index--;
        }

        private void CreateItems(List<SkillBadgeDataForSort> itemDatas, int skillBadgeSlotCategory)
        {
            if (itemDatas.Count != 0)
            {
                for (int i = 0; i < itemDatas.Count; i++)
                {
                    if (itemDatas[i].ItemData.Count <= 0) continue;

                    var skillBadgeReplaceSubFormItem = m_SkillBadgeReplaceSubFormItems.GetOrCreateItem(i, (go) => { go.name = go.name+i; }, (go) => { go.name = go.name + i; });
                    skillBadgeReplaceSubFormItem.RefreshData(skillBadgeSlotCategory, itemDatas[i].ItemData.Type, itemDatas[i].ItemData.Count, OnClickItem);
                }

                m_SkillBadgeReplaceSubFormItems.Reposition();
            }
        }

        private bool CheckBadgeAndSlotColorMatch(GenericSkillBadgeColor badgeColor, GenericSkillBadgeSlotColor slotColor)
        {
            if (badgeColor < 0 || slotColor < 0)
            {
                return false;
            }

            if (badgeColor == GenericSkillBadgeColor.Blue && (slotColor == GenericSkillBadgeSlotColor.Blue || slotColor == GenericSkillBadgeSlotColor.Purple))
            {
                return true;
            }

            if (badgeColor == GenericSkillBadgeColor.Red && (slotColor == GenericSkillBadgeSlotColor.Red || slotColor == GenericSkillBadgeSlotColor.Purple))
            {
                return true;
            }

            if (badgeColor == GenericSkillBadgeColor.Colorful && slotColor == GenericSkillBadgeSlotColor.Colorful)
            {
                return true;
            }

            return false;
        }

        public void OnClickItem(int badgeId)
        {
            if (badgeId != CurrentBadgeId)
            {
                GameEntry.LobbyLogic.InsertSkillBadge(m_HeroId, m_SkillIndex, m_SkillBadgeSlotCategory, m_GenericBadgeSlotIndex, badgeId);
            }

            InternalClose();
        }

        protected internal override void OnOpen()
        {
            base.OnOpen();
        }

        protected internal override void OnClose()
        {
            m_BadgesScrollView.ResetPosition();
            m_SkillBadgeReplaceSubFormItems.DestroyAllItems();
            base.OnClose();
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<SkillBadgeReplaceSubFormItem>
        {

        }

        private class SkillBadgeDataForSort
        {
            public ItemData ItemData;
            public int Level;
            public int Color;
        }
    }
}