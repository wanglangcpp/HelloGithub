using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ShowBuilding : Action
    {
        [SerializeField, BehaviorDesigner.Runtime.Tasks.Tooltip("It's in fact the building index in the InstanceBuilding data table, NOT the building ID in the Building data table.")]
        private int[] m_BuildingIds = null;

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance || m_BuildingIds == null)
            {
                return TaskStatus.Failure;
            }

            for (int i = 0; i < m_BuildingIds.Length; i++)
            {
                if (!GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.ShowBuilding(m_BuildingIds[i]))
                {
                    return TaskStatus.Failure;
                }
            }

            return TaskStatus.Success;
        }
    }
}
