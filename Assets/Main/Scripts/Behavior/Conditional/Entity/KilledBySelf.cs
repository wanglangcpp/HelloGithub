using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    [TaskDescription("Used right after self character is dead.")]
    public class KilledBySelf : Conditional
    {
        private CharacterMotion m_SelfMotion = null;

        public override void OnStart()
        {
            m_SelfMotion = Owner.GetComponent<Character>().Motion;
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_SelfMotion == null)
            {
                GameFramework.Log.Warning("Oops! Cannot find the motion component!");
                return TaskStatus.Failure;
            }

            if (m_SelfMotion.DeadlyImpactSourceEntity == null)
            {
                return TaskStatus.Failure;
            }

            return m_SelfMotion.Owner.Id == m_SelfMotion.DeadlyImpactSourceEntity.Id ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
