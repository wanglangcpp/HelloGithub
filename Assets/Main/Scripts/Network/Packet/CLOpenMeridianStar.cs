﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 1018_CLOpenMeridianStar.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CLOpenMeridianStar")]
    public partial class CLOpenMeridianStar : PacketBase
    {
        public CLOpenMeridianStar() { }

        public override PacketType PacketType { get { return PacketType.ClientToLobbyServer; } }

        public override int PacketActionId { get { return 1018; } }

        private int m_StarId;
        [ProtoMember(1, Name = @"StarId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int StarId
        {
            get { return m_StarId; }
            set { m_StarId = value; }
        }
    }
}
