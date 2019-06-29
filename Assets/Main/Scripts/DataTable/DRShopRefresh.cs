using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 商城刷新配置表。
    /// </summary>
    public class DRShopRefresh : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 商城类型。
        /// </summary>
        public int ShopType { get; private set; }

        /// <summary>
        /// 商城刷新时间（UTC时间）。
        /// </summary>
        public string RefreshUtcTime { get; private set; }

        /// <summary>
        /// 商城刷新时赠予免费刷新次数。
        /// </summary>
        public int RefreshFreeTimes { get; private set; }

        /// <summary>
        /// 收费刷新货币。
        /// </summary>
        public int RefreshCostCurrencyType { get; private set; }

        /// <summary>
        /// 收费刷新价格。
        /// </summary>
        public int RefreshCostCurrencyAmount { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            ShopType = int.Parse(text[index++]);
            RefreshUtcTime = text[index++];
            RefreshFreeTimes = int.Parse(text[index++]);
            RefreshCostCurrencyType = int.Parse(text[index++]);
            RefreshCostCurrencyAmount = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRShopRefresh>();
        }
    }
}
