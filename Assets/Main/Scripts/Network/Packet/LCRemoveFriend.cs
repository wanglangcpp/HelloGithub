﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 3105_LCRemoveFriend.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"LCRemoveFriend")]
    public partial class LCRemoveFriend : PacketBase
    {
        public LCRemoveFriend() { }

        public override PacketType PacketType { get { return PacketType.LobbyServerToClient; } }

        public override int PacketActionId { get { return 3105; } }

        private int m_FriendPlayerId;
        [ProtoMember(1, Name = @"FriendPlayerId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int FriendPlayerId
        {
            get { return m_FriendPlayerId; }
            set { m_FriendPlayerId = value; }
        }
    }
}
