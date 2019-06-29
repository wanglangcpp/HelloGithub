using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 武器模型表。
    /// </summary>
    public class DRWeapon : IDataRow
    {
        /// <summary>
        /// 模型编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 资源名称。
        /// </summary>
        public string ResourceName
        {
            get;
            private set;
        }

        /// <summary>
        /// 展示用资源名称。
        /// </summary>
        public string ResourceNameForShow
        {
            get;
            private set;
        }

        /// <summary>
        /// 主城用资源名称。
        /// </summary>
        public string ResourceNameForLobby
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
            ResourceNameForShow = text[index++];
            ResourceNameForLobby = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRWeapon>();
        }
    }
}
