using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class ChessBattleEnemyData
    {
        [SerializeField]
        private PlayerData m_Player = null;

        public PlayerData Player { get { return m_Player; } }

        [SerializeField]
        private ChessBattleHeroesStatus m_HeroesStatus = null;

        public ChessBattleHeroesStatus HeroesStatus { get { return m_HeroesStatus; } }

        [SerializeField]
        private LobbyHeroesData m_HeroesData = null;

        public LobbyHeroesData HeroesData { get { return m_HeroesData; } }

        [SerializeField]
        private int m_Anger = 0;

        public int Anger { get { return m_Anger; } }

        [SerializeField]
        private int m_ChessFieldIndex = -1;

        public int ChessFieldIndex { get { return m_ChessFieldIndex; } }

        public void UpdateData(LCGetChessEnemyInfo packet)
        {
            Player.UpdateData(packet.EnemyInfo);
            HeroesData.ClearAndAddData(packet.HeroesInfo);
            HeroesStatus.ClearAndAddData(packet.HeroesStatus);
            m_Anger = packet.Anger;
            m_ChessFieldIndex = packet.ChessFieldIndex;
        }
    }
}
