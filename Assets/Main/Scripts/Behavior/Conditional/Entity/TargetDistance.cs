using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class TargetDistance : Conditional
    {
        [SerializeField]
        private SharedFloat m_Distance = 0f;

        private TargetableObject m_Self = null;
        private ICanHaveTarget m_CanHaveTargetSelf = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
            m_CanHaveTargetSelf = m_Self as ICanHaveTarget;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null || m_CanHaveTargetSelf == null)
            {
                GameFramework.Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            if (!m_CanHaveTargetSelf.HasTarget)
            {
                return TaskStatus.Failure;
            }

            float distance = AIUtility.GetAttackDistance(m_Self, m_CanHaveTargetSelf.Target);
            return distance <= m_Distance.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
