using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技消耗配置表。
    /// </summary>
    public class DRArenaCost : IDataRow
    {
        /// <summary>
        /// 已进行次数。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 每日第N次进行离线竞技副本消耗金币数。
        /// </summary>
        public int PlayCostCoin { get; private set; }

        /// <summary>
        /// 每日第N次刷新离线竞技对手消耗金币数。
        /// </summary>
        public int RefreshCostCoin { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            PlayCostCoin = int.Parse(text[index++]);
            RefreshCostCoin = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRArenaCost>();
        }
    }
}
