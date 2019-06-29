using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 活跃度宝箱配置表。
    /// </summary>
    public class DRDailyQuestActiveness : IDataRow
    {
        private const int RewardTypesMaxCount = 3;

        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 所需活跃度。
        /// </summary>
        public int Activeness { get; private set; }

        private int[] m_RewardTypes;

        /// <summary>
        /// 获取属性编号列表。
        /// </summary>
        /// <returns>属性编号列表。</returns>
        public IList<int> GetRewardTypes()
        {
            return m_RewardTypes;
        }

        private int[] m_RewardCounts;

        /// <summary>
        /// 获取属性编号列表。
        /// </summary>
        /// <returns>属性编号列表。</returns>
        public IList<int> GetRewardCounts()
        {
            return m_RewardCounts;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Activeness = int.Parse(text[index++]);
            index = ParseRewardTypes(text, index);
        }

        private int ParseRewardTypes(string[] text, int index)
        {
            List<int> attrIds = new List<int>();
            List<int> attrVals = new List<int>();

            for (int i = 0; i < RewardTypesMaxCount; ++i)
            {
                int attrId = int.Parse(text[index++]);
                int attrVal = int.Parse(text[index++]);

                if (attrId > 0 && attrVal > 0)
                {
                    attrIds.Add(attrId);
                    attrVals.Add(attrVal);
                }
            }

            m_RewardTypes = attrIds.ToArray();
            m_RewardCounts = attrVals.ToArray();
            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRDailyQuestActiveness>();
        }
    }
}
