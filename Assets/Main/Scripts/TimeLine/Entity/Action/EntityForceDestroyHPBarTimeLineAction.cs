namespace Genesis.GameClient
{
    public class EntityForceDestroyHPBarTimeLineAction : EntityAbstractTimeLineAction
    {
        public EntityForceDestroyHPBarTimeLineAction(TimeLineActionData data)
            : base(data)
        {

        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            FindAndDestroyHPBar(timeLineInstance);
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            FindAndDestroyHPBar(timeLineInstance);
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

        private void FindAndDestroyHPBar(ITimeLineInstance<Entity> timeLineInstance)
        {
            var targetableOwner = timeLineInstance.Owner as TargetableObject;
            if (targetableOwner == null)
            {
                return;
            }

            var hpBar = GameEntry.Impact.GetNameBoard(targetableOwner);
            if (hpBar == null)
            {
                return;
            }

            GameEntry.Impact.DestroyNameBoard(hpBar);
        }
    }
}
