using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 切场景背景图配置表。
    /// </summary>
    public class DRLoadingBackground : IDataRow
    {
        /// <summary>
        /// 切场景背景图编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 背景图路径。
        /// </summary>
        public string TexturePath
        {
            get;
            private set;
        }

        /// <summary>
        /// 背景图出现概率权重。
        /// </summary>
        public float Weight
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
            TexturePath = text[index++];
            Weight = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRLoadingBackground>();
        }
    }
}
