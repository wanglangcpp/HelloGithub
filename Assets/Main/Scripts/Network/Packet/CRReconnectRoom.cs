﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 5005_CRReconnectRoom.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CRReconnectRoom")]
    public partial class CRReconnectRoom : PacketBase
    {
        public CRReconnectRoom() { }

        public override PacketType PacketType { get { return PacketType.ClientToRoomServer; } }

        public override int PacketActionId { get { return 5005; } }
    }
}
