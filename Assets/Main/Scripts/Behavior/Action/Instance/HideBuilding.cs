using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class HideBuilding : Action
    {
        [SerializeField]
        private int[] m_BuildingIndices = null;

        [SerializeField]
        private bool m_ContinueEvenOnFailure = true;

        private bool m_HasFailed = false;

        public override void OnStart()
        {
            m_HasFailed = false;
        }

        public override void OnReset()
        {
            m_HasFailed = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance || m_BuildingIndices == null)
            {
                return TaskStatus.Failure;
            }

            for (int i = 0; i < m_BuildingIndices.Length; i++)
            {
                if (GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.HideLivingBuilding(m_BuildingIndices[i]))
                {
                    continue;
                }

                if (m_ContinueEvenOnFailure)
                {
                    m_HasFailed = true;
                }
                else
                {
                    return TaskStatus.Failure;
                }
            }

            return m_HasFailed ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
