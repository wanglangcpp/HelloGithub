using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 好友请求列表标签页。用于 <see cref="Genesis.GameClient.FriendListForm"/>
    /// </summary>
    public class FriendListRequestsTabContent : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_EmptyText = null;

        [SerializeField]
        private UILabel m_CurrentFriendLabel = null;

        [SerializeField]
        private FriendRequestListScrollView m_RequestScrollView = null;

        private List<FriendListRequestItem> m_CachedItems = new List<FriendListRequestItem>();

        #region MonoBehaviour

        public void OnRefuseAllButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_DELETE_ALL_YOUR_FRIENDS"),
                OnClickConfirm = (o) =>
                {
                    for (int i = 0; i < m_CachedItems.Count; i++)
                        m_CachedItems[i].OnClickRefuseButton();
                },
                OnClickCancel = (o) => { },
            });
        }

        public void OnAgreeAllButtonClick()
        {
            int remainCount = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.MaxFriendCount, 50) - GameEntry.Data.Friends.Data.Count;

            GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
            {
                Mode = 2,
                Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_ACCEPT_ALL_YOUR_FRIENDS"),
                OnClickConfirm = (o) =>
                {
                    for (int i = 0; i < m_CachedItems.Count; i++)
                    {
                        if (remainCount <= i)
                        {
                            GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_HAS_REACHED_THE_UPPER_LIMIT") });
                            return;
                        }

                        m_CachedItems[i].AcceptFriend();
                    }
                },
                OnClickCancel = (o) => { },
            });
        }

        private void OnEnable()
        {
            InitScrollView();

            GameEntry.Event.Subscribe(EventId.FriendAdded, OnFriendAdded);
            GameEntry.Event.Subscribe(EventId.FriendRequestRefused, OnFriendRequestRefused);
            GameEntry.Event.Subscribe(EventId.PendingFriendRequestsDataChanged, OnFriendRequestChanged);
        }

        private void OnDisable()
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.FriendAdded, OnFriendAdded);
            GameEntry.Event.Unsubscribe(EventId.FriendRequestRefused, OnFriendRequestRefused);
            GameEntry.Event.Unsubscribe(EventId.PendingFriendRequestsDataChanged, OnFriendRequestChanged);
            DeinitScrollView();
            m_EmptyText.gameObject.SetActive(false);
        }

        #endregion MonoBehaviour

        public void InitScrollView()
        {
            int upperBound = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.MaxFriendCount, 50);
            m_CurrentFriendLabel.text = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_ONLIEN_NUMBER", GameEntry.Data.Friends.Data.Count, upperBound);

            m_CachedItems.Clear();
            var requestData = GameEntry.Data.FriendRequests.RequestList;
            for (int i = 0; i < requestData.Count; ++i)
            {
                var item = m_RequestScrollView.GetOrCreateItem(i);
                item.Refresh(requestData[i]);
                m_CachedItems.Add(item);
            }

            m_RequestScrollView.RecycleItemsAtAndAfter(requestData.Count);
            m_RequestScrollView.ResetPosition();

            m_EmptyText.gameObject.SetActive(m_CachedItems.Count <= 0);
        }

        private void DeinitScrollView()
        {
            m_RequestScrollView.RecycleAllItems();
            m_CachedItems.Clear();
        }

        private void OnFriendRequestRefused(object sender, GameEventArgs e)
        {
            InitScrollView();
        }

        private void OnFriendAdded(object sender, GameEventArgs e)
        {
            InitScrollView();
        }

        private void OnFriendRequestChanged(object sender, GameEventArgs e)
        {
            InitScrollView();
        }
        [Serializable]
        private class FriendRequestListScrollView : UIScrollViewCache<FriendListRequestItem>
        {

        }
    }
}
