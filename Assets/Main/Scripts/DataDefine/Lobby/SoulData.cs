using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class SoulData : IGenericData<SoulData, PBSoulInfo>
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private int m_Type;

        private DRSoul m_Soul
        {
            get
            {
                return GameEntry.DataTable.GetDataTable<DRSoul>().GetDataRow(m_Type);
            }
        }

        public int Key { get { return m_Id; } }

        public int Type { get { return m_Type; } }

        public int EffectId { get { return m_Soul.Type; } }

        public int Quality { get { return m_Soul.Quality; } }

        public float EffectValue { get { return m_Soul.AddValue; } }

        public string Name { get { return m_Soul.Name; } }

        public string Description { get { return m_Soul.Description; } }

        public int IconId { get { return m_Soul.IconId; } }

        public int StrengthenItemId { get { return m_Soul.StrengthenItemId; } }

        public int StrengthenItemCount { get { return m_Soul.StrengthenItemCount; } }

        public int UpgradedId { get { return m_Soul.UpgradedId; } }

        public int MaxHP
        {
            get
            {
                return EffectId == (int)AttributeType.MaxHP ? (int)EffectValue : 0;
            }
        }

        public int PhysicalAttack
        {
            get
            {
                return EffectId == (int)AttributeType.PhysicalAttack ? (int)EffectValue : 0;
            }
        }

        public int MagicAttack
        {
            get
            {
                return EffectId == (int)AttributeType.MagicAttack ? (int)EffectValue : 0;
            }
        }

        public int PhysicalDefense
        {
            get
            {
                return EffectId == (int)AttributeType.PhysicalDefense ? (int)EffectValue : 0;
            }
        }

        public int MagicDefense
        {
            get
            {
                return EffectId == (int)AttributeType.MagicDefense ? (int)EffectValue : 0;
            }
        }

        public float CriticalHitProb
        {
            get
            {
                return EffectId == (int)AttributeType.CriticalHitProb ? EffectValue : 0;
            }
        }

        public float CriticalHitRate
        {
            get
            {
                return EffectId == (int)AttributeType.CriticalHitRate ? EffectValue : 0;
            }
        }

        public float PhysicalAtkHPAbsorbRate
        {
            get
            {
                return EffectId == (int)AttributeType.PhysicalAtkHPAbsorbRate ? EffectValue : 0;
            }
        }

        public float MagicAtkHPAbsorbRate
        {
            get
            {
                return EffectId == (int)AttributeType.MagicAtkHPAbsorbRate ? EffectValue : 0;
            }
        }

        public float AntiCriticalHitProb
        {
            get
            {
                return EffectId == (int)AttributeType.AntiCriticalHitProb ? EffectValue : 0;
            }
        }

        public float OppPhysicalDfsReduceRate
        {
            get
            {
                return EffectId == (int)AttributeType.OppPhysicalDfsReduceRate ? EffectValue : 0;
            }
        }

        public float OppMagicDfsReduceRate
        {
            get
            {
                return EffectId == (int)AttributeType.OppMagicDfsReduceRate ? EffectValue : 0;
            }
        }

        public float PhysicalAtkReflectRate
        {
            get
            {
                return EffectId == (int)AttributeType.PhysicalAtkReflectRate ? EffectValue : 0;
            }
        }

        public float MagicAtkReflectRate
        {
            get
            {
                return EffectId == (int)AttributeType.MagicAtkReflectRate ? EffectValue : 0;
            }
        }

        public float RecoverHP
        {
            get
            {
                return EffectId == (int)AttributeType.RecoverHP ? EffectValue : 0f;
            }
        }

        public bool HasLevelFull
        {
            get
            {
                bool isFull = false;
                if (m_Soul.UpgradedId == -1)
                {
                    isFull = true;
                }
                return isFull;
            }
        }

        public bool CanUpgrade
        {
            get
            {
                bool isUpgrade = false;
                if (HasLevelFull)
                {
                    return isUpgrade;
                }
                int id = m_Soul.StrengthenItemId;
                int count = m_Soul.StrengthenItemCount;
                for (int i = 0; i < GameEntry.Data.Items.Data.Count; i++)
                {
                    if (GameEntry.Data.Items.Data[i].Type == id)
                    {
                        if (GameEntry.Data.Items.Data[i].Count >= count)
                        {
                            isUpgrade = true;
                        }
                        break;
                    }
                }
                return isUpgrade;
            }
        }

        public void UpdateData(PBSoulInfo data)
        {
            m_Id = data.Id;

            if (data.HasType)
            {
                m_Type = data.Type;
            }
        }
    }
}
