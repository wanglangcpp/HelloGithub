using GameFramework;

namespace Genesis.GameClient
{
    public class EntityCommonCDTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityCommonCDTimeLineActionData m_Data;

        private float CoolDownTime
        {
            get
            {
                return m_Data.CoolDownTime;
            }
        }

        public EntityCommonCDTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityCommonCDTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            var owner = timeLineInstance.Owner;
            var heroOwner = owner as HeroCharacter;
            if (heroOwner == null)
            {
                Log.Error("Owner should be a hero character to use this action, whereas it is a '{0}'", owner == null ? "null" : owner.GetType().Name);
                return;
            }

            heroOwner.StartCommonCoolDown(CoolDownTime);
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
