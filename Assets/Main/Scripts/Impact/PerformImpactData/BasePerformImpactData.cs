using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class BasePerformImpactData
    {
        public ICampable Origin
        {
            get;
            set;
        }

        public EntityData OriginData
        {
            get;
            set;
        }

        public ITargetable Target
        {
            get;
            set;
        }

        public EntityData TargetData
        {
            get;
            set;
        }

        public ImpactSourceType SourceType
        {
            get;
            set;
        }

        public ImpactAuxData AuxData
        {
            get;
            set;
        }

        public DRImpact DataRow
        {
            get;
            private set;
        }

        private int m_SkillLevel = 1;
        public int SkillLevel
        {
            get
            {
                return m_SkillLevel;
            }

            set
            {
                m_SkillLevel = Mathf.Max(value, 1);
            }
        }

        private int m_SkillIndex = -1;
        public int SkillIndex
        {
            get
            {
                return m_SkillIndex;
            }

            set
            {
                m_SkillIndex = value;
            }
        }


        public SkillBadgesData SkillBadges
        {
            get;
            set;
        }

        private BuffType m_BuffCondition = BuffType.Undefined;


        public BuffType BuffCondition
        {
            get
            {
                return m_BuffCondition;
            }

            set
            {
                m_BuffCondition = value;
            }
        }

        public virtual void SetDataRow(DRImpact dataRow)
        {
            DataRow = dataRow;
        }

        public BasePerformImpactData()
        {

        }
    }
}
