using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    [TaskDescription("Check whether the curren hero's player is during a common cool down.")]
    public class DuringCommonCD : Conditional
    {
        private HeroCharacter m_Self = null;

        public override void OnStart()
        {
            m_Self = Owner.GetComponent<HeroCharacter>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_Self == null)
            {
                Log.Warning("Self is invalid.");
                return TaskStatus.Failure;
            }

            return m_Self.IsDuringCommonCoolDown ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
