using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class MeIsSwitchingHero : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.NonInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            return instanceLogic.IsSwitchingHero ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
