﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: PBServerConfigInfo.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"PBServerConfigInfo")]
    public partial class PBServerConfigInfo
    {
        public PBServerConfigInfo() { }

        private string m_Key;
        [ProtoMember(1, Name = @"Key", IsRequired = true, DataFormat = DataFormat.Default)]
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        private string m_Value;
        [ProtoMember(2, Name = @"Value", IsRequired = true, DataFormat = DataFormat.Default)]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
    }
}
