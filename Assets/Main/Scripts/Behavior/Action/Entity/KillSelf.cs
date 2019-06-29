using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class KillSelf : Action
    {
        private TargetableObject m_Self;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<TargetableObject>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the TargetableObject component!");
                return TaskStatus.Failure;
            }

            m_Self.Data.HP = 0;
            m_Self.Motion.PerformHPDamage(m_Self, ImpactSourceType.InstanceLogic);
            return TaskStatus.Success;
        }
    }
}
