using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class HideNormalTip : Action
    {
        public override TaskStatus OnUpdate()
        {
            GameEntry.Tutorial.HideNormalTip();
            return TaskStatus.Success;
        }
    }
}
