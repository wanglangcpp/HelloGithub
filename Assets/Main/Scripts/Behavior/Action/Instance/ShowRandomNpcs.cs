using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ShowRandomNpcs : Action
    {
        [SerializeField]
        private string m_GroupKey = string.Empty;

        [SerializeField]
        private int m_MaxCountOnce = 0;

        [SerializeField]
        private bool m_ForceInstant = false;

        private List<int> m_AvailableIndices = null;

        private RandomShowNpcGroupData m_GroupData = null;

        private int m_LivingCount = 0;

        private int m_CurrentIndex = 0;

        private int m_TotalWeight = 0;

        public override void OnStart()
        {
            var instanceLogic = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic;
            var instanceData = instanceLogic.Data;
            m_CurrentIndex = 0;
            m_GroupData = instanceData.GetRandomShowNpcGroupData(m_GroupKey);
            m_LivingCount = instanceLogic.GetLivingNpcCountInRandomShowNpcGroup(m_GroupData);
            m_AvailableIndices = instanceLogic.GetAvailableIdsForRandomShowNpcGroup(m_GroupData);

            for (int i = 0; i < m_AvailableIndices.Count; ++i)
            {
                m_TotalWeight += m_GroupData.IndicesToWeights[m_AvailableIndices[i]];
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (m_GroupData.IsInterrupted)
            {
                return TaskStatus.Failure;
            }

            if (IsFinished)
            {
                return TaskStatus.Success;
            }

            do
            {
                if (!ShowOneNpc())
                {
                    return TaskStatus.Failure;
                }

                m_CurrentIndex++;
            }
            while (m_ForceInstant && !IsFinished);

            return IsFinished ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnReset()
        {
            m_GroupKey = string.Empty;
            m_AvailableIndices = null;
            m_GroupData = null;
            m_LivingCount = 0;
            m_CurrentIndex = 0;
            m_TotalWeight = 0;
        }

        private bool ShowOneNpc()
        {
            int randomIndex = GetRandomIndex(Random.value);
            int npcIndex = m_AvailableIndices[randomIndex];
            m_TotalWeight -= m_GroupData.IndicesToWeights[npcIndex];
            m_AvailableIndices.RemoveAt(randomIndex);

            var instanceLogic = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic;

            if (instanceLogic.IsNpcForbidden(npcIndex) || !instanceLogic.ShowNpc(npcIndex))
            {
                return false;
            }

            m_GroupData.IncreaseTotalGeneratedNumber();
            return true;
        }

        private int GetRandomIndex(float randomValue)
        {
            float currentWeight = 0f;
            int randomIndex = -1;
            for (int i = 0; i < m_AvailableIndices.Count; ++i)
            {
                currentWeight += m_GroupData.IndicesToWeights[m_AvailableIndices[i]];
                if (currentWeight >= Mathf.RoundToInt(randomValue * m_TotalWeight))
                {
                    randomIndex = i;
                    break;
                }
            }

            if (randomIndex < 0)
            {
                randomIndex = m_AvailableIndices.Count - 1;
            }

            return randomIndex;
        }

        private bool IsFinished
        {
            get
            {
                return m_CurrentIndex >= m_GroupData.UpperLimit - m_LivingCount || m_CurrentIndex >= m_MaxCountOnce || m_AvailableIndices.Count <= 0 || m_GroupData.GenGoalIsAchieved;
            }
        }
    }
}
