﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 3202_CLDeleteMail.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CLDeleteMail")]
    public partial class CLDeleteMail : PacketBase
    {
        public CLDeleteMail() { }

        public override PacketType PacketType { get { return PacketType.ClientToLobbyServer; } }

        public override int PacketActionId { get { return 3202; } }

        private readonly List<int> m_MailIds = new List<int>();
        [ProtoMember(1, Name = @"MailIds", DataFormat = DataFormat.TwosComplement)]
        public List<int> MailIds
        {
            get { return m_MailIds; }
        }
    }
}
