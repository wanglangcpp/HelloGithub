using GameFramework.DataTable;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 克制属性配置表。
    /// </summary>
    public class DRElement : IDataRow
    {
        private const int RestrainedElementMaxCount = 5;

        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        private int[] m_RestrainedElementIds = new int[RestrainedElementMaxCount];

        /// <summary>
        /// 克制属性编号。
        /// </summary>
        public int[] RestrainedElementIds
        {
            get
            {
                return m_RestrainedElementIds;
            }
        }

        private float[] m_RestrainingValues = new float[RestrainedElementMaxCount];

        /// <summary>
        /// 克制效果值。
        /// </summary>
        public float[] RestrainingValues
        {
            get
            {
                return m_RestrainingValues;
            }
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            for (int i = 0; i < RestrainedElementMaxCount; ++i)
            {
                m_RestrainedElementIds[i] = int.Parse(text[index++]);
                m_RestrainingValues[i] = Mathf.Clamp(float.Parse(text[index++]), -1f, 1f);
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRElement>();
        }
    }
}
