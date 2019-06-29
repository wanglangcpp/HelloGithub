using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class HideBossHPBar : Action
    {
        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic == null)
            {
                return TaskStatus.Failure;
            }

            GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.HideBossHPBar();
            return TaskStatus.Success;
        }
    }
}
