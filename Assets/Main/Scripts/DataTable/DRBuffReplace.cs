using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// Buff 替换配置表。
    /// </summary>
    public class DRBuffReplace : IDataRow
    {
        /// <summary>
        /// 源 Buff 编号
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 源 Buff 需求数量
        /// </summary>
        public int NeededCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 替换 Buff 编号
        /// </summary>
        public int TargetBuffId
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
            NeededCount = int.Parse(text[index++]);
            TargetBuffId = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRBuffReplace>();
        }
    }
}
