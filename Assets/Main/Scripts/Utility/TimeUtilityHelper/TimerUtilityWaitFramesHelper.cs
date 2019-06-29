using GameFramework;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class TimerUtilityWaitFramesHelper : MonoBehaviour, ITimerUtilityHelper
    {
        private int m_FrameCount;
        private GameFrameworkAction<object> m_Callback;
        private object m_UserData;

        public TimerUtilityWaitFramesHelper Init(int frameCount, GameFrameworkAction<object> callback, object userData)
        {
            m_FrameCount = frameCount;
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
            for (int i = 0; i < m_FrameCount; ++i)
            {
                yield return null;
            }

            if (m_Callback != null)
            {
                m_Callback(m_UserData);
            }

            Destroy(gameObject);
        }
    }
}
