using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class AirWallData : ShedObjectData
    {
        [SerializeField]
        private int m_AirWallId;

        public AirWallData(int entityId)
            : base(entityId)
        {

        }

        public new AirWall Entity
        {
            get
            {
                return base.Entity as AirWall;
            }
        }

        /// <summary>
        /// 空气墙类型编号。
        /// </summary>
        public int AirWallId
        {
            get
            {
                return m_AirWallId;
            }
            set
            {
                m_AirWallId = value;
            }
        }
    }
}
