using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 抽奖刷新配置表。
    /// </summary>
    public class DRChanceRefresh : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 抽奖类型。
        /// </summary>
        public int ChanceType { get; private set; }

        /// <summary>
        /// 抽奖刷新时间（UTC时间）。
        /// </summary>
        public string RefreshUtcTime { get; private set; }

        /// <summary>
        /// 赠予免费抽奖次数间隔秒数。
        /// </summary>
        public int GiveFreeInterval { get; private set; }

        /// <summary>
        /// 赠予抽奖免费次数。
        /// </summary>
        public int GiveFreeCount { get; private set; }

        /// <summary>
        /// 使用免费抽奖间隔秒数。
        /// </summary>
        public int UseFreeInterval { get; private set; }

        /// <summary>
        /// 抽奖刷新货币。
        /// </summary>
        public int RefreshCostCurrencyType { get; private set; }

        /// <summary>
        /// 抽奖刷新价格。
        /// </summary>
        public int RefreshCostCurrencyPrice { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            ChanceType = int.Parse(text[index++]);
            RefreshUtcTime = text[index++];
            GiveFreeInterval = int.Parse(text[index++]);
            GiveFreeCount = int.Parse(text[index++]);
            UseFreeInterval = int.Parse(text[index++]);
            RefreshCostCurrencyType = int.Parse(text[index++]);
            RefreshCostCurrencyPrice = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRChanceRefresh>();
        }
    }
}
