using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 推荐玩家标签页。用于 <see cref="Genesis.GameClient.FriendListForm"/>
    /// </summary>
    public class FriendListRecommendationTabContent : MonoBehaviour
    {
        [SerializeField]
        private ScrollViewCache m_ScrollViewCache = null;

        [SerializeField]
        private UILabel m_NoRecommendationText = null;

        private List<FriendListInvitationItem> m_CachedItemScripts = new List<FriendListInvitationItem>();

        #region MonoBehaviour

        private void OnEnable()
        {
            m_NoRecommendationText.gameObject.SetActive(false);
            GameEntry.Event.Subscribe(EventId.FriendDataChanged, OnFriendDataChanged);
            GameEntry.Event.Subscribe(EventId.GetRecommendedPlayersSuccess, OnGetRecommendedPlayersSuccess);
            GameEntry.LobbyLogic.AskForRecommendedPlayers();
        }

        private void OnDisable()
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.FriendDataChanged, OnFriendDataChanged);
            GameEntry.Event.Unsubscribe(EventId.GetRecommendedPlayersSuccess, OnGetRecommendedPlayersSuccess);
            m_ScrollViewCache.RecycleAllItems();
            m_CachedItemScripts.Clear();
            m_NoRecommendationText.gameObject.SetActive(false);
        }

        #endregion MonoBehaviour

        private void OnFriendDataChanged(object o, GameEventArgs e)
        {
            var ne = e as FriendDataChangedEventArgs;
            if (ne.ItsChangeMode == FriendDataChangedEventArgs.ChangeMode.ListRefreshed
                || ne.ItsChangeMode == FriendDataChangedEventArgs.ChangeMode.OneFriendAdded
                || ne.ItsChangeMode == FriendDataChangedEventArgs.ChangeMode.OneFriendRemoved)
            {
                var friends = GameEntry.Data.Friends;
                for (int i = 0; i < m_CachedItemScripts.Count; ++i)
                {
                    var itemScript = m_CachedItemScripts[i];
                    if (friends.CheckWhetherIsMyFriend(itemScript.PlayerId))
                    {
                        itemScript.RefreshInviteStatus(false);
                    }
                }
            }
        }

        private void OnGetRecommendedPlayersSuccess(object o, GameEventArgs e)
        {
            var ne = e as GetRecommendedPlayersSuccessEventArgs;
            var packet = ne.Packet;
            var players = new List<FriendListForm.CustomPlayerData>();
            for (int i = 0; i < packet.Players.Count; ++i)
            {
                var player = new FriendListForm.CustomPlayerData { IsMyFriend = false };
                player.Player.UpdateData(packet.Players[i]);
                players.Add(player);
            }

            m_NoRecommendationText.gameObject.SetActive(players.Count <= 0);
            for (int i = 0; i < players.Count; ++i)
            {
                var itemScript = m_ScrollViewCache.GetOrCreateItem(i);
                itemScript.Refresh(players[i].Player, !players[i].IsMyFriend && players[i].Player.Id != GameEntry.Data.Player.Id);
                m_CachedItemScripts.Add(itemScript);
            }

            m_ScrollViewCache.RecycleItemsAtAndAfter(players.Count);
            m_ScrollViewCache.ResetPosition();
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<FriendListInvitationItem>
        {

        }
    }
}
