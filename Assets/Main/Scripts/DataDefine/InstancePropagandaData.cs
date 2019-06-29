using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    [Serializable]
    public class InstancePropagandaData
    {
        [SerializeField]
        private bool m_Overrideable;

        [SerializeField]
        private float m_Duration;

        [SerializeField]
        private string m_StringKey;

        [SerializeField]
        private string[] m_Args;

        [SerializeField]
        private int m_OwnerNpcIndex = 0;

        private string m_Text;

        public string Text
        {
            get
            {
                if (m_Text == null)
                {
                    if (m_Args == null)
                    {
                        m_Text = GameEntry.Localization.GetString(m_StringKey);
                    }
                    else
                    {
                        m_Text = GameEntry.Localization.GetString(m_StringKey, m_Args);
                    }
                }

                return m_Text;
            }
        }

        public bool IsOverrideable
        {
            get
            {
                return m_Overrideable;
            }
        }

        public float Duration
        {
            get
            {
                return m_Duration;
            }
        }

        public int OwnerNpcIndex
        {
            get
            {
                return m_OwnerNpcIndex;
            }
        }

        /// <summary>
        /// Default constructor for serialization.
        /// </summary>
        public InstancePropagandaData()
        {
            // Empty.
        }

        public InstancePropagandaData(bool overrideable, float duration, string stringKey, string[] args, int npcIndex)
        {
            m_Overrideable = overrideable;
            m_Duration = duration;
            m_StringKey = stringKey;
            m_Args = args;
            m_OwnerNpcIndex = npcIndex;
        }

        public InstancePropagandaData(InstancePropagandaData other)
        {
            m_Overrideable = other.m_Overrideable;
            m_Duration = other.m_Duration;
            m_OwnerNpcIndex = other.m_OwnerNpcIndex;
            m_StringKey = other.m_StringKey;

            if (other.m_Args != null)
            {
                m_Args = new string[other.m_Args.Length];
                for (int i = 0; i < m_Args.Length; ++i)
                {
                    m_Args[i] = other.m_Args[i];
                }
            }
        }
    }
}
