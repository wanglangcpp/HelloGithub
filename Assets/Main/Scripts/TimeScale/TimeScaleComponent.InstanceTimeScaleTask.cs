using UnityEngine;

namespace Genesis.GameClient
{

    public partial class TimeScaleComponent
    {
        private class InstanceTimeScaleTask
        {
            private const float InstanceSpeedParam = 0.1f;

            private const float InstanceTimeScaleDuration = 2.5f;

            private const float InstanceTimeScaleRecoveryTime = 1f;

            private float m_CurrentSpeedParam = 0.0f;

            private float m_CurrentDuration = 0.0f;

            private float m_CurrentRecoveryTime = 0.0f;

            private float m_OriginalSpeed = 0.0f;

            private bool m_StartTasking = false;

            public bool StartTasking
            {
                get { return m_StartTasking; }
            }

            public void OnUpdate()
            {
                if (m_CurrentDuration == 0.0f)
                {
                    GameEntry.Base.GameSpeed = m_OriginalSpeed * m_CurrentSpeedParam;
                }

                m_CurrentDuration += Time.unscaledDeltaTime;
                if (m_CurrentDuration >= InstanceTimeScaleDuration)
                {
                    m_CurrentRecoveryTime += Time.unscaledDeltaTime;
                    if (m_CurrentRecoveryTime >= InstanceTimeScaleRecoveryTime)
                    {
                        GameEntry.Event.Fire(this, new TimeScaleTaskFinishEventArgs());
                        Reset();
                    }
                    else
                    {
                        m_CurrentSpeedParam = (1 - InstanceSpeedParam) / InstanceTimeScaleRecoveryTime * m_CurrentRecoveryTime;
                        GameEntry.Base.GameSpeed = m_OriginalSpeed * m_CurrentSpeedParam;
                    }
                }
            }

            public void InitInstanceTask()
            {
                if (GameEntry.Base.IsGamePaused)
                {
                    Debug.LogWarning("Can't Init Instance Task because of Game Pause in TimeScaleComponent.InstanceTimeScaleTask !");
                    return;
                }

                if (m_StartTasking)
                {
                    return;
                }
                m_StartTasking = true;
                m_CurrentSpeedParam = InstanceSpeedParam;
                m_CurrentDuration = 0.0f;
                m_CurrentRecoveryTime = 0.0f;
                m_OriginalSpeed = GameEntry.Base.GameSpeed;
            }

            public void Reset()
            {
                if (m_StartTasking)
                {
                    GameEntry.Base.GameSpeed = m_OriginalSpeed;
                }
                m_StartTasking = false;
            }
        }
    }
}
