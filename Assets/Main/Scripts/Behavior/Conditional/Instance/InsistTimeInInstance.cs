using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class InsistTimeInInstance : Conditional
    {
        [SerializeField]
        private SharedFloat m_TargetDuration = null;

        public override TaskStatus OnUpdate()
        {
            float runningDuration = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Timer.CurrentSeconds;
            return runningDuration > m_TargetDuration.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
