using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class CheckOwnerDead : Conditional
    {
        private ITargetable m_Owner = null;

        public override void OnStart()
        {
            var ownerEntity = Owner.GetComponent<Bullet>().Owner;
            if (ownerEntity == null)
            {
                return;
            }

            m_Owner = ownerEntity as ITargetable;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Owner == null)
            {
                GameFramework.Log.Warning("Owner is invalid.");
                return TaskStatus.Failure;
            }

            return m_Owner.IsDead ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
