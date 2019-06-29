using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class CheckFirstArriverCamp : Conditional
    {
        [SerializeField]
        private int regionId = 0;

        [SerializeField]
        private CampType targetCamp = CampType.Player;

        public override TaskStatus OnUpdate()
        {
            var firstArriverData = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetFirstArriverInRegion(regionId);
            return firstArriverData == null ? TaskStatus.Failure :
                firstArriverData.Camp == targetCamp ? TaskStatus.Success :
                TaskStatus.Failure;
        }
    }
}
