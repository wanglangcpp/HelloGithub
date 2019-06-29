using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class UntilDead : Decorator
    {
        private TargetableObject m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
        }

        public override bool CanExecute()
        {
            if (m_Self == null)
            {
                GameFramework.Log.Warning("Self is invalid.");
                return false;
            }

            return !m_Self.IsDead;
        }
    }
}
