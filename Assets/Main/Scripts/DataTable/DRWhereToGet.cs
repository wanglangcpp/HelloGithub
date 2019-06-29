using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 获取途径及跳转配置表。
    /// </summary>
    public class DRWhereToGet : IDataRow
    {
        private const int ParamCount = 5;

        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 类型。
        /// </summary>
        public WhereToGetType Type { get; private set; }

        /// <summary>
        /// 图标编号。
        /// </summary>
        public int IconId { get; private set; }

        private string[] m_Params;

        /// <summary>
        /// 获取参数列表。
        /// </summary>
        public IList<string> Params { get { return m_Params; } }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Type = (WhereToGetType)(int.Parse(text[index++]));
            IconId = int.Parse(text[index++]);
            index = ParseParams(text, index);
        }

        private int ParseParams(string[] text, int index)
        {
            m_Params = new string[ParamCount];
            for (int i = 0; i < ParamCount; ++i)
            {
                m_Params[i] = text[index++];
            }
            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRWhereToGet>();
        }
    }
}
