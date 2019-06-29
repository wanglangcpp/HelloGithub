namespace Genesis.GameClient
{
    public class EntityImpactShakeCameraTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityImpactShakeCameraTimeLineActionData m_Data;

        public EntityImpactShakeCameraTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityImpactShakeCameraTimeLineActionData;
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

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {
            if (eventId != (int)(EntityTimeLineEvent.Impact))
            {
                return;
            }

            if (ShouldShake(timeLineInstance))
            {
                GameEntry.CameraShaking.PerformShaking(m_Data.ShakeId);
            }
        }
    }
}
