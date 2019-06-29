namespace Genesis.GameClient
{
    public class EntitySoundTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntitySoundTimeLineActionData m_Data;
        private int m_SoundSerialId = -1;

        public EntitySoundTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntitySoundTimeLineActionData;
        }

        public int SoundId
        {
            get
            {
                return m_Data.SoundId;
            }
        }

        public bool NeedBroadcast
        {
            get
            {
                return m_Data.NeedBroadcast;
            }
        }

        public bool StopWhenBreak
        {
            get
            {
                return m_Data.StopWhenBreak;
            }
        }

        public bool StopWhenFinish
        {
            get
            {
                return m_Data.StopWhenFinish;
            }
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (NeedBroadcast || AIUtility.EntityDataIsMine(timeLineInstance.Owner.Data))
            {
                m_SoundSerialId = GameEntry.Sound.PlaySound(SoundId, timeLineInstance.Owner);
            }
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (StopWhenFinish && m_SoundSerialId >= 0)
            {
                GameEntry.Sound.StopSound(m_SoundSerialId);
                m_SoundSerialId = -1;
            }
        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {

        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {
            if (StopWhenBreak && m_SoundSerialId >= 0)
            {
                GameEntry.Sound.StopSound(m_SoundSerialId);
                m_SoundSerialId = -1;
            }
        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }
    }
}
