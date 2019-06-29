using GameFramework;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ChatForm : NGUIForm
    {
        [SerializeField]
        private UIToggle[] m_Tabs = null;

        [SerializeField]
        private ChatInput m_ChatInput = null;

        [SerializeField]
        private ChatBlackListSubForm m_ChatBlackListTemplate = null;

        [SerializeField]
        private UILabel m_TipsMessage = null;

        [SerializeField]
        private GameObject m_SendOutObj = null;

        private ChatBlackListSubForm m_UsingChatBlackListSubForm = null;
        private Dictionary<ChatType, GameFrameworkFunc<bool, bool>> m_RefreshFuncs = new Dictionary<ChatType, GameFrameworkFunc<bool, bool>>();
        private ChatType m_CurrentChatType = ChatType.None;
        private ChatDisplayData m_ChatFormData = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.GetChat, OnGetChatDataChanged);
            GameEntry.Event.Subscribe(EventId.SendChat, OnSendChatDataReturn);
            GameEntry.Event.Subscribe(EventId.GetPlayerOnlineStatus, OnGetPlayerListOnline);
            GameEntry.Event.Subscribe(EventId.AddBlackList, OnAddBlackList);
            GameEntry.Event.Subscribe(EventId.GetSystemMsg, OnGetSystemMsg);
            m_WorldListScrollView.onDragFinished += OnWorldChatDragFinished;
            m_PrivateListScrollView.onDragFinished += OnPrivateChatDragFinished;
            m_SystemListScrollView.onDragFinished += OnSystemChatDragFinished;
            GetPlayerListOnline();
            InitRefreshFuncData();
            m_WorldChatAmount = 1;
            m_PrivateChatAmount = 1;
            m_ChatCoolDownTime = GameEntry.ServerConfig.GetFloat(Constant.ServerConfig.Chat.SendWorldMessageInterval, 10);
            m_ChatFormData = userData as ChatDisplayData;
            if (m_ChatFormData == null)
            {
                if (!m_Tabs[(int)ChatType.World].value)
                {
                    m_Tabs[(int)ChatType.World].value = true;
                }
                else
                {
                    RefreshData(true);
                }
            }
            else if (m_ChatFormData.ChatType == ChatType.Private)
            {
                RefreshReminder();
                SwitchPrivateChat(m_ChatFormData.ChatPlayer);
            }
            else
            {
                RefreshReminder();
                TabValueChanged(true, (int)m_ChatFormData.ChatType);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            RefreshData(true);
        }

        protected override void OnClose(object userData)
        {
            m_WorldListScrollView.onDragFinished -= OnWorldChatDragFinished;
            m_PrivateListScrollView.onDragFinished -= OnPrivateChatDragFinished;
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.GetChat, OnGetChatDataChanged);
            GameEntry.Event.Unsubscribe(EventId.SendChat, OnSendChatDataReturn);
            GameEntry.Event.Unsubscribe(EventId.GetPlayerOnlineStatus, OnGetPlayerListOnline);
            GameEntry.Event.Unsubscribe(EventId.AddBlackList, OnAddBlackList);
            GameEntry.Event.Unsubscribe(EventId.GetSystemMsg, OnGetSystemMsg);
            m_PrivatePlayer.DestroyAllItems();
            DestroyAllChatLines();
            m_RefreshFuncs.Clear();
            base.OnClose(userData);
        }

        private void OnGetSystemMsg(object sender, GameEventArgs e)
        {
            var request = e as GetSystemMsgArgs;
            if (!string.IsNullOrEmpty(request.Sender.context))
            {
                RefreshData(false);
            }
        }

        private void GetPlayerListOnline()
        {
            List<int> playerIds = new List<int>();
            var data = GameEntry.Data.Chat.PrivateChatsData;
            for (int i = 0; i < data.Count; i++)
            {
                playerIds.Add(data[i].PrivatePlayerData.m_PlayerId);

            }
            GameEntry.LobbyLogic.GetPlayerOnlineStatus(playerIds);
        }

        private void OnGetPlayerListOnline(object sender, GameEventArgs e)
        {
            var playerOnLineList = (e as GetPlayerOnlineStatusEventArgs).OnlineStatus;
            GameEntry.Data.Chat.SetPrivatePlayerOnlineStatus(playerOnLineList);
            if (m_CurrentChatType == ChatType.Private)
            {
                RefreshPrivateChat(true);
            }
        }

        private void OnAddBlackList(object sender, GameEventArgs e)
        {
            RefreshData(false);
        }

        private void SwitchPrivateChat(PlayerData player)
        {
            PlayerChatData playerChat = new PlayerChatData();
            playerChat.UpdateData(player);
            PrivateChatPlayerData chatPlayerData = new PrivateChatPlayerData();
            chatPlayerData.UpdatePlayerData(playerChat);
            GameEntry.Data.Chat.AddPrivateChatPlayer(chatPlayerData);
            m_CurrPrivatePlayerData = playerChat;

            if (!m_Tabs[(int)ChatType.Private].value)
            {
                m_Tabs[(int)ChatType.Private].value = true;
                TabValueChanged(true, (int)ChatType.Private);
            }
            else
            {
                RefreshPrivateChat(true);
            }
        }

        private void OnGetChatDataChanged(object sender, GameEventArgs e)
        {
            if (m_WorldChatAmount < ChatAmountTh)
            {
                m_WorldChatUnreadCount++;
            }
            if (m_PrivateChatAmount < ChatAmountTh)
            {
                m_PrivateChatUnreadCount++;
            }
            LCReceiveChat msg = (e as GetChatEventArgs).Msg;
            if (msg == null)
            {
                return;
            }
            if ((int)m_CurrentChatType != msg.Channel || (m_CurrentChatType == ChatType.Private && (m_CurrPrivatePlayerData == null || m_CurrPrivatePlayerData.PlayerId != msg.Sender.Id)))
            {
                return;
            }

            RefreshData(false);
        }

        private void SetTipsMessage(int count)
        {
            m_TipsMessage.gameObject.SetActive(count > 0);
            m_TipsMessage.text = GameEntry.Localization.GetString("UI_TEXT_CHAT_UNREAD_MASSAGE", count.ToString());
        }

        public void OnClickTipsMessage()
        {
            if (m_CurrentChatType == ChatType.World)
            {
                SetWorldChatDragAmount();
            }
            else if (m_CurrentChatType == ChatType.Private)
            {
                SetPrivateChatDragAmount();
            }
        }

        private void OnSendChatDataReturn(object sender, GameEventArgs e)
        {
            RefreshData(false);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            UpdateWorldChat();
        }

        private void InitRefreshFuncData()
        {
            m_RefreshFuncs.Add(ChatType.Private, RefreshPrivateChat);
            m_RefreshFuncs.Add(ChatType.World, RefreshWorldChat);
            m_RefreshFuncs.Add(ChatType.System, RefreshSystemChat);
        }

        private void RefreshData(bool waitAFrame)
        {
            for (int i = 0; i < m_Tabs.Length; i++)
            {
                if (m_Tabs[i].value)
                {
                    TabValueChanged(true, m_Tabs[i].GetComponent<UIIntKey>().Key, waitAFrame);
                    break;
                }
            }
        }

        public void TabValueChanged(bool value, int type, bool waitAFrame = true)
        {
            if (!value)
            {
                return;
            }
            m_CurrentChatType = (ChatType)type;
            m_SendOutObj.SetActive(m_CurrentChatType != ChatType.System);
            if (!m_RefreshFuncs[(ChatType)type](waitAFrame))
            {
                Log.Warning("Refresh TabValue Function Error,Tab Type is {0}.", ((ChatType)type).ToString());
                return;
            }
        }

        private void DestroyAllChatLines()
        {
            DestroyChatLines(m_WorldChatLineList);
            DestroyChatLines(m_WorldSelfChatLineList);
            DestroyChatLines(m_PrivateChatLineList);
            DestroyChatLines(m_PrivateSelfChatLineList);
        }

        private void DestroyChatLines(List<ChatLineItem> itemList)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Destroy(itemList[i].gameObject);
            }
            itemList.Clear();
        }

        private void HideAllChatLines()
        {
            ResetSendButton();
            m_ChatInput.ClearChatInput();
            HideChatLines(m_WorldChatLineList);
            HideChatLines(m_WorldSelfChatLineList);
            HideChatLines(m_PrivateChatLineList);
            HideChatLines(m_PrivateSelfChatLineList);
            HideChatLines(m_SystemChatLineList);
        }

        private void HideChatLines(List<ChatLineItem> itemList)
        {
            for (int i = 0; i < itemList.Count;)
            {
                if (itemList[i] == null || itemList[i].gameObject == null)
                {
                    itemList.Remove(itemList[i]);
                }
                else
                {
                    itemList[i].ClearData();
                    itemList[i].gameObject.SetActive(false);
                    i++;
                }
            }
        }

        private bool IsChatLineHeightOverScrollView(UIScrollView scrollView, List<ChatLineItem> chatLines, List<ChatLineItem> selfChatLines)
        {
            bool isOver = false;
            List<ChatLineItem> dataList = new List<ChatLineItem>();
            if (chatLines != null)
            {
                dataList.AddRange(chatLines);
            }
            if (selfChatLines != null)
            {
                dataList.AddRange(selfChatLines);
            }
            float height = 0;
            for (int i = 0; i < dataList.Count; i++)
            {
                var bounds = NGUIMath.CalculateRelativeWidgetBounds(dataList[i].transform);
                height += bounds.size.y;
            }

            if (scrollView.panel != null && height >= scrollView.panel.height)
            {
                isOver = true;
            }
            return isOver;
        }

        public void OnClickSendOutBtn()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }

            if (m_CurrentChatType == ChatType.World && GameEntry.Data.Player.Level < GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Chat.UnlockWorldChannelLevel, 10))
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_CHAT_INPUT_NOT_ENOUGH_LEVEL") });
                return;
            }

            if (!m_CanWorldChatSendOut && m_CurrentChatType == ChatType.World)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_CHAT_NOTICE_FREQUENT") });
                return;
            }
            string msg = m_ChatInput.GetInputValue();
            msg = msg.Trim();
            if (msg == string.Empty)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_CHAT_NOTICE_EMPTYMESSAGE") });
                return;
            }

            if (m_CurrentChatType == ChatType.Private && m_ChatInput.PrivateChatInputPlayer == null)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_PERSONALCHAT_OBJECT") });
                return;
            }

            if (m_CurrentChatType == ChatType.World)
            {
                SetWorldChatDragAmount();
                m_CanWorldChatSendOut = false;
            }
            int receiverPlayerId = 0;
            if (m_CurrentChatType == ChatType.Private)
            {
                SetPrivateChatDragAmount();
                var playerData = m_ChatInput.PrivateChatInputPlayer;
                receiverPlayerId = playerData.PlayerId;
            }
            GameEntry.LobbyLogic.SendChat(m_CurrentChatType, receiverPlayerId, m_ChatInput.GetInputStrAndClear());
            m_ChatInput.ClearChatInputValue();
        }

        public void OnClickBlackListButton()
        {
            if (m_UsingChatBlackListSubForm == null)
            {
                m_UsingChatBlackListSubForm = CreateSubForm<ChatBlackListSubForm>("BlackList", gameObject, m_ChatBlackListTemplate.gameObject, false);
            }
            OpenSubForm(m_UsingChatBlackListSubForm);
        }

        private void RefreshReminder()
        {
            if (m_CurrentChatType == ChatType.World)
            {
                SetTipsMessage(m_WorldChatUnreadCount);
            }
            else if (m_CurrentChatType == ChatType.Private)
            {
                SetTipsMessage(m_PrivateChatUnreadCount);
            }
            else if (m_CurrentChatType == ChatType.System)
            {
                SetTipsMessage(m_SystemChatUnreadCount);
            }
            var data = GameEntry.Data.Chat.PrivateChatsData;
            bool isUnread = false;
            for (int i = 0; i < data.Count; i++)
            {
                var playerData = data[i].PrivatePlayerData;
                if (!isUnread)
                {
                    isUnread = data[i].UnReadCount > 0;
                }
                var item = GetChatPlayerItem(playerData.PlayerId);
                if (item != null)
                {
                    item.SetReminder(data[i].UnReadCount > 0);
                }
            }
            m_PrivateReminder.SetActive(isUnread);
        }
    }
}
