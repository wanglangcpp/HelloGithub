using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class PauseGameWaitClick : Action
    {
        [SerializeField]
        private string m_WidgetPath = string.Empty;

        [SerializeField]
        private bool m_HideNormalTipOnResume = true;

        public override TaskStatus OnUpdate()
        {
            return GameEntry.Tutorial.PauseGameWaitClick(m_WidgetPath, m_HideNormalTipOnResume) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
