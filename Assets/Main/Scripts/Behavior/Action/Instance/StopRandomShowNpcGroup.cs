using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    [TaskDescription("Interrupt a group of randomly showing NPC.")]
    public class StopRandomShowNpcGroup : Action
    {
        [SerializeField]
        private string m_GroupKey = string.Empty;

        public override TaskStatus OnUpdate()
        {
            var instanceData = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Data;

            if (instanceData == null)
            {
                Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            var groupData = instanceData.GetRandomShowNpcGroupData(m_GroupKey);

            if (groupData == null)
            {
                Log.Warning("Random showing NPC group '{0}' not found.", m_GroupKey);
                return TaskStatus.Failure;
            }

            groupData.Interrupt();
            return TaskStatus.Success;
        }
    }
}
