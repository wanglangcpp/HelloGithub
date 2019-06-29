using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class IsDead : Conditional
    {
        private TargetableObject m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null)
            {
                GameFramework.Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            return m_Self.IsDead ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
