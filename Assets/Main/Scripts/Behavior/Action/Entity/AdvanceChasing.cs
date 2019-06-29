using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class AdvanceChasing : AbstractChasing
    {
        [SerializeField]
        private float m_MinRadius = 1f;

        [SerializeField]
        private float m_MaxRadius = 1f;

        private float m_OriginalStoppingDistance = 0f;

        public override void OnStart()
        {
            base.OnStart();
            m_OriginalStoppingDistance = m_Self.NavAgent.stoppingDistance;
            m_Self.NavAgent.stoppingDistance = 0f;
        }

        public override void OnEnd()
        {
            base.OnEnd();
            m_Self.NavAgent.stoppingDistance = m_OriginalStoppingDistance;
        }

        protected override TaskStatus Repath()
        {
            Vector2 targetPosition = GameEntry.SceneLogic.BaseInstanceLogic.Target.CalcTargetPosition(m_Self, Random.Range(m_MinRadius, m_MaxRadius));
            if (m_SelfMotion.StartMove(targetPosition.ToVector3(m_Self.CachedTransform.position.y)))
            {
                m_Running = true;
                m_LastRepathTime = Time.time;
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

    }
}
