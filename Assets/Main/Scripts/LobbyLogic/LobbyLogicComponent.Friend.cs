using GameFramework;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        /// <summary>
        /// 发送添加好友请求。
        /// </summary>
        /// <param name="playerId">对方玩家编号。</param>
        public void SendFriendRequest(int playerId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLSendFriendRequest { PlayerId = playerId });
            }
            else
            {
                // var response = new LCSendFriendRequest();
                //GameEntry.Event.Fire(this, new FriendRequestSentEventArgs());
            }
        }

        /// <summary>
        /// 删除好友。
        /// </summary>
        /// <param name="playerId">好友玩家编号。</param>
        public void RemoveFriend(int playerId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_BUTTON_DELETE_FRIENDS"),
                    OnClickConfirm = (o) =>
                    {
                        GameEntry.Network.Send(new CLRemoveFriend { FriendPlayerId = playerId });
                    },
                    OnClickCancel = (o) => { },
                });
            }
            else
            {
                //GameEntry.Data.Friends.RemoveFriend(playerId);
                //GameEntry.Event.Fire(this, new FriendDeletedEventArgs(playerId));
                //GameEntry.Event.Fire(this, new FriendDataChangedEventArgs(FriendDataChangedEventArgs.ChangeMode.OneFriendRemoved));
                //if (GameEntry.Data.UpdateNearbyPlayerByFriends())
                //{
                //    GameEntry.Event.Fire(this, new NearbyPlayerDataChangedEventArgs());
                //}
            }
        }

        /// <summary>
        /// 搜索玩家。
        /// </summary>
        /// <param name="searchText">搜索关键字。</param>
        public void SearchForPlayers(string searchText)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                int tryParseId;
                if(int.TryParse(searchText, out tryParseId) && tryParseId >= PlayerData.IdToDisplayIdExtraValue)
                {
                    GameEntry.Network.Send(new CLSearchPlayers { Keyword = searchText, UserId = tryParseId - PlayerData.IdToDisplayIdExtraValue });
                }
                else
                {
                    GameEntry.Network.Send(new CLSearchPlayers { Keyword = searchText });
                }
                
            }
            else
            {
                //var response = new LCSearchPlayers();
                //for (int i = 0; i < 5; ++i)
                //{
                //    response.Players.Add(new PBPlayerInfo { Id = 1000 + i, Name = i + "Fake searched player".Substring(0, i + 2), Level = 20 + i, });
                //    response.IsMyFriend.Add(i % 3 == 0);
                //}

                //GameEntry.Event.Fire(this, new SearchPlayersSuccessEventArgs(response));
            }
        }

        /// <summary>
        /// 搜索玩家。
        /// </summary>
        /// <param name="displayId">玩家的显示编号。</param>
        public void SearchForPlayers(int playerDisplayId)
        {
            UnityEngine.Debug.LogError("这个方法被删除掉了，如果还存在调用，那么就有问题！");

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                //GameEntry.Network.Send(new CLSearchPlayers { DisplayId = playerDisplayId, });
            }
            else
            {
                //var response = new LCSearchPlayers();
                //for (int i = 0; i < 1; ++i)
                //{
                //    response.Players.Add(new PBPlayerInfo { Id = 1000 + i, Name = i + "Fake searched player".Substring(0, i + 2), Level = 20 + i, });
                //    response.IsMyFriend.Add(i % 3 == 0);
                //}

                //GameEntry.Event.Fire(this, new SearchPlayersSuccessEventArgs(response));
            }
        }

        /// <summary>
        /// 请求推荐的玩家列表。
        /// </summary>
        public void AskForRecommendedPlayers()
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLGetRecommendedPlayers());
            }
            else
            {
                //var response = new LCGetRecommendedPlayers();
                //for (int i = 0; i < 5; ++i)
                //{
                //    response.Players.Add(new PBPlayerInfo { Id = 2000 + i, Name = "Fake recommended player" + i, Level = 30 + i, });
                //}

                //GameEntry.Event.Fire(this, new GetRecommendedPlayersSuccessEventArgs(response));
            }
        }

        /// <summary>
        /// 向好友赠送能量。
        /// </summary>
        /// <param name="friendPlayerId">好友的玩家编号。</param>
        public void GiveEnergyToFriend(int friendPlayerId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLGiveEnergy { FriendPlayerId = friendPlayerId });
            }
            else
            {
                //var response = new CLGiveEnergyToFriend { FriendPlayerId = friendPlayerId };
                //var friend = GameEntry.Data.Friends.GetData(friendPlayerId);
                //if (friend == null)
                //{
                //    Log.Warning("Friend player '{0}' not found.", friendPlayerId);
                //    return;
                //}
                //friend.UpdateData(false, friend.CanReceiveEnergy);
                //GameEntry.Event.Fire(this, new FriendDataChangedEventArgs());
                //GameEntry.Event.Fire(this, new EnergyGivenToFriendEventArgs(friendPlayerId));
            }
        }

        /// <summary>
        /// 收取好友赠送的能量。
        /// </summary>
        /// <param name="friendPlayerId">好友的玩家编号。</param>
        public void ReceiveEnergyFromFriend(int friendPlayerId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLClaimEnergy { FriendPlayerId = friendPlayerId });
            }
            else
            {
                //var response = new CLReceiveEnergyFromFriend { FriendPlayerId = friendPlayerId };
                //var friend = GameEntry.Data.Friends.GetData(response.FriendPlayerId);
                //if (friend == null)
                //{
                //    Log.Warning("Friend player '{0}' not found.", response.FriendPlayerId);
                //    return;
                //}
                //friend.UpdateData(friend.CanGiveEnergy, false);
                //GameEntry.Event.Fire(this, new FriendDataChangedEventArgs());
                //GameEntry.Event.Fire(this, new EnergyReceivedFromFriendEventArgs(response.FriendPlayerId));
            }
        }

        /// <summary>
        /// 接受好友请求。
        /// </summary>
        /// <param name="playerId">对方玩家编号。</param>
        public void AcceptFriendRequest(int playerId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLReplyFriendRequest { PlayerId = playerId, Accept = true });
            }
            else
            {
                //var response = new LCReplyFriendRequest
                //{
                //    FriendInfo = new PBFriendInfo()
                //    {
                //        LastClaimEnergyTime = System.DateTime.UtcNow.Ticks,
                //        LastGiveEnergyTime = System.DateTime.UtcNow.Ticks,
                //        LastReceiveEnergyTime = System.DateTime.UtcNow.Ticks
                //    },

                //};

                ////GameEntry.Data.Friends.AddData(response.Player, response.CanGiveEnergy, response.CanReceiveEnergy);
                ////GameEntry.Data.FriendRequests.RemoveData(response.Player.Id);
                //GameEntry.Data.Friends.AddFriend(response.FriendInfo);
                //GameEntry.Data.FriendRequests.RemoveRequest(response.PlayerId);

                //GameEntry.Event.Fire(this, new PendingFriendRequestsDataChangedEventArgs());
                //if (GameEntry.Data.UpdateNearbyPlayerByFriends())
                //{
                //    GameEntry.Event.Fire(this, new NearbyPlayerDataChangedEventArgs());
                //}
                //GameEntry.Event.Fire(this, new FriendAddedEventArgs(playerId));
                //GameEntry.Event.Fire(this, new FriendDataChangedEventArgs(FriendDataChangedEventArgs.ChangeMode.OneFriendAdded));
            }
        }

        /// <summary>
        /// 拒绝好友请求。
        /// </summary>
        /// <param name="playerId">对方玩家编号。</param>
        public void RefuseFriendRequest(int playerId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLReplyFriendRequest { PlayerId = playerId, Accept = false });
            }
            else
            {
                //var response = new LCReplyFriendRequest
                //{

                //};

                //GameEntry.Data.FriendRequests.RemoveRequest(response.PlayerId);
                //GameEntry.Event.Fire(this, new PendingFriendRequestsDataChangedEventArgs());
                //GameEntry.Event.Fire(this, new FriendRequestRefusedEventArgs(playerId));
            }
        }
    }
}
