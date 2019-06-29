using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// VIP配置表。
    /// </summary>
    public class DRVip : IDataRow
    {
        /// <summary>
        /// VIP等级。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 升级所需经验。
        /// </summary>
        public int LevelUpExp { get; private set; }

        /// <summary>
        /// 每日购买体力次数。
        /// </summary>
        public int ExchangeEnergyCount { get; private set; }

        /// <summary>
        /// 每日购买金币次数。
        /// </summary>
        public int ExchangeCoinCount { get; private set; }

        /// <summary>
        /// 每日免费离线竞技次数。
        /// </summary>
        public int FreeArenaCount { get; private set; }

        /// <summary>
        /// 每日可购买离线竞技次数。
        /// </summary>
        public int BuyArenaCount { get; private set; }

        /// <summary>
        /// 每日免费金币资源副本次数。
        /// </summary>
        public int FreeCoinResourceInstanceCount { get; private set; }

        /// <summary>
        /// 每日免费经验资源副本次数
        /// </summary>
        public int FreeExpResourceInstanceCount { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            LevelUpExp = int.Parse(text[index++]);
            ExchangeEnergyCount = int.Parse(text[index++]);
            ExchangeCoinCount = int.Parse(text[index++]);
            FreeArenaCount = int.Parse(text[index++]);
            BuyArenaCount = int.Parse(text[index++]);
            FreeCoinResourceInstanceCount = int.Parse(text[index++]);
            FreeExpResourceInstanceCount = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRVip>();
        }
    }
}
