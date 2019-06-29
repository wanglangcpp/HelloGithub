namespace Genesis.GameClient
{
    public class EntitySelfShowEffectTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntitySelfShowEffectTimeLineActionData m_Data;

        private int m_EffectEntityId = 0;

        public EntitySelfShowEffectTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntitySelfShowEffectTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            Character character = timeLineInstance.Owner as Character;

            float ffTill;
            if (TryGetUserData(timeLineInstance, Constant.TimeLineFastForwardTillKey, out ffTill) && ffTill > StartTime)
            {
                return;
            }

            m_EffectEntityId = GameEntry.Entity.GetSerialId();
            if (character != null)
            {
                character.AddEffect(m_EffectEntityId);
            }

            GameEntry.Entity.ShowEffect(new EffectData(m_EffectEntityId, m_Data.AttachPointPath, m_Data.ResourceName, timeLineInstance.Owner.Id));
        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {
            CheckAutoStop(timeLineInstance);
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {
            CheckAutoStop(timeLineInstance);
        }

        private void CheckAutoStop(ITimeLineInstance<Entity> timeLineInstance)
        {
            Character character = timeLineInstance.Owner as Character;

            if (!GameEntry.IsAvailable || !m_Data.AutoStop || m_EffectEntityId == 0)
            {
                return;
            }

            if (character != null)
            {
                character.HideAndRemoveEffect(m_EffectEntityId);
            }
            else
            {
                if (GameEntry.Entity.HasEntity(m_EffectEntityId) || GameEntry.Entity.IsLoadingEntity(m_EffectEntityId))
                {
                    GameEntry.Entity.HideEntity(m_EffectEntityId);
                }
            }
        }
    }
}
