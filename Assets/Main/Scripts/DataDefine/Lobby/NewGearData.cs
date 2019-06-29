using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备数据（新版）。
    /// </summary>
    [Serializable]
    public class NewGearData : IGenericData<NewGearData, PBNewGearInfo>
    {
        [SerializeField]
        private int m_Type;

        [SerializeField]
        private int m_TotalQualityLevel;

        [SerializeField]
        private int m_StrengthenLevel;

        /// <summary>
        /// 关键字。
        /// </summary>
        public int Key { get { return m_Type; } }

        private DRNewGearQualityLevel m_GearQualityLevelDataRow = null;
        private DRNewGearQualityLevel m_GearNextQualityLevelDataRow = null;
        private DRNewGearStrengthenLevel m_GearStrengthenLevelDataRow = null;
        private DRNewGearStrengthenLevel m_GearNextStrengthenLevelDataRow = null;
        private Dictionary<AttributeType, float> m_CachedQualityLevelAttributes = new Dictionary<AttributeType, float>();
        private Dictionary<AttributeType, float> m_CachedNextQualityLevelAttributes = new Dictionary<AttributeType, float>();
        private Dictionary<AttributeType, float> m_CachedStrengthenLevelAttributes = new Dictionary<AttributeType, float>();
        private Dictionary<AttributeType, float> m_CachedNextStrengthenLevelAttributes = new Dictionary<AttributeType, float>();

        /// <summary>
        /// 类型编号，即 <see cref="DRNewGear"/> 中的编号。
        /// </summary>
        public int Type
        {
            get
            {
                return m_Type;
            }

            private set
            {
                if (m_Type == value)
                {
                    return;
                }

                m_Type = value;
            }
        }

        /// <summary>
        /// 总品阶。
        /// </summary>
        public int TotalQualityLevel
        {
            get
            {
                return m_TotalQualityLevel;
            }

            set
            {
                m_TotalQualityLevel = value;
                CacheGearQualityLevelDRs();
                CacheQualityLevelAttrs();
                Quality = m_GearQualityLevelDataRow.Quality;
                QualityLevel = m_GearQualityLevelDataRow.QualityLevel;
            }
        }

        /// <summary>
        /// 品质。
        /// </summary>
        public QualityType Quality { get; private set; }

        /// <summary>
        /// 品阶。
        /// </summary>
        public int QualityLevel { get; private set; }

        /// <summary>
        /// 是否已经到了顶级总品阶。
        /// </summary>
        public bool IsTopQualityLevel
        {
            get
            {
                return m_GearNextQualityLevelDataRow == null;
            }
        }

        /// <summary>
        /// 品阶编号。
        /// </summary>
        public int QualityLevelId
        {
            get
            {
                return m_GearQualityLevelDataRow.Id;
            }
        }

        /// <summary>
        /// 强化等级。
        /// </summary>
        public int StrengthenLevel
        {
            get
            {
                return m_StrengthenLevel;
            }

            set
            {
                m_StrengthenLevel = value;
                CacheGearStrengthenLevelDRs();
                CacheStrengthenLevelAttrs();
            }
        }

        /// <summary>
        /// 升级所需金币数量。
        /// </summary>
        public int StrengthenLevelRequiredCoins
        {
            get
            {
                return m_GearStrengthenLevelDataRow.RequiredCoins;
            }
        }

        /// <summary>
        /// 品质升级所需物品Ids。
        /// </summary>
        public IList<int> QualityLevelUpItemIds
        {
            get
            {
                return m_GearQualityLevelDataRow.GetRequiredItemIds();
            }
        }

        /// <summary>
        /// 品质升级所需物品数量。
        /// </summary>
        public IList<int> QualityLevelUpItemCounts
        {
            get
            {
                return m_GearQualityLevelDataRow.GetRequiredItemCounts();
            }
        }

        /// <summary>
        /// 强化等级编号。
        /// </summary>
        public int StrengthenLevelId
        {
            get
            {
                return m_GearStrengthenLevelDataRow.Id;
            }
        }

        /// <summary>
        /// 是否已达到最高强化等级。
        /// </summary>
        public bool IsTopStrengthenLevel
        {
            get
            {
                return m_GearNextStrengthenLevelDataRow == null;
            }
        }

        /// <summary>
        /// 获取整型属性。
        /// </summary>
        /// <param name="attrType">属性类型。</param>
        /// <param name="flag">获取标记。</param>
        /// <returns>属性值。</returns>
        public int GetIntAttribute(AttributeType attrType, NewGearAttrFlag flag = 0)
        {
            return Mathf.RoundToInt(GetFloatAttribute(attrType, flag));
        }

        /// <summary>
        /// 获取浮点型属性。
        /// </summary>
        /// <param name="attrType">属性类型。</param>
        /// <param name="flag">获取标记。</param>
        /// <returns>属性值。</returns>
        public float GetFloatAttribute(AttributeType attrType, NewGearAttrFlag flag = 0)
        {
            var strengthenLevelAttrDict = ((flag & NewGearAttrFlag.NextStrengthenLevel) == 0) ? m_CachedStrengthenLevelAttributes : m_CachedNextStrengthenLevelAttributes;
            var qualityLevelAttrDict = ((flag & NewGearAttrFlag.NextTotalQualityLevel) == 0) ? m_CachedQualityLevelAttributes : m_CachedNextQualityLevelAttributes;

            float strengthenLevelAttr;
            if (!strengthenLevelAttrDict.TryGetValue(attrType, out strengthenLevelAttr))
            {
                strengthenLevelAttr = 0f;
            }

            float qualityLevelAttr;
            if (!qualityLevelAttrDict.TryGetValue(attrType, out qualityLevelAttr))
            {
                qualityLevelAttr = 0f;
            }

            return strengthenLevelAttr + qualityLevelAttr;
        }

        /// <summary>
        /// 获取升序装备升品属性类型列表。
        /// </summary>
        /// <returns>升序属性类型列表。</returns>
        public IList<AttributeType> GetSortedQualityUpAttributeTypes()
        {
            var attrTypes = new HashSet<AttributeType>();
            var attrDicts = new IDictionary<AttributeType, float>[]
            {
                m_CachedNextQualityLevelAttributes,
                m_CachedQualityLevelAttributes,
            };

            for (int i = 0; i < attrDicts.Length; ++i)
            {
                foreach (var kv in attrDicts[i])
                {
                    attrTypes.Add(kv.Key);
                }
            }

            var list = new List<AttributeType>(attrTypes);
            list.Sort();
            return list.ToArray();
        }

        /// <summary>
        /// 获取升序装备强化属性类型列表。
        /// </summary>
        /// <returns>升序属性类型列表。</returns>
        public IList<AttributeType> GetSortedStrenthenLevelAttributeTypes()
        {
            var attrTypes = new HashSet<AttributeType>();
            var attrDicts = new IDictionary<AttributeType, float>[]
            {
                m_CachedStrengthenLevelAttributes,
                m_CachedNextStrengthenLevelAttributes,
            };

            for (int i = 0; i < attrDicts.Length; ++i)
            {
                foreach (var kv in attrDicts[i])
                {
                    attrTypes.Add(kv.Key);
                }
            }

            var list = new List<AttributeType>(attrTypes);
            list.Sort();
            return list.ToArray();
        }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <param name="data">原始数据。</param>
        public void UpdateData(PBNewGearInfo data)
        {
            Type = data.TypeId;

            if (data.HasStrengthenLevel)
            {
                StrengthenLevel = data.StrengthenLevel;
            }

            if (data.HasTotalQualityLevel)
            {
                TotalQualityLevel = data.TotalQualityLevel;
            }
        }


        private void CacheGearStrengthenLevelDRs()
        {
            var dataTable = GameEntry.DataTable.GetDataTable<DRNewGearStrengthenLevel>();
            var dataRows = dataTable.GetAllDataRows();

            bool found = false;
            for (int i = 0; i < dataRows.Length; ++i)
            {
                if (dataRows[i].GearId == Type && dataRows[i].Level == m_StrengthenLevel)
                {
                    m_GearStrengthenLevelDataRow = dataRows[i];
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Log.Error("Gear strengthen level '{0}' not found for gear id '{1}'.", StrengthenLevel.ToString(), Type.ToString());
                return;
            }

            m_GearNextStrengthenLevelDataRow = null;
            if (m_GearStrengthenLevelDataRow.NextId > 0)
            {
                m_GearNextStrengthenLevelDataRow = dataTable[m_GearStrengthenLevelDataRow.NextId];
            }
        }

        private void CacheGearQualityLevelDRs()
        {
            var dataTable = GameEntry.DataTable.GetDataTable<DRNewGearQualityLevel>();
            var dataRows = dataTable.GetAllDataRows();

            bool found = false;
            for (int i = 0; i < dataRows.Length; ++i)
            {
                if (dataRows[i].GearId == Type && dataRows[i].TotalQualityLevel == m_TotalQualityLevel)
                {
                    m_GearQualityLevelDataRow = dataRows[i];
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Log.Error("Gear total quality level '{0}' not found for gear id '{1}'.", m_TotalQualityLevel.ToString(), Type.ToString());
                return;
            }

            m_GearNextQualityLevelDataRow = null;
            if (m_GearQualityLevelDataRow.NextId > 0)
            {
                m_GearNextQualityLevelDataRow = dataTable[m_GearQualityLevelDataRow.NextId];
            }
        }

        private void CacheQualityLevelAttrs()
        {
            m_CachedQualityLevelAttributes.Clear();
            m_CachedNextQualityLevelAttributes.Clear();

            CacheAttrs(m_GearQualityLevelDataRow.GetAttrIds(), m_GearQualityLevelDataRow.GetAttrVals(), m_CachedQualityLevelAttributes);

            if (m_GearNextQualityLevelDataRow != null)
            {
                CacheAttrs(m_GearNextQualityLevelDataRow.GetAttrIds(), m_GearNextQualityLevelDataRow.GetAttrVals(), m_CachedNextQualityLevelAttributes);
            }
        }

        private void CacheStrengthenLevelAttrs()
        {
            m_CachedStrengthenLevelAttributes.Clear();
            m_CachedNextStrengthenLevelAttributes.Clear();

            CacheAttrs(m_GearStrengthenLevelDataRow.GetAttrIds(), m_GearStrengthenLevelDataRow.GetattrVals(), m_CachedStrengthenLevelAttributes);

            if (m_GearNextStrengthenLevelDataRow != null)
            {
                CacheAttrs(m_GearNextStrengthenLevelDataRow.GetAttrIds(), m_GearNextStrengthenLevelDataRow.GetattrVals(), m_CachedNextStrengthenLevelAttributes);
            }
        }

        private static void CacheAttrs(IList<int> attrIds, IList<float> attrVals, Dictionary<AttributeType, float> attrDict)
        {
            for (int i = 0; i < attrIds.Count; ++i)
            {
                attrDict.Add((AttributeType)attrIds[i], attrVals[i]);
            }
        }
    }
}
