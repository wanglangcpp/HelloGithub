using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 图标配置表。
    /// </summary>
    public class DRIcon : IDataRow
    {
        /// <summary>
        /// 图标编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 图集名称。
        /// </summary>
        public string AtlasName
        {
            get;
            private set;
        }

        /// <summary>
        /// 精灵图名称。
        /// </summary>
        public string SpriteName
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
            AtlasName = text[index++];
            SpriteName = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRIcon>();
        }
    }
}
