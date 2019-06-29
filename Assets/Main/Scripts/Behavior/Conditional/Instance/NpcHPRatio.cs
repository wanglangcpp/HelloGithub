using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class NpcHPRatio : Conditional
    {
        [SerializeField]
        private int m_NpcID = 0;
        [SerializeField]
        private float m_TargetRatio = 0f;

        [SerializeField]
        private OrderRelationType m_OrderRelation = OrderRelationType.EqualTo;

        public override TaskStatus OnUpdate()
        {
            var heroes = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetNpcFromIndex(m_NpcID);

            if (heroes == null || heroes.Data == null)
            {
                return TaskStatus.Failure;
            }

            return OrderRelationUtility.AreSatisfying<float>(heroes.Data.HPRatio, m_TargetRatio, m_OrderRelation) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
