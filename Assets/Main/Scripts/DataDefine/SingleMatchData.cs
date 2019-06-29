using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// pvp单人匹配，本方阵容数据
    /// </summary>
    [Serializable]
    public class SingleMatchData : IGenericData<SingleMatchData, PBArenaPlayerAndTeamInfo>
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private long m_StartTime;

        [SerializeField]
        private RoomStateType m_State;

        [SerializeField]
        private PlayerData m_Player = new PlayerData();
        [SerializeField]
        private bool m_RoomReconnect = false;

        [SerializeField]
        private int m_InstanceId = 0;

        //public RoomPlayerData GetPlayer(int playerId)
        //{
        //    return m_Players.GetData(playerId);
        //}
        [SerializeField]
        private int m_Rank = 0;
        [SerializeField]
        private List<LobbyHeroData> m_HeroDatas = new List<LobbyHeroData>();
        [SerializeField]
        private Vector2 m_Position = Vector2.zero;
        [SerializeField]
        private float m_Rotation = 0f;
        [SerializeField]
        private int m_CurrentHeroIndex = 0;
        [SerializeField]
        private int m_SceneId = 1;
        [SerializeField]
        private SinglePvpResultDisplayData displayData;
        public SinglePvpResultDisplayData DisplayData { get { return displayData; } }
        public int CurrentHeroIndex { get { return m_CurrentHeroIndex; } set { m_CurrentHeroIndex = value; } }
        public Vector2 Position { get { return m_Position; } set { m_Position = value; } }
        public float Rotation { get { return m_Rotation; } set { m_Rotation = value; } }

        public int Key { get { return m_Id; } }
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public bool InRoom { get { return m_Id > 0; } }
        public int Index { get { return m_InstanceId; } }

        public long StartTime
        {
            get
            {
                DateTime startTime = new DateTime(m_StartTime, DateTimeKind.Utc);
                DateTime now = GameEntry.Time.LobbyServerUtcTime;

                return (long)(now - startTime).TotalSeconds;
            }
        }

        public RoomStateType State
        {
            set
            {
                m_State = value;
            }
            get
            {
                return m_State;
            }
        }
        public PlayerData Player
        {
            get { return m_Player; }
        }
        public bool HasReconnected
        {
            get
            {
                return m_RoomReconnect;
            }
            set
            {
                m_RoomReconnect = value;
            }
        }

        public int Rank
        {
            get { return m_Rank; }
        }
        public List<LobbyHeroData> HeroDatas
        {
            get { return m_HeroDatas; }
        }

        public int SceneId { get { return m_SceneId; } }
        public int MyScore { get; private set; }
        public void ClearData()
        {
            HasReconnected = false;
            m_Id = 0;
            m_StartTime = 0L;
            m_State = RoomStateType.Finish;
            m_HeroDatas.Clear();
            displayData = null;
        }

        public void UpdateData(PBArenaPlayerAndTeamInfo data)
        {
            //m_Id = data.Id;

            //if (data.StartTime != null)
            //{
            //    m_StartTime = data.StartTime;
            //}

            //m_State = (RoomStateType)data.State;

            //m_Players.ClearAndAddData(data.RoomPlayerInfo);
        }

        public void UpdateData(PBArenaPlayerAndTeamInfo info, int index, int sceneId, int myScore)
        {
            m_RoomReconnect = true;
            m_Id = info.PlayerInfo.Id;
            m_HeroDatas.Clear();
            m_InstanceId = index;
            m_SceneId = sceneId;
            this.MyScore = myScore;
            for (int i = 0; i < info.HeroTeam.Count; i++)
            {
                LobbyHeroData data = new LobbyHeroData();
                data.UpdateData(info.HeroTeam[i]);
                m_HeroDatas.Add(data);
            }
            //m_Players.AddData(data.PlayerInfo);
        }

        //PBRoomPlayerInfo Convert2RoomPlayer(PBArenaPlayerAndTeamInfo arenaPlayerInfo)
        //{
        //    PBRoomPlayerInfo roomPlayerInfo = new PBRoomPlayerInfo();
        //    roomPlayerInfo.PlayerInfo = arenaPlayerInfo.PlayerInfo;
        //    roomPlayerInfo
        //}
        public void SetStartTime(long startTime)
        {
            m_StartTime = startTime;
        }
        public void SetDisplayData(SinglePvpResultDisplayData data)
        {
            displayData = data;
        }
        public void SetRank(int rank)
        {
            this.m_Rank = rank;
        }
        public void SetScore(int score)
        {
            this.MyScore = score;
        }
    }
}
