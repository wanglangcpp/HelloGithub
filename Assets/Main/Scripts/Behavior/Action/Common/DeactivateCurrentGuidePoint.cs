using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    [TaskDescription("Try deactivating current guide point.")]
    public class DeactivateCurrentGuidePoint : Action
    {
        public override TaskStatus OnUpdate()
        {
            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            var guidePoints = instanceLogic.GuidePointSet;
            return guidePoints.DeactivateCurrentGuidePoint() ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
