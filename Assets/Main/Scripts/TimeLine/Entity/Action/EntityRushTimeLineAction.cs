using GameFramework;

namespace Genesis.GameClient
{
    public class EntityRushTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityRushTimeLineActionData m_Data;

        public EntityRushTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityRushTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            var character = timeLineInstance.Owner as Character;
            if (character == null)
            {
                Log.Warning("Time line owner has to be a character.");
                return;
            }

            character.Motion.StartSkillRushing(m_Data);
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {
            var character = timeLineInstance.Owner as Character;
            if (character == null)
            {
                Log.Warning("Time line owner has to be a character.");
                return;
            }

            character.Motion.StopSkillRushing();
        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {
            var character = timeLineInstance.Owner as Character;
            if (character == null)
            {
                Log.Warning("Time line owner has to be a character.");
                return;
            }

            if (character.Motion == null)
            {
                return;
            }

            character.Motion.StopSkillRushing();
        }
    }
}
