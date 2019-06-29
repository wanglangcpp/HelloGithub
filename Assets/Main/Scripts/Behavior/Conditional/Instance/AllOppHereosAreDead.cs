using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class AllOppHeroesAreDead : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            var instanceLogic = GameEntry.SceneLogic.BasePvpaiInstanceLogic;
            if (instanceLogic == null)
            {
                Log.Warning("Not in a player v.s. player AI instance.");
                return TaskStatus.Failure;
            }

            return instanceLogic.OppHeroesData.AnyIsAlive ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
