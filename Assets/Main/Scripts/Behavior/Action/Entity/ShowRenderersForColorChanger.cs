using BehaviorDesigner.Runtime.Tasks;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Entity")]
    public class ShowRenderersForColorChanger : Action
    {
        private ColorChanger m_ColorChanger = null;
        private bool m_ShouldFail = false;

        public override void OnStart()
        {
            m_ColorChanger = Owner.GetComponent<ColorChanger>();

            if (m_ColorChanger == null)
            {
                m_ShouldFail = true;
            }
            else
            {
                m_ColorChanger.ShowAllRenderers();
            }
        }

        public override void OnReset()
        {
            m_ColorChanger = null;
            m_ShouldFail = false;
        }

        public override TaskStatus OnUpdate()
        {
            return m_ShouldFail ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
