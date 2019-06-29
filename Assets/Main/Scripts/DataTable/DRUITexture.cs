using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 图标配置表。
    /// </summary>
    public class DRUITexture : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 贴图路径。
        /// </summary>
        public string TexturePath
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
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRUITexture>();
        }
    }
}
