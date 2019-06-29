namespace Genesis.GameClient
{
    /// <summary>
    /// 实体要求其发出的子弹释放技能的时间轴行为。
    /// </summary>
    public class EntityAskBulletToPerformSkillTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityAskBulletToPerformSkillTimeLineActionData m_Data;

        public EntityAskBulletToPerformSkillTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityAskBulletToPerformSkillTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            GameEntry.Event.Fire(timeLineInstance.Owner, new OwnerAskToPerformSkillEventArgs(timeLineInstance.Owner.Id, m_Data.SkillId, m_Data.TargetBulletTypeId));
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
