using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    [TaskDescription("Decorator that only overrides the priority of its child.")]
    public class PriorityOverrider : Decorator
    {
        [SerializeField]
        private float m_Priority = 0f;

        private TaskStatus m_ExecutionStatus = TaskStatus.Inactive;

        public override bool CanExecute()
        {
            return m_ExecutionStatus == TaskStatus.Inactive || m_ExecutionStatus == TaskStatus.Running;
        }

        public override float GetPriority()
        {
            return m_Priority;
        }

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            m_ExecutionStatus = childStatus;
        }

        public override void OnEnd()
        {
            m_ExecutionStatus = TaskStatus.Inactive;
        }
    }
}
