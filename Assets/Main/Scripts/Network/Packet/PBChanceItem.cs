﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: PBChanceItem.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"PBChanceItem")]
    public partial class PBChanceItem
    {
        public PBChanceItem() { }

        private PBItemInfo m_ItemInfo;
        [ProtoMember(1, Name = @"ItemInfo", IsRequired = true, DataFormat = DataFormat.Default)]
        public PBItemInfo ItemInfo
        {
            get { return m_ItemInfo; }
            set { m_ItemInfo = value; }
        }

        private int m_DummyIndex;
        [ProtoMember(2, Name = @"DummyIndex", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int DummyIndex
        {
            get { return m_DummyIndex; }
            set { m_DummyIndex = value; }
        }
    }
}
