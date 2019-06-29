using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 货币兑换表。
    /// </summary>
    public class DRExchange : IDataRow
    {
        /// <summary>
        /// 货币兑换Id（当天兑换次数）。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 兑换金币数量。
        /// </summary>
        public int Coin { get; private set; }

        /// <summary>
        /// 兑换金币需要的钻石。
        /// </summary>
        public int CoinCostMoney { get; private set; }

        /// <summary>
        /// 兑换体力数。
        /// </summary>
        public int Energy { get; private set; }

        /// <summary>
        /// 兑换体力所需要的钻石。
        /// </summary>
        public int EnergyCostMoney { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Coin = int.Parse(text[index++]);
            CoinCostMoney = int.Parse(text[index++]);
            Energy = int.Parse(text[index++]);
            EnergyCostMoney = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRExchange>();
        }
    }
}
