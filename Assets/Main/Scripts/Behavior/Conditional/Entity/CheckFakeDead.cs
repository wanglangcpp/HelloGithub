using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckFakeDead : Conditional
    {
        private ITargetable m_Owner = null;

        public override void OnStart()
        {
            var entity = GetComponent<Entity>();
            if (entity == null)
            {
                return;
            }

            m_Owner = entity as ITargetable;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Owner == null)
            {
                GameFramework.Log.Warning("Owner is invalid.");
                return TaskStatus.Failure;
            }

            return m_Owner.IsFakeDead ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
