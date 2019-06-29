using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class SetIntInstanceParamNpc : SetIntInstanceParam
    {
        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            return base.OnUpdate();
        }
    }
}
