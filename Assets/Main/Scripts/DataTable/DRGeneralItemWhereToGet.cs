using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 物品获取途径配置表。
    /// </summary>
    public class DRGeneralItemWhereToGet : IDataRow
    {
        private const int WhereToGetMaxCount = 10;

        /// <summary>
        /// 物品编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 获取途径描述。
        /// </summary>
        public string WhereToGetText { get; private set; }

        private int[] m_WhereToGetIds;

        /// <summary>
        /// 获取『获取途径』编号列表。
        /// </summary>
        /// <returns>『获取途径』编号列表。</returns>
        public IList<int> GetWhereToGetIds()
        {
            return m_WhereToGetIds;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            WhereToGetText = text[index++];
            index = ParseWhereToGet(text, index);
        }

        private int ParseWhereToGet(string[] text, int index)
        {
            List<int> whereToGetIds = new List<int>();

            for (int i = 0; i < WhereToGetMaxCount; ++i)
            {
                var whereToGetId = int.Parse(text[index++]);

                if (whereToGetId > 0)
                {
                    whereToGetIds.Add(whereToGetId);
                }
            }

            m_WhereToGetIds = whereToGetIds.ToArray();
            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRGeneralItemWhereToGet>();
        }
    }
}
