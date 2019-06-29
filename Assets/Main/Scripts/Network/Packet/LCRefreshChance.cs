﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 2203_LCRefreshChance.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"LCRefreshChance")]
    public partial class LCRefreshChance : PacketBase
    {
        public LCRefreshChance() { }

        public override PacketType PacketType { get { return PacketType.LobbyServerToClient; } }

        public override int PacketActionId { get { return 2203; } }

        private PBChanceInfo m_ChanceInfo;
        [ProtoMember(1, Name = @"ChanceInfo", IsRequired = true, DataFormat = DataFormat.Default)]
        public PBChanceInfo ChanceInfo
        {
            get { return m_ChanceInfo; }
            set { m_ChanceInfo = value; }
        }

        private PBPlayerInfo m_PlayerInfo = null;
        [ProtoMember(2, Name = @"PlayerInfo", IsRequired = false, DataFormat = DataFormat.Default)]
        [System.ComponentModel.DefaultValue(null)]
        public PBPlayerInfo PlayerInfo
        {
            get { return m_PlayerInfo; }
            set { m_PlayerInfo = value; }
        }
    }
}
