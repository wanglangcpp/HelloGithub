namespace Genesis.GameClient
{
    public class EntityEnterAltSkillTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityEnterAltSkillTimeLineActionData m_Data;

        public EntityEnterAltSkillTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityEnterAltSkillTimeLineActionData;
        }

        public int AltSkillId
        {
            get
            {
                return m_Data.AltSkillId;
            }
        }

        public int AltSkillLevel
        {
            get
            {
                return m_Data.AltSkillLevel;
            }
        }

        public float? KeepTime
        {
            get
            {
                return m_Data.KeepTime;
            }
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            HeroCharacter heroCharacter = timeLineInstance.Owner as HeroCharacter;
            if (heroCharacter == null)
            {
                return;
            }

            heroCharacter.EnterAltSkills(AltSkillId, AltSkillLevel, KeepTime);
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
