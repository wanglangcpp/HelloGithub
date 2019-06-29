using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 抽奖花费配置表。
    /// </summary>
    public class DRChanceCost : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 抽奖类型。
        /// </summary>
        public int ChanceType
        {
            get;
            private set;
        }

        /// <summary>
        /// 已抽奖次数。
        /// </summary>
        public int ChanceIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// 货币类型。
        /// </summary>
        public int CurrencyType
        {
            get;
            private set;
        }

        /// <summary>
        /// 购买货币消耗。
        /// </summary>
        public int Cost
        {
            get;
            private set;
        }

        /// <summary>
        /// 全部购买货币消耗。
        /// </summary>
        public int CostAll
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            ChanceType = int.Parse(text[index++]);
            ChanceIndex = int.Parse(text[index++]);
            CurrencyType = int.Parse(text[index++]);
            Cost = int.Parse(text[index++]);
            CostAll = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRChanceCost>();
        }
    }
}
