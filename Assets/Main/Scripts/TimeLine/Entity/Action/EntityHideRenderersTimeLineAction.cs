using GameFramework;

namespace Genesis.GameClient
{
    public class EntityHideRenderersTimeLineAction : EntityAbstractTimeLineAction
    {
        private ColorChanger m_CachedColorChanger;

        public EntityHideRenderersTimeLineAction(TimeLineActionData data)
            : base(data)
        {

        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            m_CachedColorChanger = timeLineInstance.Owner.GetComponent<ColorChanger>();
            if (m_CachedColorChanger == null)
            {
                Log.Warning("The owner '{0}' doesn't have a ColorChange component.", timeLineInstance.Owner);
                return;
            }

            m_CachedColorChanger.HideAllRenderers();
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {
            Reset();
        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {
            Reset();
        }

        private void Reset()
        {
            m_CachedColorChanger.ShowAllRenderers();
        }
    }
}
