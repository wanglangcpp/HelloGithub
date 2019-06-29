using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class PortalData : ShedObjectData
    {
        [SerializeField]
        private int m_PortalId = 0;

        [SerializeField]
        private float m_Radius = 1f;

        [SerializeField]
        private PortalParam[] m_PortalParams = null;

        public PortalData(int entityId)
            : base(entityId)
        {

        }

        public new Portal Entity
        {
            get
            {
                return base.Entity as Portal;
            }
        }

        /// <summary>
        /// 传送点类型编号。
        /// </summary>
        public int PortalId
        {
            get
            {
                return m_PortalId;
            }
            set
            {
                m_PortalId = value;
            }
        }

        /// <summary>
        /// 传送点半径。
        /// </summary>
        public float Radius
        {
            get
            {
                return m_Radius;
            }
            set
            {
                m_Radius = value;
            }
        }

        /// <summary>
        /// 传送点参数。
        /// </summary>
        public PortalParam[] PortalParams
        {
            get
            {
                return m_PortalParams;
            }
            set
            {
                m_PortalParams = value;
            }
        }

        /// <summary>
        /// 作为出口时激活的寻径点组。
        /// </summary>
        public int? GuidePointGroupToActivateOnExit { get; set; }

        /// <summary>
        /// 显示时激活的寻径点组。
        /// </summary>
        public int? GuidePointGroupToActivateOnShow { get; internal set; }
    }
}
