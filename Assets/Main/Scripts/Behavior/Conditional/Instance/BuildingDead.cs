using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class BuildingDead : Conditional
    {
        [SerializeField]
        private int[] m_BuildingIds = null;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return TaskStatus.Failure;
            }

            for (int i = 0; i < m_BuildingIds.Length; i++)
            {
                if (!GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.IsBuildingDead(m_BuildingIds[i]))
                {
                    return TaskStatus.Failure;
                }
            }

            return TaskStatus.Success;
        }
    }
}
