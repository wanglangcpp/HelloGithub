using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备品质及品阶配置表。
    /// </summary>
    public class DRNewGearQualityLevel : IDataRow
    {
        private const int AttrMaxCount = 2;
        private const int ItemTypeMaxCount = 3;

        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 装备编号。
        /// </summary>
        public int GearId { get; private set; }

        /// <summary>
        /// 下一品阶编号。
        /// </summary>
        public int NextId { get; private set; }

        /// <summary>
        /// 总品阶。
        /// </summary>
        public int TotalQualityLevel { get; private set; }

        /// <summary>
        /// 品质。
        /// </summary>
        public QualityType Quality { get; private set; }

        /// <summary>
        /// 品阶。
        /// </summary>
        public int QualityLevel { get; private set; }

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
        public IList<float> GetAttrVals()
        {
            return m_AttrVals;
        }

        private int[] m_RequiredItemIds;

        /// <summary>
        /// 获取此品阶要求道具的编号列表。
        /// </summary>
        /// <returns>此品阶要求道具的编号列表。</returns>
        public IList<int> GetRequiredItemIds()
        {
            return m_RequiredItemIds;
        }

        private int[] m_RequiredItemCounts;

        /// <summary>
        /// 获取此品阶要求道具的个数列表。
        /// </summary>
        /// <returns>此品阶要求道具的个数列表。</returns>
        public IList<int> GetRequiredItemCounts()
        {
            return m_RequiredItemCounts;
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
            TotalQualityLevel = int.Parse(text[index++]);
            Quality = (QualityType)(int.Parse(text[index++]));
            QualityLevel = int.Parse(text[index++]);
            index = ParseAttributes(text, index);
            index = ParseSynthItems(text, index);
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

        private int ParseSynthItems(string[] text, int index)
        {
            List<int> itemIds = new List<int>();
            List<int> itemCounts = new List<int>();

            for (int i = 0; i < ItemTypeMaxCount; ++i)
            {
                int itemId = int.Parse(text[index++]);
                int itemCount = int.Parse(text[index++]);

                if (itemId > 0 && itemCount > 0)
                {
                    itemIds.Add(itemId);
                    itemCounts.Add(itemCount);
                }
            }

            m_RequiredItemIds = itemIds.ToArray();
            m_RequiredItemCounts = itemCounts.ToArray();
            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRNewGearQualityLevel>();
        }
    }
}
