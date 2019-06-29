using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class ChatsData
    {
        private static string LocalSettingPrivateChatData = "LocalSettingPrivateChatData_";

        private static string LocalSettingChatBlackListData = "LocalSettingChatBlackListData_";

        public static int MaxWorldChatCount = 50;

        [Serializable]
        public class PrivateChatList
        {
            public List<PrivateChatPlayerData> PrivateList = new List<PrivateChatPlayerData>();
        }
        /// <summary>
        /// 私人聊天信息
        /// </summary>
        private PrivateChatList m_PrivateChatsList = new PrivateChatList();

        /// <summary>
        /// 世界聊天信息
        /// </summary>
        private List<BaseChatData> m_WorldChatList = new List<BaseChatData>();

        /// <summary>
        /// 需要在主界面滚动的系统消息
        /// </summary>
        private List<SystemChatData> m_SystemBroadcastMsgList = new List<SystemChatData>();

        /// <summary>
        /// 系统消息
        /// </summary>
        private List<SystemChatData> m_SystemMsgList = new List<SystemChatData>();

        [Serializable]
        public class ChatBlackList
        {
            public List<PlayerChatData> BlackList = new List<PlayerChatData>();
        }

        /// <summary>
        /// 聊天黑名单
        /// </summar>
        private ChatBlackList m_ChatBlackList = new ChatBlackList();

        [SerializeField]
        private float m_LastSendWorldChatTime = 0;// DateTime.Today;

        public List<PlayerChatData> BlackList
        {
            get
            {
                return m_ChatBlackList.BlackList;
            }
        }

        public List<PrivateChatPlayerData> PrivateChatsData
        {
            get
            {
                return m_PrivateChatsList.PrivateList;
            }
        }

        public void SetPrivatePlayerOnlineStatus(List<PBOnlineStatus> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                var player = GetPrivateChatPlayer(players[i].PlayerId);
                if (player == null)
                {
                    continue;
                }
                else
                {
                    player.m_PlayerData.IsOnline = players[i].IsOnline;
                }
            }
            m_PrivateChatsList.PrivateList.Sort(PrivateChatPlayerComparer);
        }

        public bool IsPrivateChatHasPlayer(int playerId)
        {
            for (int i = 0; i < m_PrivateChatsList.PrivateList.Count; i++)
            {
                if (m_PrivateChatsList.PrivateList[i].PrivatePlayerData.PlayerId == playerId)
                {
                    return true;
                }
            }
            return false;
        }

        public PrivateChatPlayerData GetPrivateChatPlayer(int playerId)
        {
            for (int i = 0; i < m_PrivateChatsList.PrivateList.Count; i++)
            {
                if (m_PrivateChatsList.PrivateList[i].PrivatePlayerData.PlayerId == playerId)
                {
                    return m_PrivateChatsList.PrivateList[i];
                }
            }
            return null;
        }

        public void AddPrivateChatPlayer(PrivateChatPlayerData data)
        {
            if (IsPrivateChatHasPlayer(data.PrivatePlayerData.PlayerId))
            {
                return;
            }
            m_PrivateChatsList.PrivateList.Add(data);
            m_PrivateChatsList.PrivateList.Sort(PrivateChatPlayerComparer);
            for (int i = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Chat.PrivateSavePlayerCount, 20); i < m_PrivateChatsList.PrivateList.Count;)
            {
                m_PrivateChatsList.PrivateList.Remove(m_PrivateChatsList.PrivateList[m_PrivateChatsList.PrivateList.Count - 1]);
            }
            WriteLocalPrivateChatData();
        }

        public static int PrivateChatPlayerComparer(PrivateChatPlayerData a, PrivateChatPlayerData b)
        {
            if (a.PrivatePlayerData.IsOnline && !b.PrivatePlayerData.IsOnline)
            {
                return -1;
            }

            if (!a.PrivatePlayerData.IsOnline && b.PrivatePlayerData.IsOnline)
            {
                return 1;
            }

            return b.LastTime.CompareTo(a.LastTime);
        }

        public void RemovePrivateChatPlayer(int playerId)
        {
            for (int i = 0; i < m_PrivateChatsList.PrivateList.Count; i++)
            {
                if (m_PrivateChatsList.PrivateList[i].PrivatePlayerData.PlayerId == playerId)
                {
                    m_PrivateChatsList.PrivateList.Remove(m_PrivateChatsList.PrivateList[i]);
                    break;
                }
            }
            WriteLocalPrivateChatData();
        }

        public bool IsChatBlackList(int playerId)
        {
            for (int i = 0; i < m_ChatBlackList.BlackList.Count; i++)
            {
                if (m_ChatBlackList.BlackList[i].PlayerId == playerId)
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveChatBlackList(int playerId)
        {
            for (int i = 0; i < m_ChatBlackList.BlackList.Count; i++)
            {
                if (m_ChatBlackList.BlackList[i].PlayerId == playerId)
                {
                    UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_CHAT_REMOVE_SHIELD", m_ChatBlackList.BlackList[i].PlayerName));
                    m_ChatBlackList.BlackList.Remove(m_ChatBlackList.BlackList[i]);
                    break;
                }
            }

            WriteLocalChatBlackListData();
        }

        public void RemoveChatBlackList(PlayerChatData playerData)
        {
            RemoveChatBlackList(playerData.PlayerId);
        }

        public void AddChatBlackList(PlayerChatData playerData)
        {
            if (m_ChatBlackList.BlackList.Count >= GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Chat.MaxBlackListCount, 20))
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_CHAT_SHIELDING_QUANTITY_LIMIT"));
                return;
            }
            if (m_ChatBlackList.BlackList.Contains(playerData))
            {
                return;
            }
            m_ChatBlackList.BlackList.Add(playerData);
            RemovePrivateChatPlayer(playerData.PlayerId);
            for (int i = 0; i < m_WorldChatList.Count;)
            {
                var player = m_WorldChatList[i].Sender == null ? m_WorldChatList[i].Receiver : m_WorldChatList[i].Sender;
                if (player == null || player.PlayerId != playerData.PlayerId)
                {
                    i++;
                    continue;
                }
                m_WorldChatList.Remove(m_WorldChatList[i]);
            }
            WriteLocalChatBlackListData();
            GameEntry.Event.Fire(this, new AddBlackListEventArgs());
        }

        public float LastSendWorldChatTime
        {
            set
            {
                m_LastSendWorldChatTime = value;
            }
        }

        public List<BaseChatData> WorldChatList
        {
            get
            {
                return m_WorldChatList;
            }
        }

        public int WorldUnreadCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < m_WorldChatList.Count; i++)
                {
                    if (!m_WorldChatList[i].IsRead)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public int GetPrivatePlayerUnreadChatCount(int playerId)
        {
            if (GetPrivateChatPlayer(playerId) == null)
            {
                return 0;
            }
            int count = 0;
            var chatList = GetPrivateChatPlayer(playerId).PrivateChatsData;
            for (int i = 0; chatList != null && i < chatList.Count; i++)
            {
                if (!chatList[i].IsRead)
                {
                    count++;
                }
            }
            return count;
        }

        public float LastSendWordChatDuration
        {
            get
            {
                //TimeSpan duration = GameEntry.Time.LobbyServerUtcTime.Subtract(m_LastSendWorldChatTime);
                //return duration.TotalSeconds;
                return Time.time - m_LastSendWorldChatTime;
            }
        }

        /// <summary>
        /// 获取主界面系统消息的广播消息
        /// </summary>
        public SystemChatData ChatSystemBroadCastMsg
        {
            get
            {
                SystemChatData chatData = null;
                if (m_SystemBroadcastMsgList.Count <= 0)
                {
                    return chatData;
                }

                m_SystemBroadcastMsgList.Sort(SystemChatComparer);
                if (m_SystemBroadcastMsgList.Count > Constant.SystemChatMaxBroadcastNum)
                {
                    m_SystemBroadcastMsgList.RemoveRange(Constant.SystemChatMaxBroadcastNum - 1, m_SystemBroadcastMsgList.Count - Constant.SystemChatMaxBroadcastNum);
                }
                chatData = m_SystemBroadcastMsgList[0];
                m_SystemBroadcastMsgList.Remove(m_SystemBroadcastMsgList[0]);
                return chatData;
            }
        }
        public List<SystemChatData> ChatSystemMsgList
        {
            get
            {
                return m_SystemMsgList;
            }
        }


        public static int SystemChatComparer(SystemChatData a, SystemChatData b)
        {
            return a.Priority.CompareTo(b.Priority);
        }

        public void UpdateData(LCSendChat packet)
        {
            BaseChatData data = null;
            switch ((ChatType)packet.Channel)
            {
                case ChatType.Private:
                    PrivateChatData privateChat = new PrivateChatData();
                    privateChat.UpdateData(packet);
                    data = privateChat;
                    break;
                case ChatType.World:
                    WorldChatData worldChat = new WorldChatData();
                    worldChat.UpdateData(packet);
                    data = worldChat;
                    break;
                default:
                    break;
            }
            AddChatListItem(data);
        }

        public void UpdateData(LCReceiveChat packet)
        {
            BaseChatData data = null;
            switch ((ChatType)packet.Channel)
            {
                case ChatType.Private:
                    PrivateChatData privateChat = new PrivateChatData();
                    privateChat.UpdateData(packet);
                    data = privateChat;
                    break;
                case ChatType.World:
                    if (packet.Sender != null)
                    {
                        WorldChatData worldChat = new WorldChatData();
                        worldChat.UpdateData(packet);
                        data = worldChat;
                    }
                    else
                    {
                        SystemChatData systemChat = new SystemChatData();
                        systemChat.UpdateData(packet);
                        data = systemChat;
                    }
                    break;
                default:
                    Log.Error("packet's channel is false ,ChatsData UpdateData, channel is {0}.", packet.Channel);
                    break;
            }
            AddChatListItem(data);
        }

        public void UpdateData(LCPushSystemAnnouncement packet)
        {
            SystemChatData systemChat = new SystemChatData();
            systemChat.UpdateData(packet);
            AddChatListItem(systemChat);
        }

        public void UpdateData(LCSystemNotify packet)
        {
            SystemChatData systemChat = new SystemChatData();
            systemChat.UpdateData(packet);
            AddChatListItem(systemChat);
        }

        public bool PrivateChatRequestsReminder
        {
            get
            {
                for (int i = 0; i < m_PrivateChatsList.PrivateList.Count; i++)
                {
                    if (m_PrivateChatsList.PrivateList[i].UnReadCount > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool HasSystemBroadCastMsg
        {
            get
            {
                return m_SystemBroadcastMsgList.Count > 0;
            }
        }

        public void ChatPrepare()
        {
            ReadLocalPrivateChatData();
            ReadLocalChatBlackListData();
        }

        private void ReadLocalPrivateChatData()
        {
            m_PrivateChatsList.PrivateList.Clear();
            string key = LocalSettingPrivateChatData + GameEntry.Data.Player.Id + GameEntry.Data.Account.ServerData.Id;
            PrivateChatList privateChat = GameEntry.Setting.GetObject<PrivateChatList>(key);
            if (privateChat == null)
            {
                return;
            }
            m_PrivateChatsList.PrivateList.AddRange(privateChat.PrivateList);
        }

        public void WriteLocalPrivateChatData()
        {
            string key = LocalSettingPrivateChatData + GameEntry.Data.Player.Id + GameEntry.Data.Account.ServerData.Id;
            GameEntry.Setting.SetObject(key, m_PrivateChatsList);
        }

        private void ReadLocalChatBlackListData()
        {
            m_ChatBlackList.BlackList.Clear();
            string key = LocalSettingChatBlackListData + GameEntry.Data.Player.Id + GameEntry.Data.Account.ServerData.Id;
            ChatBlackList blackPlayers = GameEntry.Setting.GetObject<ChatBlackList>(key);
            if (blackPlayers == null)
            {
                return;
            }
            for (int i = 0; i < blackPlayers.BlackList.Count; i++)
            {
                m_ChatBlackList.BlackList.Add(blackPlayers.BlackList[i]);
            }
        }

        private void WriteLocalChatBlackListData()
        {
            string key = LocalSettingChatBlackListData + GameEntry.Data.Player.Id + GameEntry.Data.Account.ServerData.Id;
            GameEntry.Setting.SetObject(key, m_ChatBlackList);
        }

        private void AddChatListItem(BaseChatData chatData)
        {
            if (chatData == null)
            {
                Log.Warning("BaseChatData is invalid!");
            }

            for (int i = 0; i < m_ChatBlackList.BlackList.Count; i++)
            {
                if ((chatData.Receiver != null && chatData.Receiver.PlayerId == m_ChatBlackList.BlackList[i].PlayerId) ||
                    (chatData.Sender != null && chatData.Sender.PlayerId == m_ChatBlackList.BlackList[i].PlayerId))
                {
                    return;
                }
            }

            switch (chatData.Type)
            {
                case ChatType.Private:
                    var privateChatData = chatData as PrivateChatData;
                    int playerId = privateChatData.Sender == null ? privateChatData.Receiver.PlayerId : privateChatData.Sender.PlayerId;
                    if (IsPrivateChatHasPlayer(playerId))
                    {
                        GetPrivateChatPlayer(playerId).UpdateData(privateChatData);
                    }
                    else
                    {
                        PrivateChatPlayerData chatPlayerData = new PrivateChatPlayerData();
                        chatPlayerData.UpdateData(privateChatData);
                        m_PrivateChatsList.PrivateList.Add(chatPlayerData);
                    }
                    m_PrivateChatsList.PrivateList.Sort(PrivateChatPlayerComparer);
                    for (int i = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Chat.PrivateSavePlayerCount, 20); i < m_PrivateChatsList.PrivateList.Count;)
                    {
                        m_PrivateChatsList.PrivateList.Remove(m_PrivateChatsList.PrivateList[m_PrivateChatsList.PrivateList.Count - 1]);
                    }
                    WriteLocalPrivateChatData();
                    break;
                case ChatType.System:
                    var systemChatData = chatData as SystemChatData;
                    if (systemChatData.NeedScroll || systemChatData.SystemNotifyType != 4)
                    {
                        m_SystemBroadcastMsgList.Add(systemChatData);
                    }
                    if (m_WorldChatList.Count >= MaxWorldChatCount)
                    {
                        m_WorldChatList.Remove(m_WorldChatList[0]);
                    }
                    m_WorldChatList.Add(chatData);
                    m_SystemMsgList.Add(systemChatData);
                    break;
                case ChatType.World:
                    if (m_WorldChatList.Count >= MaxWorldChatCount)
                    {
                        m_WorldChatList.Remove(m_WorldChatList[0]);
                    }
                    m_WorldChatList.Add(chatData);
                    break;
                default:
                    break;
            }
        }
    }
}
