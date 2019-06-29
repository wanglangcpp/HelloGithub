using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        /// <summary>
        /// 请求装备强化升级。
        /// </summary>
        /// <param name="heroId">英雄编号。</param>
        /// <param name="gearId">装备编号。</param>
        public void RequestNewGearStrengthen(int heroId, int gearId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLNewGearStrengthen { HeroId = heroId, GearTypeId = gearId });
                return;
            }

            var lobbyHero = GameEntry.Data.LobbyHeros.GetData(heroId);
            var response = new LCNewGearStrengthen
            {
                PlayerInfo = new PBPlayerInfo
                {
                    Id = GameEntry.Data.Player.Id,
                    Coin = Mathf.Max(GameEntry.Data.Player.Coin - 1000, 0),
                },

                HeroInfo = new PBLobbyHeroInfo
                {
                    Type = heroId,
                },
            };

            var gears = lobbyHero.NewGears.Data;
            for (int i = 0; i < gears.Count; ++i)
            {
                var gear = gears[i];
                response.HeroInfo.NewGearInfos.Add(new PBNewGearInfo
                {
                    TypeId = gear.Type,
                    StrengthenLevel = gearId == gear.Type ? Mathf.Min(gear.StrengthenLevel + 1, 20) : gear.StrengthenLevel,
                    TotalQualityLevel = gear.StrengthenLevel,
                });
            }

            LCNewGearStrengthenHandler.Handle(this, response);
        }

        /// <summary>
        /// 请求装备品质升级。
        /// </summary>
        /// <param name="heroId"></param>
        /// <param name="gearId"></param>
        public void RequestNewGearQualityLevelUp(int heroId, int gearId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLNewGearQualityLevelUp { HeroId = heroId, GearTypeId = gearId });
                return;
            }

            var lobbyHero = GameEntry.Data.LobbyHeros.GetData(heroId);
            var response = new LCNewGearQualityLevelUp
            {
                HeroInfo = new PBLobbyHeroInfo
                {
                    Type = heroId,
                },
            };

            var gears = lobbyHero.NewGears.Data;
            for (int i = 0; i < gears.Count; ++i)
            {
                var gear = gears[i];
                response.HeroInfo.NewGearInfos.Add(new PBNewGearInfo
                {
                    TypeId = gear.Type,
                    StrengthenLevel = gear.StrengthenLevel,
                    TotalQualityLevel = gearId == gear.Type ? Mathf.Min(gear.TotalQualityLevel + 1, 20) : gear.TotalQualityLevel,
                });
            }

            LCNewGearQualityLevelUpHandler.Handle(this, response);
        }
    }
}
