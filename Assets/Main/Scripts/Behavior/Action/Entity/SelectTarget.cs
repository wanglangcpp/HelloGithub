using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class SelectTarget : Action
    {
        [SerializeField]
        private SharedFloat m_Distance = 1f;

        private ICanHaveTarget m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>() as ICanHaveTarget;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null)
            {
                GameFramework.Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            ITargetable target = AIUtility.GetNearestTarget(m_Self, RelationType.Hostile, m_Distance.Value);
            if (target != null)
            {
                m_Self.Target = target;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            m_Distance = 1f;
        }
    }
}
