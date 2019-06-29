using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class RandomShowNpcTotalCount : Conditional
    {
        [SerializeField]
        private string m_GroupKey = string.Empty;

        public override TaskStatus OnUpdate()
        {
            var groupData = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Data.GetRandomShowNpcGroupData(m_GroupKey);

            return (groupData.GenGoalIsAchieved || groupData.IsInterrupted) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
