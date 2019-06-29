using GameFramework;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        /// <summary>
        /// 装备锻造活动 -- 创建队伍。
        /// </summary>
        public void GearFoundryCreateTeam()
        {
//             if (!GameEntry.OfflineMode.OfflineModeEnabled)
//             {
//                 GameEntry.Network.Send(new CLCreateGearFoundryTeam());
//             }
//             else
//             {
//                 var data = GameEntry.Data.GearFoundry;
//                 if (data.HasTeam)
//                 {
//                     return;
//                 }
// 
//                 var response = new LCCreateGearFoundryTeam
//                 {
//                     TeamId = 1000,
//                 };
// 
//                 GameEntry.Data.GearFoundry.UpdateDataAsCreator(response.TeamId);
//                 GameEntry.Event.Fire(this, new GearFoundryTeamCreatedEventArgs());
//             }
        }

        /// <summary>
        /// 装备锻造活动 -- 匹配队伍。
        /// </summary>
        /// <param name="level">匹配等级。-1 为完全随机。</param>
        public void GearFoundryMatchTeam(int level)
        {
//             if (!GameEntry.OfflineMode.OfflineModeEnabled)
//             {
//                 GameEntry.Network.Send(new CLMatchGearFoundryTeam { MatchMode = level });
//             }
//             else
//             {
//                 var response = new LCMatchGearFoundryTeam
//                 {
//                     Data = new PBGearFoundryInfo
//                     {
//                         TeamId = 4000,
//                         NextFoundryTimeInTicks = 0,
//                         Progress = new PBGearFoundryProgressInfo
//                         {
//                             CurrentProgress = 2,
//                             CurrentLevel = 1,
//                         },
//                     },
//                 };
// 
//                 for (int i = 0; i < 3; ++i)
//                 {
//                     response.Data.RewardFlags.Add(false);
//                 }
// 
//                 int playerCount = 3;
//                 for (int i = 0; i < playerCount; ++i)
//                 {
//                     response.Data.Players.Add(new PBGearFoundryPlayerInfo
//                     {
//                         Player = new PBPlayerInfo
//                         {
//                             Id = (i == playerCount - 1 ? GameEntry.Data.Player.Id : i + 10000),
//                             Name = (i == playerCount - 1 ? GameEntry.Data.Player.Name : "Fake player " + i),
//                             PortraitType = (i == playerCount - 1 ? GameEntry.Data.Player.PortraitType : i + 1),
//                         },
//                         FoundryCount = i * 2,
//                     });
//                 }
// 
//                 GameEntry.Data.GearFoundry.UpdateData(response.Data);
//                 GameEntry.Event.Fire(this, new GearFoundryTeamJoinedEventArgs());
//             }
        }

        /// <summary>
        /// 装备锻造活动 -- 邀请好友。
        /// </summary>
        /// <param name="friendPlayerId">好友玩家编号。</param>
        public void GearFoundryInviteFriend(int friendPlayerId)
        {
//             if (!GameEntry.OfflineMode.OfflineModeEnabled)
//             {
//                 GameEntry.Network.Send(new CLInviteFriendForGearFoundry { FriendPlayerId = friendPlayerId });
//             }
//             else
//             {
//                 //var response = new LCInviteFriendForGearFoundry { IsActive = true };
//                 GameEntry.Event.Fire(this, new GearFoundryInvitationSentEventArgs(friendPlayerId));
//             }
        }

        /// <summary>
        /// 装备锻造活动 -- 回应邀请。
        /// </summary>
        /// <param name="accept">是否接受邀请。</param>
        /// <param name="inviterPlayerId">邀请者玩家编号。</param>
        /// <param name="teamId">邀请者所在队伍。</param>
        public void GearFoundryRespondToInvitation(bool accept, int inviterPlayerId, int teamId)
        {
//             if (!GameEntry.OfflineMode.OfflineModeEnabled)
//             {
//                 GameEntry.Network.Send(new CLRespondToGearFoundryInvitation { Accept = accept, InviterPlayerId = inviterPlayerId, TeamId = teamId });
//             }
//             else
//             {
//                 var response = new LCRespondToGearFoundryInvitation { Accept = false, InviterPlayerId = inviterPlayerId, TeamId = teamId };
//                 GameEntry.Data.GearFoundry.Invitations.RemoveData(response.InviterPlayerId, response.TeamId);
//                 GameEntry.Event.Fire(this, new GearFoundryInvitationRespondedEventArgs(response.Accept, response.InviterPlayerId, response.TeamId));
//             }
        }

        /// <summary>
        /// 装备锻造活动 -- 踢出队员。
        /// </summary>
        /// <param name="playerId"></param>
        public void GearFoundryKickPlayer(int playerId)
        {
//             var data = GameEntry.Data.GearFoundry;
//             if (!data.AmILeader)
//             {
//                 Log.Error("I am not the leader of the team, so I cannot kick the player.");
//                 return;
//             }
// 
//             if (data.Players.GetData(playerId) == null)
//             {
//                 Log.Error("The player '{0}' is not int your team.", playerId.ToString());
//                 return;
//             }
// 
//             if (!GameEntry.OfflineMode.OfflineModeEnabled)
//             {
//                 GameEntry.Network.Send(new CLKickPlayerFromGearFoundryTeam { PlayerId = playerId });
//             }
//             else
//             {
//                 // 不处理回包 LCKickPlayerFromGearFoundryTeam，等待 LCPlayerListInGearFoundryTeam 包。
//                 var pushedPacket = new LCPlayerListInGearFoundryTeam();
//                 for (int i = 0; i < 3; ++i)
//                 {
//                     if (playerId == i + 1)
//                     {
//                         continue;
//                     }
// 
//                     pushedPacket.Players.Add(new PBGearFoundryPlayerInfo
//                     {
//                         Player = new PBPlayerInfo
//                         {
//                             Id = i + 1,
//                             Name = "Fake player " + i,
//                             PortraitType = i + 1,
//                         },
//                         FoundryCount = i * 2,
//                     });
//                 }
// 
//                 data.UpdateData(pushedPacket.Players);
//             }
        }

        /// <summary>
        /// 装备锻造活动 -- 离开团队。
        /// </summary>
        public void GearFoundryLeaveTeam()
        {
//             var data = GameEntry.Data.GearFoundry;
//             if (data.TeamId < 0)
//             {
//                 Log.Error("Not in a team already.");
//                 return;
//             }
// 
//             if (!GameEntry.OfflineMode.OfflineModeEnabled)
//             {
//                 GameEntry.Network.Send(new CLLeaveGearFoundryTeam());
//             }
//             else
//             {
//                 data.LeaveTeam();
//                 GameEntry.Event.Fire(this, new GearFoundryLeftTeamEventArgs());
//             }
        }

        /// <summary>
        /// 装备锻造活动 -- 执行锻造操作。
        /// </summary>
        public void GearFoundryPerform()
        {
//             if (!GameEntry.OfflineMode.OfflineModeEnabled)
//             {
//                 GameEntry.Network.Send(new CLPerformFoundry());
//             }
//             else
//             {
//                 var data = GameEntry.Data.GearFoundry;
//                 var response = new LCPerformFoundry
//                 {
//                     PerformerPlayerId = GameEntry.Data.Player.Id,
//                     Progress = new PBGearFoundryProgressInfo
//                     {
//                         CurrentLevel = 3, //data.Progress.CurrentLevel + 1,
//                         CurrentProgress = 0, //data.Progress.CurrentProgress + 1,
//                     },
//                     NextFoundryTimeInTicks = GameEntry.Time.LobbyServerUtcTime.AddSeconds(3.0).Ticks,
//                 };
//                 response.RewardFlags.AddRange(new bool[] { false, false, false });
// 
//                 data.UpdateData(response.Progress);
//                 data.UpdateData(response.RewardFlags);
//                 data.UpdateData(response.NextFoundryTimeInTicks);
// 
//                 var players = data.Players.Data;
//                 for (int i = 0; i < players.Count; ++i)
//                 {
//                     if (players[i].Id == response.PerformerPlayerId)
//                     {
//                         players[i].UpdateData(players[i].FoundryCount + 1);
//                         break;
//                     }
//                 }
// 
//                 GameEntry.Event.Fire(this, new GearFoundryPerformedEventArgs(true));
//             }
        }

        /// <summary>
        /// 装备锻造活动 -- 领奖。
        /// </summary>
        /// <param name="foundryLevel">锻造等级。</param>
        public void GearFoundryClaimReward(int foundryLevel)
        {
//             if (!GameEntry.OfflineMode.OfflineModeEnabled)
//             {
//                 GameEntry.Network.Send(new CLGetFoundryReward { Level = foundryLevel });
//             }
//             else
//             {
//                 var response = new LCGetFoundryReward();
//                 response.RewardFlags.AddRange(new bool[] { false, false, false });
// 
//                 var data = GameEntry.Data.GearFoundry;
//                 data.UpdateData(response.RewardFlags);
//                 GameEntry.Event.Fire(this, new GearFoundryRewardClaimedEventArgs());
//             }
        }
    }
}
