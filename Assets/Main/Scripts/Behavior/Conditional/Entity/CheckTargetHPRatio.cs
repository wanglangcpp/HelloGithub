using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckTargetHPRatio : Conditional
    {
        [SerializeField]
        private float m_TargetRatio = 0f;

        [SerializeField]
        private OrderRelationType m_OrderRelation = OrderRelationType.EqualTo;

        private NpcCharacter m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<NpcCharacter>();
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null)
            {
                GameFramework.Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            if (!(m_Self as ICanHaveTarget).HasTarget)
            {
                return TaskStatus.Failure;
            }

            var target = m_Self.Target as TargetableObject;
            var currentVal = target.Data.BaseHPRatio;
            return OrderRelationUtility.AreSatisfying<float>(currentVal, m_TargetRatio, m_OrderRelation) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}