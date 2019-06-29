using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckHPRatio : Conditional
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
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            var currentVal = m_Self.Data.BaseHPRatio;
            return OrderRelationUtility.AreSatisfying<float>(currentVal, m_TargetRatio, m_OrderRelation) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
