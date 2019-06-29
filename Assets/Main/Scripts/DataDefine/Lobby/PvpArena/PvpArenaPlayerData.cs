using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 敌方PVP竞技玩家数据类。
    /// </summary>
    [Serializable]
    public class PvpArenaPlayerAndTeamData : IGenericData<PvpArenaPlayerAndTeamData, PBArenaPlayerAndTeamInfo>
    {
        [SerializeField]
        private int m_Rank = 0;
        [SerializeField]
        private PlayerData m_Player = new PlayerData();
        [SerializeField]
        private List<LobbyHeroData> m_HeroDatas = new List<LobbyHeroData>();
        [SerializeField]
        private int m_ServerId;
        [SerializeField]
        private string m_ServerName;
        [SerializeField]
        private int m_Index = 0;
        [SerializeField]
        private Vector2 m_Position = Vector2.zero;
        [SerializeField]
        private float m_Rotation = 0f;
        [SerializeField]
        private int m_CurrentHeroIndex = 0;
        public int CurrentHeroIndex { get { return m_CurrentHeroIndex; } set { m_CurrentHeroIndex = value; } }
        public Vector2 Position { get { return m_Position; } set { m_Position = value; } }
        public float Rotation { get { return m_Rotation; } set { m_Rotation = value; } }
        /// <summary>
        /// 是否处于掉线状态，1v1使用
        /// </summary>
        public bool DisConnected { set; get; }
        public int Rank
        {
            get { return m_Rank; }
        }
        public PlayerData Player
        {
            get { return m_Player; }
        }
        public List<LobbyHeroData> HeroDatas
        {
            get { return m_HeroDatas; }
        }

        public int Key
        {
            get { return Player.Key; }
        }

        public int ServerId { get { return m_ServerId; } }
        public string ServerName { get { return m_ServerName; } }

        public int Index { get { return m_Index; } set { m_Index = value; } }

        public int oppScore { get; private set; }

        public void UpdateData(PBArenaPlayerAndTeamInfo pb, int serverId, string serverName, int index, int oppScore)
        {
            m_Rank = pb.Rank;
            m_Player.UpdateData(pb.PlayerInfo);
            m_HeroDatas.Clear();
            this.oppScore = oppScore;
            for (int i = 0; i < pb.HeroTeam.Count; i++)
            {
                LobbyHeroData data = new LobbyHeroData();
                data.UpdateData(pb.HeroTeam[i]);
                m_HeroDatas.Add(data);
            }
            m_ServerId = serverId;
            m_ServerName = serverName;
            m_Index = index;
        }

        public void UpdateData(PBArenaPlayerAndTeamInfo data)
        {
            UpdateData(data, 0, "", 1, 0);
        }
    }
}
