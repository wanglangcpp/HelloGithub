using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class NeedShowRandomNpcs : Conditional
    {
        [SerializeField]
        private string m_GroupKey = string.Empty;

        [SerializeField]
        private int m_LowerLimit = 0;

        public override TaskStatus OnUpdate()
        {
            var groupData = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Data.GetRandomShowNpcGroupData(m_GroupKey);

            if (groupData.IsInterrupted)
            {
                return TaskStatus.Failure;
            }

            int livingCount = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetLivingNpcCountInRandomShowNpcGroup(groupData);

            return livingCount < m_LowerLimit ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
