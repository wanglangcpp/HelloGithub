﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 1004_CLEnterInstance.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CLEnterInstance")]
    public partial class CLEnterInstance : PacketBase
    {
        public CLEnterInstance() { }

        public override PacketType PacketType { get { return PacketType.ClientToLobbyServer; } }

        public override int PacketActionId { get { return 1004; } }

        private int m_InstanceType;
        [ProtoMember(1, Name = @"InstanceType", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int InstanceType
        {
            get { return m_InstanceType; }
            set { m_InstanceType = value; }
        }
    }
}
