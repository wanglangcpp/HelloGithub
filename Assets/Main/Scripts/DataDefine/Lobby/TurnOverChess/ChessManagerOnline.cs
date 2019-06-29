using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class ChessManagerOnline : ChessManagerBase
    {
        private const string LastRefreshTime = "ChessManagerOnline.LastRefreshTime";

        private bool NeedRefresh
        {
            get
            {
                var cachedTimeStr = GameEntry.Setting.GetString(LastRefreshTime);
                DateTime lastRefreshTime;
                if (string.IsNullOrEmpty(cachedTimeStr))
                {
                    lastRefreshTime = DateTime.MinValue;
                }
                else
                {
                    lastRefreshTime = DateTime.Parse(cachedTimeStr);
                }

                return DateTime.UtcNow.Date > lastRefreshTime.Date;
            }
        }

        internal ChessManagerOnline()
        {

        }

        public override void RefreshChessBoard()
        {
            if (HasData && !NeedRefresh)
            {
                m_EventComponent.Fire(this, new ChessBoardRefreshSuccessEventArgs());
                return;
            }

            var packet = new CLGetChessBoard { IsReset = false };
            GameEntry.Network.Send(packet);
        }

        public override void ResetChessBoard()
        {
            if (RemainingResetCount <= 0)
            {
                m_EventComponent.Fire(this, new ChessBoardRefreshFailureEventArgs());
                return;
            }

            var packet = new CLGetChessBoard { IsReset = true };
            GameEntry.Network.Send(packet);
        }

        public override void GetEnemyData(int chessFieldIndex)
        {
            if (!(this[chessFieldIndex] is BattleChessField))
            {
                m_EventComponent.Fire(this, new ChessBoardGetEnemyDataFailureEventArgs(chessFieldIndex));
            }

            var packet = new CLGetChessEnemyInfo { ChessFieldIndex = chessFieldIndex };
            GameEntry.Network.Send(packet);
        }

        protected override void OnChessBoardRefreshSuccessInternal(object o, GameEventArgs e)
        {
            var ne = e as ChessBoardRefreshSuccessEventArgs;
            var packet = ne.Packet;
            if (ne.Packet == null)
            {
                base.OnChessBoardRefreshSuccessInternal(o, e);
                return;
            }

            ClearChessBoard();

            RemainingResetCount = packet.TokenCount;
            RemainingChanceCount = packet.PlayCount;
            ChessBoardColCount = packet.Width;
            ChessBoardRowCount = packet.ChessBoard.Count / ChessBoardColCount;
            StarEnergyObtained = packet.GotStarEnergy;
            CoinObtained = packet.GotCoin;
            MoneyObtained = packet.GotMoney;

            for (int i = 0; i < packet.GotItems.Count; ++i)
            {
                m_GoodsObtained.Add(new KeyValuePair<int, int>(packet.GotItems[i].Type, packet.GotItems[i].Count));
            }

            for (int i = 0; i < packet.GotGears.Count; ++i)
            {
                m_GoodsObtained.Add(new KeyValuePair<int, int>(packet.GotGears[i].Type, 1));
            }

            for (int i = 0; i < packet.GotEpigraphs.Count; ++i)
            {
                m_GoodsObtained.Add(new KeyValuePair<int, int>(packet.GotEpigraphs[i].Type, 1));
            }

            for (int i = 0; i < packet.GotSouls.Count; ++i)
            {
                m_GoodsObtained.Add(new KeyValuePair<int, int>(packet.GotSouls[i].Type, 1));
            }

            for (int i = 0; i < packet.ChessBoard.Count; ++i)
            {
                var chessFieldRawData = packet.ChessBoard[i];
                if (chessFieldRawData.Color == (int)ChessFieldColor.Gray)
                {
                    int parentId = chessFieldRawData.HasParent ? chessFieldRawData.Parent : -1;
                    AddChessField(new NormalChessField(ChessFieldColor.Gray, chessFieldRawData.IsOpened, chessFieldRawData.IsFree, parentId));
                }
                else
                {
                    int freeCount = chessFieldRawData.HasFreeCount ? chessFieldRawData.FreeCount : -1;
                    AddChessField(new BattleChessField((ChessFieldColor)chessFieldRawData.Color, chessFieldRawData.IsOpened, freeCount, chessFieldRawData.Children));
                }
            }

            GameEntry.Setting.SetString(LastRefreshTime, DateTime.UtcNow.ToString());
            GameEntry.Setting.Save();
            base.OnChessBoardRefreshSuccessInternal(o, e);
        }

        protected override void OnChessFieldOpenSuccessInternal(object o, GameEventArgs e)
        {
            var ne = e as ChessBoardOpenChessFieldSuccessEventArgs;
            if (m_OpenBattleChessFieldClosure != null)
            {
                var index = m_OpenBattleChessFieldClosure.Index;
                var battleChessField = this[index] as BattleChessField;

                var children = new List<int>();
                for (int i = 0; i < battleChessField.ChildCount; ++i)
                {
                    children.Add(battleChessField.GetChildId(i));
                }
                this[index] = new BattleChessField(battleChessField.Color, battleChessField.IsOpened, ne.Packet.FreeCount, children);
            }

            var packet = ne.Packet;
            StarEnergyObtained += packet.RewardStarEnergy;
            CoinObtained += packet.RewardCoins;
            MoneyObtained += packet.RewardMoney;

            ne.ReceiveGoodsData = CollectRewards(packet.RewardCoins, packet.RewardMoney, packet.RewardStarEnergy,
                packet.ReceivedItems == null ? null : packet.ReceivedItems.ItemInfo,
                packet.ReceivedItems == null ? null : packet.ReceivedItems.GearInfo,
                packet.ReceivedItems == null ? null : packet.ReceivedItems.SoulInfo,
                packet.ReceivedItems == null ? null : packet.ReceivedItems.EpigraphInfo);
            base.OnChessFieldOpenSuccessInternal(o, e);
        }

        protected override void OnBombSuccessInternal(object o, GameEventArgs e)
        {
            var ne = e as ChessBoardBombSuccessEventArgs;
            var packet = ne.Packet;
            StarEnergyObtained += packet.RewardStarEnergy;
            CoinObtained += packet.RewardCoins;
            MoneyObtained += packet.RewardMoney;

            CollectRewards(packet.RewardCoins, packet.RewardMoney, packet.RewardStarEnergy, packet.ReceivedItems.ItemInfo, packet.ReceivedItems.GearInfo, packet.ReceivedItems.SoulInfo, packet.ReceivedItems.EpigraphInfo);
            base.OnBombSuccessInternal(o, e);
        }

        protected override void SendOpenNormalChessFieldMessage(int index)
        {
            GameEntry.Network.Send(CreateOpenChessFieldRequest(index));
        }

        protected override void SendOpenBattleChessFieldMessage(int index, bool battleWon)
        {

            var instanceLogic = GameEntry.SceneLogic.BasePvpaiInstanceLogic;

            if (instanceLogic == null)
            {
                Log.Error("Logic of player v.s. player AI instance is invalid.");
                return;
            }

            var packet = CreateOpenChessFieldRequest(index);
            packet.BattleWon = battleWon;

            var oppHeroesData = instanceLogic.OppHeroesData;
            packet.EnemyAnger = 0; // TODO: Use real data

            var oppHeroes = oppHeroesData.GetHeroes();
            for (int i = 0; i < oppHeroes.Length; ++i)
            {
                packet.EnemyHeroesHP.Add(oppHeroes[i].HP);
            }

            var myHeroesData = instanceLogic.MyHeroesData;
            packet.MyAnger = 0; // TODO: Use real data

            var myHeroes = myHeroesData.GetHeroes();
            for (int i = 0; i < myHeroes.Length; ++i)
            {
                packet.MyHeroesStatus.Add(new PBLobbyHeroStatus
                {
                    Type = myHeroes[i].HeroId,
                    CurHP = myHeroes[i].HP,
                });
            }

            GameEntry.Network.Send(packet);
        }

        protected override void SendBombMessage()
        {
            var packet = new CLBombChessBoard();
            GameEntry.Network.Send(packet);
        }

        private CLOpenChessField CreateOpenChessFieldRequest(int chessFieldIndex)
        {
            var packet = new CLOpenChessField { FieldId = chessFieldIndex };
            for (int i = 0; i < m_CachedChangeList.Count; ++i)
            {
                var index = m_CachedChangeList[i];
                var field = this[index];

                var battleField = field as BattleChessField;
                if (battleField != null)
                {
                    var pb = new PBChessField
                    {
                        Color = (int)field.Color,
                        IsOpened = field.IsOpened,
                        FreeCount = battleField.RemainingCount,
                        Index = index,
                    };

                    for (int j = 0; j < battleField.ChildCount; ++j)
                    {
                        pb.Children.Add(battleField.GetChildId(j));
                    }

                    packet.ModifiedChessField.Add(pb);
                    continue;
                }

                var normalField = field as NormalChessField;
                if (normalField != null)
                {
                    packet.ModifiedChessField.Add(new PBChessField
                    {
                        Color = (int)field.Color,
                        IsOpened = field.IsOpened,
                        IsFree = normalField.IsFree,
                        Index = index,
                        Parent = normalField.ParentId,
                    });
                    continue;
                }
            }

            return packet;
        }

        protected void OnPerItemAdded(int typeId, int deltaCount)
        {
            bool found = false;
            for (int j = 0; j < m_GoodsObtained.Count; ++j)
            {
                if (m_GoodsObtained[j].Key == typeId)
                {
                    m_GoodsObtained[j] = new KeyValuePair<int, int>(m_GoodsObtained[j].Key, deltaCount + m_GoodsObtained[j].Value);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                m_GoodsObtained.Add(new KeyValuePair<int, int>(typeId, deltaCount));
            }
        }

        protected void OnPerOtherGoodsAdded(int typeId)
        {
            m_GoodsObtained.Add(new KeyValuePair<int, int>(typeId, 1));
        }

        protected ReceivedGeneralItemsViewData CollectRewards(int coins, int money, int starEnergy,
            IList<PBItemInfo> rewardItems, IList<PBGearInfo> rewardGears, IList<PBSoulInfo> rewardSouls, IList<PBEpigraphInfo> rewardEpigraphs)
        {
            var helper = new RewardCollectionHelper();
            helper.OnPerItemAdded += OnPerItemAdded;
            helper.OnPerGearAdded += OnPerOtherGoodsAdded;
            helper.OnPerSoulAdded += OnPerOtherGoodsAdded;
            helper.OnPerEpigraphAdded += OnPerOtherGoodsAdded;
            helper.SetCurrency(CurrencyType.Coin, coins);
            helper.SetCurrency(CurrencyType.Money, money);
            helper.SetCurrency(CurrencyType.MeridianToken, starEnergy);
            helper.AddItems(rewardItems);
            return helper.ReceiveGoodsData;
        }
    }
}
