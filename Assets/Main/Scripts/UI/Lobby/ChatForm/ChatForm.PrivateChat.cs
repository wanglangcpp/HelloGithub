using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ChatForm
    {
        [SerializeField]
        private ChatLineItem m_PrivateChatLineTemplate = null;

        [SerializeField]
        private ChatLineItem m_PrivateSelfChatLineTemplate = null;

        [SerializeField]
        private ScrollViewCache m_PrivatePlayer = null;

        [SerializeField]
        private UIScrollView m_PrivateListScrollView = null;

        [SerializeField]
        private UITable m_PrivateListTable = null;

        [SerializeField]
        private GameObject m_PrivateReminder = null;

        private List<ChatLineItem> m_PrivateChatLineList = new List<ChatLineItem>();
        private List<ChatLineItem> m_PrivateSelfChatLineList = new List<ChatLineItem>();
        private PlayerChatData m_CurrPrivatePlayerData = null;
        private int m_PrivateChatUnreadCount = 0;
        private float m_PrivateChatAmount = 1;

        private bool RefreshPrivateChat(bool waitAFrame)
        {
            m_WorldChatUnreadCount = 0;
            m_WorldChatAmount = 1;
            ResetSendButton();
            StartCoroutine(RefreshPrivateChatCo(waitAFrame));
            return true;
        }

        private IEnumerator RefreshPrivateChatCo(bool waitAFrame)
        {
            if (waitAFrame)
            {
                yield return null;
            }

            var chatData = GameEntry.Data.Chat.PrivateChatsData;
            List<PrivateChatPlayerData> data = new List<PrivateChatPlayerData>();
            data.AddRange(chatData);
            if (data.Count == 0)
            {
                HideAllChatLines();
                yield break;
            }
            PlayerChatData selectrPrivatePlayerData = m_CurrPrivatePlayerData;
            if (selectrPrivatePlayerData == null || !GameEntry.Data.Chat.IsPrivateChatHasPlayer(selectrPrivatePlayerData.PlayerId))
            {
                selectrPrivatePlayerData = data[0].PrivatePlayerData;
            }

            if (data.Count > 1 && (selectrPrivatePlayerData.PlayerId == data[1].m_PlayerData.PlayerId) && data[0].UnReadCount > 0)
            {
                var param = data[0];
                data[0] = data[1];
                data[1] = param;
            }

            for (int i = 0; i < data.Count; i++)
            {
                PrivateChatPlayerItem playerItem = null;
                var playerData = data[i].PrivatePlayerData;
                playerItem = m_PrivatePlayer.GetOrCreateItem(i);
                playerItem.InitChatPlayer(playerData, OnClickSelectPlayer);
                if (playerData.PlayerId == selectrPrivatePlayerData.PlayerId)
                {
                    playerItem.gameObject.name = "ChatPlayer00";
                }
                else if (i < 9)
                {
                    playerItem.gameObject.name = "ChatPlayer0" + (i + 1).ToString();
                }
                else
                {
                    playerItem.gameObject.name = "ChatPlayer" + (i + 1).ToString();
                }
            }
            m_PrivatePlayer.RecycleItemAt(data.Count);
            m_PrivatePlayer.ResetPosition();
            yield return null;
            OnSelectPlayer(selectrPrivatePlayerData, waitAFrame);
        }

        private void OnSetSelect(PlayerChatData playerData)
        {
            for (int i = 0; i < m_PrivatePlayer.Count; i++)
            {
                m_PrivatePlayer.GetItem(i).Select = m_PrivatePlayer.GetItem(i).PlayerId == playerData.PlayerId;
            }
        }

        private void OnClickSelectPlayer(PlayerChatData playerData, bool waitAFrame)
        {
            m_PrivateChatUnreadCount = 0;
            m_PrivateChatAmount = 1;
            OnSelectPlayer(playerData, waitAFrame);
        }

        private void OnSelectPlayer(PlayerChatData playerData, bool waitAFrame)
        {
            m_CurrPrivatePlayerData = playerData;
            OnSetSelect(playerData);
            HideAllChatLines();
            m_PrivateListTable.direction = UITable.Direction.Down;
            m_ChatInput.PrivateChatInputPlayer = playerData;
            List<PrivateChatData> privateChatsData = GameEntry.Data.Chat.GetPrivateChatPlayer(playerData.PlayerId).PrivateChatsData;
            int n = 0;
            for (int i = 0; privateChatsData != null && i < privateChatsData.Count; i++)
            {
                ChatLineItem data = null;
                var chatData = privateChatsData[i];
                if (chatData.IsMe)
                {
                    if ((i - n) < m_PrivateSelfChatLineList.Count)
                    {
                        data = m_PrivateSelfChatLineList[i - n];
                    }
                    else
                    {
                        data = NGUITools.AddChild(m_PrivateListTable.gameObject, m_PrivateSelfChatLineTemplate.gameObject).GetComponent<ChatLineItem>();
                        m_PrivateSelfChatLineList.Add(data);
                    }
                }
                else
                {
                    if (i < m_PrivateChatLineList.Count)
                    {
                        data = m_PrivateChatLineList[i];
                    }
                    else
                    {
                        data = NGUITools.AddChild(m_PrivateListTable.gameObject, m_PrivateChatLineTemplate.gameObject).GetComponent<ChatLineItem>();
                        m_PrivateChatLineList.Add(data);
                    }
                    n++;
                }

                if (i < 10)
                {
                    data.gameObject.name = "ChatLine0" + i.ToString();
                }
                else
                {
                    data.gameObject.name = "ChatLine" + i.ToString();
                }
                data.gameObject.SetActive(true);
                data.InitChatLine(chatData, OnClickChatLineReturn);
            }
            m_PrivateListTable.Reposition();
            RefreshReminder();

            if (m_PrivateChatAmount < ChatAmountTh)
            {
                return;
            }

            if (IsChatLineHeightOverScrollView(m_PrivateListScrollView, m_PrivateChatLineList, m_PrivateSelfChatLineList))
            {
                m_PrivateChatAmount = m_PrivateListScrollView.GetDragAmount().y;
                SetPrivateChatDragAmount();
            }
            else
            {
                m_PrivateListScrollView.ResetPosition();
            }
        }

        private void OnPrivateChatDragFinished()
        {
            m_PrivateChatAmount = m_PrivateListScrollView.GetDragAmount().y;
            if (m_PrivateChatAmount >= ChatAmountTh)
            {
                m_PrivateChatUnreadCount = 0;
            }
            RefreshReminder();
        }

        private void SetPrivateChatDragAmount()
        {
            if (m_PrivateChatAmount >= ChatAmountTh)
            {
                return;
            }
            m_PrivateListScrollView.SetDragAmount(0f, 1f, false);
            m_PrivateChatAmount = 1;
            m_PrivateChatUnreadCount = 0;
            RefreshReminder();
        }

        private PrivateChatPlayerItem GetChatPlayerItem(int playerId)
        {
            for (int i = 0; i < m_PrivatePlayer.Count; i++)
            {
                var item = m_PrivatePlayer.GetItem(i);
                if (item.isActiveAndEnabled && item.PlayerId == playerId)
                {
                    return item;
                }
            }
            return null;
        }

        private void HideAllPrivatePlayers()
        {
            for (int i = 0; i < m_PrivatePlayer.Count; i++)
            {
                m_PrivatePlayer.GetItem(i).gameObject.SetActive(false);
            }
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<PrivateChatPlayerItem>
        {

        }
    }
}
