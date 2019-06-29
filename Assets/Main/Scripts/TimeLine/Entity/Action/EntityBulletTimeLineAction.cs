namespace Genesis.GameClient
{
    public class EntityBulletTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityBulletTimeLineActionData m_Data;

        public EntityBulletTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityBulletTimeLineActionData;
        }

        private int BulletId
        {
            get
            {
                return m_Data.BulletId ?? 0;
            }
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            float ffTill;
            if (TryGetUserData(timeLineInstance, Constant.TimeLineFastForwardTillKey, out ffTill) && ffTill > StartTime)
            {
                return;
            }

            int skillIndex = GetSkillIndex(timeLineInstance);
            int skillLevel = GetSkillLevel(timeLineInstance);
            GameEntry.SceneLogic.BaseInstanceLogic.ShowBullet(BulletId, timeLineInstance.Owner, skillIndex, skillLevel);
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
