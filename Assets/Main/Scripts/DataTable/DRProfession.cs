using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 职业配置表。
    /// </summary>
    public class DRProfession : IDataRow
    {
        public int Id
        {
            get;
            protected set;
        }

        public string NameKey
        {
            get;
            private set;
        }

        public string IconSpriteName
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
            NameKey = text[index++];
            IconSpriteName = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRProfession>();
        }
    }
}
