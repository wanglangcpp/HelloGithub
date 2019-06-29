using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class StopBehavior : Conditional
    {
        private NpcCharacter m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<NpcCharacter>();
        }

        public override TaskStatus OnUpdate()
        {
            if (m_Self == null)
            {
                GameFramework.Log.Warning("Npc is invalid.");
                return TaskStatus.Failure;
            }

            m_Self.Behavior.RestartWhenComplete = false;

            return TaskStatus.Success;
        }
    }
}
