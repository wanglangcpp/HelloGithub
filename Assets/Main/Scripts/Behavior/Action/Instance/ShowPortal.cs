using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ShowPortal : Action
    {
        [SerializeField]
        private int m_PortalId = 0;

        [SerializeField]
        private PortalParam[] m_PortalParams = null;

        [SerializeField]
        private bool m_ActivateGuidePointGroupOnExit = false;

        [SerializeField]
        private int m_GuidePointGroupOnExit = 0;

        [SerializeField]
        private bool m_ActivateGuidePointGroupOnShow = false;

        [SerializeField]
        private int m_GuidePointGroupOnShow = 0;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            if (!GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.ShowPortal(m_PortalId, m_PortalParams,
                m_ActivateGuidePointGroupOnExit ? m_GuidePointGroupOnExit : (int?)null,
                m_ActivateGuidePointGroupOnShow ? m_GuidePointGroupOnShow : (int?)null))
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            m_PortalId = 0;
            m_PortalParams = null;
        }
    }
}
