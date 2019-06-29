using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class SimpleChasing : AbstractChasing
    {
        [SerializeField]
        private bool m_ChangeStoppingDistance = false;

        [SerializeField]
        private float m_StoppingDistance = 2f;

        private float m_OriginalStoppingDistance = 0f;

        public override void OnStart()
        {
            base.OnStart();

            if (m_ChangeStoppingDistance)
            {
                m_OriginalStoppingDistance = m_Self.NavAgent.stoppingDistance;

                float stoppingDistance = m_StoppingDistance;

                var targetableTarget = m_CanHaveTargetSelf.Target as TargetableObject;
                if (targetableTarget != null)
                {
                    stoppingDistance += targetableTarget.ImpactCollider.radius;
                }

                m_Self.NavAgent.stoppingDistance = stoppingDistance;
            }
        }

        public override void OnEnd()
        {
            if (m_ChangeStoppingDistance)
            {
                m_Self.NavAgent.stoppingDistance = m_OriginalStoppingDistance;
            }

            base.OnEnd();
        }

        protected override TaskStatus Repath()
        {
            if (m_SelfMotion.StartMove((m_CanHaveTargetSelf.Target as Entity).CachedTransform.position))
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
