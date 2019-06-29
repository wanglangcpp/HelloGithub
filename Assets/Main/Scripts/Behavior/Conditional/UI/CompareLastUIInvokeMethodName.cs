using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class CompareLastUIInvokeMethodName : Conditional
    {
        [SerializeField]
        private SharedString m_CompareTo = string.Empty;

        private IUpdatableUIFragment m_Owner = null;

        public override void OnStart()
        {
            m_Owner = UIUtility.GetUpdatableUIFragment(gameObject);
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Owner == null)
            {
                return TaskStatus.Failure;
            }

            return m_CompareTo.Value == m_Owner.LastUIInvoke.MethodName ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
