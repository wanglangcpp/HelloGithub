﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 2009_CLChangeHeroTeamChess.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CLChangeHeroTeamChess")]
    public partial class CLChangeHeroTeamChess : PacketBase
    {
        public CLChangeHeroTeamChess() { }

        public override PacketType PacketType { get { return PacketType.ClientToLobbyServer; } }

        public override int PacketActionId { get { return 2009; } }

        private PBHeroTeamInfo m_HeroTeamInfo;
        [ProtoMember(3, Name = @"HeroTeamInfo", IsRequired = true, DataFormat = DataFormat.Default)]
        public PBHeroTeamInfo HeroTeamInfo
        {
            get { return m_HeroTeamInfo; }
            set { m_HeroTeamInfo = value; }
        }
    }
}
