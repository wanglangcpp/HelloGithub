using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class InitRandomShowNpcGroup : Action
    {
        /// <summary>
        /// 组关键字。
        /// </summary>
        [SerializeField]
        private string m_GroupKey = string.Empty;

        /// <summary>
        /// NPC 索引区间。
        /// </summary>
        [SerializeField]
        private IntRange[] m_NpcIndexRanges = null;

        /// <summary>
        /// 是否使用随机权重。
        /// </summary>
        [SerializeField]
        private bool m_UseWeights = false;

        /// <summary>
        /// 对应于索引区间的随机权重，与 NPC 索引区间一一对应。每个区间对应的权重将适用于区间中的每个 NPC 索引号。仅 <see cref="m_UseWeights"/> 为真时有效。
        /// </summary>
        [SerializeField]
        private int[] m_NpcIndexRangeWeights = null;

        /// <summary>
        /// 总共生成 NPC 的目标个数。
        /// </summary>
        [SerializeField]
        private int m_TargetTotalCount = 0;

        /// <summary>
        /// 本组同时存活 NPC 的最大数量。
        /// </summary>
        [SerializeField]
        private int m_UpperLimit = 4;

        public override TaskStatus OnUpdate()
        {
            var npcIndicesToWeights = new Dictionary<int, int>();

            for (int i = 0; i < m_NpcIndexRanges.Length; ++i)
            {
                IntRange range = m_NpcIndexRanges[i];
                int weight = !m_UseWeights ? 1 : i >= m_NpcIndexRangeWeights.Length ? 0 : m_NpcIndexRangeWeights[i];
                for (int index = range.MinValue; index <= range.MaxValue; index++)
                {
                    npcIndicesToWeights[index] = weight;
                }
            }

            GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Data.AddRandomShowNpcGroupData(m_GroupKey, npcIndicesToWeights, m_TargetTotalCount, m_UpperLimit);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_GroupKey = string.Empty;
            m_NpcIndexRanges = null;
        }
    }
}
