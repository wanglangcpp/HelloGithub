using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class ChatForm
    {
        [SerializeField]
        private ChatLineItem m_SystemChatLineTemplate = null;

        [SerializeField]
        private UIScrollView m_SystemListScrollView = null;

        [SerializeField]
        private UITable m_SystemListTable = null;

        private List<ChatLineItem> m_SystemChatLineList = new List<ChatLineItem>();

        private int m_SystemChatUnreadCount = 0;
        private float m_SystemChatAmount = 1;

        public bool RefreshSystemChat(bool waitAFrame)
        {
            RefreshReminder();
            m_PrivateChatUnreadCount = 0;
            m_PrivateChatAmount = 1;
            m_WorldChatUnreadCount = 0;
            m_WorldChatAmount = 1;
            StartCoroutine(RefreshSystemChatCo(waitAFrame));
            return true;
        }

        private IEnumerator RefreshSystemChatCo(bool waitAFrame)
        {
            if (waitAFrame)
            {
                yield return null;
            }
            HideAllChatLines();
            var chatDataList = GameEntry.Data.Chat.ChatSystemMsgList;
            for (int i = 0; i < chatDataList.Count; i++)
            {
                ChatLineItem data = null;
                var chatData = chatDataList[i];
                if (chatData.Type == ChatType.System)
                {
                    if (i < m_SystemChatLineList.Count)
                    {
                        data = m_SystemChatLineList[i];
                    }
                    else
                    {
                        data = CreatSystemChatLine(chatData);
                    }
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
            RepositionSystemChat();
        }

        private void RepositionSystemChat()
        {
            m_SystemListTable.Reposition();
            if (m_SystemChatAmount < ChatAmountTh)
            {
                return;
            }

            if (IsChatLineHeightOverScrollView(m_SystemListScrollView, m_SystemChatLineList, null))
            {
                m_SystemChatAmount = m_SystemListScrollView.GetDragAmount().y;
                SetSystemChatDragAmount();
            }
            else
            {
                m_SystemListScrollView.ResetPosition();
            }
        }

        private void OnSystemChatDragFinished()
        {
            m_SystemChatAmount = m_SystemListScrollView.GetDragAmount().y;
            if (m_SystemChatAmount >= ChatAmountTh)
            {
                m_SystemChatUnreadCount = 0;
            }
            RefreshReminder();
        }

        private void SetSystemChatDragAmount()
        {
            if (m_SystemChatAmount >= ChatAmountTh)
            {
                return;
            }
            m_SystemListScrollView.SetDragAmount(0f, 1f, false);
            m_SystemChatAmount = 1;
            m_SystemChatUnreadCount = 0;
            RefreshReminder();
        }

        private ChatLineItem CreatSystemChatLine(BaseChatData chatData)
        {
            ChatLineItem go = null;
            if (chatData.Type == ChatType.System)
            {
                go = NGUITools.AddChild(m_SystemListTable.gameObject, m_SystemChatLineTemplate.gameObject).GetComponent<ChatLineItem>();
                m_SystemChatLineList.Add(go);
            }
            return go;
        }
    }

}
