using System;
using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class RoomData : IGenericData<RoomData, PBRoomInfo>
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private long m_StartTime;

        [SerializeField]
        private RoomStateType m_State;

        [SerializeField]
        private RoomPlayersData m_Players = new RoomPlayersData();

        [SerializeField]
        private bool m_RoomReconnect = false;

        [SerializeField]
        private int m_InstanceId = 0;

        [SerializeField]
        private string m_Token = "";

        public int Key { get { return m_Id; } }
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public RoomPlayersData Players { get { return m_Players; } }
        public bool InRoom { get { return m_Id > 0; } }
        public int InstanceId { get { return m_InstanceId; } }

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
        /// <summary>
        /// 是否处在重连状态
        /// </summary>
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

        public string Token { get { return m_Token; } set { m_Token = value; } }

        public void ClearData()
        {
            HasReconnected = false;
            m_Id = 0;
            m_StartTime = 0L;
            m_State = RoomStateType.Finish;
            m_Players.ClearData();
        }

        public void UpdateData(PBRoomInfo data)
        {
            m_Id = data.Id;

            if (data.StartTime != null)
            {
                m_StartTime = data.StartTime;
            }

            m_State = (RoomStateType)data.State;

            m_Players.ClearAndAddData(data.RoomPlayerInfo);
        }

        public void UpdateData(PBPlayerPvpInfo data)
        {
            m_RoomReconnect = true;
            m_Id = data.RoomId;
            m_InstanceId = data.InstanceId;
        }
        public void UpdateData(PBArenaPlayerAndTeamInfo data,int id,string token)
        {
            m_RoomReconnect = true;
            m_Id = id;
            m_Token = token;
            m_Players.ClearData();
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

        public RoomPlayerData GetPlayer(int playerId)
        {
            return m_Players.GetData(playerId);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Id: ");
            sb.Append(Id.ToString());
            sb.Append(", State: ");
            sb.Append(State.ToString());
            sb.Append(", StartTime: ");
            sb.Append(StartTime.ToString());
            sb.Append(", PlayersData: {");
            sb.Append(Players.ToString());
            sb.Append("}");
            return sb.ToString();
        }
    }
}
