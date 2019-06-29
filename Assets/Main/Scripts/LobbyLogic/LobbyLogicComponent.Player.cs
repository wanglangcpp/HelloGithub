using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void ChangePlayerPortrait(int newPortraitId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLChangePortrait { PortraitType = newPortraitId });
                return;
            }

            var playerData = GameEntry.Data.Player;

            var response = new LCChangePortrait
            {
                PlayerInfo = new PBPlayerInfo
                {
                    Id = playerData.Id,
                    PortraitType = newPortraitId,
                }
            };

            LCChangePortraitHandler.Handle(this, response);
        }

        public void GetPlayerInfo(int playerId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                var msg = new CLGetPlayerDetail
                {
                    PlayerId = playerId,
                };

                GameEntry.Network.Send(msg);
                return;
            }

            var response = new LCGetPlayerDetail
            {
                //PlayerInfo = new PBPlayerInfo
                //{
                //    Id = playerId,
                //    DisplayId = 12345678,
                //    Name = "Fake other player",
                //},
            };

            var heroTypes = new int[] { 1, 2, 16 };

            for (int i = 0; i < heroTypes.Length; ++i)
            {
                //var hero = new PBLobbyHeroInfo
                //{
                //    Type = heroTypes[i],
                //    Level = 1 + 1,
                //    StarLevel = 1 + 1,
                //};

                //hero.SkillLevels.AddRange(Constant.DefaultHeroSkillLevels);
                //response.HeroTeam.Add(hero);
            }

            LCGetPlayerDetailHandler.Handle(this, response);
        }

        public void GetPlayerOnlineStatus(List<int> playerIds)
        {
            if (playerIds.Count <= 0)
            {
                return;
            }
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                var msg = new CLGetOnlineStatus();
                msg.PlayerId.AddRange(playerIds);
                GameEntry.Network.Send(msg);
            }
        }
    }
}
