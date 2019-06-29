namespace Genesis.GameClient
{
    public class EntityLeaveAltSkillTimeLineAction : EntityAbstractTimeLineAction
    {
        //private EntityLeaveAltSkillTimeLineActionData m_Data;

        public EntityLeaveAltSkillTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            //m_Data = data as EntityLeaveAltSkillTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            HeroCharacter heroCharacter = timeLineInstance.Owner as HeroCharacter;
            if (heroCharacter == null)
            {
                return;
            }

            heroCharacter.LeaveAltSkills();
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
