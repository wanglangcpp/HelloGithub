using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ReminderComponent : MonoBehaviour
    {
        private Dictionary<int, HeroGearReminder> m_HeroGearReminders = new Dictionary<int, HeroGearReminder>();

        /// <summary>
        /// 判断装备是否可强化。
        /// </summary>
        /// <param name="heroTypeId">英雄种类编号。</param>
        /// <param name="gearPosition">装备位置。</param>
        /// <returns>装备是否可强化</returns>
        public bool GearCanBeStrengthened(int heroTypeId, GearPosition gearPosition)
        {
            HeroGearReminder r;
            if (!m_HeroGearReminders.TryGetValue(heroTypeId, out r) || r.StrengthenFlags == null)
            {
                return false;
            }

            bool ret = false;
            r.StrengthenFlags.TryGetValue(gearPosition, out ret);
            return ret;
        }

        /// <summary>
        /// 判断装备是否可升级。
        /// </summary>
        /// <param name="heroTypeId">英雄种类编号。</param>
        /// <param name="gearPosition">装备位置。</param>
        /// <returns>装备是否可升级。</returns>
        public bool GearCanLevelUp(int heroTypeId, GearPosition gearPosition)
        {
            HeroGearReminder r;
            if (!m_HeroGearReminders.TryGetValue(heroTypeId, out r) || r.LevelUpFlags == null)
            {
                return false;
            }

            bool ret = false;
            r.LevelUpFlags.TryGetValue(gearPosition, out ret);
            return ret;
        }

        private static void RefreshHeroGearLevelUpFlag(IDataTable<DRGearLevelUp> dtGearLevelUp, GearData gear, HeroGearReminder r)
        {
            DRGearLevelUp drGearLevelUp = dtGearLevelUp.GetDataRow(gear.Quality);
            if (drGearLevelUp == null)
            {
                return;
            }

            GearPosition gearPosition = (GearPosition)gear.Position;
            r.LevelUpFlags[gearPosition] = (gear.Level < GameEntry.Data.Player.Level)
                && (drGearLevelUp.LevelUpCostCoin[gear.Level - 1] < GameEntry.Data.Player.Coin);
        }

        private static void RefreshHeroGearStrengthenFlag(IDataTable<DRItem> dtItem, GearData gear, HeroGearReminder r)
        {
            int itemId = gear.StrengthenItemId;
            if (itemId <= 0)
            {
                return;
            }

            GearPosition gearPosition = (GearPosition)gear.Position;

            if (gear.StrengthenLevel >= Constant.MaxGearStrengthenLevel)
            {
                r.StrengthenFlags[gearPosition] = false;
                return;
            }

            DRItem materialData = dtItem.GetDataRow(itemId);
            if (materialData == null)
            {
                r.StrengthenFlags[gearPosition] = false;
                return;
            }

            var materialItem = GameEntry.Data.Items.GetData(itemId);
            r.StrengthenFlags[gearPosition] = (materialItem != null && materialItem.Count > 0);
        }

        private HeroGearReminder AssureHeroGearReminder(int heroTypeId)
        {
            HeroGearReminder r;
            if (!m_HeroGearReminders.TryGetValue(heroTypeId, out r))
            {
                r = new HeroGearReminder { HeroTypeId = heroTypeId };
                m_HeroGearReminders.Add(heroTypeId, r);
            }

            return r;
        }

        private class HeroGearReminder
        {
            internal int HeroTypeId = 0;
            internal Dictionary<GearPosition, bool> StrengthenFlags = new Dictionary<GearPosition, bool>();
            internal Dictionary<GearPosition, bool> LevelUpFlags = new Dictionary<GearPosition, bool>();

            public HeroGearReminder()
            {

            }
        }
    }
}
