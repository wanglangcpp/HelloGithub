using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class CheckRequest : Conditional
    {
        [SerializeField]
        private SharedInt m_RequestId = null;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            return GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.IsRequestComplete(m_RequestId.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
