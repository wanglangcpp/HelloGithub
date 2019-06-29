using UnityEngine;

namespace Genesis.GameClient
{
    public class EntityReplacementTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityReplacementTimeLineActionData m_Data;

        public EntityReplacementTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityReplacementTimeLineActionData;
        }

        public int ReplacedTimeLineId
        {
            get
            {
                return m_Data.ReplacedTimeLineId ?? 0;
            }
        }

        public int ReplacementTimeLineId
        {
            get
            {
                return m_Data.ReplacementTimeLineId ?? 0;
            }
        }

        public float ReplacementWaitTime
        {
            get
            {
                return m_Data.ReplacementWaitTime ?? 0f;
            }
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            Character character = timeLineInstance.Owner as Character;
            character.Motion.ReplaceSkillInfo.SetReplaceSkill(ReplacedTimeLineId, ReplacementTimeLineId, Time.time + ReplacementWaitTime);
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
