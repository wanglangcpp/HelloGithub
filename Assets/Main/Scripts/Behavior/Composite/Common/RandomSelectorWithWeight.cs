using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace Genesis.GameClient.Behavior
{
    /// <summary>
    /// 按权重(概率)随机选择执行子节点。
    /// </summary>
    [TaskCategory("Game/Common")]
    [TaskDescription("Similar to RandomSelector, but with an optional array to assign the weight (probability) of each child.")]
    [TaskIcon("{SkinColor}RandomSelectorIcon.png.png")]
    public class RandomSelectorWithWeight : Composite
    {
        [SerializeField]
        private int m_Seed = 0;

        [SerializeField]
        private bool m_UseSeed = false;

        [SerializeField]
        [Tooltip("The more the weight, the more probable its corresponding task is executed first.")]
        private float[] m_Weights = null;

        private List<int> m_ChildIndexList = new List<int>();
        private Queue<int> m_ChildrenExecutionOrder = new Queue<int>();
        private List<float> m_ShuffledWeights = new List<float>();
        private TaskStatus m_ExecutionStatus = TaskStatus.Inactive;

        public override void OnAwake()
        {
            if (m_UseSeed)
            {
#if UNITY_5_3
                Random.seed = m_Seed;
#else
                Random.InitState(m_Seed);
#endif
            }

            m_ChildIndexList.Clear();
            for (int i = 0; i < children.Count; ++i)
            {
                m_ChildIndexList.Add(i);
            }

            m_ShuffledWeights.Clear();
            if (m_Weights == null) m_Weights = new float[0];
            int j;
            for (j = 0; j < children.Count && j < m_Weights.Length; ++j)
            {
                m_ShuffledWeights.Add(m_Weights[j] > 0f ? m_Weights[j] : 0f);
            }

            for (/* Empty initializer */; j < children.Count; ++j)
            {
                m_ShuffledWeights.Add(0f);
            }
        }

        public override void OnStart()
        {
            ShuffleChilden();
        }

        public override int CurrentChildIndex()
        {
            return m_ChildrenExecutionOrder.Peek();
        }

        public override bool CanExecute()
        {
            return m_ChildrenExecutionOrder.Count > 0 && m_ExecutionStatus != TaskStatus.Success;
        }

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            if (m_ChildrenExecutionOrder.Count > 0)
            {
                m_ChildrenExecutionOrder.Dequeue();
            }
            m_ExecutionStatus = childStatus;
        }

        public override void OnConditionalAbort(int childIndex)
        {
            m_ChildrenExecutionOrder.Clear();
            m_ExecutionStatus = TaskStatus.Inactive;
            ShuffleChilden();
        }

        public override void OnEnd()
        {
            m_ExecutionStatus = TaskStatus.Inactive;
            m_ChildrenExecutionOrder.Clear();
        }

        public override void OnReset()
        {
            m_Seed = 0;
            m_UseSeed = false;
        }

        private void ShuffleChilden()
        {
            // Ref: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
            for (int i = m_ChildIndexList.Count; i > 0; --i)
            {
                // 计算累积权重。
                List<float> cumulativeWeights = new List<float>();
                for (int k = 0; k < i; ++k)
                {
                    if (k == 0)
                    {
                        cumulativeWeights.Add(m_ShuffledWeights[k]);
                    }
                    else
                    {
                        cumulativeWeights.Add(m_ShuffledWeights[k] + cumulativeWeights[k - 1]);
                    }
                }

                // 用二分法将随机权重转换成索引号 j。
                float randomWeight = Random.Range(0f, cumulativeWeights[i - 1]);
                int j = CalcIndexForWeight(randomWeight, cumulativeWeights, 0, cumulativeWeights.Count);

                int index = m_ChildIndexList[j];
                float weight = m_ShuffledWeights[j];
                m_ChildrenExecutionOrder.Enqueue(index);
                m_ChildIndexList[j] = m_ChildIndexList[i - 1];
                m_ShuffledWeights[j] = m_ShuffledWeights[i - 1];
                m_ChildIndexList[i - 1] = index;
                m_ShuffledWeights[i - 1] = weight;
            }
        }

        private int CalcIndexForWeight(float randomWeight, IList<float> cumulativeWeights, int beg, int end)
        {
            if (end - beg <= 1)
            {
                return beg;
            }

            int mid = beg + (end - beg) / 2;
            if (randomWeight > cumulativeWeights[mid - 1])
            {
                return CalcIndexForWeight(randomWeight, cumulativeWeights, mid, end);
            }

            return CalcIndexForWeight(randomWeight, cumulativeWeights, beg, mid);
        }
    }
}
