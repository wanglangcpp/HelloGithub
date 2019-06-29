using GameFramework;

namespace Genesis.GameClient
{
    public class EntityChangeColorTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityChangeColorTimeLineActionData m_Data;

        public EntityChangeColorTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityChangeColorTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            var targetableSelf = timeLineInstance.Owner as TargetableObject;
            if (targetableSelf == null)
            {
                Log.Error("Owner is not a TargetableObject but a '{0}'", timeLineInstance.Owner == null ? "null" : timeLineInstance.Owner.GetType().Name);
                return;
            }

            targetableSelf.StartColorChange(m_Data.ColorChangeId, m_Data.ColorChangeDuration);
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {

        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {

        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }
    }
}
