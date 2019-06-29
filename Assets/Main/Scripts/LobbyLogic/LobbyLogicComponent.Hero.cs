using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        /// <summary>
        /// 请求改变出战队伍。
        /// </summary>
        /// <param name="heroTeam">新的出战队伍。</param>
        public void ChangeHeroTeam(List<int> heroTeam, HeroTeamType type)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLChangeHeroTeam request = new CLChangeHeroTeam();
                request.HeroTeamInfo = new PBHeroTeamInfo();
                request.HeroTeamInfo.HeroType.AddRange(heroTeam);
                request.HeroTeamInfo.Type = (int)type;
                GameEntry.Network.Send(request);
            }
            else
            {
                var heroTypes = GameEntry.Data.HeroTeams.GetData((int)type).HeroType;
                heroTypes.Clear();
                heroTypes.AddRange(heroTeam);
                GameEntry.Event.Fire(this, new HeroTeamDataChangedEventArgs(type));
            }
        }

        /// <summary>
        /// 请求改变翻翻棋的出战队伍。
        /// </summary>
        /// <param name="heroTeam">新的出战队伍。</param>
        public void ChangeChessBattleHeroTeam(List<int> heroTeam)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLChangeHeroTeamChess request = new CLChangeHeroTeamChess();
                request.HeroTeamInfo = new PBHeroTeamInfo();
                request.HeroTeamInfo.HeroType.AddRange(heroTeam);
                GameEntry.Network.Send(request);
            }
            else
            {
                GameEntry.Data.ChessBattleMe.HeroTeam.HeroType.Clear();
                GameEntry.Data.ChessBattleMe.HeroTeam.HeroType.AddRange(heroTeam);
                GameEntry.Event.Fire(this, new HeroTeamDataChangedChessBattleEventArgs());
            }
        }

        /// <summary>
        /// 合成英雄。
        /// </summary>
        /// <param name="heroType">英雄类型编号。</param>
        public void ComposeHero(int heroType)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLComposeHero { HeroType = heroType, });
                return;
            }
        }

        /// <summary>
        /// 英雄技能升级。
        /// </summary>
        /// <param name="heroType">英雄类型编号。</param>
        /// <param name="skillIndex">技能索引号。</param>
        public void HeroSkillLevelUp(int heroType, int skillIndex)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLUpgradeSkill request = new CLUpgradeSkill();
                request.HeroType = heroType;
                request.SkillIndex = skillIndex;
                GameEntry.Network.Send(request);
            }
        }

        /// <summary>
        /// 钻石开启技能徽章位。
        /// </summary>
        /// <param name="heroId">英雄类型编号。</param>
        /// <param name="skillIndex">技能索引号。</param>
        /// <param name="badgeSlotCategory">徽章位类型。</param>
        /// <param name="genericBadgeSlotIndex">通用徽章位索引。</param>
        public void ActivateSkillBadgeSlot(int heroId, int skillIndex, int badgeSlotCategory, int genericBadgeSlotIndex)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLActivateSkillBadgeSlotWithMoney request = new CLActivateSkillBadgeSlotWithMoney();
                request.HeroId = heroId;
                request.SkillIndex = skillIndex;
                request.BadgeSlotCategory = badgeSlotCategory;
                request.GenericBadgeSlotIndex = genericBadgeSlotIndex;
                GameEntry.Network.Send(request);
            }
        }

        /// <summary>
        /// 装备徽章。
        /// </summary>
        /// <param name="heroId">英雄类型编号。</param>
        /// <param name="skillIndex">技能索引号。</param>
        /// <param name="badgeSlotCategory">徽章位类型。</param>
        /// <param name="genericBadgeSlotIndex">通用徽章位索引。</param>
        /// <param name="badgeId">徽章编号。</param>
        public void InsertSkillBadge(int heroId, int skillIndex, int badgeSlotCategory, int genericBadgeSlotIndex, int badgeId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLInsertSkillBadge request = new CLInsertSkillBadge();
                request.HeroId = heroId;
                request.SkillIndex = skillIndex;
                request.BadgeSlotCategory = badgeSlotCategory;
                request.GenericBadgeSlotIndex = genericBadgeSlotIndex;
                request.BadgeId = badgeId;
                GameEntry.Network.Send(request);
            }
        }

        /// <summary>
        /// 卸下徽章。
        /// </summary>
        /// <param name="heroId">英雄类型编号。</param>
        /// <param name="skillIndex">技能索引号。</param>
        /// <param name="badgeSlotCategory">徽章位类型。</param>
        /// <param name="genericBadgeSlotIndex">通用徽章位索引。</param>
        public void RemoveSkillBadge(int heroId, int skillIndex, int badgeSlotCategory, int genericBadgeSlotIndex)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLRemoveSkillBadge request = new CLRemoveSkillBadge();
                request.HeroId = heroId;
                request.SkillIndex = skillIndex;
                request.BadgeSlotCategory = badgeSlotCategory;
                request.GenericBadgeSlotIndex = genericBadgeSlotIndex;
                GameEntry.Network.Send(request);
            }
        }

        public void LevelUpSkillBadge(bool isOpenThisFromHeroPanel, int sourceBadgeId, int confirmAdvanceCount, int heroId = 0, int skillIndex = 0, int badgeSlotCateGory = 0, int genericBadgeSlotIndex = 0)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                if (isOpenThisFromHeroPanel)
                {
                    CLLevelUpSkillBadge request = new CLLevelUpSkillBadge();
                    request.SourceBadgeId = sourceBadgeId;
                    request.LevelUpBadgeCount = confirmAdvanceCount;
                    request.HeroId = heroId;
                    request.SkillIndex = skillIndex;
                    request.BadgeSlotCategory = badgeSlotCateGory;
                    request.GenericBadgeSlotIndex = genericBadgeSlotIndex;
                    GameEntry.Network.Send(request);
                }
                else
                {
                    CLLevelUpSkillBadge request = new CLLevelUpSkillBadge();
                    request.SourceBadgeId = sourceBadgeId;
                    request.LevelUpBadgeCount = confirmAdvanceCount;
                    GameEntry.Network.Send(request);
                }
            }
        }

        /// <summary>
        /// 请求提升英雄品阶。
        /// </summary>
        /// <param name="heroType">英雄类型编号。</param>
        public void RequestIncreaseHeroQuality(int heroType)
        {
            var lobbyHeroData = GameEntry.Data.LobbyHeros.GetData(heroType);
            if (lobbyHeroData == null)
            {
                throw new System.ArgumentException(string.Format("Lobby hero '{0}' not found.", heroType.ToString()));
            }

            var dt = GameEntry.DataTable.GetDataTable<DRHeroQualityLevel>();
            var dr = dt[lobbyHeroData.QualityLevelId];
            var nextId = dr.NextId;

            // Has reached quality level already.
            if (nextId <= 0)
            {
                throw new System.NotSupportedException(string.Format("Lobby hero '{0}' has reached the top quality level.", heroType.ToString()));
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                var request = new CLIncreaseHeroQualityLevel
                {
                    HeroTypeId = heroType,
                };

                GameEntry.Network.Send(request);
                return;
            }

            var nextDR = dt[nextId];
            LCIncreaseHeroQualityLevelHandler.Handle(this, new LCIncreaseHeroQualityLevel
            {
                HeroInfo = new PBLobbyHeroInfo
                {
                    Type = heroType,
                    TotalQualityLevel = nextDR.QualityLevel,
                },
            });
        }

        /// <summary>
        /// 请求放入英雄升品道具。
        /// </summary>
        /// <param name="heroType">英雄类型编号。</param>
        /// <param name="qualityItemSlotIndex">道具槽索引号。</param>
        public void RequestUseHeroQualityItem(int heroType, int qualityItemSlotIndex)
        {
            if (qualityItemSlotIndex < 0 || qualityItemSlotIndex >= Constant.HeroQualityLevelItemSlotCount)
            {
                throw new System.ArgumentOutOfRangeException("Slot index '{0}' is illegal.", qualityItemSlotIndex.ToString());
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                var request = new CLUseHeroQualityItem
                {
                    HeroTypeId = heroType,
                    QualityItemSlotIndex = qualityItemSlotIndex,
                };

                GameEntry.Network.Send(request);
                return;
            }

            var response = new LCUseHeroQualityItem
            {
                HeroInfo = new PBLobbyHeroInfo
                {
                    Type = heroType,
                },
            };

            response.HeroInfo.QualityItemSlots.AddRange(new bool[] { true, false, true, false, true, false });
            LCUseHeroQualityItemHandler.Handle(this, response);
        }

        /// <summary>
        /// 请求合成英雄升品道具。
        /// </summary>
        /// <param name="targetItemId">要合成的英雄升品道具的编号。</param>
        public void RequestSynthesizeHeroQualityItem(int targetItemId)
        {
            if (targetItemId < Constant.GeneralItem.MinHeroQualityItemId || targetItemId > Constant.GeneralItem.MaxHeroQualityItemId)
            {
                throw new System.ArgumentOutOfRangeException(string.Format("Illegal hero quality item id '{0}'.", targetItemId.ToString()));
            }

            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                var request = new CLSynthesizeHeroQualityItem
                {
                    TargetItemId = targetItemId,
                };

                GameEntry.Network.Send(request);
                return;
            }

            var response = new LCSynthesizeHeroQualityItem { };
            int itemCount = 0;
            var itemData = GameEntry.Data.HeroQualityItems.GetData(targetItemId);
            if (itemData != null)
            {
                itemCount = itemData.Count;
            }

            response.HeroQualityItemInfos.Add(new PBItemInfo { Type = targetItemId, Count = itemCount + 1 });
            LCSynthesizeHeroQualityItemHandler.Handle(this, response);
        }
    }
}
