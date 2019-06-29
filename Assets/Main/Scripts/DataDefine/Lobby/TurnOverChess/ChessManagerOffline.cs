using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Genesis.GameClient
{
    public class ChessManagerOffline : ChessManagerBase
    {
        private const int GrayFieldCount = 85;
        private const int RedFieldCount = 5;
        private const int YellowFieldCount = 5;
        private const int GreenFieldCount = 5;

        internal ChessManagerOffline()
        {

        }

        public override void RefreshChessBoard()
        {
            if (HasData)
            {
                m_EventComponent.Fire(this, new ChessBoardRefreshSuccessEventArgs());
                return;
            }

            RemainingResetCount = 1;
            RemainingChanceCount = 15;
            ChessBoardColCount = 10;
            ChessBoardRowCount = 10;
            GenerateChessBoard();
            UpdateHeroTeam();
            m_EventComponent.Fire(this, new ChessBoardRefreshSuccessEventArgs());
        }

        public override void ResetChessBoard()
        {
            if (RemainingResetCount <= 0)
            {
                m_EventComponent.Fire(this, new ChessBoardRefreshFailureEventArgs());
                return;
            }

            ClearChessBoard();
            RemainingResetCount = 0;
            RemainingChanceCount = 15;
            ChessBoardColCount = 10;
            ChessBoardRowCount = 10;
            GenerateChessBoard();
            UpdateHeroTeam();
            m_EventComponent.Fire(this, new ChessBoardRefreshSuccessEventArgs());
        }

        public override void GetEnemyData(int chessFieldIndex)
        {
            if (!(this[chessFieldIndex] is BattleChessField))
            {
                m_EventComponent.Fire(this, new ChessBoardGetEnemyDataFailureEventArgs(chessFieldIndex));
            }

            FakeEnemyData(chessFieldIndex);
            m_EventComponent.Fire(this, new ChessBoardGetEnemyDataSuccessEventArgs(chessFieldIndex));
        }

        protected override void SendOpenNormalChessFieldMessage(int index)
        {
            m_EventComponent.Fire(this, new ChessBoardOpenChessFieldSuccessEventArgs());
        }

        protected override void SendOpenBattleChessFieldMessage(int index, bool battleWon)
        {
            m_EventComponent.Fire(this, new ChessBoardOpenChessFieldSuccessEventArgs());
        }

        protected override void SendBombMessage()
        {
            m_EventComponent.Fire(this, new ChessBoardBombSuccessEventArgs());
        }

        private void GenerateChessBoard()
        {
            var weightList = new List<int> { GrayFieldCount, RedFieldCount, YellowFieldCount, GreenFieldCount };
            for (int i = 0; i < ChessBoardCount; i++)
            {
                int totalWeight = 0;
                for (int j = 0; j < weightList.Count; ++j)
                {
                    totalWeight += weightList[j];
                }

                int randomValue = Random.Range(0, totalWeight);
                int colorId = 0;
                for (int j = 0; j < weightList.Count; j++)
                {
                    randomValue -= weightList[j];
                    if (randomValue < 0)
                    {
                        colorId = j + 1;
                        break;
                    }
                }

                ChessField chessField;

                switch (colorId)
                {
                    case (int)ChessFieldColor.Red:
                        chessField = new BattleChessField(ChessFieldColor.Red, false, Random.Range(8, 11), null);
                        break;
                    case (int)ChessFieldColor.Yellow:
                        chessField = new BattleChessField(ChessFieldColor.Yellow, false, Random.Range(5, 8), null);
                        break;
                    case (int)ChessFieldColor.Green:
                        chessField = new BattleChessField(ChessFieldColor.Green, false, Random.Range(2, 5), null);
                        break;
                    default:
                        chessField = new NormalChessField(ChessFieldColor.Gray, false, false, 0);
                        break;
                }

                AddChessField(chessField);
                weightList[colorId - 1]--;
            }
        }

        private void UpdateHeroTeam()
        {
            var packet = new LCGetChessBoard();
            GameEntry.Data.ChessBattleMe.UpdateData(packet);
        }

        private void FakeEnemyData(int chessFieldIndex)
        {
            var enemyData = m_EnemyDataGetter();
            var fakePacket = new LCGetChessEnemyInfo
            {
                Success = true,
                Anger = 10,
                EnemyInfo = new PBPlayerInfo
                {
                    Id = 999999,
                    Name = "Fake Chess Enemy",
                    Level = 10,
                    Exp = 10,
                    VipLevel = 2,
                    VipExp = 250,
                },
                ChessFieldIndex = chessFieldIndex,
            };

            var heroesInfo = fakePacket.HeroesInfo;
            for (int i = 1; i <= 3; ++i)
            {
                var heroInfo = new PBLobbyHeroInfo
                {
                    Type = i,
                    Level = 10 + i,
                    Exp = 55 + i,
                    StarLevel = i,
                    ConsciousnessLevel = i,
                    ElevationLevel = i,
                };
                heroInfo.SkillLevels.AddRange(new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
                heroesInfo.Add(heroInfo);
            }

            var heroesStatus = fakePacket.HeroesStatus;
            heroesStatus.Add(new PBLobbyHeroStatus
            {
                Type = 2,
                CurHP = 4000,
            });

            enemyData.UpdateData(fakePacket);
        }

    }
}
