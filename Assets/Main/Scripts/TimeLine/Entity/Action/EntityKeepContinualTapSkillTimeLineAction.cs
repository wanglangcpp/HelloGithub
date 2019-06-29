using GameFramework;

namespace Genesis.GameClient
{
    public class EntityKeepContinualTapSkillTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityKeepContinualTapSkillTimeLineActionData m_Data;

        private int m_InputCount = 0;
        private float[] m_InputCheckTimePoints = null;
        private int m_CurrentInputCheckIntervalIndex = 0;
        private GameFrameworkAction<ITimeLineInstance<Entity>, float> m_OnUpdate = null;

        public EntityKeepContinualTapSkillTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityKeepContinualTapSkillTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            if (!(timeLineInstance.Owner is HeroCharacter))
            {
                Log.Error("Owner should be HeroCharacter, but is actually '{0}'", timeLineInstance.Owner == null ? "null" : timeLineInstance.Owner.GetType().Name);
                return;
            }

            var inputCheckIntervals = m_Data.GetInputCheckIntervals();
            m_InputCheckTimePoints = new float[inputCheckIntervals.Count];

            for (int i = 0; i < inputCheckIntervals.Count; ++i)
            {
                if (inputCheckIntervals[i] <= 0f)
                {
                    Log.Warning("Input check interval {0} should be positive.", i);
                }

                if (i == 0)
                {
                    m_InputCheckTimePoints[i] = inputCheckIntervals[i];
                }
                else
                {
                    m_InputCheckTimePoints[i] = m_InputCheckTimePoints[i - 1] + inputCheckIntervals[i];
                }
            }

            m_CurrentInputCheckIntervalIndex = 0;
            ResetInput();

            if (GameEntry.Data.Room.InRoom && !(timeLineInstance.Owner is MeHeroCharacter))
            {
                m_OnUpdate = OnUpdate_DontCheckInput;
            }
            else
            {
                m_OnUpdate = OnUpdate_Default;
            }
        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            m_OnUpdate(timeLineInstance, elapseSeconds);
        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {
            var senderAsCharacterMotion = sender as CharacterMotion;
            if (senderAsCharacterMotion == null || senderAsCharacterMotion.Owner != timeLineInstance.Owner || eventId != (int)EntityTimeLineEvent.ContinualTapSkill)
            {
                return;
            }

            HandleInput();
        }

        private void HandleInput()
        {
            m_InputCount++;
        }

        private void ResetInput()
        {
            m_InputCount = 0;
        }

        private void OnUpdate_Default(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            var currentTime = timeLineInstance.CurrentTime - StartTime;
            while (m_CurrentInputCheckIntervalIndex < m_InputCheckTimePoints.Length && currentTime >= m_InputCheckTimePoints[m_CurrentInputCheckIntervalIndex])
            {
                if (m_InputCount <= 0)
                {
                    GameEntry.Event.Fire(this, new SkillContinualTapUpdateInputEventArgs(timeLineInstance.Owner.Id, false));
                    FastForwardSelfAndCommonCD(timeLineInstance);
                    return;
                }

                m_CurrentInputCheckIntervalIndex++;
                GameEntry.Event.Fire(this, new SkillContinualTapUpdateInputEventArgs(timeLineInstance.Owner.Id, true));
                ResetInput();
            }
        }

        private void OnUpdate_DontCheckInput(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {
            var currentTime = timeLineInstance.CurrentTime - StartTime;
            while (m_CurrentInputCheckIntervalIndex < m_InputCheckTimePoints.Length && currentTime >= m_InputCheckTimePoints[m_CurrentInputCheckIntervalIndex])
            {
                m_CurrentInputCheckIntervalIndex++;
            }
        }
    }
}
