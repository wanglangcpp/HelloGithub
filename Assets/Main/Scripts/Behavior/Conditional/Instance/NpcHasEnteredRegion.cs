using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class NpcHasEnteredRegion : Conditional
    {
        [SerializeField]
        private int m_NpcIndex = 0;

        [SerializeField]
        private int m_RegionId = 0;

        public override TaskStatus OnUpdate()
        {
            return GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.HasNpcEnteredRegion(m_NpcIndex, m_RegionId) ?
                TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
