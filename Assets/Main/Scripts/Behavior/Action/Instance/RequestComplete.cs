using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class RequestComplete : Action
    {
        [SerializeField]
        private SharedInt m_RequestId = 0;

        [SerializeField]
        private SharedBool m_Success = true;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.SetRequestComplete(m_RequestId.Value, m_Success.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_RequestId = 0;
            m_Success = true;
        }
    }
}
