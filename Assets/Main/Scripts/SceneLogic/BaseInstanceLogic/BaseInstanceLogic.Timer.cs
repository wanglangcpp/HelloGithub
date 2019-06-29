using UnityEngine;

namespace Genesis.GameClient
{
    public abstract partial class BaseInstanceLogic
    {
        protected InstanceTimer m_InstanceTimer = null;

        public InstanceTimer Timer
        {
            get
            {
                return m_InstanceTimer;
            }
        }

        public class InstanceTimer
        {
            private readonly Type m_TimerType;
            private readonly float m_TotalTime;
            private readonly float m_AlertTime;

            private float m_ElapseSeconds = 0f;
            private float? m_StopTime;
            private float m_StartTime;

            public bool IsPaused { get; set; }

            public float CurrentSeconds
            {
                get
                {
                    return m_ElapseSeconds;
                }
            }

            public float LeftSeconds
            {
                get
                {
                    return Mathf.Max(0f, m_TotalTime - CurrentSeconds);
                }
            }

            public bool IsAlert
            {
                get
                {
                    return LeftSeconds <= m_AlertTime;
                }
            }

            public string FormattedTimeString
            {
                get
                {
                    string formatString = IsAlert ? "UI_TEXT_TIMER_ALERT" : "UI_TEXT_TIMER_NORMAL";
                    switch (m_TimerType)
                    {
                        case Type.CountUp:
                            return GameEntry.Localization.GetString(formatString, (int)CurrentSeconds / 60, (int)CurrentSeconds % 60);
                        case Type.CountDown:
                            return GameEntry.Localization.GetString(formatString, (int)LeftSeconds / 60, (int)LeftSeconds % 60);
                        default:
                            return string.Empty;
                    }
                }
            }

            private bool HasStarted
            {
                get
                {
                    return Time.time >= m_StartTime;
                }
            }

            private bool IsStopped
            {
                get
                {
                    return m_StopTime != null;
                }
            }

            public InstanceTimer(int timerType, float totalTime, float alertTime)
            {
                m_TimerType = (Type)timerType;
                m_TotalTime = totalTime;
                m_AlertTime = alertTime;
                m_StopTime = null;
            }

            public InstanceTimer(int timerType, float totalTime, float alertTime, float startTime)
            {
                m_TimerType = (Type)timerType;
                m_TotalTime = totalTime;
                m_AlertTime = alertTime;
                m_StartTime = startTime;

                if (HasStarted)
                {
                    m_ElapseSeconds = Time.time - m_StartTime;
                }

                m_StopTime = null;
            }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (!HasStarted || IsPaused || IsStopped)
                {
                    return;
                }

                m_ElapseSeconds += elapseSeconds;
            }

            public void StopTimer()
            {
                m_StopTime = Time.time;
            }

            private enum Type
            {
                Undefined = 0,

                /// <summary>
                /// 正计时。
                /// </summary>
                CountUp = 1,

                /// <summary>
                /// 倒计时。
                /// </summary>
                CountDown = 2,
            }
        }
    }
}
