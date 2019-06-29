﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 2004_LCGetChessEnemyInfo.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"LCGetChessEnemyInfo")]
    public partial class LCGetChessEnemyInfo : PacketBase
    {
        public LCGetChessEnemyInfo() { }

        public override PacketType PacketType { get { return PacketType.LobbyServerToClient; } }

        public override int PacketActionId { get { return 2004; } }

        private bool m_Success;
        [ProtoMember(1, Name = @"Success", IsRequired = true, DataFormat = DataFormat.Default)]
        public bool Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private PBPlayerInfo m_EnemyInfo;
        [ProtoMember(2, Name = @"EnemyInfo", IsRequired = true, DataFormat = DataFormat.Default)]
        public PBPlayerInfo EnemyInfo
        {
            get { return m_EnemyInfo; }
            set { m_EnemyInfo = value; }
        }

        private readonly List<PBLobbyHeroInfo> m_HeroesInfo = new List<PBLobbyHeroInfo>();
        [ProtoMember(3, Name = @"HeroesInfo", DataFormat = DataFormat.Default)]
        public List<PBLobbyHeroInfo> HeroesInfo
        {
            get { return m_HeroesInfo; }
        }

        private int m_Anger;
        [ProtoMember(4, Name = @"Anger", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int Anger
        {
            get { return m_Anger; }
            set { m_Anger = value; }
        }

        private readonly List<PBLobbyHeroStatus> m_HeroesStatus = new List<PBLobbyHeroStatus>();
        [ProtoMember(5, Name = @"HeroesStatus", DataFormat = DataFormat.Default)]
        public List<PBLobbyHeroStatus> HeroesStatus
        {
            get { return m_HeroesStatus; }
        }

        private int m_ChessFieldIndex;
        [ProtoMember(6, Name = @"ChessFieldIndex", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int ChessFieldIndex
        {
            get { return m_ChessFieldIndex; }
            set { m_ChessFieldIndex = value; }
        }
    }
}
