﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: PBInstanceDrop.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"PBInstanceDrop")]
    public partial class PBInstanceDrop
    {
        public PBInstanceDrop() { }

        private readonly List<PBDropInfo> m_Items = new List<PBDropInfo>();
        [ProtoMember(1, Name = @"Items", DataFormat = DataFormat.Default)]
        public List<PBDropInfo> Items
        {
            get { return m_Items; }
        }
    }
}
