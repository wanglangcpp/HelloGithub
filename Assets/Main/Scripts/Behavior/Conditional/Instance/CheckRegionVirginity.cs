using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class CheckRegionVirginity : Conditional
    {
        [SerializeField]
        private int m_RegionId = 0;

        public override TaskStatus OnUpdate()
        {
            return GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.IsRegionVirgin(m_RegionId) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
