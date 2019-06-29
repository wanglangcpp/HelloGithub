namespace Genesis.GameClient
{
    public class EntityShakeCameraTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityShakeCameraTimeLineActionData m_Data;

        public EntityShakeCameraTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityShakeCameraTimeLineActionData;
        }

        private bool ShouldShake(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (timeLineInstance.Owner is MeHeroCharacter)
            {
                if (m_Data.AffectMe)
                {
                    return true;
                }
            }
            else
            {
                if (m_Data.AffectOthers)
                {
                    return true;
                }
            }

            return false;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (ShouldShake(timeLineInstance))
            {
                GameEntry.CameraShaking.PerformShaking(m_Data.ShakeId);
            }
        }
    }
}
