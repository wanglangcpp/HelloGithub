using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class HasLastUIInvoke : Conditional
    {
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

            return m_Owner.LastUIInvoke == null ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
