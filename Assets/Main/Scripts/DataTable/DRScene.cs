using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 场景配置表。
    /// </summary>
    public class DRScene : IDataRow
    {
        /// <summary>
        /// 场景编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 场景名称。
        /// </summary>
        public string Name
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

        /// <summary>
        /// 背景音乐编号。
        /// </summary>
        public int BackgroundMusicId
        {
            get;
            private set;
        }

        /// <summary>
        /// 投影器 X 偏移量。
        /// </summary>
        public float ProjectorOffsetX
        {
            get;
            private set;
        }

        /// <summary>
        /// 投影器 Y 偏移量。
        /// </summary>
        public float ProjectorOffsetY
        {
            get;
            private set;
        }

        /// <summary>
        /// 投影尺寸。
        /// </summary>
        public float ProjectorSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 高光因子。
        /// </summary>
        public float HighlightFactor
        {
            get;
            private set;
        }

        /// <summary>
        /// 高光着色。
        /// </summary>
        public Color HighlightColor
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
            Name = text[index++];
            ResourceName = text[index++];
            BackgroundMusicId = int.Parse(text[index++]);
            ProjectorOffsetX = float.Parse(text[index++]);
            ProjectorOffsetY = float.Parse(text[index++]);
            ProjectorSize = float.Parse(text[index++]);
            HighlightFactor = float.Parse(text[index++]);

            Color hlColor;
            UnityEngine.ColorUtility.TryParseHtmlString(text[index++], out hlColor);
            HighlightColor = hlColor;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRScene>();
        }
    }
}
