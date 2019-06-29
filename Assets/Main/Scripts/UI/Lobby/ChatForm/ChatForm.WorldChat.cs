using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ChatForm
    {
        [SerializeField]
        private ChatLineItem m_WorldChatLineTemplate = null;

        [SerializeField]
        private ChatLineItem m_WorldSelfChatLineTemplate = null;

        [SerializeField]
        private UILabel m_SendButtonLabel = null;

        [SerializeField]
        private UIButton m_SendButton = null;

        [SerializeField]
        private UIScrollView m_WorldListScrollView = null;

        [SerializeField]
        private UITable m_WorldListTable = null;

        private List<ChatLineItem> m_WorldChatLineList = new List<ChatLineItem>();
        private List<ChatLineItem> m_WorldSelfChatLineList = new List<ChatLineItem>();
        private float m_ChatCoolDownTime = 0;
        private bool m_CanWorldChatSendOut = false;
        private int m_WorldChatUnreadCount = 0;
        private float m_WorldChatAmount = 1;

        private const float ChatAmountTh = 0.99f;

        public bool RefreshWorldChat(bool waitAFrame)
        {
            RefreshReminder();
            ResetSendButton();
            m_PrivateChatUnreadCount = 0;
            m_PrivateChatAmount = 1;
            StartCoroutine(RefreshWorldChatCo(waitAFrame));
            return true;
        }

        private IEnumerator RefreshWorldChatCo(bool waitAFrame)
        {
            if (waitAFrame)
            {
                yield return null;
            }

            HideAllChatLines();
            m_WorldListTable.direction = UITable.Direction.Down;
            var chatDataList = GameEntry.Data.Chat.WorldChatList;
            int n = 0;
            for (int i = 0; i < chatDataList.Count; i++)
            {
                ChatLineItem data = null;
                var chatData = chatDataList[i];
                if (chatData.Type == ChatType.World && chatData.Receiver != null)
                {
                    if ((i - n) < m_WorldSelfChatLineList.Count)
                    {
                        data = m_WorldSelfChatLineList[i - n];
                    }
                    else
                    {
                        data = CreatWorldChatLine(chatData);
                    }
                }
                else
                {
                    if (i < m_WorldChatLineList.Count)
                    {
                        data = m_WorldChatLineList[i];
                    }
                    else
                    {
                        data = CreatWorldChatLine(chatData);
                    }
                    n++;
                }
                data.gameObject.SetActive(true);
                if (i < 10)
                {
                    data.gameObject.name = "ChatLine0" + i.ToString();
                }
                else
                {
                    data.gameObject.name = "ChatLine" + i.ToString();
                }
                data.InitChatLine(chatData, OnClickChatLineReturn);
            }

            m_CanWorldChatSendOut = Mathf.Clamp((float)GameEntry.Data.Chat.LastSendWordChatDuration,0, m_ChatCoolDownTime) >= m_ChatCoolDownTime;
            RepositionWorldChat();
        }

        private void RepositionWorldChat()
        {
            m_WorldListTable.Reposition();
            if (m_WorldChatAmount < ChatAmountTh)
            {
                return;
            }

            if (IsChatLineHeightOverScrollView(m_WorldListScrollView, m_WorldChatLineList, m_WorldSelfChatLineList))
            {
                m_WorldChatAmount = m_WorldListScrollView.GetDragAmount().y;
                SetWorldChatDragAmount();
            }
            else
            {
                m_WorldListScrollView.ResetPosition();
            }
        }

        private void OnWorldChatDragFinished()
        {
            m_WorldChatAmount = m_WorldListScrollView.GetDragAmount().y;
            if (m_WorldChatAmount >= ChatAmountTh)
            {
                m_WorldChatUnreadCount = 0;
            }
            RefreshReminder();
        }

        private void SetWorldChatDragAmount()
        {
            if (m_WorldChatAmount >= ChatAmountTh)
            {
                return;
            }
            m_WorldListScrollView.SetDragAmount(0f, 1f, false);
            m_WorldChatAmount = 1;
            m_WorldChatUnreadCount = 0;
            RefreshReminder();
        }

        private ChatLineItem CreatWorldChatLine(BaseChatData chatData)
        {
            ChatLineItem go = null;
            if (chatData.Type == ChatType.World && chatData.Receiver != null)
            {
                go = NGUITools.AddChild(m_WorldListTable.gameObject, m_WorldSelfChatLineTemplate.gameObject).GetComponent<ChatLineItem>();
                m_WorldSelfChatLineList.Add(go);
            }
            else
            {
                go = NGUITools.AddChild(m_WorldListTable.gameObject, m_WorldChatLineTemplate.gameObject).GetComponent<ChatLineItem>();
                m_WorldChatLineList.Add(go);
            }
            return go;
        }

        private void OnClickChatLineReturn(ChatLineItem item)
        {
            if (item.ChatData == null)
            {
                return;
            }
            PlayerChatData chatPlayer = item.ChatData.Receiver == null || item.ChatData.Receiver.m_PlayerId <= 0 ? item.ChatData.Sender : item.ChatData.Receiver;
            PBPlayerInfo player = new PBPlayerInfo();
            player.Id = chatPlayer.PlayerId;
            player.Name = chatPlayer.PlayerName;
            player.Level = chatPlayer.Level;
            player.VipLevel = chatPlayer.VipLevel;
            player.PortraitType = chatPlayer.PortraitType;
            player.IsOnline = chatPlayer.IsOnline;
            player.Might = chatPlayer.Might;
            PlayerData data = new PlayerData();
            data.UpdateData(player);
            GameEntry.UI.OpenUIForm(UIFormId.PlayerSummaryForm, new PlayerSummaryFormDisplayData { ShowPlayerData = data, OnClickCloseReturn = PlayerSummaryFormReturn, EnableInvite = true });
        }

        private void PlayerSummaryFormReturn(object data)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            if (data != null)
            {
                PlayerData player = data as PlayerData;
                SwitchPrivateChat(player);
            }
            else if (m_CurrentChatType == ChatType.World)
            {
                RefreshWorldChat(false);
            }
        }

        private void UpdateWorldChat()
        {
            if (m_CanWorldChatSendOut || m_CurrentChatType != ChatType.World)
            {
                return;
            }
            float dur = Mathf.Clamp((float)GameEntry.Data.Chat.LastSendWordChatDuration, 0, m_ChatCoolDownTime);
            if (dur >= m_ChatCoolDownTime)
            {
                m_CanWorldChatSendOut = true;
                ResetSendButton();
            }
            else
            {
                m_SendButton.isEnabled = false;
                m_SendButtonLabel.text = GameEntry.Localization.GetString("UI_TEXT_TIME_SECOND", m_ChatCoolDownTime -  dur);
            }
        }

        private void ResetSendButton()
        {
            m_SendButton.isEnabled = true;
            m_SendButtonLabel.text = GameEntry.Localization.GetString("UI_BUTTON_SENDOUT");
        }
    }
}
