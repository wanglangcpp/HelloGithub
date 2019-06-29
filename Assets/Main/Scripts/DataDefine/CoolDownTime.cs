using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class CoolDownTime
    {
        [SerializeField]
        private float m_CurrentCoolDownSeconds;

        [SerializeField]
        private float m_TotalCoolDownSeconds;

        public CoolDownTime()
        {
            TotalCoolDownSeconds = 0f;
            CurrentCoolDownSeconds = 0f;
        }

        public CoolDownTime(float totalCoolDownSeconds)
        {
            TotalCoolDownSeconds = totalCoolDownSeconds;
            CurrentCoolDownSeconds = 0f;
        }

        public CoolDownTime(float currentCoolDownSeconds, float totalCoolDownSeconds)
        {
            m_TotalCoolDownSeconds = totalCoolDownSeconds;
            CurrentCoolDownSeconds = currentCoolDownSeconds;
        }

        /// <summary>
        /// 是否暂停。
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// 是否完成了冷却。
        /// </summary>
        public bool IsReady
        {
            get
            {
                return m_CurrentCoolDownSeconds >= m_TotalCoolDownSeconds;
            }
        }

        public float TotalCoolDownSeconds
        {
            get
            {
                return m_TotalCoolDownSeconds;
            }
            set
            {
                if (value < 0f)
                {
                    m_TotalCoolDownSeconds = 0f;
                    return;
                }

                m_TotalCoolDownSeconds = value;
            }
        }

        public float CurrentCoolDownSeconds
        {
            get
            {
                return m_CurrentCoolDownSeconds;
            }
            set
            {
                if (value < 0f)
                {
                    m_CurrentCoolDownSeconds = 0f;
                    return;
                }

                if (value > m_TotalCoolDownSeconds)
                {
                    m_CurrentCoolDownSeconds = m_TotalCoolDownSeconds;
                    return;
                }

                m_CurrentCoolDownSeconds = value;
            }
        }

        public float CurrentCoolDownTimeRatio
        {
            get
            {
                return m_TotalCoolDownSeconds > 0f ? m_CurrentCoolDownSeconds / m_TotalCoolDownSeconds : 1f;
            }
        }

        public float RemainingCoolDownSeconds
        {
            get
            {
                return m_TotalCoolDownSeconds - m_CurrentCoolDownSeconds;
            }
        }

        public float RemainingCoolDownTimeRatio
        {
            get
            {
                return m_TotalCoolDownSeconds > 0f ? RemainingCoolDownSeconds / m_TotalCoolDownSeconds : 0f;
            }
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (IsPaused) { return; }

            CurrentCoolDownSeconds += elapseSeconds;
        }

        public void Reset(float totalCoolDownSeconds, bool shouldPause)
        {
            TotalCoolDownSeconds = totalCoolDownSeconds;
            CurrentCoolDownSeconds = 0f;
            IsPaused = shouldPause;
        }

        public void SetReady()
        {
            m_CurrentCoolDownSeconds = m_TotalCoolDownSeconds;
        }
    }
}
