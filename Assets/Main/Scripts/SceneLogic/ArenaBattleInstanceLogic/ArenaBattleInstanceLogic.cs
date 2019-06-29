using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技战斗副本逻辑。
    /// </summary>
    public partial class ArenaBattleInstanceLogic : BasePvpaiInstanceLogic
    {
        public override InstanceLogicType Type
        {
            get
            {
                return InstanceLogicType.ArenaBattle;
            }
        }

        protected override int OppPlayerId
        {
            get
            {
                return GameEntry.Data.OfflineArenaOpponent.Key;
            }
        }

        protected override PlayerHeroesData PrepareMyHeroesData(UnityEngine.Vector2 spawnPosition, float spawnRotation)
        {
            var heroesData = new PlayerHeroesData();
            var heroTypes = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Arena).HeroType;
            for (int i = 0; i < heroTypes.Count; i++)
            {
                if (heroTypes[i] <= 0)
                {
                    continue;
                }

                var instanceHeroData = GetHeroData(GameEntry.Entity.GetSerialId(), heroTypes[i], spawnPosition, spawnRotation);
                heroesData.Add(instanceHeroData);
            }
            return heroesData;
        }

        protected override PlayerHeroesData PrepareOppHeroesData()
        {
            var heroesData = new PlayerHeroesData();

            var lobbyHeroesData = GameEntry.Data.OfflineArenaOpponent.Heroes.Data;
            for (int i = 0; i < lobbyHeroesData.Count; ++i)
            {
                var instanceHeroData = GetHeroData(GameEntry.Entity.GetSerialId(), lobbyHeroesData[i], false, CampType.Enemy, Vector2.zero, 0f);
                heroesData.Add(instanceHeroData);
            }

            return heroesData;
        }
    }
}
