using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备强化等级配置表。
    /// </summary>
    public class DRNewGearStrengthenLevel : IDataRow
    {
        private const int AttrMaxCount = 2;

        /// <summary>
        /// 等级编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 装备编号。
        /// </summary>
        public int GearId { get; private set; }

        /// <summary>
        /// 下一等级编号。
        /// </summary>
        public int NextId { get; private set; }

        /// <summary>
        /// 等级。
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// 升至本级所需金币数量。
        /// </summary>
        public int RequiredCoins { get; private set; }


        private int[] m_AttrIds;

        /// <summary>
        /// 获取属性编号列表。
        /// </summary>
        /// <returns>属性编号列表。</returns>
        public IList<int> GetAttrIds()
        {
            return m_AttrIds;
        }

        private float[] m_AttrVals;

        /// <summary>
        /// 获取属性值列表。
        /// </summary>
        /// <returns>属性值列表。</returns>
        public IList<float> GetattrVals()
        {
            return m_AttrVals;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            GearId = int.Parse(text[index++]);
            NextId = int.Parse(text[index++]);
            Level = int.Parse(text[index++]);
            RequiredCoins = int.Parse(text[index++]);
            index = ParseAttributes(text, index);
        }

        private int ParseAttributes(string[] text, int index)
        {
            List<int> attrIds = new List<int>();
            List<float> attrVals = new List<float>();

            for (int i = 0; i < AttrMaxCount; ++i)
            {
                int attrId = int.Parse(text[index++]);
                float attrVal = float.Parse(text[index++]);

                if (attrId > 0 && attrVal > 0)
                {
                    attrIds.Add(attrId);
                    attrVals.Add(attrVal);
                }
            }

            m_AttrIds = attrIds.ToArray();
            m_AttrVals = attrVals.ToArray();
            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRNewGearStrengthenLevel>();
        }
    }
}
