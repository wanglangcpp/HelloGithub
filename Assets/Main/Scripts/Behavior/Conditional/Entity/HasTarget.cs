using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class HasTarget : Conditional
    {
        private ICanHaveTarget m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>() as ICanHaveTarget;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null)
            {
                GameFramework.Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            return m_Self.HasTarget ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
