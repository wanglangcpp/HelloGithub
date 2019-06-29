using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 特效配置表。
    /// </summary>
    public class DREffect : IDataRow
    {
        /// <summary>
        /// 特效编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 资源名称。
        /// </summary>
        public string ResourceName
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
            ResourceName = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DREffect>();
        }
    }
}
