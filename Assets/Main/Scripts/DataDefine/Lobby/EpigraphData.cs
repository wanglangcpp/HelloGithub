using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class EpigraphData : IGenericData<EpigraphData, PBEpigraphInfo>
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private int m_Level;

        private DREpigraph m_EpigraphRow = null;

        public int Key { get { return m_Id; } }
        public int Level { get { return m_Level; } set { m_Level = value; } }

        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
                if (m_EpigraphRow != null && m_EpigraphRow.Id == m_Id)
                {
                    return;
                }

                if (m_Id == 0)
                {
                    m_EpigraphRow = null;
                    return;
                }

                var dataTable = GameEntry.DataTable.GetDataTable<DREpigraph>();
                m_EpigraphRow = dataTable.GetDataRow(m_Id);
                if (m_EpigraphRow == null)
                {
                    return;
                }
            }
        }

        public string DTName { get { return m_EpigraphRow.Name; } }

        public string DTDescription { get { return m_EpigraphRow.Description; } }

        public int DTAttributeType { get { return m_EpigraphRow.AttributeType; } }

        public float DTAttributeValue { get { return m_EpigraphRow.AddValue[Level - 1]; } }

        public int DTQuality { get { return m_EpigraphRow.Quality; } }

        public int DTIconId { get { return m_EpigraphRow.IconId; } }

        public int DTPieceId { get { return m_EpigraphRow.PieceId; } }

        public float DTNextAttributeValue
        {
            get
            {
                if (Level + 1 > Constant.MaxEpigraphLevel)
                {
                    return 0;
                }
                else
                {
                    return m_EpigraphRow.AddValue[Level];
                }
            }
        }

        /// <summary>
        /// 获取当前铭文升级需要的铭文碎片
        /// </summary>
        public int UpgradeChipCount
        {
            get
            {
                int count = 0;
                if (Level < Constant.MaxEpigraphLevel)
                {
                    count = m_EpigraphRow.CostPieceCount[Level - 1];
                }
                return count;
            }
        }

        /// <summary>
        /// 增加物理攻击加成
        /// </summary>
        public float PhysicalAtkIncreaseRate
        {
            get
            {
                return GetAttributeValue(AttributeType.PhysicalAtkIncreaseRate);
            }
        }

        /// <summary>
        /// 法术攻击加成
        /// </summary>
        public float MagicAtkIncreaseRate
        {
            get
            {
                return GetAttributeValue(AttributeType.MagicAtkIncreaseRate);
            }
        }

        /// <summary>
        /// 物理防御加成
        /// </summary>
        public float PhysicalDfsIncreaseRate
        {
            get
            {
                return GetAttributeValue(AttributeType.PhysicalDfsIncreaseRate);
            }
        }

        /// <summary>
        /// 法术防御加成
        /// </summary>
        public float MagicDfsIncreaseRate
        {
            get
            {
                return GetAttributeValue(AttributeType.MagicDfsIncreaseRate);
            }
        }

        /// <summary>
        /// 增加HP加成
        /// </summary>
        public float MaxHPIncreaseRate
        {
            get
            {
                return GetAttributeValue(AttributeType.MaxHPIncreaseRate);
            }
        }

        /// <summary>
        /// 怒气上涨速率
        /// </summary>
        public float AngerIncreaseRate
        {
            get
            {
                return GetAttributeValue(AttributeType.AngerIncreaseRate);
            }
        }

        private float GetAttributeValue(AttributeType type)
        {
            float deltaValue = 0;

            if (m_EpigraphRow != null && m_EpigraphRow.AttributeType == (int)type && Level < Constant.MaxEpigraphLevel)
            {
                deltaValue = m_EpigraphRow.AddValue[Level];
            }

            return deltaValue;
        }

        public void UpdateData(PBEpigraphInfo data)
        {
            Id = data.Type;
            if (data.HasLevel)
            {
                Level = data.Level;
            }
        }
    }
}
