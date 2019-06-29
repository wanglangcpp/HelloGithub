using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckDamageValue : Conditional
    {
        [SerializeField]
        private string m_CustomKey = null;

        [SerializeField]
        private int m_DamageValue = 0;

        [SerializeField]
        private OrderRelationType m_OrderRelation = OrderRelationType.EqualTo;

        private Entity m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<Entity>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the Entity component!");
                return TaskStatus.Failure;
            }

            int currentVal = GameEntry.SceneLogic.BaseInstanceLogic.Stat.GetDamageValue(m_Self.Id, m_CustomKey);
            return OrderRelationUtility.AreSatisfying<float>(currentVal, m_DamageValue, m_OrderRelation) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
