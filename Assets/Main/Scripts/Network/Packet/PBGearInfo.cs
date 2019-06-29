﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: PBGearInfo.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"PBGearInfo")]
    public partial class PBGearInfo
    {
        public PBGearInfo() { }

        private int m_Id;
        [ProtoMember(1, Name = @"Id", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        private int? m_Type;
        [ProtoMember(2, Name = @"Type", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int Type
        {
            get { return m_Type ?? default(int); }
            set { m_Type = value; }
        }
        public bool HasType { get { return m_Type != null; } }
        private void ResetType() { m_Type = null; }
        private bool ShouldSerializeType() { return HasType; }

        private int? m_Level;
        [ProtoMember(3, Name = @"Level", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int Level
        {
            get { return m_Level ?? default(int); }
            set { m_Level = value; }
        }
        public bool HasLevel { get { return m_Level != null; } }
        private void ResetLevel() { m_Level = null; }
        private bool ShouldSerializeLevel() { return HasLevel; }

        private int? m_StrengthenLevel;
        [ProtoMember(4, Name = @"StrengthenLevel", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int StrengthenLevel
        {
            get { return m_StrengthenLevel ?? default(int); }
            set { m_StrengthenLevel = value; }
        }
        public bool HasStrengthenLevel { get { return m_StrengthenLevel != null; } }
        private void ResetStrengthenLevel() { m_StrengthenLevel = null; }
        private bool ShouldSerializeStrengthenLevel() { return HasStrengthenLevel; }
    }
}
