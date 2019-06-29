using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄升品道具配置表。
    /// </summary>
    public class DRHeroQualityItem : IDataRow
    {
        private const int AttrMaxCount = 2;
        private const int ItemTypeMaxCount = 3;

        /// <summary>
        /// 品阶编号。
        /// </summary>
        public int Id { get; private set; }


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 排序参数。
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// 品质。
        /// </summary>
        public int Quality { get; private set; }

        /// <summary>
        /// 图标编号。
        /// </summary>
        public int IconId { get; private set; }

        /// <summary>
        /// 卖店价格。
        /// </summary>
        public int Price { get; private set; }

        /// <summary>
        /// 合成所需价格。
        /// </summary>
        public int SynthCoinCost { get; private set; }

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


        private int[] m_SynthItemIds;

        /// <summary>
        /// 获取合成需要道具的编号列表。
        /// </summary>
        /// <returns>品质提升道具编号列表。</returns>
        public IList<int> GetSynthItemIds()
        {
            return m_SynthItemIds;
        }

        private int[] m_SynthItemCounts;

        /// <summary>
        /// 获取合成需要道具的个数列表。
        /// </summary>
        /// <returns>品质提升道具需要个数列表。</returns>
        public IList<int> GetSynthItemCounts()
        {
            return m_SynthItemCounts;
        }


        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Name = text[index++];
            Description = text[index++];
            Order = int.Parse(text[index++]);
            Quality = int.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            Price = int.Parse(text[index++]);
            SynthCoinCost = int.Parse(text[index++]);
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

            m_SynthItemIds = itemIds.ToArray();
            m_SynthItemCounts = itemCounts.ToArray();
            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRHeroQualityItem>();
        }
    }
}
