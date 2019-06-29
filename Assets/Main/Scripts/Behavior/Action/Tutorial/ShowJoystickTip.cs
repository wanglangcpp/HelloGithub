using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class ShowJoystickTip : Action
    {
        public override TaskStatus OnUpdate()
        {
            return GameEntry.Tutorial.ShowJoystickTip() ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
