using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄品阶配置表。
    /// </summary>
    public class DRHeroQualityLevel : IDataRow
    {
        private const int AttrMaxCount = 8;
        private const int ItemTypeMaxCount = Constant.HeroQualityLevelItemSlotCount;

        /// <summary>
        /// 品阶编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 英雄编号。
        /// </summary>
        public int HeroId { get; private set; }

        /// <summary>
        /// 总品阶。
        /// </summary>
        public int TotalQualityLevel { get; private set; }

        /// <summary>
        /// 品质。
        /// </summary>
        public int Quality { get; private set; }

        /// <summary>
        /// 品阶。
        /// </summary>
        public int QualityLevel { get; private set; }

        /// <summary>
        /// 下一品阶编号。
        /// </summary>
        public int NextId { get; private set; }

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

        private int[] m_QualityItemIds;

        /// <summary>
        /// 获取品质提升道具编号列表。
        /// </summary>
        /// <returns>品质提升道具编号列表。</returns>
        public IList<int> GetQualityItemIds()
        {
            return m_QualityItemIds;
        }

        private int[] m_QualityItemCounts;

        /// <summary>
        /// 获取品质提升道具需要个数列表。
        /// </summary>
        /// <returns>品质提升道具需要个数列表。</returns>
        public IList<int> GetQualityItemCounts()
        {
            return m_QualityItemCounts;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            HeroId = int.Parse(text[index++]);
            TotalQualityLevel = int.Parse(text[index++]);
            Quality = int.Parse(text[index++]);
            QualityLevel = int.Parse(text[index++]);
            NextId = int.Parse(text[index++]);
            index = ParseAttributes(text, index);
            index = ParseQualityItems(text, index);
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

        private int ParseQualityItems(string[] text, int index)
        {
            List<int> itemIds = new List<int>();
            List<int> itemCounts = new List<int>();

            for (int i = 0; i < ItemTypeMaxCount; ++i)
            {
                int itemId = int.Parse(text[index++]);
                int itemCount = int.Parse(text[index++]);

                itemIds.Add(itemId);
                itemCounts.Add(itemCount);
            }

            m_QualityItemIds = itemIds.ToArray();
            m_QualityItemCounts = itemCounts.ToArray();
            return index;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRHeroQualityLevel>();
        }

    }
}
