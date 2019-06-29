using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        /// <summary>
        /// 判断单人副本是否开启。
        /// </summary>
        /// <param name="instanceId">副本编号。</param>
        /// <returns>单人副本是否开启。</returns>
        public bool InstanceIsOpen(int instanceId)
        {
            var levelData = GameEntry.Data.InstanceGroups.GetLevelById(instanceId);
            if (levelData == null)
                return false;
            else
                return levelData.IsOpen;
        }

        /// <summary>
        /// 领取副本章节宝箱的奖励
        /// </summary>
        /// <param name="chapterId">副本章节ID</param>
        /// <param name="chestIndex">宝箱序号。0-1-2，从0开始</param>
        public void PickUpChestReward(int chapterId, int chestIndex)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLOpenInstanceGroupChest request = new CLOpenInstanceGroupChest();
                request.InstanceGroupId = chapterId;
                request.ChestIndex = chestIndex;

                GameEntry.Network.Send(request);
            }
        }

        public void SweepLevel(int levelId)
        {
            CLCleanOutInstance request = new CLCleanOutInstance();
            request.InstanceType = levelId;

            GameEntry.Network.Send(request);
        }

        /// <summary>
        /// 进入单人副本。
        /// </summary>
        /// <param name="instanceTypeId">副本编号。</param>
        public void EnterInstance(int instanceTypeId)
        {
            int usePower = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Instance.CostEnergy, 6);

            if (!UIUtility.CheckEnergy(usePower))
            {
                return;
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLEnterInstance request = new CLEnterInstance();
                request.InstanceType = instanceTypeId;
                GameEntry.Network.Send(request);
            }
            else
            {
                if (GameEntry.Procedure.CurrentProcedure is ProcedureMain)
                {
                    GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(InstanceLogicType.SinglePlayer, instanceTypeId, true));
                }
            }
        }

        /// <summary>
        /// 进入翻翻棋战斗副本。
        /// </summary>
        /// <param name="chessFieldIndex">棋子编号。</param>
        public void EnterChessBattle(int chessFieldIndex)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                var request = new CLEnterChessBattle
                {
                    ChessFieldIndex = chessFieldIndex,
                };
                GameEntry.Network.Send(request);
            }
            else
            {
                var chessBattleMe = GameEntry.Data.ChessBattleMe;
                var currentHeroTeam = new List<int>(chessBattleMe.HeroTeam.HeroType);
                currentHeroTeam.RemoveAll(heroType => !chessBattleMe.HeroIsAlive(heroType));
                var pbHeroTeam = new PBHeroTeamInfo();
                pbHeroTeam.HeroType.AddRange(currentHeroTeam);
                chessBattleMe.UpdateData(pbHeroTeam);

                if (GameEntry.Procedure.CurrentProcedure is ProcedureMain)
                {
                    GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(InstanceLogicType.ChessBattle, Constant.DefaultPlayerVSPlayerAIInstanceId, true));
                }
            }
        }

        /// <summary>
        /// 进入离线竞技战斗副本。
        /// </summary>
        /// <param name="isRevenge">是否复仇之战。</param>
        public void EnterArenaBattle(int arenaPlayerId)
        {
            GameEntry.Data.OfflineArenaOpponent.UpdateData(GameEntry.Data.OfflineArena.Enermies.GetData(arenaPlayerId));
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLEnterArena
                {
                    ArenaPlayerId = arenaPlayerId,
                });
                return;
            }

            if (GameEntry.Procedure.CurrentProcedure is ProcedureMain)
            {
                GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenOfflineArena, true);
                GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(InstanceLogicType.ArenaBattle, Constant.DefaultPlayerVSPlayerAIInstanceId, true));
            }
        }

        /// <summary>
        /// 离开离线竞技战斗副本。
        /// </summary>
        /// <param name="win">是否胜利。</param>
        public void LeaveArenaBattle(int arenaPlayerId, bool win)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLLeaveArena { ArenaPlayerId = arenaPlayerId, Win = win });
                return;
            }
        }

        /// <summary>
        /// 复活英雄。
        /// </summary>
        public void ReviveHeroes()
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                // TODO: 修改为真实网络通信。
                GameEntry.Event.Fire(this, new ReviveHeroesEventArgs());
            }
            else
            {
                GameEntry.Event.Fire(this, new ReviveHeroesEventArgs());
            }
        }


        /// <summary>
        /// 进入资源副本。
        /// </summary>
        /// <param name="instanceForResourceId">资源副本编号。</param>
        public void EnterInstanceForResource(int instanceForResourceId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLEnterInstanceForResource { InstanceForResourceId = instanceForResourceId });
                return;
            }

            var response = new LCEnterInstanceForResource { InstanceForResourceId = instanceForResourceId, PlayedCount = 4, };
            LCEnterInstanceForResourceHandler.Handle(this, response);
        }

        /// <summary>
        /// 离开资源副本。
        /// </summary>
        /// <param name="instanceForResourceId">资源副本编号。</param>
        /// <param name="rewardLevel">奖励等级。</param>
        /// <param name="deadDropCoins">死亡怪掉落金币。</param>
        /// <param name="win">是否胜利。</param>
        public void LeaveInstanceForResource(int instanceForResourceId, int rewardLevel, int deadDropCoins, bool win)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLLeaveInstanceForResource
                {
                    InstanceForResourceId = instanceForResourceId,
                    Win = win,
                    RewardLevel = rewardLevel,
                    DropCoins = deadDropCoins,
                });
                return;
            }

            var response = new LCLeaveInstanceForResource { InstanceForResourceId = instanceForResourceId, Win = win, RewardCoin = 1000 };
            response.PlayerInfo = new PBPlayerInfo { Id = GameEntry.Data.Player.Id, Coin = GameEntry.Data.Player.Coin + response.RewardCoin };

            response.CompoundItemInfos.Add(new PBCompoundItemInfo
            {
                ItemInfo = new PBItemInfo { Type = 202101, Count = 100 },
            });
            response.CompoundItemInfos.Add(new PBCompoundItemInfo
            {
                ItemInfo = new PBItemInfo { Type = 202202, Count = 50 },
            });

            LCLeaveInstanceForResourceHandler.Handle(this, response);
        }

        /// <summary>
        /// 进入模拟乱斗副本。
        /// </summary>
        /// <param name="mimicMeleeInstanceId">模拟乱斗副本编号。</param>
        public void EnterMimicMeleeInstance(int mimicMeleeInstanceId)
        {
            if (GameEntry.Procedure.CurrentProcedure is ProcedureMain)
            {
                GameEntry.Event.Fire(this, new WillChangeSceneEventArgs(InstanceLogicType.MimicMelee, mimicMeleeInstanceId, true));
            }
        }
    }
}
