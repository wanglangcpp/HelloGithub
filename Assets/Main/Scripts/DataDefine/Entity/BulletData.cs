using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class BulletData : ShedObjectData, IImpactDataProvider
    {
        [SerializeField]
        private int m_BulletId;

        [SerializeField]
        private CampType m_Camp;

        [SerializeField]
        private float m_Height;

        [SerializeField]
        private float m_Speed;

        [SerializeField]
        private TransformParentType m_TransformParentType;

        [SerializeField]
        private int m_PhysicalAttack;

        [SerializeField]
        private int m_PhysicalDefense;

        [SerializeField]
        private int m_MagicAttack;

        [SerializeField]
        private int m_MagicDefense;

        [SerializeField]
        private float m_OppPhysicalDfsReduceRate;

        [SerializeField]
        private float m_OppMagicDfsReduceRate;

        [SerializeField]
        private float m_PhysicalAtkHPAbsorbRate;

        [SerializeField]
        private float m_MagicAtkHPAbsorbRate;

        [SerializeField]
        private float m_PhysicalAtkReflectRate;

        [SerializeField]
        private float m_MagicAtkReflectRate;

        [SerializeField]
        private float m_DamageReductionRate;

        [SerializeField]
        private float m_CriticalHitProb;

        [SerializeField]
        private float m_CriticalHitRate;

        [SerializeField]
        private float m_AntiCriticalHitProb;

        [SerializeField]
        private float m_DamageRandomRange;

        [SerializeField]
        private int m_AdditionalDamage;

        [SerializeField]
        private int m_ElementId;

        [SerializeField]
        private bool m_Reboundable;

        [SerializeField]
        private int m_OwnerSkillIndex = -1;

        [SerializeField]
        private int m_OwnerSkillLevel = 1;

        public BulletData(int entityId)
            : base(entityId)
        {

        }

        public new Bullet Entity
        {
            get
            {
                return base.Entity as Bullet;
            }
        }

        /// <summary>
        /// 子弹类型编号。
        /// </summary>
        public int BulletId
        {
            get
            {
                return m_BulletId;
            }
            set
            {
                m_BulletId = value;
            }
        }

        /// <summary>
        /// 子弹阵营。
        /// </summary>
        public CampType Camp
        {
            get
            {
                return m_Camp;
            }
            set
            {
                m_Camp = value;
            }
        }

        /// <summary>
        /// 距离地面高度。
        /// </summary>
        public float Height
        {
            get
            {
                return m_Height;
            }
            set
            {
                m_Height = value;
            }
        }

        /// <summary>
        /// 速度。
        /// </summary>
        public float Speed
        {
            get
            {
                return m_Speed;
            }
            set
            {
                m_Speed = value;
            }
        }

        /// <summary>
        /// 用于伤害计算的状态。
        /// </summary>
        public StateForImpactCalc StateForImpactCalc
        {
            get
            {
                return StateForImpactCalc.Normal;
            }
        }

        /// <summary>
        /// 最大生命值。
        /// </summary>
        public int MaxHP
        {
            get
            {
                throw new NotImplementedException("MaxHP");
            }
        }

        /// <summary>
        /// 物理攻击。
        /// </summary>
        public int PhysicalAttack
        {
            get
            {
                return m_PhysicalAttack;
            }
            set
            {
                m_PhysicalAttack = value;
            }
        }

        /// <summary>
        /// 物理防御。
        /// </summary>
        public int PhysicalDefense
        {
            get
            {
                return m_PhysicalDefense;
            }
            set
            {
                m_PhysicalDefense = value;
            }
        }

        /// <summary>
        /// 法术攻击。
        /// </summary>
        public int MagicAttack
        {
            get
            {
                return m_MagicAttack;
            }
            set
            {
                m_MagicAttack = value;
            }
        }

        /// <summary>
        /// 法术防御。
        /// </summary>
        public int MagicDefense
        {
            get
            {
                return m_MagicDefense;
            }
            set
            {
                m_MagicDefense = value;
            }
        }

        /// <summary>
        /// 降低对方物理防御百分比。
        /// </summary>
        public float OppPhysicalDfsReduceRate
        {
            get
            {
                return m_OppPhysicalDfsReduceRate;
            }
            set
            {
                m_OppPhysicalDfsReduceRate = value;
            }
        }

        /// <summary>
        /// 降低对方法术防御百分比。
        /// </summary>
        public float OppMagicDfsReduceRate
        {
            get
            {
                return m_OppMagicDfsReduceRate;
            }
            set
            {
                m_OppMagicDfsReduceRate = value;
            }
        }

        /// <summary>
        /// 物理伤害吸血率。
        /// </summary>
        public float PhysicalAtkHPAbsorbRate
        {
            get
            {
                return m_PhysicalAtkHPAbsorbRate;
            }
            set
            {
                m_PhysicalAtkHPAbsorbRate = value;
            }
        }

        /// <summary>
        /// 法术伤害吸血率。
        /// </summary>
        public float MagicAtkHPAbsorbRate
        {
            get
            {
                return m_MagicAtkHPAbsorbRate;
            }
            set
            {
                m_MagicAtkHPAbsorbRate = value;
            }
        }

        /// <summary>
        /// 物理伤害反击率。
        /// </summary>
        public float PhysicalAtkReflectRate
        {
            get
            {
                return m_PhysicalAtkReflectRate;
            }
            set
            {
                m_PhysicalAtkReflectRate = value;
            }
        }

        /// <summary>
        /// 法术伤害反击率。
        /// </summary>
        public float MagicAtkReflectRate
        {
            get
            {
                return m_MagicAtkReflectRate;
            }
            set
            {
                m_MagicAtkReflectRate = value;
            }
        }

        /// <summary>
        /// 受击伤害减免率。
        /// </summary>
        public float DamageReductionRate
        {
            get
            {
                return m_DamageReductionRate;
            }
            set
            {
                m_DamageReductionRate = value;
            }
        }

        /// <summary>
        /// 暴击率。
        /// </summary>
        public float CriticalHitProb
        {
            get
            {
                return m_CriticalHitProb;
            }
            set
            {
                m_CriticalHitProb = value;
            }
        }

        /// <summary>
        /// 暴击伤害倍数。
        /// </summary>
        public float CriticalHitRate
        {
            get
            {
                return m_CriticalHitRate;
            }
            set
            {
                m_CriticalHitRate = value;
            }
        }

        /// <summary>
        /// 免除暴击率。
        /// </summary>
        public float AntiCriticalHitProb
        {
            get
            {
                return m_AntiCriticalHitProb;
            }
            set
            {
                m_AntiCriticalHitProb = value;
            }
        }

        /// <summary>
        /// 伤害浮动率。
        /// </summary>
        public float DamageRandomRate
        {
            get
            {
                return m_DamageRandomRange;
            }
            set
            {
                m_DamageRandomRange = value;
            }
        }

        /// <summary>
        /// 附加伤害。
        /// </summary>
        public int AdditionalDamage
        {
            get
            {
                return m_AdditionalDamage;
            }
            set
            {
                m_AdditionalDamage = value;
            }
        }

        public TransformParentType TransformParentType
        {
            get
            {
                return m_TransformParentType;
            }
            set
            {
                m_TransformParentType = value;
            }
        }

        /// <summary>
        /// 克制类属性编号。
        /// </summary>
        public int ElementId
        {
            get
            {
                return m_ElementId;
            }
            set
            {
                m_ElementId = value;
            }
        }

        /// <summary>
        /// 是否可反弹。
        /// </summary>
        public bool IsReboundable
        {
            get
            {
                return m_Reboundable;
            }
            set
            {
                m_Reboundable = value;
            }
        }

        /// <summary>
        /// 所有者技能索引。
        /// </summary>
        public int OwnerSkillIndex
        {
            get
            {
                return m_OwnerSkillIndex;
            }

            set
            {
                m_OwnerSkillIndex = value;
            }
        }

        /// <summary>
        /// 所有者技能等级。
        /// </summary>
        public int OwnerSkillLevel
        {
            get
            {
                return m_OwnerSkillLevel;
            }

            set
            {
                m_OwnerSkillLevel = value;
            }
        }
    }
}
