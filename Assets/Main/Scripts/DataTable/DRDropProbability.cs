using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 掉落包概率配置表。
    /// </summary>
    public class DRDropProbability : IDataRow
    {
        /// <summary>
        /// 掉落包编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 普通掉落概率。
        /// </summary>
        public float NormalRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 精英掉落概率。
        /// </summary>
        public float EliteRate
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
            NormalRate = float.Parse(text[index++]);
            EliteRate = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRDropProbability>();
        }

    }
}
