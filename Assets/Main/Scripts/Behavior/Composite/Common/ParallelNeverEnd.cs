using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Common")]
    [TaskDescription("Similar to Parallel and ParallelSelector, but will always be running regardless of the status of its children.")]
    [TaskIcon("{SkinColor}ParallelIcon.png")]
    public class ParallelNeverEnd : Parallel
    {
        private bool anyChildHasStarted = false;

        public override void OnReset()
        {
            base.OnReset();
            anyChildHasStarted = false;
        }

        public override void OnChildStarted(int childIndex)
        {
            base.OnChildStarted(childIndex);
            anyChildHasStarted = true;
        }

        public override TaskStatus OverrideStatus(TaskStatus status)
        {
            // If no child has started to run, don't return TaskStatus.Running; otherwise the children will never be started.
            return anyChildHasStarted ? TaskStatus.Running : TaskStatus.Success;
        }
    }
}
