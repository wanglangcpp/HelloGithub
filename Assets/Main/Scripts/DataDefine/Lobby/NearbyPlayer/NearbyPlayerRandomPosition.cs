using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class NearbyPlayerMovement
    {
        [SerializeField]
        private bool m_IsArriveTargetPos = false;

        [SerializeField]
        private bool m_IsStartMove = false;

        public Vector3 TargetPosition { get; set; }

        public float StayTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public bool IsArriveTargetPos
        {
            get
            {
                return m_IsArriveTargetPos;
            }
            set
            {
                m_IsArriveTargetPos = value;
            }
        }

        public bool IsStartMove
        {
            get
            {
                return m_IsStartMove;
            }
            set
            {
                m_IsStartMove = value;
            }
        }
    }
}
