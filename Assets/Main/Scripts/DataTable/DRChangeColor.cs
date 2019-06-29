using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 实体变色表。
    /// </summary>
    public class DRChangeColor : IDataRow
    {
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 着色器参数 _RimColor。
        /// </summary>
        public Color RimColor
        {
            get;
            private set;
        }

        /// <summary>
        /// 着色器参数 _InnerColor。
        /// </summary>
        public Color InnerColor
        {
            get;
            private set;
        }

        /// <summary>
        /// 着色器参数 _InnerColorPower。
        /// </summary>
        public float InnerColorPower
        {
            get;
            private set;
        }

        /// <summary>
        /// 着色器参数 _RimPower。
        /// </summary>
        public float RimPower
        {
            get;
            private set;
        }

        /// <summary>
        /// 着色器参数 _AlphaPower。
        /// </summary>
        public float AlphaPower
        {
            get;
            private set;
        }

        /// <summary>
        /// 着色器参数 _AllPower。
        /// </summary>
        public float AllPower
        {
            get;
            private set;
        }

        /// <summary>
        /// 变色开始的过渡时间。
        /// </summary>
        public float BeginDuration
        {
            get;
            private set;
        }

        /// <summary>
        /// 变色结束的过渡时间。
        /// </summary>
        public float EndDuration
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

            Color tmpColor;

            if (!UnityEngine.ColorUtility.TryParseHtmlString(text[index++], out tmpColor))
            {
                Log.Error("RimColor cannot be parsed.");
                return;
            }
            RimColor = tmpColor;

            if (!UnityEngine.ColorUtility.TryParseHtmlString(text[index++], out tmpColor))
            {
                Log.Error("InnerColor cannot be parsed.");
                return;
            }
            InnerColor = tmpColor;

            InnerColorPower = float.Parse(text[index++]);
            RimPower = float.Parse(text[index++]);
            AlphaPower = float.Parse(text[index++]);
            AllPower = float.Parse(text[index++]);
            BeginDuration = float.Parse(text[index++]);
            EndDuration = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRChangeColor>();
        }
    }
}
