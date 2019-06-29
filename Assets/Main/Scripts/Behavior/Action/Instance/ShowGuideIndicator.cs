using BehaviorDesigner.Runtime.Tasks;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    /// <summary>
    /// 请求显示向导指示标志。
    /// </summary>
    [TaskCategory("Game/Instance")]
    [TaskDescription("Request the instance logic to SHOW the guide indicator.")]
    public class ShowGuideIndicator : Action
    {
        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            GameEntry.SceneLogic.BaseInstanceLogic.CanShowGuideIndicator = true;
            return TaskStatus.Success;
        }
    }
}
