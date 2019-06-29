using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class HideJoystickTip : Action
    {
        public override TaskStatus OnUpdate()
        {
            GameEntry.Tutorial.HideJoystickTip();
            return TaskStatus.Success;
        }
    }
}
