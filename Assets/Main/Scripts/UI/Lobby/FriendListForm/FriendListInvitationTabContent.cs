using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 附近玩家及搜索玩家标签页。用于 <see cref="Genesis.GameClient.FriendListForm"/>
    /// </summary>
    public class FriendListInvitationTabContent : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_SearchNote = null;

        [SerializeField]
        private UILabel m_SearchText = null;

        [SerializeField]
        private UIInput m_SearchInput = null;

        [SerializeField]
        private UILabel m_NobodyText = null;

        [SerializeField]
        private UILabel m_FriendNumberLabel = null;

        [SerializeField]
        private FriendSearchScrollView m_SearchScrollView = null;

        private List<FriendListInvitationItem> m_ScrollViewItems = new List<FriendListInvitationItem>();

        // Called by NGUI via reflection.
        public void OnSearchTextChanged()
        {
            m_SearchNote.gameObject.SetActive(string.IsNullOrEmpty(m_SearchText.text));
        }

        // Called by NGUI via reflection.
        public void OnSearch()
        {
            var searchText = (m_SearchText.text ?? string.Empty).Trim();

            if (searchText.Length <= 0)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_FIND_PLAYER_INFORMATION_ERROR") });
                return;
            }
            ClearScrollViewList();
            GameEntry.LobbyLogic.SearchForPlayers(searchText);
        }

        public void OnOneKeyInvitClick()
        {
            if(GameEntry.Data.Friends.Data.Count >= GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.MaxFriendCount, 10))
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_HAS_REACHED_THE_UPPER_LIMIT") });
                return;
            }

            for (int i = 0; i < m_ScrollViewItems.Count; i++)
                if(m_ScrollViewItems[i].EnableInvite)
                    m_ScrollViewItems[i].OnClickInviteButton();
        }

        #region MonoBahviour

        private void OnEnable()
        {
            RefreshFriendCount();

            m_SearchNote.gameObject.SetActive(true);
            GameEntry.Event.Subscribe(EventId.FriendDataChanged, OnFriendDataChanged);
            GameEntry.Event.Subscribe(EventId.SearchPlayersSuccess, OnSearchPlayersSuccess);
            GameEntry.Event.Subscribe(EventId.GetRecommendedPlayersSuccess, OnGetRecommendedPlayersSuccess);

            GameEntry.LobbyLogic.AskForRecommendedPlayers();
        }

        private void OnDisable()
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.FriendDataChanged, OnFriendDataChanged);
            GameEntry.Event.Unsubscribe(EventId.SearchPlayersSuccess, OnSearchPlayersSuccess);
            GameEntry.Event.Unsubscribe(EventId.GetRecommendedPlayersSuccess, OnGetRecommendedPlayersSuccess);

            m_SearchInput.value = string.Empty;
            ClearScrollViewList();
            m_NobodyText.gameObject.SetActive(false);
        }

        #endregion MonoBahviour

        private void RefreshFriendCount()
        {
            int upperBound = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.MaxFriendCount, 50);
            m_FriendNumberLabel.text = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_ONLIEN_NUMBER", GameEntry.Data.Friends.Data.Count, upperBound);
        }

        private void OnGetRecommendedPlayersSuccess(object o, GameEventArgs e)
        {
            var ne = e as GetRecommendedPlayersSuccessEventArgs;
            var recommendedPlayers = ne.Packet.Players;
            var players = new List<PlayerData>();
            for (int i = 0; i < recommendedPlayers.Count; ++i)
            {
                PlayerData player = new PlayerData();
                player.UpdateData(recommendedPlayers[i]);
                players.Add(player);
            }

            if (players.Count <= 0)
            {
                m_NobodyText.gameObject.SetActive(true);
                m_NobodyText.text = GameEntry.Localization.GetString("UI_TEXT_NORECOMMENDATION");
            }
            else
            {
                m_NobodyText.gameObject.SetActive(false);
            }

            players.Sort(SortFriend);
            FillScrollViewList(players);
        }

        /// <summary>
        /// 好友推荐排序，在线的优先，其次是等级差小的靠上
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private int SortFriend(PlayerData p1, PlayerData p2)
        {
            if (p1.IsOnline == p2.IsOnline)
            {
                int playerLevel = GameEntry.Data.Player.Level;
                return Math.Abs(playerLevel - p1.Level).CompareTo(Math.Abs(playerLevel - p2.Level));
            }
            else
            {
                return p2.IsOnline.CompareTo(p1.IsOnline);
            }
        }

        private void FillScrollViewList(List<PlayerData> players)
        {
            m_ScrollViewItems.Clear();
            for (int i = 0; i < players.Count; ++i)
            {
                var itemScript = m_SearchScrollView.GetOrCreateItem(i);
                bool enableInvite = !GameEntry.Data.Friends.CheckWhetherIsMyFriend(players[i].Id) && players[i].Id != GameEntry.Data.Player.Id;
                itemScript.Refresh(players[i], enableInvite);
                m_ScrollViewItems.Add(itemScript);
            }

            m_SearchScrollView.RecycleItemsAtAndAfter(players.Count);
            m_SearchScrollView.ResetPosition();
        }

        private void ClearScrollViewList()
        {
            m_SearchScrollView.RecycleItemsAtAndAfter(0);
        }

        private void RefreshCachedItemsFromFriends()
        {
            var friends = GameEntry.Data.Friends;
            for (int i = 0; i < m_ScrollViewItems.Count; ++i)
            {
                int playerId = m_ScrollViewItems[i].PlayerId;
                bool enableInvite = !friends.CheckWhetherIsMyFriend(playerId) && playerId != GameEntry.Data.Player.Id;
                m_ScrollViewItems[i].RefreshInviteStatus(enableInvite);
            }
        }

        private void OnFriendDataChanged(object o, GameEventArgs e)
        {
            var ne = e as FriendDataChangedEventArgs;
            if (ne.ItsChangeMode == FriendDataChangedEventArgs.ChangeMode.ListRefreshed
                || ne.ItsChangeMode == FriendDataChangedEventArgs.ChangeMode.OneFriendAdded
                || ne.ItsChangeMode == FriendDataChangedEventArgs.ChangeMode.OneFriendRemoved)
            {
                RefreshFriendCount();
                RefreshCachedItemsFromFriends();
            }
        }

        private void OnSearchPlayersSuccess(object o, GameEventArgs e)
        {
            var ne = e as SearchPlayersSuccessEventArgs;
            var searchedPlayers = ne.Packet.Players;

            var players = new List<PlayerData>();
            for (int i = 0; i < searchedPlayers.Count; ++i)
            {
                PlayerData player = new PlayerData();
                player.UpdateData(searchedPlayers[i]);
                players.Add(player);
            }

            if (players.Count <= 0)
            {
                m_NobodyText.gameObject.SetActive(true);
                m_NobodyText.text = GameEntry.Localization.GetString("UI_TEXT_NOBODYNOTICE");
            }
            else
            {
                m_NobodyText.gameObject.SetActive(false);
            }

            FillScrollViewList(players);
        }

        [Serializable]
        private class FriendSearchScrollView : UIScrollViewCache<FriendListInvitationItem>
        {

        }
    }
}
