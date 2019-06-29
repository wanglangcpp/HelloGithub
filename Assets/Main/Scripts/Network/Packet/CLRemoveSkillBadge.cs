﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 1407_CLRemoveSkillBadge.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CLRemoveSkillBadge")]
    public partial class CLRemoveSkillBadge : PacketBase
    {
        public CLRemoveSkillBadge() { }

        public override PacketType PacketType { get { return PacketType.ClientToLobbyServer; } }

        public override int PacketActionId { get { return 1407; } }

        private int m_HeroId;
        [ProtoMember(1, Name = @"HeroId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int HeroId
        {
            get { return m_HeroId; }
            set { m_HeroId = value; }
        }

        private int m_SkillIndex;
        [ProtoMember(2, Name = @"SkillIndex", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int SkillIndex
        {
            get { return m_SkillIndex; }
            set { m_SkillIndex = value; }
        }

        private int m_BadgeSlotCategory;
        [ProtoMember(3, Name = @"BadgeSlotCategory", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int BadgeSlotCategory
        {
            get { return m_BadgeSlotCategory; }
            set { m_BadgeSlotCategory = value; }
        }

        private int? m_GenericBadgeSlotIndex;
        [ProtoMember(4, Name = @"GenericBadgeSlotIndex", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int GenericBadgeSlotIndex
        {
            get { return m_GenericBadgeSlotIndex ?? default(int); }
            set { m_GenericBadgeSlotIndex = value; }
        }
        public bool HasGenericBadgeSlotIndex { get { return m_GenericBadgeSlotIndex != null; } }
        private void ResetGenericBadgeSlotIndex() { m_GenericBadgeSlotIndex = null; }
        private bool ShouldSerializeGenericBadgeSlotIndex() { return HasGenericBadgeSlotIndex; }
    }
}
