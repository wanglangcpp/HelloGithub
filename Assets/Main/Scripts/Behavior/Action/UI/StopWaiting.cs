using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class StopWaiting : Action
    {
        public override TaskStatus OnUpdate()
        {
            GameEntry.Waiting.StopWaiting(WaitingType.Default, "BehaviorDesigner");
            return TaskStatus.Success;
        }
    }
}
