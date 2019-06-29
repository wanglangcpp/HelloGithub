using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技玩家数据类。
    /// </summary>
    [Serializable]
    public class OfflineArenaPlayerData : IGenericData<OfflineArenaPlayerData, PBArenaPlayerAndTeamInfo>
    {
        [SerializeField]
        private PlayerData m_Player = new PlayerData();

        public PlayerData Player
        {
            get { return m_Player; }
        }

        [SerializeField]
        private LobbyHeroesData m_Heroes = new LobbyHeroesData();

        public LobbyHeroesData Heroes
        {
            get { return m_Heroes; }
        }

        [SerializeField]
        private int m_Rank = 0;

        public int Rank
        {
            get { return m_Rank; }
        }

        public int Key
        {
            get { return Player.Key; }
        }

        public void UpdateData(OfflineArenaPlayerData pb)
        {
            m_Player = pb.Player;
            m_Heroes = pb.Heroes;
            m_Rank = pb.Rank;
        }

        public void UpdateData(PBArenaPlayerAndTeamInfo pb)
        {
            m_Rank = pb.Rank;
            m_Player.UpdateData(pb.PlayerInfo);

            // 若不满足条件则认为 pb 中没有关于英雄的信息。
            if (pb.HeroTeam.Count > 0)
            {
                m_Heroes.ClearAndAddData(pb.HeroTeam);
            }
        }
    }
}
