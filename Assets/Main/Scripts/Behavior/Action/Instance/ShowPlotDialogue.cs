using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Instance")]
    public class ShowPlotDialogue : Action
    {
        [SerializeField]
        private int m_PlotDialogueId = 0;

        private bool m_IsPlotLoadFinished = false;

        public override void OnStart()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                return;
            }

            m_IsPlotLoadFinished = false;
            GameEntry.UI.OpenUIForm(UIFormId.PlotDialogueForm, new PlotDialogDisplayData
            {
                PlotDialogId = m_PlotDialogueId,
                PlotReturn = PlotDialogueOpenReturn,
            });
        }

        public override TaskStatus OnUpdate()
        {
            if (!GameEntry.SceneLogic.IsInstance)
            {
                GameFramework.Log.Warning("Not in an instance.");
                return TaskStatus.Failure;
            }

            if (m_IsPlotLoadFinished)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        public void PlotDialogueOpenReturn()
        {
            m_IsPlotLoadFinished = true;
        }
    }
}
