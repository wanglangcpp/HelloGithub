using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class SelectTargetInCamp : Action
    {
        [SerializeField]
        private float m_Distance = 1f;

        [SerializeField]
        private CampType m_CampType = CampType.Enemy;

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
                GameFramework.Log.Warning("NPC is invalid.");
                return TaskStatus.Failure;
            }

            ITargetable target = AIUtility.GetNearestTarget(m_Self, m_CampType, m_Distance);
            if (target != null)
            {
                m_CanHaveTargetSelf.Target = target;
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
