using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/UI")]
    public class StartWaiting : Action
    {
        public override TaskStatus OnUpdate()
        {
            GameEntry.Waiting.StartWaiting(WaitingType.Default, "BehaviorDesigner");
            return TaskStatus.Success;
        }
    }
}
