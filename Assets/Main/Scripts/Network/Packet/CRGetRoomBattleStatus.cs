﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 5014_CRGetRoomBattleStatus.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CRGetRoomBattleStatus")]
    public partial class CRGetRoomBattleStatus : PacketBase
    {
        public CRGetRoomBattleStatus() { }

        public override PacketType PacketType { get { return PacketType.ClientToRoomServer; } }

        public override int PacketActionId { get { return 5014; } }
    }
}
