using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class UpdateBuffIdsToAddForRegion : Action
    {
        [SerializeField]
        private int m_RegionId = 0;

        [SerializeField]
        private int[] m_BuffIds = null;

        [SerializeField]
        private CampType[] m_Camps = null;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.IsAvailable || GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic == null)
            {
                return TaskStatus.Failure;
            }

            return GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.UpdateBuffIdsToAddForRegion(m_RegionId, m_BuffIds, m_Camps) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
