using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class ShowNormalTip : Action
    {
        [SerializeField]
        private TutorialNormalTipDisplayData m_DisplayData = null;

        public override TaskStatus OnUpdate()
        {
            return GameEntry.Tutorial.ShowNormalTip(m_DisplayData) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
