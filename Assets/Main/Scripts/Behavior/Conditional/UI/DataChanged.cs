using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class DataChanged : Conditional
    {
        private IUpdatableUIFragment m_Owner = null;

        public override void OnStart()
        {
            m_Owner = UIUtility.GetUpdatableUIFragment(gameObject);
        }

        public override TaskStatus OnUpdate()
        {
            if (!m_Owner.DataChanged)
            {
                return TaskStatus.Failure;
            }

            m_Owner.ClearDataChanged();
            return TaskStatus.Success;
        }
    }
}
