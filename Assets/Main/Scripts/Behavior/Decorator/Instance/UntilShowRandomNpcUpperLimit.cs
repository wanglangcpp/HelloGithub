using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class UntilShowRandomNpcUpperLimit : Decorator
    {
        [SerializeField]
        private string m_GroupKey = string.Empty;

        public override void OnStart()
        {

        }

        public override bool CanExecute()
        {
            var groupData = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.Data.GetRandomShowNpcGroupData(m_GroupKey);

            if (groupData.IsInterrupted)
            {
                return false;
            }

            return GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic.GetLivingNpcCountInRandomShowNpcGroup(groupData) < groupData.UpperLimit
                && !groupData.GenGoalIsAchieved;
        }
    }
}
