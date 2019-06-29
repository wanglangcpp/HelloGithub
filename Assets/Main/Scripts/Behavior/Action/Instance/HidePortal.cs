using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class HidePortal : Action
    {
        [SerializeField]
        private int m_PortalId = 0;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (!GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.HidePortal(m_PortalId))
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_PortalId = 0;
        }
    }
}
