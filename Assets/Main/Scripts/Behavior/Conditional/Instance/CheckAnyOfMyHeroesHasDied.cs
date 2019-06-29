using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class CheckAnyOfMyHeroesHasDied : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            return GameEntry.SceneLogic.BaseInstanceLogic.AnyOfMyHeroesHasDied ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
