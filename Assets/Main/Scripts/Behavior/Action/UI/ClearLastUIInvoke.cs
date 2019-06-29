using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class ClearLastUIInvoke : Action
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

            m_Owner.ClearLastUIInvoke();
            return TaskStatus.Success;
        }
    }
}
