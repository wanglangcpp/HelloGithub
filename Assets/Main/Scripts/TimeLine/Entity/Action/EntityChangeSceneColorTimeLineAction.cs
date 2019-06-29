using GameFramework;

namespace Genesis.GameClient
{
    public class EntityChangeSceneColorTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityChangeSceneColorTimeLineActionData m_Data;

        public EntityChangeSceneColorTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityChangeSceneColorTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            var meOwner = timeLineInstance.Owner as MeHeroCharacter;

            if (meOwner == null)
            {
                Log.Info("Owner is not a 'MeHeroCharacter' but a '{0}'", timeLineInstance.Owner.GetType().Name);
                return;
            }

            GameEntry.CameraPostEffect.StartSceneColorChange(m_Data.TargetColor, m_Data.Attack, m_Data.Sustain, m_Data.Release);
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
