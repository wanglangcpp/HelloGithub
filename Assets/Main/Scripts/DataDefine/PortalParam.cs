using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class PortalParam
    {
        [SerializeField]
        private int m_PortalId = 0;

        [SerializeField]
        private float m_Ratio = 0f;

        public int PortalId
        {
            get
            {
                return m_PortalId;
            }
        }

        public float Ratio
        {
            get
            {
                return m_Ratio;
            }
        }
    }
}
