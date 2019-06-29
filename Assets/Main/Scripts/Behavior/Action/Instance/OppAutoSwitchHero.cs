using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    [TaskDescription("Use OppNeedsAutoSwitchHero to judge whether this action should be performed.")]
    public class OppAutoSwitchHero : Action
    {
        public override TaskStatus OnUpdate()
        {
            if (!(GameEntry.SceneLogic.BaseInstanceLogic is BasePvpaiInstanceLogic))
            {
                Log.Warning("Not in a player v.s. player AI instance.");
                return TaskStatus.Failure;
            }

            GameEntry.SceneLogic.BasePvpaiInstanceLogic.OppTryAutoSwitchHero();
            return TaskStatus.Success;
        }
    }
}
