using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class GearData : IGenericData<GearData, PBGearInfo>
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private int m_Type;

        [SerializeField]
        private int m_Level;

        [SerializeField]
        private int m_StrengthenLevel;

        public int Key { get { return m_Id; } }
        public int Id { get { return m_Id; } }
        public PBGearInfo GearInfo { get; set; }

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
                m_GearDataRow = null;
            }
        }

        public int Level { get { return m_Level; } }

        public int StrengthenLevel { get { return m_StrengthenLevel; } }

        private DRGear m_GearDataRow = null;

        private DRGear GearDataRow
        {
            get
            {
                if (m_GearDataRow == null)
                {
                    var dataTable = GameEntry.DataTable.GetDataTable<DRGear>();
                    m_GearDataRow = dataTable.GetDataRow(Type);
                    if (m_GearDataRow == null)
                    {
                        Log.Error("Cannot found gear with type '{0}'.", Type);
                        return null;
                    }
                }

                return m_GearDataRow;
            }
        }

        private float CalcProperty(float baseValue, float levelCoef, float strengthenLevelCoef)
        {
            return (baseValue + levelCoef * (Level - 1)) * (1 + (StrengthenLevel * strengthenLevelCoef));
        }

        public int MaxHP
        {
            get
            {
                return GetMaxHP(m_Level, m_StrengthenLevel);
            }
        }

        public int GetMaxHP(int level, int strengthenLevel)
        {
            return Mathf.RoundToInt((GearDataRow.MaxHP + GearDataRow.LCMaxHP * (level - 1)) * (1 + (strengthenLevel * GearDataRow.SLCMaxHPIncreaseRate)));
        }

        public int PhysicalAttack
        {
            get
            {
                return GetPhysicalAttack(m_Level, m_StrengthenLevel);
            }
        }

        public int GetPhysicalAttack(int level, int strengthenLevel)
        {
            return Mathf.RoundToInt((GearDataRow.PhysicalAttack + GearDataRow.LCPhysicalAttack * (level - 1)) * (1 + (strengthenLevel * GearDataRow.SLCPhysicalAtkIncreaseRate)));
        }

        public int PhysicalDefense
        {
            get
            {
                return GetPhysicalDefense(m_Level, m_StrengthenLevel);
            }
        }

        public int GetPhysicalDefense(int level, int strengthenLevel)
        {
            return Mathf.RoundToInt((GearDataRow.PhysicalDefense + GearDataRow.LCPhysicalDefense * (level - 1)) * (1 + (strengthenLevel * GearDataRow.SLCPhysicalDfsIncreaseRate)));
        }

        public int MagicAttack
        {
            get
            {
                return GetMagicAttack(m_Level, m_StrengthenLevel);
            }
        }

        public int GetMagicAttack(int level, int strengthenLevel)
        {
            return Mathf.RoundToInt((GearDataRow.MagicAttack + GearDataRow.LCMagicAttack * (level - 1)) * (1 + (strengthenLevel * GearDataRow.SLCMagicAtkIncreaseRate)));
        }

        public int MagicDefense
        {
            get
            {
                return GetMagicDefense(m_Level, m_StrengthenLevel);
            }
        }

        public int GetMagicDefense(int level, int strengthenLevel)
        {
            return Mathf.RoundToInt((GearDataRow.MagicDefense + GearDataRow.LCMagicDefense * (level - 1)) * (1 + (strengthenLevel * GearDataRow.SLCMagicDfsIncreaseRate)));
        }

        public float LCMaxHP
        {
            get
            {
                return GearDataRow.LCMaxHP;
            }
        }

        public float LCPhysicalAttack
        {
            get
            {
                return GearDataRow.LCPhysicalAttack;
            }
        }

        public float LCPhysicalDefense
        {
            get
            {
                return GearDataRow.LCPhysicalDefense;
            }
        }

        public float LCMagicAttack
        {
            get
            {
                return GearDataRow.LCMagicAttack;
            }
        }

        public float LCMagicDefense
        {
            get
            {
                return GearDataRow.LCMagicDefense;
            }
        }

        public float SLCMaxHPIncreaseRate
        {
            get
            {
                return GearDataRow.SLCMaxHPIncreaseRate;
            }
        }

        public float SLCPhysicalAtkIncreaseRate
        {
            get
            {
                return GearDataRow.SLCPhysicalAtkIncreaseRate;
            }
        }

        public float SLCPhysicalDfsIncreaseRate
        {
            get
            {
                return GearDataRow.SLCPhysicalDfsIncreaseRate;
            }
        }

        public float SLCMagicAtkIncreaseRate
        {
            get
            {
                return GearDataRow.SLCMagicAtkIncreaseRate;
            }
        }

        public float SLCMagicDfsIncreaseRate
        {
            get
            {
                return GearDataRow.SLCMagicDfsIncreaseRate;
            }
        }

        public float ReducedSkillCoolDownRate
        {
            get
            {
                return GearDataRow.ReducedSkillCoolDownRate;
            }
        }

        public int StrengthenItemId
        {
            get
            {
                return GearDataRow.StrengthenItemId;
            }
        }

        public float CriticalHitProb
        {
            get
            {
                return GearDataRow.CriticalHitProb;
            }
        }

        public float CriticalHitRate
        {
            get
            {
                return GearDataRow.CriticalHitRate;
            }
        }

        public float PhysicalAtkReflectRate
        {
            get
            {
                return GearDataRow.PhysicalAtkReflectRate;
            }
        }

        public float MagicAtkReflectRate
        {
            get
            {
                return GearDataRow.MagicAtkReflectRate;
            }
        }

        public float OppPhysicalDfsReduceRate
        {
            get
            {
                return GearDataRow.OppPhysicalDfsReduceRate;
            }
        }

        public float OppMagicDfsReduceRate
        {
            get
            {
                return GearDataRow.OppMagicDfsReduceRate;
            }
        }

        public float DamageReductionRate
        {
            get
            {
                return GearDataRow.DamageReductionRate;
            }
        }

        public int AdditionalDamage
        {
            get
            {
                return GearDataRow.AdditionalDamage;
            }
        }

        public float AntiCriticalHitProb
        {
            get
            {
                return GearDataRow.AntiCriticalHitProb;
            }
        }

        public float PhysicalAtkHPAbsorbRate
        {
            get
            {
                return GearDataRow.PhysicalAtkHPAbsorbRate;
            }
        }

        public float MagicAtkHPAbsorbRate
        {
            get
            {
                return GearDataRow.MagicAtkHPAbsorbRate;
            }
        }

        public int Quality
        {
            get
            {
                return GearDataRow.Quality;
            }
        }

        public int Position
        {
            get
            {
                return GearDataRow.Type;
            }
        }

        public string Name
        {
            get
            {
                return GearDataRow.Name;
            }
        }

        public int IconId
        {
            get
            {
                return GearDataRow.IconId;
            }
        }

        public int Price
        {
            get
            {
                return GearDataRow.Price;
            }
        }

        public float ReducedHeroSwitchCDRate
        {
            get
            {
                return GearDataRow.ReducedHeroSwitchCDRate;
            }
        }

        public void UpdateData(PBGearInfo data)
        {
            GearInfo = data;
            m_Id = data.Id;

            if (data.HasType)
            {
                m_Type = data.Type;
            }

            if (data.HasLevel)
            {
                m_Level = data.Level;
            }

            if (data.HasStrengthenLevel)
            {
                m_StrengthenLevel = data.StrengthenLevel;
            }
        }
    }
}
