using GameFramework;

namespace Genesis.GameClient
{
    public class EntityBreakTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityBreakTimeLineActionData m_Data;

        public EntityBreakTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityBreakTimeLineActionData;
        }

        public bool CanBreakByMove
        {
            get
            {
                return m_Data.CanBreakByMove ?? false;
            }
        }

        public bool CanBreakBySkill
        {
            get
            {
                return m_Data.CanBreakBySkill ?? false;
            }
        }

        public bool CanBreakByImpact
        {
            get
            {
                return m_Data.CanBreakByImpact ?? false;
            }
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            Character character = timeLineInstance.Owner as Character;

            if (character == null)
            {
                Log.Error("Time line instance owner should be Character but is a '{0}'.", timeLineInstance.Owner == null ? "null" : timeLineInstance.Owner.GetType().Name);
                return;
            }

            if (character.Motion == null)
            {
                Log.Error("character.Motion is null!");
                return;
            }

            if (character.Motion.ReplaceSkillInfo == null)
            {
                Log.Error("character.Motion.ReplaceSkillInfo is null!");
                return;
            }

            if (character.Motion.ReplaceSkillInfo.NeedReplaceSkill())
            {
                character.Motion.BreakCurrentSkill(SkillEndReasonType.BreakBySkill);
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
            var myUserData = userData as EntityTimeLineEventUserData ?? EntityTimeLineEventUserData.DefaultInstance;

            bool shouldBreak = (eventId == (int)EntityTimeLineEvent.Move && CanBreakByMove
                || eventId == (int)EntityTimeLineEvent.Skill && CanBreakBySkill
                || eventId == (int)EntityTimeLineEvent.StateImpact && CanBreakByImpact
                );

            if (!shouldBreak)
            {
                CallBreakFailure(myUserData);
                return;
            }

            timeLineInstance.Break();

            var characterMotion = timeLineInstance.Owner.Motion as CharacterMotion;
            if (!characterMotion.Skilling)
            {
                Log.Warning("Oops, the character motion is not in skill state.");
                return;
            }

            CallBreakSuccess(myUserData);
        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        private void CallBreakSuccess(EntityTimeLineEventUserData customUserData)
        {
            if (customUserData.OnBreakTimeLineSuccess != null)
            {
                customUserData.OnBreakTimeLineSuccess(customUserData.UserData);
            }
        }

        private void CallBreakFailure(EntityTimeLineEventUserData customUserData)
        {
            if (customUserData.OnBreakTimeLineFailure != null)
            {
                customUserData.OnBreakTimeLineFailure(customUserData.UserData);
            }
        }
    }
}
