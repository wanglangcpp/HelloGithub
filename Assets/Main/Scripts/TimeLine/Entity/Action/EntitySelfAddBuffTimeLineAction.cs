namespace Genesis.GameClient
{
    public class EntitySelfAddBuffTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntitySelfAddBuffTimeLineActionData m_Data;

        public EntitySelfAddBuffTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntitySelfAddBuffTimeLineActionData;
        }

        public int BuffId
        {
            get
            {
                return m_Data.BuffId;
            }
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            Character owner = timeLineInstance.Owner as Character;

            if (GameEntry.SceneLogic.BaseInstanceLogic.CanAddBuffInSkillTimeLine(owner, owner))
            {
                owner.AddBuff(BuffId, owner.Data, OfflineBuffPool.GetNextSerialId(), null);
            }
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
