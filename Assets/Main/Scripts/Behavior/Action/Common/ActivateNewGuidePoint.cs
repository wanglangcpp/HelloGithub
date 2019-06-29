using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    [TaskDescription("Try activating a new guide point in the current instance if any.")]
    public class ActivateNewGuidePoint : Action
    {
        public override TaskStatus OnUpdate()
        {
            var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
            var guidePoints = instanceLogic.GuidePointSet;

            guidePoints.ActivateNewGuidePoint();
            return TaskStatus.Success;
        }
    }
}
