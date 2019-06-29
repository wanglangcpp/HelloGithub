using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class AllMyHeroesAreDead : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.NonInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            return GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData.AnyIsAlive ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
