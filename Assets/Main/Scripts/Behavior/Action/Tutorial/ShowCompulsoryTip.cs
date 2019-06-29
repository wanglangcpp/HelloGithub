using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class ShowCompulsoryTip : Action
    {
        [SerializeField]
        private TutorialCompulsoryTipDisplayData m_DisplayData = null;

        public override TaskStatus OnUpdate()
        {
            return GameEntry.Tutorial.ShowCompulsoryTip(m_DisplayData) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
