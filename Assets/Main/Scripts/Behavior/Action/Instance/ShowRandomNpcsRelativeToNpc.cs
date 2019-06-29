using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ShowRandomNpcsRelativeToNpc : Action
    {
        [SerializeField]
        private string m_GroupKey = string.Empty;

        [SerializeField]
        private int m_MaxCountOnce = 0;

        [SerializeField]
        private bool m_ForceInstant = false;

        [SerializeField]
        private float m_RandomRadius = 1.0f;

        [SerializeField]
        private int m_RetryCount = 20;

        [SerializeField]
        private int m_TargetNpcIndex = 0;

        private List<int> m_AvailableIndices = null;

        private RandomShowNpcGroupData m_GroupData = null;

        private int m_LivingCount = 0;

        private int m_CurrentIndex = 0;

        private NpcCharacter m_Target = null;

        public override void OnStart()
        {
            m_Target = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetNpcFromIndex(m_TargetNpcIndex);
            var instanceLogic = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic;
            var instanceData = instanceLogic.Data;
            m_CurrentIndex = 0;
            m_GroupData = instanceData.GetRandomShowNpcGroupData(m_GroupKey);
            m_LivingCount = instanceLogic.GetLivingNpcCountInRandomShowNpcGroup(m_GroupData);
            m_AvailableIndices = instanceLogic.GetAvailableIdsForRandomShowNpcGroup(m_GroupData);
        }

        public override void OnReset()
        {
            m_GroupKey = string.Empty;
            m_AvailableIndices = null;
            m_GroupData = null;
            m_LivingCount = 0;
            m_CurrentIndex = 0;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_GroupData.IsInterrupted)
            {
                return TaskStatus.Failure;
            }

            if (m_Target == null || m_Target.IsDead)
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

        private bool ShowOneNpc()
        {
            var randomIndex = Random.Range(0, m_AvailableIndices.Count);
            var npcIndex = m_AvailableIndices[randomIndex];
            m_AvailableIndices.RemoveAt(randomIndex);

            var instanceLogic = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic;

            if (instanceLogic.IsNpcForbidden(npcIndex))
            {
                return false;
            }

            var path = new NavMeshPath();

            for (int i = 0; i < m_RetryCount; ++i)
            {
                var vec = GeneratePosition(i);
                var npcPosition = AIUtility.SamplePosition(vec);
                bool gotPath = NavMesh.CalculatePath(m_Target.CachedTransform.localPosition, npcPosition, NavMesh.AllAreas, path);
                if (gotPath && path.status == NavMeshPathStatus.PathComplete)
                {
                    if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.ShowNpcRelativeToNpc(npcIndex, vec))
                    {
                        m_GroupData.IncreaseTotalGeneratedNumber();
                        return true;
                    }
                }
            }

            return false;
        }

        private Vector3 GeneratePosition(int times)
        {
            Vector2 vec = Vector2.zero;

            if (m_Target == null)
            {
                return vec;
            }
            vec = m_Target.CachedTransform.localPosition.ToVector2();
            vec += Random.insideUnitCircle * m_RandomRadius;
            return vec;
        }

        private bool IsFinished
        {
            get
            {
                return m_CurrentIndex >= m_GroupData.UpperLimit - m_LivingCount || m_CurrentIndex >= m_MaxCountOnce || m_AvailableIndices.Count == 0 || m_GroupData.GenGoalIsAchieved;
            }
        }
    }
}
