using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 寻径点集配置表。
    /// </summary>
    public class DRGuidePointSet : IDataRow
    {
        private const int GuidePointMaxCount = Constant.GuidePointMaxCountInInstance;

        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 寻径点列表。
        /// </summary>
        private Vector2[] m_GuidePoints;

        /// <summary>
        /// 寻径点所在组列表。
        /// </summary>
        private int[] m_GuidePointGroups;

        /// <summary>
        /// 获取寻径点列表。
        /// </summary>
        /// <returns>寻径点列表。</returns>
        public IList<Vector2> GetGuidePoints()
        {
            return m_GuidePoints;
        }

        /// <summary>
        /// 获取寻径点所在组列表。
        /// </summary>
        /// <returns>寻径点所在组列表。</returns>
        public IList<int> GetGuidePointGroups()
        {
            return m_GuidePointGroups;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            index = ParseGuidePointsAndGroups(text, index);
        }

        private int ParseGuidePointsAndGroups(string[] text, int index)
        {
            var guidePoints = new List<Vector2>();
            var guidePointGroups = new List<int>();

            for (int i = 0; i < GuidePointMaxCount; i++)
            {
                var vecStr = text[index++].Trim('"');
                if (string.IsNullOrEmpty(vecStr))
                {
                    index++;
                }
                else
                {
                    guidePoints.Add(ConverterEx.ParseVector2(vecStr).Value);
                    guidePointGroups.Add(int.Parse(text[index++]));
                }
            }

            m_GuidePoints = guidePoints.ToArray();
            m_GuidePointGroups = guidePointGroups.ToArray();
            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRGuidePointSet>();
        }
    }
}
