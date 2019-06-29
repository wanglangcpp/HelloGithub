﻿//----------------------------------------------------------------------------------------------------
// This code was auto generated by tools.
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// Proto source: 2002_CLOpenChessField.proto
//----------------------------------------------------------------------------------------------------

using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable, ProtoContract(Name = @"CLOpenChessField")]
    public partial class CLOpenChessField : PacketBase
    {
        public CLOpenChessField() { }

        public override PacketType PacketType { get { return PacketType.ClientToLobbyServer; } }

        public override int PacketActionId { get { return 2002; } }

        private int m_FieldId;
        [ProtoMember(1, Name = @"FieldId", IsRequired = true, DataFormat = DataFormat.TwosComplement)]
        public int FieldId
        {
            get { return m_FieldId; }
            set { m_FieldId = value; }
        }

        private int? m_EnemyAnger;
        [ProtoMember(2, Name = @"EnemyAnger", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int EnemyAnger
        {
            get { return m_EnemyAnger ?? default(int); }
            set { m_EnemyAnger = value; }
        }
        public bool HasEnemyAnger { get { return m_EnemyAnger != null; } }
        private void ResetEnemyAnger() { m_EnemyAnger = null; }
        private bool ShouldSerializeEnemyAnger() { return HasEnemyAnger; }

        private readonly List<int> m_EnemyHeroesHP = new List<int>();
        [ProtoMember(3, Name = @"EnemyHeroesHP", DataFormat = DataFormat.TwosComplement)]
        public List<int> EnemyHeroesHP
        {
            get { return m_EnemyHeroesHP; }
        }

        private readonly List<PBLobbyHeroStatus> m_MyHeroesStatus = new List<PBLobbyHeroStatus>();
        [ProtoMember(4, Name = @"MyHeroesStatus", DataFormat = DataFormat.Default)]
        public List<PBLobbyHeroStatus> MyHeroesStatus
        {
            get { return m_MyHeroesStatus; }
        }

        private int? m_MyAnger;
        [ProtoMember(5, Name = @"MyAnger", IsRequired = false, DataFormat = DataFormat.TwosComplement)]
        public int MyAnger
        {
            get { return m_MyAnger ?? default(int); }
            set { m_MyAnger = value; }
        }
        public bool HasMyAnger { get { return m_MyAnger != null; } }
        private void ResetMyAnger() { m_MyAnger = null; }
        private bool ShouldSerializeMyAnger() { return HasMyAnger; }

        private readonly List<PBChessField> m_ModifiedChessField = new List<PBChessField>();
        [ProtoMember(6, Name = @"ModifiedChessField", DataFormat = DataFormat.Default)]
        public List<PBChessField> ModifiedChessField
        {
            get { return m_ModifiedChessField; }
        }

        private bool? m_BattleWon;
        [ProtoMember(7, Name = @"BattleWon", IsRequired = false, DataFormat = DataFormat.Default)]
        public bool BattleWon
        {
            get { return m_BattleWon ?? default(bool); }
            set { m_BattleWon = value; }
        }
        public bool HasBattleWon { get { return m_BattleWon != null; } }
        private void ResetBattleWon() { m_BattleWon = null; }
        private bool ShouldSerializeBattleWon() { return HasBattleWon; }
    }
}
