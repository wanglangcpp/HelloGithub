using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 随机出现 NPC 的相关组数据。
    /// </summary>
    [Serializable]
    public class RandomShowNpcGroupData
    {
        [SerializeField]
        private string m_GroupKey;

        /// <summary>
        /// 用于标识 NPC 组唯一性的键。
        /// </summary>
        public string GroupKey
        {
            get
            {
                return m_GroupKey;
            }
        }

        private Dictionary<int, int> m_IndicesToWeights;

        /// <summary>
        /// 可用的种怪表中的索引号与随机权重。
        /// </summary>
        public IDictionary<int, int> IndicesToWeights
        {
            get
            {
                return m_IndicesToWeights;
            }
        }

        [SerializeField]
        private int m_TotalGenerated = 0;

        /// <summary>
        /// 本组总共生成的 NPC 个数。
        /// </summary>
        public int TotalGenerated
        {
            get
            {
                return m_TotalGenerated;
            }
        }

        [SerializeField]
        private int m_TotalGeneratedGoal = 0;

        /// <summary>
        /// 本组总共生成 NPC 的目标个数。
        /// </summary>
        public int TotalGeneratedGoal
        {
            get
            {
                return m_TotalGeneratedGoal;
            }
        }

        [SerializeField]
        private bool m_Interrupted = false;

        /// <summary>
        /// 是否被打断。
        /// </summary>
        public bool IsInterrupted
        {
            get
            {
                return m_Interrupted;
            }
        }

        public void IncreaseTotalGeneratedNumber(int inc = 1)
        {
            m_TotalGenerated += inc;
        }

        /// <summary>
        /// 打断。
        /// </summary>
        public void Interrupt()
        {
            m_Interrupted = true;
        }

        /// <summary>
        /// 生成总量是否达成目标。
        /// </summary>
        public bool GenGoalIsAchieved
        {
            get
            {
                return TotalGenerated >= TotalGeneratedGoal;
            }
        }

        [SerializeField]
        private int m_UpperLimit = 0;

        /// <summary>
        /// 本组 NPC 可同时存活的个数上限。
        /// </summary>
        public int UpperLimit
        {
            get
            {
                return m_UpperLimit;
            }
        }

        public RandomShowNpcGroupData(string groupKey, IDictionary<int, int> indicesToWeights, int targetTotalGenerated, int upperLimit)
        {
            m_GroupKey = groupKey;

            m_IndicesToWeights = new Dictionary<int, int>(indicesToWeights);
            m_TotalGenerated = 0;
            m_TotalGeneratedGoal = targetTotalGenerated;
            m_UpperLimit = upperLimit;
        }
    }
}
