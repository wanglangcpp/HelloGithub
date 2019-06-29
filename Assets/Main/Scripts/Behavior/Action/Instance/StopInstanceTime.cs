using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    internal class StopInstanceTime : Action
    {
        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Timer == null)
            {
                return TaskStatus.Failure;
            }

            GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Timer.StopTimer();
            return TaskStatus.Success;
        }
    }
}
