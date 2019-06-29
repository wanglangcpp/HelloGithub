using GameFramework;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class TimerUtilityWaitSecondsHelper : MonoBehaviour, ITimerUtilityHelper
    {
        private float m_Seconds;
        private GameFrameworkAction<object> m_Callback;
        private object m_UserData;

        public TimerUtilityWaitSecondsHelper Init(float seconds, GameFrameworkAction<object> callback, object userData)
        {
            m_Seconds = seconds;
            m_Callback = callback;
            m_UserData = userData;
            return this;
        }

        public void Interrupt()
        {
            Destroy(gameObject);
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(m_Seconds);
            if (m_Callback != null)
            {
                m_Callback(m_UserData);
            }

            Destroy(gameObject);
        }
    }
}
