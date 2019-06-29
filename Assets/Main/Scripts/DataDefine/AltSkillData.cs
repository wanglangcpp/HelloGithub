using GameFramework;
using GameFramework.DataTable;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class AltSkillData
    {
        [SerializeField]
        private int m_SkillId;

        [SerializeField]
        private int m_SkillLevel;

        [SerializeField]
        private float m_LeftTime;

        [SerializeField]
        private CoolDownTime[] m_SkillCD;

        private DRAltSkill m_DRAltSkill;

        public AltSkillData()
        {
            m_SkillId = 0;
            m_SkillLevel = 0;
            m_LeftTime = 0f;
            m_DRAltSkill = null;

            m_SkillCD = new CoolDownTime[Constant.TotalSkillGroupCount];
            for (int i = 0; i < Constant.TotalSkillGroupCount; i++)
            {
                m_SkillCD[i] = new CoolDownTime();
            }
        }

        public bool Enabled
        {
            get
            {
                return m_SkillId > 0;
            }
        }

        public int SkillId
        {
            get
            {
                return m_SkillId;
            }
            set
            {
                m_SkillId = value;

                if (m_SkillId > 0)
                {
                    IDataTable<DRAltSkill> dtAltSkill = GameEntry.DataTable.GetDataTable<DRAltSkill>();
                    m_DRAltSkill = dtAltSkill.GetDataRow(m_SkillId);
                    if (m_DRAltSkill == null)
                    {
                        Log.Warning("Can not load alt skill '{0}' from data table.", m_SkillId.ToString());
                        return;
                    }
                }
                else
                {
                    m_DRAltSkill = null;
                }
            }
        }

        public int SkillLevel
        {
            get
            {
                return m_SkillLevel;
            }
            set
            {
                m_SkillLevel = value;
            }
        }

        public bool IsForever
        {
            get
            {
                return m_LeftTime < 0f;
            }
        }

        public float LeftTime
        {
            get
            {
                return m_LeftTime;
            }
            set
            {
                m_LeftTime = value;
            }
        }

        public CoolDownTime[] SkillCD
        {
            get
            {
                return m_SkillCD;
            }
        }

        public DRAltSkill DRAltSkill
        {
            get
            {
                return m_DRAltSkill;
            }
        }

        public CoolDownTime GetSkillCoolDownTime(int index)
        {
            return m_SkillCD[index];
        }
    }
}
