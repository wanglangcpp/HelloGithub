﻿using GameFramework;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class TimerUtilityWaitEndOfFrameHelper : MonoBehaviour, ITimerUtilityHelper
    {
        private GameFrameworkAction<object> m_Callback;
        private object m_UserData;

        public TimerUtilityWaitEndOfFrameHelper Init(GameFrameworkAction<object> callback, object userData)
        {
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
            yield return new WaitForEndOfFrame();
            if (m_Callback != null)
            {
                m_Callback(m_UserData);
            }

            Destroy(gameObject);
        }
    }
}
