using GameFramework;
using System.Net;
using UnityEngine;

namespace Genesis.GameClient
{
    public class Ping
    {
        private string m_IPString = null;
        private float m_PingInterval = 0f;

        private float m_PingValue = 0f;
        private float m_NextPingTime = 0f;
        private UnityEngine.Ping m_Ping = null;

        public Ping(string ipString, float pingInterval = 3f)
        {
            IPAddress ipAddress = null;
            if (!IPAddress.TryParse(ipString, out ipAddress))
            {
                Log.Error("Ping IP string is invalid.");
                return;
            }

            m_IPString = ipAddress.ToString();
            m_PingInterval = pingInterval;
        }

        public string IPString
        {
            get
            {
                return m_IPString;
            }
        }

        public float PingValue
        {
            get
            {
                return m_PingValue;
            }
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_Ping == null || Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (Time.unscaledTime >= m_NextPingTime)
                {
                    m_Ping = new UnityEngine.Ping(m_IPString);
                }
            }
            else if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                m_PingValue = 0f;
                m_NextPingTime = Time.unscaledTime;
                m_Ping.DestroyPing();
                m_Ping = null;
            }
            else if (m_Ping.isDone)
            {
                m_PingValue = m_Ping.time;
                m_NextPingTime = Time.unscaledTime + m_PingInterval;
                m_Ping.DestroyPing();
                m_Ping = null;
            }
        }

        public void Shutdown()
        {
            if (m_Ping == null)
            {
                return;
            }

            m_Ping.DestroyPing();
            m_Ping = null;
        }
    }
}
