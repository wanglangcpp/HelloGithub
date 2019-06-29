using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ShowBossHPBar : Action
    {
        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic == null)
            {
                return TaskStatus.Failure;
            }

            if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.ShowBossHPBar())
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}
