using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class ShowTutorialMask : Action
    {
        [SerializeField]
        private UIFormId m_UIFormId = UIFormId.Main;

        [SerializeField]
        private string m_UIFormName = string.Empty;

        public override TaskStatus OnUpdate()
        {
            if (string.IsNullOrEmpty(m_UIFormName))
            {
                GameEntry.Tutorial.ShowMask(m_UIFormId);
            }
            else
            {
                GameEntry.Tutorial.ShowMask((UIFormId)System.Enum.Parse(typeof(UIFormId), m_UIFormName, true));
            }

            return TaskStatus.Success;
        }
    }
}
