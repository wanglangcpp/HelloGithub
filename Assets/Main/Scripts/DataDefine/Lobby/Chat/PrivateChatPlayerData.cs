using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class PrivateChatPlayerData
    {
        [SerializeField]
        public List<PrivateChatData> m_PrivateChatsData = new List<PrivateChatData>();

        [SerializeField]
        public PlayerChatData m_PlayerData = null;

        [SerializeField]
        public long m_LastTime = 0;

        public List<PrivateChatData> PrivateChatsData
        {
            get
            {
                return m_PrivateChatsData;
            }
        }

        public PlayerChatData PrivatePlayerData
        {
            get
            {
                return m_PlayerData;
            }
        }

        public int UnReadCount
        {
            get
            {
                int count = 0;
                for (int i = 0; m_PrivateChatsData != null && i < m_PrivateChatsData.Count; i++)
                {
                    if (!m_PrivateChatsData[i].IsRead)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public long LastTime
        {
            get
            {
                return m_LastTime;
            }
        }

        public void UpdatePlayerData(PlayerChatData data)
        {
            if (data == null)
            {
                return;
            }
            m_PlayerData = data;
            m_LastTime = GameEntry.Time.LobbyServerUtcTime.Ticks;
        }

        public void UpdateData(PrivateChatData data)
        {
            if (data == null)
            {
                return;
            }
            m_PlayerData = data.Sender == null || data.Sender.PlayerId <= 0 ? data.Receiver : data.Sender;
            data.IsMe = data.Sender == null;
            if (data.Sender == null)
            {
                data.UpdateSender(GameEntry.Data.Player);
            }
            AddPrivateChatData(data);
            m_LastTime = data.ChatTime.Ticks;
        }

        private void AddPrivateChatData(PrivateChatData data)
        {
            for (int i = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Chat.PrivateSaveMessageCountPerPlayer, 50); i < m_PrivateChatsData.Count;)
            {
                m_PrivateChatsData.Remove(m_PrivateChatsData[0]);
            }
            m_PrivateChatsData.Add(data);
        }
    }
}
