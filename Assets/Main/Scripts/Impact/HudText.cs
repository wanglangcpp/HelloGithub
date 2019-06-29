using UnityEngine;

namespace Genesis.GameClient
{
    public class HudText : MonoBehaviour
    {
        [SerializeField]
        private int m_Type;

        [SerializeField]
        private float m_StartTime;

        [SerializeField]
        private Vector3 m_WorldOffset;

        [SerializeField]
        private UILabel m_TextLabel = null;

        [SerializeField]
        private int m_PoolIndex = -1;

        public int Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        public float StartTime
        {
            get
            {
                return m_StartTime;
            }
            set
            {
                m_StartTime = value;
            }
        }

        public Vector3 WorldOffset
        {
            get
            {
                return m_WorldOffset;
            }
            set
            {
                m_WorldOffset = value;
            }
        }

        public UILabel TextLabel
        {
            get
            {
                return m_TextLabel;
            }
        }

        public int PoolIndex
        {
            get
            {
                return m_PoolIndex;
            }
            set
            {
                m_PoolIndex = value;
            }
        }
    }
}
