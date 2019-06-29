﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 4102_RCPushEntityPerformSkillEnd.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"RCPushEntityPerformSkillEnd")]
    public partial class RCPushEntityPerformSkillEnd : PacketBase
    {
        public RCPushEntityPerformSkillEnd() { }

        public override PacketType PacketType { get { return PacketType.RoomServerToClient; } }

        public override int PacketActionId { get { return 4102; } }

        private int m_PlayerId;
        [ProtoMember(1, Name = @"PlayerId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int PlayerId
        {
            get { return m_PlayerId; }
            set { m_PlayerId = value; }
        }

        private int m_EntityId;
        [ProtoMember(2, Name = @"EntityId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int EntityId
        {
            get { return m_EntityId; }
            set { m_EntityId = value; }
        }

        private PBTransformInfo m_Transform;
        [ProtoMember(3, Name = @"Transform", IsRequired = true, DataFormat = DataFormat.Default)]
        public PBTransformInfo Transform
        {
            get { return m_Transform; }
            set { m_Transform = value; }
        }

        private int m_SkillId;
        [ProtoMember(4, Name = @"SkillId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int SkillId
        {
            get { return m_SkillId; }
            set { m_SkillId = value; }
        }

        private int m_SkillIndex;
        [ProtoMember(5, Name = @"SkillIndex", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int SkillIndex
        {
            get { return m_SkillIndex; }
            set { m_SkillIndex = value; }
        }

        private int m_Reason;
        [ProtoMember(6, Name = @"Reason", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int Reason
        {
            get { return m_Reason; }
            set { m_Reason = value; }
        }
    }
}
