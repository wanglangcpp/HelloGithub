using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class VipInfoData : IGenericData<VipInfoData, PBVipPrivilegeInfo>
    {
        [SerializeField]
        private VipPrivilegeType m_Type;

        [SerializeField]
        private int m_UsedVipPrivilegeCount;

        public int Key
        {
            get
            {
                return (int)m_Type;
            }
        }

        /// <summary>
        /// 购买金币次数。
        /// </summary>
        public int UsedVipPrivilegeCount
        {
            get
            {
                return m_UsedVipPrivilegeCount;
            }
            set
            {
                m_UsedVipPrivilegeCount = value;
            }
        }

        public void UpdateData(PBVipPrivilegeInfo data)
        {
            m_Type = (VipPrivilegeType)data.VipPrivilegeType;
            m_UsedVipPrivilegeCount = data.UsedVipPrivilegeCount;
        }
    }
}
