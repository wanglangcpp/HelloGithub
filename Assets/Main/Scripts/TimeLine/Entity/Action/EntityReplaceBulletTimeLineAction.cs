using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// 子弹将自身替换为其他子弹的时间轴行为。
    /// </summary>
    public class EntityReplaceBulletTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityReplaceBulletTimeLineActionData m_Data;

        public EntityReplaceBulletTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityReplaceBulletTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            var bulletOwner = timeLineInstance.Owner as Bullet;
            if (bulletOwner == null)
            {
                Log.Error("Owner is not a bullet but a '{0}'", (timeLineInstance.Owner == null ? "null" : timeLineInstance.Owner.GetType().Name));
                return;
            }

            GameEntry.SceneLogic.BaseInstanceLogic.ReplaceBullet(m_Data.ReplaceBulletId, bulletOwner);
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
