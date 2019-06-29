using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class SetRewardLevel : Action
    {
        [SerializeField]
        private int m_RewardLevel = 0;

        public override TaskStatus OnUpdate()
        {
            var instanceLogic = GameEntry.SceneLogic.InstanceForResourceLogic;
            if (instanceLogic == null)
            {
                GameFramework.Log.Warning("Not in a instance-for-resource.");
                return TaskStatus.Failure;
            }

            instanceLogic.SetRewardLevel(m_RewardLevel);
            return TaskStatus.Success;
        }
    }
}
