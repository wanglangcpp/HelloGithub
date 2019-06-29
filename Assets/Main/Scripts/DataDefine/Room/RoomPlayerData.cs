using System;
using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class RoomPlayerData : IGenericData<RoomPlayerData, PBRoomPlayerInfo>
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private int m_Might;

        [SerializeField]
        private string m_Name;

        [SerializeField]
        private bool m_IsFemale;

        [SerializeField]
        private int m_PortraitType;

        [SerializeField]
        private int m_Level;

        [SerializeField]
        private int m_VipLevel;

        [SerializeField]
        private Vector2 m_RoomPosition;

        [SerializeField]
        private float m_RoomRotation;

        [SerializeField]
        private RoomHeroesData m_Heroes = new RoomHeroesData();

        [SerializeField]
        private int m_PvpScore = 0;

        [SerializeField]
        private int m_ServerId = 0;

        [SerializeField]
        private int m_InBattleEntityId = 0;

        public int Key { get { return m_Id; } }
        public int Id { get { return m_Id; } }
        public string Name { get { return m_Name; } }
        public bool IsFemale { get { return m_IsFemale; } }
        public int PortraitType { get { return m_PortraitType; } }
        public int Level { get { return m_Level; } }
        public int VipLevel { get { return m_VipLevel; } }
        public Vector2 RoomPosition { get { return m_RoomPosition; } }
        public RoomHeroesData Heroes { get { return m_Heroes; } }
        public float RoomRotation { get { return m_RoomRotation; } }
        public int Might { get { return m_Might; } }
        public int PvpScore { get { return m_PvpScore; } }
        public int ServerId { get { return m_ServerId; } }
        public int InBattleEntityId { get { return m_InBattleEntityId; } }

        public void UpdateData(PBRoomPlayerInfo data)
        {
            m_Id = data.PlayerInfo.Id;

            if (data.PlayerInfo.HasName)
            {
                m_Name = data.PlayerInfo.Name;
            }

            if (data.PlayerInfo.HasIsFemale)
            {
                m_IsFemale = data.PlayerInfo.IsFemale;
            }

            if (data.PlayerInfo.HasPortraitType)
            {
                m_PortraitType = data.PlayerInfo.PortraitType;
            }

            if (data.PlayerInfo.HasLevel)
            {
                m_Level = data.PlayerInfo.Level;
            }

            if (data.PlayerInfo.HasVipLevel)
            {
                m_VipLevel = data.PlayerInfo.VipLevel;
            }

            if (data.PlayerInfo.HasMight)
            {
                m_Might = data.PlayerInfo.Might;
            }

            if (data.HasInBattleEntity)
            {
                m_InBattleEntityId = data.InBattleEntity;
            }

            m_PvpScore = data.Score;

            m_ServerId = data.LobbyServerId;

            m_Heroes.ClearAndAddData(data.RoomHeroInfo);
        }

        public RoomHeroData GetHero(int heroType)
        {
            return m_Heroes.GetData(heroType);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Id: ");
            sb.Append(Id.ToString());
            sb.Append(", Name: ");
            sb.Append(Name);
            sb.Append(", Heroes: {");
            sb.Append(Heroes.ToString());
            sb.Append("}");
            return sb.ToString();
        }
    }
}
