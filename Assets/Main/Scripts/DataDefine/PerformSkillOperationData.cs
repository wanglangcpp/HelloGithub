using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 请求释放技能操作的数据。
    /// </summary>
    [Serializable]
    public class PerformSkillOperationData
    {
        [SerializeField]
        private int m_SkillId;

        [SerializeField]
        private bool m_IsContinualTap;

        [SerializeField]
        private bool m_IsInCombo;

        [SerializeField]
        private bool m_ForcePerform;

        [SerializeField]
        private int m_SkillIndex;

        /// <summary>
        /// 技能编号。
        /// </summary>
        public int SkillId
        {
            get
            {
                return m_SkillId;
            }

            set
            {
                m_SkillId = value;
            }
        }

        /// <summary>
        /// 技能索引。
        /// </summary>
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

        /// <summary>
        /// 是否为连续点击技能。
        /// </summary>
        public bool IsContinualTap
        {
            get
            {
                return m_IsContinualTap;
            }

            set
            {
                m_IsContinualTap = value;
            }
        }

        /// <summary>
        /// 是否为连续技的一部分。
        /// </summary>
        public bool IsInCombo
        {
            get
            {
                return m_IsInCombo;
            }

            set
            {
                m_IsInCombo = value;
            }
        }

        /// <summary>
        /// 是否强制释放。
        /// </summary>
        public bool ForcePerform
        {
            get
            {
                return m_ForcePerform;
            }

            set
            {
                m_ForcePerform = value;
            }
        }
    }
}
