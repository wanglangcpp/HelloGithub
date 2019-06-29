using GameFramework;

namespace Genesis.GameClient
{
    public class EntityAutoJumpTimeLineAction : EntityAbstractTimeLineAction
    {
        public EntityAutoJumpTimeLineAction(TimeLineActionData data) : base(data)
        {
            
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {

            var owner = timeLineInstance.Owner as HeroCharacter;

            //这个Action目前只适用于英雄释放蓄力技能，所以在此加一个判断
            if (owner == null)
            {
                Log.Error("Owner should be HeroCharacter, but is actually '{0}'", timeLineInstance.Owner == null ? "null" : timeLineInstance.Owner.GetType().Name);
                return;
            }

            timeLineInstance.FastForward(timeLineInstance.Duration - timeLineInstance.CurrentTime, true);
        }

    }
}
