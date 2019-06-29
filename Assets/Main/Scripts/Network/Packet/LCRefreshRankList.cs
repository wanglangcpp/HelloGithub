﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 3401_LCRefreshRankList.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"LCRefreshRankList")]
    public partial class LCRefreshRankList : PacketBase
    {
        public LCRefreshRankList() { }

        public override PacketType PacketType { get { return PacketType.LobbyServerToClient; } }

        public override int PacketActionId { get { return 3401; } }

        private int m_RankType;
        [ProtoMember(1, Name = @"RankType", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int RankType
        {
            get { return m_RankType; }
            set { m_RankType = value; }
        }

        private readonly List<string> m_RankParams = new List<string>();
        [ProtoMember(2, Name = @"RankParams", DataFormat = DataFormat.Default)]
        public List<string> RankParams
        {
            get { return m_RankParams; }
        }

        private readonly List<PBRankInfo> m_RankInfo = new List<PBRankInfo>();
        [ProtoMember(3, Name = @"RankInfo", DataFormat = DataFormat.Default)]
        public List<PBRankInfo> RankInfo
        {
            get { return m_RankInfo; }
        }

        private int? m_MyRank;
        [ProtoMember(4, Name = @"MyRank", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int MyRank
        {
            get { return m_MyRank ?? default(int); }
            set { m_MyRank = value; }
        }
        public bool HasMyRank { get { return m_MyRank != null; } }
        private void ResetMyRank() { m_MyRank = null; }
        private bool ShouldSerializeMyRank() { return HasMyRank; }

        private int? m_MyScore;
        [ProtoMember(5, Name = @"MyScore", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int MyScore
        {
            get { return m_MyScore ?? default(int); }
            set { m_MyScore = value; }
        }
        public bool HasMyScore { get { return m_MyScore != null; } }
        private void ResetMyScore() { m_MyScore = null; }
        private bool ShouldSerializeMyScore() { return HasMyScore; }
    }
}
