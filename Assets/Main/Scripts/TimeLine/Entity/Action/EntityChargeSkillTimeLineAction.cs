using GameFramework;

namespace Genesis.GameClient
{
    public class EntityChargeSkillTimeLineAction : EntityAbstractTimeLineAction
    {
        public EntityChargeSkillTimeLineAction(TimeLineActionData data) : base(data)
        {
            m_Data = data as EntityChargeSkillTimeLineActionData;
        }

        private EntityChargeSkillTimeLineActionData m_Data;

        private bool m_EnablePerform;

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            var owner = timeLineInstance.Owner as HeroCharacter;

            if (owner == null)
            {
                Log.Error("Owner should be HeroCharacter, but is actually '{0}'", timeLineInstance.Owner == null ? "null" : timeLineInstance.Owner.GetType().Name);
                return;
            }

            m_EnablePerform = false;
        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            base.OnUpdate(timeLineInstance, elapseSeconds);

            if (m_EnablePerform && m_Data.JumpImmediatelyOnRelease)
            {
                timeLineInstance.FastForward(m_Data.JumpSkillActionTime - timeLineInstance.CurrentTime, true);
                GameEntry.Event.FireNow(this, new PerformChargeSkillEventArgs(PerformChargeSkillEventArgs.OperateType.EventHide, 0));
            }
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {
            base.OnFinish(timeLineInstance);

            if (m_EnablePerform && !m_Data.JumpImmediatelyOnRelease)
            {
                timeLineInstance.FastForward(m_Data.JumpSkillActionTime - timeLineInstance.CurrentTime, true);
                GameEntry.Event.FireNow(this, new PerformChargeSkillEventArgs(PerformChargeSkillEventArgs.OperateType.EventHide, 0));
            }
        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {
            if (eventId != (int)EntityTimeLineEvent.PerformChargeSkill)
                return;

            m_EnablePerform = true;
        }
    }
}
