﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: PBVipInfo.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"PBVipInfo")]
    public partial class PBVipInfo
    {
        public PBVipInfo() { }

        private readonly List<PBVipPrivilegeInfo> m_VipPrivilegeInfo = new List<PBVipPrivilegeInfo>();
        [ProtoMember(1, Name = @"VipPrivilegeInfo", DataFormat = DataFormat.Default)]
        public List<PBVipPrivilegeInfo> VipPrivilegeInfo
        {
            get { return m_VipPrivilegeInfo; }
        }
    }
}
