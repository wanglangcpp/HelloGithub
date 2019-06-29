using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    public abstract class CheckInstanceParam<T> : Conditional
        where T : System.IComparable<T>
    {
        [SerializeField]
        protected string m_Key = string.Empty;

        [SerializeField]
        protected T m_TargetValue = default(T);

        [SerializeField]
        protected OrderRelationType m_TargetRelation = OrderRelationType.EqualTo;

        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic == null)
            {
                return TaskStatus.Failure;
            }

            var instanceData = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Data;

            if (!instanceData.HasMiscParam(m_Key))
            {
                return TaskStatus.Failure;
            }

            T currentValue = instanceData.GetMiscParam<T>(m_Key);

            return OrderRelationUtility.AreSatisfying(currentValue, m_TargetValue, m_TargetRelation) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
