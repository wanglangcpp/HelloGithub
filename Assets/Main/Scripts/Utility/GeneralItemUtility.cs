using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 物品工具类。
    /// </summary>
    public static class GeneralItemUtility
    {
        /// <summary>
        /// 获取物品名称。
        /// </summary>
        /// <param name="id">物品编号。</param>
        /// <returns>物品名称。</returns>
        public static string GetGeneralItemName(int id)
        {
            GeneralItemType type = GetGeneralItemType(id);
            switch (type)
            {
                case GeneralItemType.Gear:
                    {
                        var dtGear = GameEntry.DataTable.GetDataTable<DRGear>();
                        DRGear drGear = dtGear.GetDataRow(id);
                        if (drGear != null)
                        {
                            return drGear.Name;
                        }
                    }
                    break;
                case GeneralItemType.Soul:
                    {
                        var dtSoul = GameEntry.DataTable.GetDataTable<DRSoul>();
                        DRSoul drSoul = dtSoul.GetDataRow(id);
                        if (drSoul != null)
                        {
                            return drSoul.Name;
                        }
                    }
                    break;
                case GeneralItemType.Epigraph:
                    {
                        var dtEpigraph = GameEntry.DataTable.GetDataTable<DREpigraph>();
                        DREpigraph drEpigraph = dtEpigraph.GetDataRow(id);
                        if (drEpigraph != null)
                        {
                            return drEpigraph.Name;
                        }
                    }
                    break;
                case GeneralItemType.Item:
                    {
                        var dtItem = GameEntry.DataTable.GetDataTable<DRItem>();
                        DRItem drItem = dtItem.GetDataRow(id);
                        if (drItem != null)
                        {
                            return drItem.Name;
                        }
                    }
                    break;
                case GeneralItemType.QualityItem:
                    {
                        var dtItem = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>();
                        DRHeroQualityItem drItem = dtItem.GetDataRow(id);
                        if (drItem != null)
                        {
                            return drItem.Name;
                        }
                    }
                    break;
                case GeneralItemType.SkillBadge:
                    {
                        var dr = GetSkillBadgeDataRow(id);
                        if (dr != null)
                        {
                            return dr.Name;
                        }
                    }
                    break;
                case GeneralItemType.ChargeItem:
                    {
                        //var dtItem = GameEntry.DataTable.GetDataTable<DRCharge>();
                        //DRCharge drItem = dtItem.GetDataRow(id);
                        ChargeInfo drItem = GameEntry.Data.ChargeTable.Find(x => x.Id == id);
                        if (drItem != null)
                        {
                            return drItem.Name;
                        }
                    }
                    break;
            }

            Log.Warning("Can not get general item name id by type id '{0}'.", id.ToString());
            return string.Empty;
        }

        /// <summary>
        /// 获取物品描述。
        /// </summary>
        /// <param name="id">物品编号。</param>
        /// <returns>物品描述。</returns>
        public static string GetGeneralItemDescription(int id)
        {
            GeneralItemType type = GetGeneralItemType(id);
            switch (type)
            {
                case GeneralItemType.Gear:
                    {
                        var dtGear = GameEntry.DataTable.GetDataTable<DRGear>();
                        DRGear drGear = dtGear.GetDataRow(id);
                        if (drGear != null)
                        {
                            return drGear.Description;
                        }
                    }
                    break;
                case GeneralItemType.Soul:
                    {
                        var dtSoul = GameEntry.DataTable.GetDataTable<DRSoul>();
                        DRSoul drSoul = dtSoul.GetDataRow(id);
                        if (drSoul != null)
                        {
                            return drSoul.Description;
                        }
                    }
                    break;
                case GeneralItemType.Epigraph:
                    {
                        var dtEpigraph = GameEntry.DataTable.GetDataTable<DREpigraph>();
                        DREpigraph drEpigraph = dtEpigraph.GetDataRow(id);
                        if (drEpigraph != null)
                        {
                            return drEpigraph.Description;
                        }
                    }
                    break;
                case GeneralItemType.Item:
                    {
                        var dtItem = GameEntry.DataTable.GetDataTable<DRItem>();
                        DRItem drItem = dtItem.GetDataRow(id);
                        if (drItem != null)
                        {
                            return drItem.Description;
                        }
                    }
                    break;
                case GeneralItemType.QualityItem:
                    {
                        var dtItem = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>();
                        DRHeroQualityItem drItem = dtItem.GetDataRow(id);
                        if (drItem != null)
                        {
                            return drItem.Description;
                        }
                    }
                    break;
                case GeneralItemType.SkillBadge:
                    {
                        var dr = GetSkillBadgeDataRow(id);
                        if (dr != null)
                        {
                            return dr.Description;
                        }
                    }
                    break;
                case GeneralItemType.ChargeItem:
                    {
                        //var dtEpigraph = GameEntry.DataTable.GetDataTable<DRCharge>();
                        //DRCharge drEpigraph = dtEpigraph.GetDataRow(id);
                        ChargeInfo drEpigraph = GameEntry.Data.ChargeTable.Find(x => x.Id == id);
                        if (drEpigraph != null)
                        {
                            return drEpigraph.Description;
                        }
                    }
                    break;
            }

            Log.Warning("Can not get general item description id by type id '{0}'.", id.ToString());
            return string.Empty;
        }

        /// <summary>
        /// 获取物品类型。
        /// </summary>
        /// <param name="id">物品编号。</param>
        /// <returns>物品类型。</returns>
        public static GeneralItemType GetGeneralItemType(int id)
        {
            if (id >= Constant.GeneralItem.MinSkillBadgeId && id <= Constant.GeneralItem.MaxSkillBadgeId)
            {
                return GeneralItemType.SkillBadge;
            }

            if (id >= Constant.GeneralItem.MinSoulId && id <= Constant.GeneralItem.MaxSoulId)
            {
                return GeneralItemType.Soul;
            }

            if (id >= Constant.GeneralItem.MinEpigraphId && id <= Constant.GeneralItem.MaxEpigraphId)
            {
                return GeneralItemType.Epigraph;
            }

            if (id >= Constant.GeneralItem.MinHeroQualityItemId && id <= Constant.GeneralItem.MaxHeroQualityItemId)
            {
                return GeneralItemType.QualityItem;
            }
            if (id >= Constant.GeneralItem.MinChargeItem && id <= Constant.GeneralItem.MaxChargeItem)
            {
                return GeneralItemType.ChargeItem;
            }
            return GeneralItemType.Item;
        }

        /// <summary>
        /// 获取技能徽章背景。
        /// </summary>
        /// <param name="category">徽章类型。</param>
        /// <returns></returns>
        public static string GetSkillBadgeBgSpriteName(SkillBadgeSlotCategory category, int level)
        {
            switch (category)
            {
                case SkillBadgeSlotCategory.Specific:
                    return string.Format("SkillBadgeBg_Specific{0}", level.ToString());
                case SkillBadgeSlotCategory.Generic:
                default:
                    return string.Format("SkillBadgeBg_Generic{0}", level.ToString());
            }
        }

        /// <summary>
        /// 获取技能徽章类别。
        /// </summary>
        /// <param name="badgeId">徽章编号。</param>
        /// <returns>技能徽章类别。</returns>
        public static SkillBadgeSlotCategory GetSkillBadgeCateogry(int badgeId)
        {
            if (badgeId >= Constant.GeneralItem.MinGenericSkillBadgeId && badgeId <= Constant.GeneralItem.MaxGenericSkillBadgeId)
            {
                return SkillBadgeSlotCategory.Generic;
            }

            if (badgeId >= Constant.GeneralItem.MinSpecificSkillBadgeId && badgeId <= Constant.GeneralItem.MaxSpecificSkillBadgeId)
            {
                return SkillBadgeSlotCategory.Specific;
            }


            Log.Warning("Badge ID '{0}' is out of range.", badgeId.ToString());
            return SkillBadgeSlotCategory.Undefined;
        }

        /// <summary>
        /// 获取物品品质。
        /// </summary>
        /// <param name="id">物品编号。</param>
        /// <returns>物品品质。</returns>
        public static int GetGeneralItemQuality(int id)
        {
            GeneralItemType type = GetGeneralItemType(id);
            switch (type)
            {
                case GeneralItemType.Gear:
                    {
                        var dtGear = GameEntry.DataTable.GetDataTable<DRGear>();
                        DRGear drGear = dtGear.GetDataRow(id);
                        if (drGear != null)
                        {
                            return drGear.Quality;
                        }
                    }
                    break;
                case GeneralItemType.Soul:
                    {
                        var dtSoul = GameEntry.DataTable.GetDataTable<DRSoul>();
                        DRSoul drSoul = dtSoul.GetDataRow(id);
                        if (drSoul != null)
                        {
                            return drSoul.Quality;
                        }
                    }
                    break;
                case GeneralItemType.Epigraph:
                    {
                        var dtEpigraph = GameEntry.DataTable.GetDataTable<DREpigraph>();
                        DREpigraph drEpigraph = dtEpigraph.GetDataRow(id);
                        if (drEpigraph != null)
                        {
                            return drEpigraph.Quality;
                        }
                    }
                    break;
                case GeneralItemType.Item:
                    {
                        var dtItem = GameEntry.DataTable.GetDataTable<DRItem>();
                        DRItem drItem = dtItem.GetDataRow(id);
                        if (drItem != null)
                        {
                            return drItem.Quality;
                        }
                    }
                    break;
                case GeneralItemType.QualityItem:
                    {
                        var dtItem = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>();
                        DRHeroQualityItem drItem = dtItem.GetDataRow(id);
                        if (drItem != null)
                        {
                            return drItem.Quality;
                        }
                    }
                    break;
                case GeneralItemType.SkillBadge:
                    {
                        var drItem = GetSkillBadgeDataRow(id);
                        if (drItem != null)
                        {
                            return drItem.Level;
                        }
                    }
                    break;
            }

            Log.Warning("Can not get general item quality by type id '{0}'.", id.ToString());
            return 0;
        }

        /// <summary>
        /// 获取物品品质颜色。
        /// </summary>
        /// <param name="id">物品编号。</param>
        /// <returns>物品品质颜色。</returns>
        public static Color GetGeneralItemQualityColor(int id)
        {
            return ColorUtility.GetColorForQuality(GetGeneralItemQuality(id));
        }

        /// <summary>
        /// 获取物品图标编号。
        /// </summary>
        /// <param name="id">物品编号。</param>
        /// <returns>物品图标编号。</returns>
        public static int GetGeneralItemIconId(int id)
        {
            GeneralItemType goodType = GetGeneralItemType(id);
            switch (goodType)
            {
                case GeneralItemType.Gear:
                    {
                        var dtGear = GameEntry.DataTable.GetDataTable<DRGear>();
                        DRGear drGear = dtGear.GetDataRow(id);
                        if (drGear != null)
                        {
                            return drGear.IconId;
                        }
                    }
                    break;
                case GeneralItemType.Soul:
                    {
                        var dtSoul = GameEntry.DataTable.GetDataTable<DRSoul>();
                        DRSoul drSoul = dtSoul.GetDataRow(id);
                        if (drSoul != null)
                        {
                            return drSoul.IconId;
                        }
                    }
                    break;
                case GeneralItemType.Epigraph:
                    {
                        var dtEpigraph = GameEntry.DataTable.GetDataTable<DREpigraph>();
                        DREpigraph drEpigraph = dtEpigraph.GetDataRow(id);
                        if (drEpigraph != null)
                        {
                            return drEpigraph.IconId;
                        }
                    }
                    break;
                case GeneralItemType.Item:
                    {
                        var dtItem = GameEntry.DataTable.GetDataTable<DRItem>();
                        DRItem drItem = dtItem.GetDataRow(id);
                        if (drItem != null)
                        {
                            return drItem.IconId;
                        }
                    }
                    break;
                case GeneralItemType.QualityItem:
                    {
                        var dtItem = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>();
                        DRHeroQualityItem drItem = dtItem.GetDataRow(id);
                        if (drItem != null)
                        {
                            return drItem.IconId;
                        }
                    }
                    break;
                case GeneralItemType.SkillBadge:
                    return GetSkillBadgeIconId(id);

                case GeneralItemType.ChargeItem:
                    {
                        //var dtItem = GameEntry.DataTable.GetDataTable<DRCharge>();
                        //DRCharge drItem = dtItem.GetDataRow(id);
                        ChargeInfo drItem = GameEntry.Data.ChargeTable.Find(x => x.Id == id);
                        if (drItem != null)
                        {
                            return drItem.IconId;
                        }
                    }
                    break;
            }

            Log.Warning("Can not get general item icon id by type id '{0}'.", id.ToString());
            return 0;
        }

        public static int GetGeneralItemCount(int id)
        {
            GeneralItemType goodType = GetGeneralItemType(id);
            switch (goodType)
            {
                case GeneralItemType.ChargeItem:
                case GeneralItemType.Gear:
                case GeneralItemType.Soul:
                case GeneralItemType.Epigraph:
                    throw new System.NotSupportedException();
                case GeneralItemType.Item:
                    {
                        ItemData itemData = GameEntry.Data.Items.GetData(id);
                        return itemData == null ? 0 : itemData.Count;
                    }
                case GeneralItemType.QualityItem:
                    {
                        ItemData itemData = GameEntry.Data.HeroQualityItems.GetData(id);
                        return itemData == null ? 0 : itemData.Count;
                    }
                case GeneralItemType.SkillBadge:
                    {
                        ItemData itemData = GameEntry.Data.SkillBadgeItems.GetData(id);
                        return itemData == null ? 0 : itemData.Count;
                    }
            }

            Log.Warning("Can not get general item icon id by type id '{0}'.", id.ToString());
            return 0;
        }

        public static List<GearDataWithHero> GetAllGearData()
        {
            List<GearDataWithHero> data = new List<GearDataWithHero>();
            return data;
        }

        public static List<SoulDataWithHero> GetAllSoulData()
        {
            List<SoulDataWithHero> data = new List<SoulDataWithHero>();
            return data;
        }

        /// <summary>
        /// 获取道具类型。
        /// </summary>
        /// <param name="id">道具编号。</param>
        /// <returns>道具类型。</returns>
        public static ItemType GetItemType(int id)
        {
            var dt = GameEntry.DataTable.GetDataTable<DRItem>();
            DRItem dr = dt.GetDataRow(id);
            if (dr == null)
            {
                Log.Warning("Item '{0}' not found.", id.ToString());
                return ItemType.OtherItem;
            }

            return (ItemType)dr.Type;
        }

        /// <summary>
        /// 获取装备位置。
        /// </summary>
        /// <param name="typeId">种类编号。</param>
        /// <returns>装备位置。</returns>
        public static GearPosition GetGearPosition(int typeId)
        {
            var dt = GameEntry.DataTable.GetDataTable<DRGear>();
            DRGear dr = dt.GetDataRow(typeId);
            if (dr == null)
            {
                Log.Warning("Gear '{0}' not found.", typeId.ToString());
                return GearPosition.Armor;
            }

            return (GearPosition)dr.Type;
        }

        /// <summary>
        /// 更新道具数据。
        /// </summary>
        /// <param name="itemInfo">原始数据。</param>
        public static void UpdateItemsData(PBItemInfo itemInfo)
        {
            if (itemInfo == null)
            {
                return;
            }

            UpdateItemsData(new PBItemInfo[] { itemInfo });
        }

        /// <summary>
        /// 更新道具数据。
        /// </summary>
        /// <param name="itemInfos">原始数据。</param>
        public static void UpdateItemsData(IList<PBItemInfo> itemInfos)
        {
            var normalItemsData = GameEntry.Data.Items;
            var heroQualityItemsData = GameEntry.Data.HeroQualityItems;
            var skillBadgeItemsData = GameEntry.Data.SkillBadgeItems;

            for (int i = 0; i < itemInfos.Count; ++i)
            {
                var itemInfo = itemInfos[i];
                ItemsData itemsData = null;
                if (itemInfo.Type >= Constant.GeneralItem.MinHeroQualityItemId && itemInfo.Type <= Constant.GeneralItem.MaxHeroQualityItemId)
                {
                    itemsData = heroQualityItemsData;
                }
                else if (itemInfo.Type >= Constant.GeneralItem.MinSkillBadgeId && itemInfo.Type <= Constant.GeneralItem.MaxSkillBadgeId)
                {
                    itemsData = skillBadgeItemsData;
                }
                else
                {
                    itemsData = normalItemsData;
                }

                var itemData = itemsData.GetData(itemInfo.Type);
                if (itemData == null)
                {
                    if (itemInfo.Count > 0)
                    {
                        itemsData.AddData(itemInfo);
                    }

                    continue;
                }

                if (itemInfo.Count > 0)
                {
                    itemData.UpdateData(itemInfo);
                }
                else
                {
                    itemsData.RemoveData(itemInfo.Type);
                }
            }
        }

        /// <summary>
        /// 获取徽章数据行。
        /// </summary>
        /// <param name="itemId">徽章编号。</param>
        /// <returns>徽章数据行。</returns>
        public static DRBaseSkillBadge GetSkillBadgeDataRow(int itemId)
        {
            var dataTableComponent = GameEntry.DataTable;

            if (itemId >= Constant.GeneralItem.MinGenericSkillBadgeId && itemId <= Constant.GeneralItem.MaxGenericSkillBadgeId)
            {
                return dataTableComponent.GetDataTable<DRGenericSkillBadge>().GetDataRow(itemId);
            }

            if (itemId >= Constant.GeneralItem.MinSpecificSkillBadgeId && itemId <= Constant.GeneralItem.MaxSpecificSkillBadgeId)
            {
                return dataTableComponent.GetDataTable<DRSpecificSkillBadge>().GetDataRow(itemId);
            }

            return null;
        }

        /// <summary>
        /// 获取徽章图标编号。
        /// </summary>
        /// <param name="itemId">徽章编号。</param>
        /// <returns>徽章图标编号。</returns>
        public static int GetSkillBadgeIconId(int itemId)
        {
            var dataTableComponent = GameEntry.DataTable;

            if (itemId >= Constant.GeneralItem.MinGenericSkillBadgeId && itemId <= Constant.GeneralItem.MaxGenericSkillBadgeId)
            {
                var dr = dataTableComponent.GetDataTable<DRGenericSkillBadge>().GetDataRow(itemId);
                if (dr == null)
                {
                    Log.Warning("Cannot get icon ID for skill badge '{0}'.", itemId.ToString());
                    return 0;
                }

                return dr.IconId;
            }

            if (itemId >= Constant.GeneralItem.MinSpecificSkillBadgeId && itemId <= Constant.GeneralItem.MaxSpecificSkillBadgeId)
            {
                var dr = dataTableComponent.GetDataTable<DRSpecificSkillBadge>().GetDataRow(itemId);
                if (dr == null)
                {
                    Log.Warning("Cannot get icon ID for skill badge '{0}'.", itemId.ToString());
                    return 0;
                }

                int skillGroupId = dr.OriginalSkillGroupId;
                var drSkillGroup = dataTableComponent.GetDataTable<DRSkillGroup>().GetDataRow(skillGroupId);
                if (drSkillGroup == null)
                {
                    Log.Warning("Cannot get icon ID for skill badge '{0}'.", itemId.ToString());
                    return 0;
                }

                var skillId = drSkillGroup.SkillId;
                var drSkill = dataTableComponent.GetDataTable<DRSkill>().GetDataRow(skillId);
                if (drSkill == null)
                {
                    Log.Warning("Cannot get icon ID for skill badge '{0}'.", itemId.ToString());
                    return 0;
                }

                return drSkill.IconId;
            }

            Log.Warning("Cannot get icon ID for skill badge '{0}'.", itemId.ToString());
            return 0;
        }
    }
}
