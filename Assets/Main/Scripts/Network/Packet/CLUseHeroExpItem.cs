﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 1033_CLUseHeroExpItem.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CLUseHeroExpItem")]
    public partial class CLUseHeroExpItem : PacketBase
    {
        public CLUseHeroExpItem() { }

        public override PacketType PacketType { get { return PacketType.ClientToLobbyServer; } }

        public override int PacketActionId { get { return 1033; } }

        private int m_HeroId;
        [ProtoMember(1, Name = @"HeroId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int HeroId
        {
            get { return m_HeroId; }
            set { m_HeroId = value; }
        }

        private PBItemInfo m_ItemInfo;
        [ProtoMember(2, Name = @"ItemInfo", IsRequired = true, DataFormat = DataFormat.Default)]
        public PBItemInfo ItemInfo
        {
            get { return m_ItemInfo; }
            set { m_ItemInfo = value; }
        }
    }
}
