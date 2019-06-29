using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class InstanceComplete : Action
    {
        [SerializeField]
        private SharedBool m_Success = true;
        public override TaskStatus OnUpdate()
        {
            if (m_Success.Value)
            {
                GameEntry.SceneLogic.BaseInstanceLogic.SetInstanceSuccess(InstanceSuccessReason.ClaimedByAI);
            }
            else
            {
                //目前特定为护送npc失败
                GameEntry.SceneLogic.BaseInstanceLogic.SetInstanceFailure(InstanceFailureReason.Escort);
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_Success = true;
        }
    }
}
