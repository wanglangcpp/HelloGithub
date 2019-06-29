﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 1004_LCEnterInstance.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"LCEnterInstance")]
    public partial class LCEnterInstance : PacketBase
    {
        public LCEnterInstance() { }

        public override PacketType PacketType { get { return PacketType.LobbyServerToClient; } }

        public override int PacketActionId { get { return 1004; } }

        private int m_InstanceType;
        [ProtoMember(1, Name = @"InstanceType", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int InstanceType
        {
            get { return m_InstanceType; }
            set { m_InstanceType = value; }
        }

        private PBPlayerInfo m_PlayerInfo;
        [ProtoMember(2, Name = @"PlayerInfo", IsRequired = true, DataFormat = DataFormat.Default)]
        public PBPlayerInfo PlayerInfo
        {
            get { return m_PlayerInfo; }
            set { m_PlayerInfo = value; }
        }
    }
}
