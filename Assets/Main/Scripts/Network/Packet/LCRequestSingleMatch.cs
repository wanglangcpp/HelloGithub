﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 5007_LCRequestSingleMatch.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"LCRequestSingleMatch")]
    public partial class LCRequestSingleMatch : PacketBase
    {
        public LCRequestSingleMatch() { }

        public override PacketType PacketType { get { return PacketType.LobbyServerToClient; } }

        public override int PacketActionId { get { return 5007; } }

        private int m_ErrorCode;
        [ProtoMember(1, Name = @"ErrorCode", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int ErrorCode
        {
            get { return m_ErrorCode; }
            set { m_ErrorCode = value; }
        }
    }
}
