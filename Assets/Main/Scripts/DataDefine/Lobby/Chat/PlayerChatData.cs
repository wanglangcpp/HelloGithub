using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class PlayerChatData
    {
        public int m_PlayerId = 0;

        public int m_Level = 0;

        public int m_PortraitType = 0;

        public int m_VipLevel = 0;

        public string m_PlayerName = string.Empty;

        public bool m_IsOnline = true;

        public int m_Might = 0;

        public int PlayerId { get { return m_PlayerId; } }
        public int Level { get { return m_Level; } }
        public int PortraitType { get { return m_PortraitType; } }
        public int VipLevel { get { return m_VipLevel; } }
        public string PlayerName { get { return m_PlayerName; } }
        public int Might { get { return m_Might; } }

        public bool IsOnline
        {
            get { return m_IsOnline; }
            set { m_IsOnline = value; }
        }        

        public void UpdateData(PlayerData data)
        {
            m_PlayerId = data.Id;
            m_PlayerName = data.Name;
            m_Level = data.Level;
            m_PortraitType = data.PortraitType;
            m_VipLevel = data.VipLevel;
            m_IsOnline = data.IsOnline;
        }

        public void UpdateData(PBPlayerInfo data)
        {
            m_PlayerId = data.Id;

            if (data.HasName)
            {
                m_PlayerName = data.Name;
            }

            if (data.HasLevel)
            {
                m_Level = data.Level;
            }

            if (data.HasPortraitType)
            {
                m_PortraitType = data.PortraitType;
            }

            if (data.HasVipLevel)
            {
                m_VipLevel = data.VipLevel;
            }

            if (data.HasIsOnline)
            {
                m_IsOnline = data.IsOnline;
            }

            if (data.HasMight)
            {
                m_Might = data.Might;
            }
        }
    }
}
