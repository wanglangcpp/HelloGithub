using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class HideTutorialMask : Action
    {
        public override TaskStatus OnUpdate()
        {
            GameEntry.Tutorial.HideMask();
            return TaskStatus.Success;
        }
    }
}
