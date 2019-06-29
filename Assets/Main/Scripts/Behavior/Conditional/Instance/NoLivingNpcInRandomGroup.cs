using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    [TaskDescription("Check whether there is any living NPC in a random NPC showing group. Usually this should be used after the generation goal has been achieved.")]
    public class NoLivingNpcInRandomGroup : Conditional
    {
        [SerializeField]
        private string m_GroupKey = string.Empty;

        public override TaskStatus OnUpdate()
        {
            var groupData = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Data.GetRandomShowNpcGroupData(m_GroupKey);
            var livingCount = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetLivingNpcCountInRandomShowNpcGroup(groupData);

            return livingCount <= 0 ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
