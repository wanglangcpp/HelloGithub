﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 5016_CLGetSinlgePVPRanks.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CLGetSinlgePVPRanks")]
    public partial class CLGetSinlgePVPRanks : PacketBase
    {
        public CLGetSinlgePVPRanks() { }

        public override PacketType PacketType { get { return PacketType.ClientToLobbyServer; } }

        public override int PacketActionId { get { return 5016; } }

        private int m_Type;
        [ProtoMember(1, Name = @"Type", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
    }
}
