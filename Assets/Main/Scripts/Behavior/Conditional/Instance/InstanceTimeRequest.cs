using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class InstanceTimeRequest : Conditional
    {
        [SerializeField]
        private SharedFloat m_RequestTime = null;

        public override TaskStatus OnUpdate()
        {
            if (GameEntry.SceneLogic.BaseInstanceLogic.Timer == null)
            {
                return TaskStatus.Failure;
            }

            float runningDuration = GameEntry.SceneLogic.BaseInstanceLogic.Timer.CurrentSeconds;
            return runningDuration > m_RequestTime.Value ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
