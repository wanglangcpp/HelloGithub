using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class RoomHeroData : IGenericData<RoomHeroData, PBRoomHeroInfo>
    {
        public int Key
        {
            get
            {
                return EntityId;
            }
        }

        [SerializeField]
        private int m_EntityId;

        public int EntityId
        {
            get
            {
                return m_EntityId;
            }
        }

        [SerializeField]
        private int m_AdditionalDamage;

        public int AdditionalDamage
        {
            get
            {
                return m_AdditionalDamage;
            }
        }

        [SerializeField]
        private float m_AntiCriticalHitProb;

        public float AntiCriticalHitProb
        {
            get
            {
                return m_AntiCriticalHitProb;
            }
        }

        [SerializeField]
        private CampType m_Camp;

        public CampType Camp
        {
            get
            {
                return m_Camp;
            }
        }

        [SerializeField]
        private float m_CriticalHitProb;

        public float CriticalHitProb
        {
            get
            {
                return m_CriticalHitProb;
            }
        }

        [SerializeField]
        private float m_CriticalHitRate;

        public float CriticalHitRate
        {
            get
            {
                return m_CriticalHitRate;
            }
        }

        [SerializeField]
        private float m_DamageReductionRate;

        public float DamageReductionRate
        {
            get
            {
                return m_DamageReductionRate;
            }
        }

        [SerializeField]
        private float m_HeroSwitchCoolDown;

        public float ReducedHeroSwitchCD
        {
            get
            {
                return m_HeroSwitchCoolDown;
            }
        }

        [SerializeField]
        private int m_HP;

        public int HP
        {
            get
            {
                return m_HP;
            }
        }

        [SerializeField]
        private float m_MagicAtkHPAbsorbRate;

        public float MagicAtkHPAbsorbRate
        {
            get
            {
                return m_MagicAtkHPAbsorbRate;
            }
        }

        [SerializeField]
        private float m_MagicAtkReflectRate;

        public float MagicAtkReflectRate
        {
            get
            {
                return m_MagicAtkReflectRate;
            }
        }

        [SerializeField]
        private int m_MagicAttack;

        public int MagicAttack
        {
            get
            {
                return m_MagicAttack;
            }
        }

        [SerializeField]
        private int m_MagicDefense;

        public int MagicDefense
        {
            get
            {
                return m_MagicDefense;
            }
        }

        [SerializeField]
        private int m_MaxHP;

        public int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        [SerializeField]
        private float m_OppMagicDfsReduceRate;

        public float OppMagicDfsReduceRate
        {
            get
            {
                return m_OppMagicDfsReduceRate;
            }
        }

        [SerializeField]
        private float m_OppPhysicalDfsReduceRate;

        public float OppPhysicalDfsReduceRate
        {
            get
            {
                return m_OppPhysicalDfsReduceRate;
            }
        }

        [SerializeField]
        private float m_PhysicalAtkHPAbsorbRate;

        public float PhysicalAtkHPAbsorbRate
        {
            get
            {
                return m_PhysicalAtkHPAbsorbRate;
            }
        }

        [SerializeField]
        private float m_PhysicalAtkReflectRate;

        public float PhysicalAtkReflectRate
        {
            get
            {
                return m_PhysicalAtkReflectRate;
            }
        }

        [SerializeField]
        private int m_PhysicalAttack;

        public int PhysicalAttack
        {
            get
            {
                return m_PhysicalAttack;
            }
        }

        [SerializeField]
        private int m_PhysicalDefense;

        public int PhysicalDefense
        {
            get
            {
                return m_PhysicalDefense;
            }
        }

        [SerializeField]
        private float m_RecoverHP;

        public float RecoverHP
        {
            get
            {
                return m_RecoverHP;
            }
        }

        [SerializeField]
        private float m_ReducedSkillCoolDown;

        public float ReducedSkillCoolDown
        {
            get
            {
                return m_ReducedSkillCoolDown;
            }
        }

        [SerializeField]
        private float m_SteadyRecoverSpeed;

        public float SteadyRecoverSpeed
        {
            get
            {
                return m_SteadyRecoverSpeed;
            }
        }

        [SerializeField]
        private float m_MaxSteadyValue;

        public float MaxSteadyValue
        {
            get
            {
                return m_MaxSteadyValue;
            }
        }

        [SerializeField]
        private float m_SteadyValue;

        public float Steady
        {
            get
            {
                return m_SteadyValue;
            }
        }

        [SerializeField]
        private bool m_SteadyBarStatus;

        public bool SteadyBarStatus
        {
            get
            {
                return m_SteadyBarStatus;
            }
        }

        [SerializeField]
        private List<PBBuffInfo> m_Buffs;

        public List<PBBuffInfo> Buffs
        {
            get
            {
                return m_Buffs;
            }
        }

        public int Type { get; internal set; }
        public string Name { get; internal set; }
        public int ElementId { get; internal set; }
        public int Level { get; internal set; }

        public void UpdateData(PBRoomHeroInfo pb)
        {
            PBLobbyHeroInfo lobbyHeroInfo = pb.LobbyHeroInfo;

            m_EntityId = pb.EntityId;

            Type = lobbyHeroInfo.Type;

            if (lobbyHeroInfo.HasLevel)
            {
                Level = lobbyHeroInfo.Level;
            }

            if (pb.HasAdditionalDamage)
            {
                m_AdditionalDamage = pb.AdditionalDamage;
            }

            if (pb.HasAntiCriticalHitProb)
            {
                m_AntiCriticalHitProb = pb.AntiCriticalHitProb;
            }

            if (pb.HasCamp)
            {
                m_Camp = (CampType)pb.Camp;
            }

            if (pb.HasCriticalHitProb)
            {
                m_CriticalHitProb = pb.CriticalHitProb;
            }

            if (pb.HasCriticalHitRate)
            {
                m_CriticalHitRate = pb.CriticalHitRate;
            }

            if (pb.HasDamageReductionRate)
            {
                m_DamageReductionRate = pb.DamageReductionRate;
            }

            if (pb.HasHP)
            {
                m_HP = pb.HP;
            }

            if (pb.HasMagicAtkHPAbsorbRate)
            {
                m_MagicAtkHPAbsorbRate = pb.MagicAtkHPAbsorbRate;
            }

            if (pb.HasMagicAtkReflectRate)
            {
                m_MagicAtkReflectRate = pb.MagicAtkReflectRate;
            }

            if (pb.HasMagicAttack)
            {
                m_MagicAttack = pb.MagicAttack;
            }

            if (pb.HasMagicDefense)
            {
                m_MagicDefense = pb.MagicDefense;
            }

            if (pb.HasMaxHP)
            {
                m_MaxHP = pb.MaxHP;
            }

            if (pb.HasOppMagicDfsReduceRate)
            {
                m_OppMagicDfsReduceRate = pb.OppMagicDfsReduceRate;
            }

            if (pb.HasOppPhysicalDfsReduceRate)
            {
                m_OppPhysicalDfsReduceRate = pb.OppPhysicalDfsReduceRate;
            }

            if (pb.HasPhysicalAtkHPAbsorbRate)
            {
                m_PhysicalAtkHPAbsorbRate = pb.PhysicalAtkHPAbsorbRate;
            }

            if (pb.HasPhysicalAtkReflectRate)
            {
                m_PhysicalAtkReflectRate = pb.PhysicalAtkReflectRate;
            }

            if (pb.HasPhysicalAttack)
            {
                m_PhysicalAttack = pb.PhysicalAttack;
            }

            if (pb.HasPhysicalDefense)
            {
                m_PhysicalDefense = pb.PhysicalDefense;
            }

            if (lobbyHeroInfo.SkillLevels.Count > 0)
            {
                // TODO
                //SkillLevels.Clear();
                //for (int i = 0; i < lobbyHeroInfo.SkillLevels.Count; i++)
                //{
                //    SkillLevels.Add(lobbyHeroInfo.SkillLevels[i]);
                //}
            }

            if (pb.HasReducedSkillCoolDownRate)
            {
                m_ReducedSkillCoolDown = pb.ReducedSkillCoolDownRate;
            }

            if (pb.HasHeroSwitchCoolDownRate)
            {
                m_HeroSwitchCoolDown = pb.HeroSwitchCoolDownRate;
            }

            if (pb.HasRecoverHP)
            {
                m_RecoverHP = pb.RecoverHP;
            }

            if (pb.HasMaxSteadyValue)
            {
                m_MaxSteadyValue = pb.MaxSteadyValue;
            }

            if (pb.HasSteadyRecoverSpeed)
            {
                m_SteadyRecoverSpeed = pb.SteadyRecoverSpeed;
            }

            if (pb.HasSteadyValue)
            {
                m_SteadyValue = pb.SteadyValue;
            }

            if (pb.HasSteadyBarStatus)
            {
                m_SteadyBarStatus = pb.SteadyBarStatus;
                if (!m_SteadyBarStatus)
                {
                    DateTime breakTime = new DateTime(pb.LastBreakSteadyTime, DateTimeKind.Utc);
                    var duration = (GameEntry.Time.LobbyServerUtcTime - breakTime).TotalSeconds;
                    m_SteadyValue = (float)duration * m_SteadyRecoverSpeed;
                }
            }

            m_Buffs = pb.Buffs;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Key: ");
            sb.Append(Key.ToString());
            sb.Append(", EntityId: ");
            sb.Append(EntityId.ToString());
            return sb.ToString();
        }
    }
}
