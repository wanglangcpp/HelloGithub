using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    [TaskDescription("Check whether the current instance logic has any active guide point.")]
    public class HasActiveGuidePoint : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            var guidePoints = GameEntry.SceneLogic.BaseInstanceLogic.GuidePointSet;
            return guidePoints.ActiveGuidePoint == null ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
