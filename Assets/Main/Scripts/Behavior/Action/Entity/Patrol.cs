using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class Patrol : Action
    {
        [SerializeField]
        private float m_PositionError = .0f;

        [SerializeField]
        private float m_MinWaitingDuration = 1f;

        [SerializeField]
        private float m_MaxWaitingDuration = 1f;

        [SerializeField]
        private bool m_SampleTargetPosition = false;

        private NpcCharacter m_Self = null;
        private float m_CachedStoppingDistance = 0f;
        private bool m_FirstUpdate = false;
        private float m_WaitingDuration = 0f;
        private float m_WaitingStartTime = -1f;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<NpcCharacter>();
            m_CachedStoppingDistance = m_Self.NavAgent.stoppingDistance;
            m_Self.NavAgent.stoppingDistance = 0f;
            m_FirstUpdate = true;
            m_WaitingDuration = 0f;
        }

        public override void OnEnd()
        {
            m_Self.NavAgent.stoppingDistance = m_CachedStoppingDistance;
            m_Self.Motion.StopMove();
        }

        public override void OnReset()
        {
            m_Self = null;
            m_CachedStoppingDistance = 0f;
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the NpcCharacter component!");
                return TaskStatus.Failure;
            }

            if (!AIUtility.TargetCanBeSelected(m_Self))
            {
                return TaskStatus.Failure;
            }

            // Patrol times run out.
            if (m_Self.PatrolTimes == 0)
            {
                return TaskStatus.Failure;
            }

            var patrolPoints = m_Self.GetPatrolPoints();

            // No patrol points.
            if (patrolPoints.Count <= 0)
            {
                return TaskStatus.Failure;
            }

            var currentPatrolIndex = m_Self.PatrolPointIndex;
            var currentPatrolPoint = patrolPoints[currentPatrolIndex];
            if (Vector2.Distance(currentPatrolPoint, m_Self.CachedTransform.localPosition.ToVector2()) <= m_PositionError)
            {
                m_Self.IncrementPatrolIndex();
                if (!m_FirstUpdate)
                {
                    // Start waiting.
                    m_WaitingDuration = Random.Range(m_MinWaitingDuration, m_MaxWaitingDuration);
                    m_WaitingStartTime = Time.time;
                }
            }

            if (m_FirstUpdate)
            {
                m_FirstUpdate = false;
            }

            // Update waiting.
            if (m_WaitingStartTime >= 0f && Time.time - m_WaitingStartTime < m_WaitingDuration)
            {
                return TaskStatus.Running;
            }

            // Stop waiting.
            if (m_WaitingStartTime >= 0f)
            {
                m_WaitingStartTime = -1f;
            }

            // Patrol times run out.
            if (m_Self.PatrolTimes == 0)
            {
                return TaskStatus.Success;
            }

            // Move to target point.
            currentPatrolIndex = m_Self.PatrolPointIndex;
            currentPatrolPoint = patrolPoints[currentPatrolIndex];
            var targetPosition = m_SampleTargetPosition ? AIUtility.SamplePosition(currentPatrolPoint) : currentPatrolPoint.ToVector3(m_Self.CachedTransform.localPosition.y);
            m_Self.Motion.StartMove(targetPosition);
            return TaskStatus.Running;
        }
    }
}
