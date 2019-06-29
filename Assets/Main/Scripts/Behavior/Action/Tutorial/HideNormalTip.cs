using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class HideCompulsoryTip : Action
    {
        public override TaskStatus OnUpdate()
        {
            GameEntry.Tutorial.HideCompulsoryTip();
            return TaskStatus.Success;
        }
    }
}
