﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: PBMonster.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"PBMonster")]
    public partial class PBMonster
    {
        public PBMonster() { }

        private int m_MonsterId;
        [ProtoMember(1, Name = @"MonsterId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int MonsterId
        {
            get { return m_MonsterId; }
            set { m_MonsterId = value; }
        }

        private int m_MonsterCount;
        [ProtoMember(2, Name = @"MonsterCount", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int MonsterCount
        {
            get { return m_MonsterCount; }
            set { m_MonsterCount = value; }
        }
    }
}
