﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 2603_CLNPCDialogue.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CLNPCDialogue")]
    public partial class CLNPCDialogue : PacketBase
    {
        public CLNPCDialogue() { }

        public override PacketType PacketType { get { return PacketType.ClientToLobbyServer; } }

        public override int PacketActionId { get { return 2603; } }

        private int m_NPCId;
        [ProtoMember(1, Name = @"NPCId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int NPCId
        {
            get { return m_NPCId; }
            set { m_NPCId = value; }
        }

        private int? m_TaskId;
        [ProtoMember(2, Name = @"TaskId", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int TaskId
        {
            get { return m_TaskId ?? default(int); }
            set { m_TaskId = value; }
        }
        public bool HasTaskId { get { return m_TaskId != null; } }
        private void ResetTaskId() { m_TaskId = null; }
        private bool ShouldSerializeTaskId() { return HasTaskId; }
    }
}
