using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class MoveToPosition : Action
    {
        [SerializeField]
        private Vector2 m_TargetPosition = Vector2.zero;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Whether the target position needs to be sampled using NavMesh.")]
        [SerializeField]
        private bool m_SampleTargetPosition = false;

        [SerializeField]
        private float m_StoppingDistance = 0f;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("Time interval for navigation.")]
        [SerializeField]
        private float m_NavInterval = 1f;

        [SerializeField]
        private float m_PositionError = .1f;

        [SerializeField]
        private float m_TimeOut = 0f;

        private float m_StartTime = 0f;
        private Character m_Self = null;
        private float m_CachedStoppingDistance = 0f;
        private bool m_ShouldFail = false;
        private float m_Time = -1f;
        private bool m_StartMoveSuccess = false;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Character>();
            m_ShouldFail = m_Self == null;

            if (m_ShouldFail)
            {
                return;
            }

            m_StartTime = Time.time;
            m_CachedStoppingDistance = m_Self.NavAgent.stoppingDistance;
            m_Self.NavAgent.stoppingDistance = m_StoppingDistance;
            m_Time = -m_NavInterval;
            m_StartMoveSuccess = false;
        }

        public override void OnEnd()
        {
            if (m_ShouldFail)
            {
                return;
            }

            m_Self.NavAgent.stoppingDistance = m_CachedStoppingDistance;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null)
            {
                Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            if (m_ShouldFail)
            {
                return TaskStatus.Failure;
            }

            if (m_TimeOut > 0f && Time.time - m_StartTime > m_TimeOut)
            {
                return TaskStatus.Success;
            }

            if (Vector2.Distance(m_TargetPosition, m_Self.CachedTransform.localPosition.ToVector2()) <= m_PositionError + m_StoppingDistance)
            {
                return TaskStatus.Success;
            }

            if (Time.time - m_Time <= m_NavInterval && m_StartMoveSuccess)
            {
                return TaskStatus.Running;
            }

            m_StartMoveSuccess = m_Self.Motion.StartMove(m_SampleTargetPosition ? AIUtility.SamplePosition(m_TargetPosition) : m_TargetPosition.ToVector3(m_Self.CachedTransform.localPosition.y));
            return TaskStatus.Running;
        }
    }
}
