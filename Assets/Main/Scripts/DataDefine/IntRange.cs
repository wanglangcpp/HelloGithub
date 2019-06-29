using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public struct IntRange
    {
        [SerializeField]
        private int m_Min;

        [SerializeField]
        private int m_Max;

        public int MinValue
        {
            get
            {
                return m_Min;
            }
        }

        public int MaxValue
        {
            get
            {
                return m_Max;
            }
        }

        public IntRange(int min, int max)
        {
            m_Min = min;
            m_Max = max;
        }
    }
}
