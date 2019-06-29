using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 声音配置表。
    /// </summary>
    public class DRSound : IDataRow
    {
        /// <summary>
        /// 声音编号。
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
        /// 优先级。
        /// </summary>
        public int Priority
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否循环。
        /// </summary>
        public bool Loop
        {
            get;
            private set;
        }

        /// <summary>
        /// 音量。
        /// </summary>
        public float Volume
        {
            get;
            private set;
        }

        /// <summary>
        /// 声音空间混合量。
        /// </summary>
        public float SpatialBlend
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
            Priority = int.Parse(text[index++]);
            Loop = bool.Parse(text[index++]);
            Volume = float.Parse(text[index++]);
            SpatialBlend = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSound>();
        }
    }
}
