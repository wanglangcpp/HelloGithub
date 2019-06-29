using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 技能徽章装备位开启条件表。
    /// </summary>
    public class DRSkillBadgeSlotUnlockCondition : IDataRow
    {
        /// <summary>
        /// 条件编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 是否仅能用钻石开启。
        /// </summary>
        public bool MoneyOnly { get; private set; }

        /// <summary>
        /// 直接开启消耗的钻石数量。
        /// </summary>
        public int DirectUnlockMoneyCost { get; private set; }

        /// <summary>
        /// 技能等级。
        /// </summary>
        public int SkillLevel { get; private set; }

        /// <summary>
        /// 英雄总品阶。
        /// </summary>
        public int HeroTotalQualityLevel { get; private set; }

        /// <summary>
        /// 英雄星级。
        /// </summary>
        public int HeroStarLevel { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            MoneyOnly = bool.Parse(text[index++]);
            DirectUnlockMoneyCost = int.Parse(text[index++]);
            SkillLevel = int.Parse(text[index++]);
            HeroTotalQualityLevel = int.Parse(text[index++]);
            HeroStarLevel = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSkillBadgeSlotUnlockCondition>();
        }
    }
}
