using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 翻翻棋战斗副本逻辑。
    /// </summary>
    public partial class ChessBattleInstanceLogic : BasePvpaiInstanceLogic
    {
        public override InstanceLogicType Type
        {
            get
            {
                return InstanceLogicType.ChessBattle;
            }
        }

        protected override int OppPlayerId
        {
            get
            {
                return GameEntry.Data.ChessBattleEnemy.Player.Id;
            }
        }

        protected override PlayerHeroesData PrepareMyHeroesData(Vector2 spawnPosition, float spawnRotation)
        {
            var heroesData = new PlayerHeroesData();
            var heroTypes = GameEntry.Data.ChessBattleMe.HeroTeam.HeroType;
            for (int i = 0; i < heroTypes.Count; i++)
            {
                if (heroTypes[i] <= 0)
                {
                    continue;
                }

                var instanceHeroData = GetHeroData(GameEntry.Entity.GetSerialId(), heroTypes[i], spawnPosition, spawnRotation);
                var heroStatus = GameEntry.Data.ChessBattleMe.HeroesStatus.GetData(heroTypes[i]);
                if (heroStatus != null)
                {
                    instanceHeroData.HP = heroStatus.CurHP;
                }
                heroesData.Add(instanceHeroData);
            }
            return heroesData;
        }

        protected override PlayerHeroesData PrepareOppHeroesData()
        {
            var heroesData = new PlayerHeroesData();

            var lobbyHeroesData = GameEntry.Data.ChessBattleEnemy.HeroesData.Data;
            for (int i = 0; i < lobbyHeroesData.Count; ++i)
            {
                var instanceHeroData = GetHeroData(GameEntry.Entity.GetSerialId(), lobbyHeroesData[i], false, CampType.Enemy, Vector2.zero, 0f);
                var heroStatus = GameEntry.Data.ChessBattleEnemy.HeroesStatus.GetData(lobbyHeroesData[i].Type);
                if (heroStatus != null)
                {
                    if (heroStatus.CurHP <= 0)
                    {
                        continue;
                    }

                    instanceHeroData.HP = heroStatus.CurHP;
                }

                heroesData.Add(instanceHeroData);
            }

            return heroesData;
        }
    }
}
