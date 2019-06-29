using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class MeridianData : IGenericData<MeridianData, PBMeridianInfo>
    {
        [SerializeField]
        private int m_Id;

        private int m_MeridianProgress = 0;
        private List<DRMeridian> m_AllMeridianRow = new List<DRMeridian>();

        public List<DRMeridian> MeridianRow
        {
            get
            {
                return m_AllMeridianRow;
            }
        }

        public int Key { get { return m_Id; } }

        public AttributeType GetMeridianAttributeValue(int id, out int value)
        {
            int maxDictance = 0;
            int attackDictance = 0;
            int defenseDictance = 0;
            if (id <= 0)
            {
                maxDictance = m_AllMeridianRow[id].MaxHP;
                attackDictance = m_AllMeridianRow[id].Attack;
                defenseDictance = m_AllMeridianRow[id].Defense;
            }
            else
            {
                maxDictance = m_AllMeridianRow[id].MaxHP - m_AllMeridianRow[id - 1].MaxHP;
                attackDictance = m_AllMeridianRow[id].Attack - m_AllMeridianRow[id - 1].Attack;
                defenseDictance = m_AllMeridianRow[id].Defense - m_AllMeridianRow[id - 1].Defense;
            }
            if (maxDictance != 0)
            {
                value = maxDictance;
                return AttributeType.MaxHP;
            }

            if (attackDictance != 0)
            {
                value = attackDictance;
                return AttributeType.PhysicalAttack;
            }

            if (defenseDictance != 0)
            {
                value = defenseDictance;
                return AttributeType.PhysicalDefense;
            }
            value = 0;
            return AttributeType.Unspecified;
        }

        /// <summary>
        /// 星图增加最大hp
        /// </summary>
        public int MaxHP
        {
            get
            {
                return GetAttributeValue(AttributeType.MaxHP);
            }
        }

        /// <summary>
        /// 星图增加物理攻击力
        /// </summary>
        public int PhysicalAttack
        {
            get
            {
                return GetAttributeValue(AttributeType.PhysicalAttack);
            }
        }

        /// <summary>
        /// 星图物理防御力
        /// </summary>
        public int PhysicalDefense
        {
            get
            {
                return GetAttributeValue(AttributeType.PhysicalDefense);
            }
        }

        private int GetAttributeValue(AttributeType type)
        {
            int deltaValue = 0;
            if (MeridianProgress > 0)
            {
                var dataRows = GameEntry.DataTable.GetDataTable<DRMeridian>();
                var dataRow = dataRows.GetDataRow(MeridianProgress);
                if (dataRow == null)
                {
                    Log.Error("Cannot find Meridian '{0}'.", MeridianProgress);
                    return 0;
                }
                if (type == AttributeType.MaxHP)
                {
                    deltaValue = dataRow.MaxHP;
                }
                else if (type == AttributeType.PhysicalAttack)
                {
                    deltaValue = dataRow.Attack;
                }
                else if (type == AttributeType.PhysicalDefense)
                {
                    deltaValue = dataRow.Defense;
                }
            }

            return deltaValue;
        }

        public int MeridianProgress
        {
            get { return m_MeridianProgress; }
            set { m_MeridianProgress = value; }
        }

        public void UpdateData(PBMeridianInfo data)
        {
            if (m_AllMeridianRow.Count == 0)
            {
                var dataRows = GameEntry.DataTable.GetDataTable<DRMeridian>().GetAllDataRows();
                for (int i = 0; i < dataRows.Length; i++)
                {
                    m_AllMeridianRow.Add(dataRows[i]);
                }
            }
            MeridianProgress = data.MeridianProgress;
        }
    }
}
