using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class NpcCharacterData : CharacterData, IMeleeHeroData
    {
        [SerializeField]
        private int m_NpcId;

        [SerializeField]
        private Vector2 m_HomePosition;

        [SerializeField]
        private bool m_CountForPlayerKill;

        [SerializeField]
        private NpcCategory m_Category;

        [SerializeField]
        protected int m_ChestId = -1;

        [SerializeField]
        private bool m_ShowName = false;

        [SerializeField]
        private int m_MeleeExp = 0;

        [SerializeField]
        private bool m_IsFakeHero = false;

        [SerializeField]
        private int m_MeleeLevel = 0;

        [SerializeField]
        private int m_MeleeExpAtCurrentLevel = 0;

        [SerializeField]
        private int m_MeleeScore = 0;

        public NpcCharacterData(int entityId)
            : base(entityId)
        {

        }

        public new NpcCharacter Entity
        {
            get
            {
                return base.Entity as NpcCharacter;
            }
        }

        public int NpcId
        {
            get
            {
                return m_NpcId;
            }
            set
            {
                m_NpcId = value;
            }
        }

        public Vector2 HomePosition
        {
            get
            {
                return m_HomePosition;
            }
            set
            {
                m_HomePosition = value;
            }
        }

        /// <summary>
        /// 是否计算如玩家击杀数量
        /// </summary>
        public bool CountForPlayerKill
        {
            get
            {
                return m_CountForPlayerKill;
            }
            set
            {
                m_CountForPlayerKill = value;
            }
        }

        public NpcCategory Category
        {
            get
            {
                return m_Category;
            }
            set
            {
                m_Category = value;
            }
        }

        public bool ShowName
        {
            get
            {
                return m_ShowName;
            }
            set
            {
                m_ShowName = value;
            }
        }

        public int ChestId
        {
            get
            {
                return m_ChestId;
            }
            set
            {
                m_ChestId = value;
            }
        }

        /// <summary>
        /// 乱斗被杀死转换成的经验值。
        /// </summary>
        public int MeleeExp
        {
            get
            {
                return m_MeleeExp;
            }

            set
            {
                m_MeleeExp = value;
            }
        }

        /// <summary>
        /// 是否为假英雄。
        /// </summary>
        public bool IsFakeHero
        {
            get
            {
                return m_IsFakeHero;
            }

            set
            {
                m_IsFakeHero = value;
            }
        }

        /// <summary>
        /// 乱斗过程中的等级。
        /// </summary>
        public int MeleeLevel
        {
            get
            {
                return m_MeleeLevel;
            }

            set
            {
                m_MeleeLevel = value;
            }
        }

        /// <summary>
        /// 乱斗过程中当前等级的经验值。
        /// </summary>
        public int MeleeExpAtCurrentLevel
        {
            get
            {
                return m_MeleeExpAtCurrentLevel;
            }

            set
            {
                m_MeleeExpAtCurrentLevel = value;
            }
        }

        /// <summary>
        /// 乱斗积分。
        /// </summary>
        public int MeleeScore
        {
            get
            {
                return m_MeleeScore;
            }

            set
            {
                m_MeleeScore = value;
            }
        }

        /// <summary>
        /// 增加乱斗经验值。
        /// </summary>
        /// <param name="deltaExp">经验增量。</param>
        public void AddMeleeExp(int deltaExp)
        {
            this.AddMeleeExp(deltaExp, OnMeleeLevelUp);
        }

        private void OnMeleeLevelUp()
        {
            var mimicMeleeBaseDataRow = GameEntry.DataTable.GetDataTable<DRMimicMeleeBase>().GetDataRow(MeleeLevel);
            Steady.MaxSteady = mimicMeleeBaseDataRow.Steady;
            Steady.SteadyRecoverSpeed = mimicMeleeBaseDataRow.SteadyRecoverSpeed;
            var hpRatio = (float)HP / MaxHP;
            MaxHP = mimicMeleeBaseDataRow.MaxHPBase;
            HP = Mathf.RoundToInt(MaxHP * hpRatio);
            PhysicalAttack = mimicMeleeBaseDataRow.PhysicalAttackBase;
            PhysicalDefense = mimicMeleeBaseDataRow.PhysicalDefenseBase;
            DamageRandomRate = mimicMeleeBaseDataRow.DamageRandomRate;
        }
    }
}
