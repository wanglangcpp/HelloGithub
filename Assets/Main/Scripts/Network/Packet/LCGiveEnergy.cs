﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 3106_LCGiveEnergy.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"LCGiveEnergy")]
    public partial class LCGiveEnergy : PacketBase
    {
        public LCGiveEnergy() { }

        public override PacketType PacketType { get { return PacketType.LobbyServerToClient; } }

        public override int PacketActionId { get { return 3106; } }

        private int? m_TodayGiveCount;
        [ProtoMember(1, Name = @"TodayGiveCount", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int TodayGiveCount
        {
            get { return m_TodayGiveCount ?? default(int); }
            set { m_TodayGiveCount = value; }
        }
        public bool HasTodayGiveCount { get { return m_TodayGiveCount != null; } }
        private void ResetTodayGiveCount() { m_TodayGiveCount = null; }
        private bool ShouldSerializeTodayGiveCount() { return HasTodayGiveCount; }

        private PBFriendInfo m_FriendInfo;
        [ProtoMember(2, Name = @"FriendInfo", IsRequired = true, DataFormat = DataFormat.Default)]
        public PBFriendInfo FriendInfo
        {
            get { return m_FriendInfo; }
            set { m_FriendInfo = value; }
        }
    }
}
