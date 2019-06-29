using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 子弹反弹器数据。
    /// </summary>
    [Serializable]
    public class BulletRebounderData : ShedObjectData
    {
        [SerializeField]
        private int m_BulletRebounderId;

        public BulletRebounderData(int entityId)
            : base(entityId)
        {

        }

        public new BulletRebounder Entity
        {
            get
            {
                return base.Entity as BulletRebounder;
            }
        }

        /// <summary>
        /// 子弹反弹器类型编号。
        /// </summary>
        public int BulletRebounderId
        {
            get
            {
                return m_BulletRebounderId;
            }
            set
            {
                m_BulletRebounderId = value;
            }
        }
    }
}
