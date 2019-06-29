using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    public class CompareSharedInt : Conditional
    {
        [SerializeField]
        private SharedInt m_First = null;

        [SerializeField]
        private SharedInt m_Second = null;

        [SerializeField]
        private OrderRelationType m_OrderRelationType = OrderRelationType.EqualTo;

        public override TaskStatus OnUpdate()
        {
            return OrderRelationUtility.AreSatisfying(m_First.Value, m_Second.Value, m_OrderRelationType) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
