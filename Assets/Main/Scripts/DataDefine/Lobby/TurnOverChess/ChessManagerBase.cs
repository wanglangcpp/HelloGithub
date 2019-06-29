using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using System.Text;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public abstract class ChessManagerBase : IChessManager
    {
        protected class OpenBattleChessFieldClosure
        {
            public int Index { get; private set; }
            public bool BattleWon { get; private set; }

            public OpenBattleChessFieldClosure(int index, bool battleWon)
            {
                Index = index;
                BattleWon = battleWon;
            }
        }

        protected OpenBattleChessFieldClosure m_OpenBattleChessFieldClosure = null;

        private List<ChessField> m_ChessBoard = new List<ChessField>();
        private HashSet<int> m_FreeFieldIndices = new HashSet<int>();
        protected EventComponent m_EventComponent;
        protected List<KeyValuePair<int, int>> m_GoodsObtained = new List<KeyValuePair<int, int>>();
        protected IList<int> m_CachedChangeList = null;
        protected GameFrameworkFunc<ChessBattleEnemyData> m_EnemyDataGetter = null;

        public int RemainingResetCount { get; protected set; }

        public int RemainingChanceCount { get; protected set; }

        public int ChessBoardColCount { get; protected set; }

        public int ChessBoardRowCount { get; protected set; }

        public int StarEnergyObtained { get; protected set; }

        public int CoinObtained { get; protected set; }

        public int MoneyObtained { get; protected set; }

        public KeyValuePair<int, int>[] GoodsObtained
        {
            get
            {
                return m_GoodsObtained.ToArray();
            }
        }

        public bool HasData
        {
            get
            {
                return m_ChessBoard.Count > 0;
            }
        }

        public int FreeFieldCount
        {
            get
            {
                return m_FreeFieldIndices.Count;
            }
        }

        public int ChessBoardCount
        {
            get
            {
                return ChessBoardColCount * ChessBoardRowCount;
            }
        }

        public bool IsComplete
        {
            get
            {
                for (int i = 0; i < ChessBoardCount; ++i)
                {
                    var normalChessField = this[i] as NormalChessField;
                    if (normalChessField != null && !normalChessField.IsOpened)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public ChessField this[int index]
        {
            get
            {
                return m_ChessBoard[index];
            }

            protected set
            {
                var oldNormalField = m_ChessBoard[index] as NormalChessField;
                var newNormalField = value as NormalChessField;

                if (IsAvailbleFreeField(oldNormalField) && !IsAvailbleFreeField(newNormalField))
                {
                    m_FreeFieldIndices.Remove(index);
                }
                else if (!IsAvailbleFreeField(oldNormalField) && IsAvailbleFreeField(newNormalField))
                {
                    m_FreeFieldIndices.Add(index);
                }

                m_ChessBoard[index] = value;
            }
        }

        public virtual void Init(EventComponent eventComponent, GameFrameworkFunc<ChessBattleEnemyData> enemyDataGetter)
        {
            m_EventComponent = eventComponent;
            m_EventComponent.Subscribe(EventId.ChessBoardRefreshSuccess, OnChessBoardRefreshSuccessInternal);
            m_EventComponent.Subscribe(EventId.ChessBoardRefreshFailure, OnChessBoardRefreshFailureInternal);
            m_EventComponent.Subscribe(EventId.ChessBoardOpenChessFieldSuccess, OnChessFieldOpenSuccessInternal);
            m_EventComponent.Subscribe(EventId.ChessBoardOpenChessFieldFailure, OnChessFieldOpenFailureInternal);
            m_EventComponent.Subscribe(EventId.ChessBoardBombSuccess, OnBombSuccessInternal);
            m_EventComponent.Subscribe(EventId.ChessBoardBombFailure, OnBombFailureInternal);
            m_EventComponent.Subscribe(EventId.ChessBoardGetEnemyDataSuccess, OnGetEnemyDataSuccessInternal);
            m_EventComponent.Subscribe(EventId.ChessBoardGetEnemyDataFailure, OnGetEnemyDataFailureInternal);

            m_EnemyDataGetter = enemyDataGetter;
        }

        public virtual void ShutDown()
        {
            m_EventComponent.Unsubscribe(EventId.ChessBoardRefreshSuccess, OnChessBoardRefreshSuccessInternal);
            m_EventComponent.Unsubscribe(EventId.ChessBoardRefreshFailure, OnChessBoardRefreshFailureInternal);
            m_EventComponent.Unsubscribe(EventId.ChessBoardOpenChessFieldSuccess, OnChessFieldOpenSuccessInternal);
            m_EventComponent.Unsubscribe(EventId.ChessBoardOpenChessFieldFailure, OnChessFieldOpenFailureInternal);
            m_EventComponent.Unsubscribe(EventId.ChessBoardBombSuccess, OnBombSuccessInternal);
            m_EventComponent.Unsubscribe(EventId.ChessBoardBombFailure, OnBombFailureInternal);
            m_EventComponent.Unsubscribe(EventId.ChessBoardGetEnemyDataSuccess, OnGetEnemyDataSuccessInternal);
            m_EventComponent.Unsubscribe(EventId.ChessBoardGetEnemyDataFailure, OnGetEnemyDataFailureInternal);
            m_EventComponent = null;

            m_EnemyDataGetter = null;
        }

        public abstract void RefreshChessBoard();

        public abstract void ResetChessBoard();

        public abstract void GetEnemyData(int chessFieldIndex);

        public void OpenNormalChessField(int index)
        {
            var data = this[index];

            var normalChessField = data as NormalChessField;
            if (normalChessField == null)
            {
                Log.Error("Chess field is not a normal one at index '{0}'", index);
                return;
            }

            if (normalChessField.IsOpened)
            {
                m_EventComponent.Fire(this, new ChessBoardOpenChessFieldFailureEventArgs());
                return;
            }

            if (normalChessField.IsFree)
            {
                m_CachedChangeList = OpenFreeChessField(normalChessField, index);
                SendOpenNormalChessFieldMessage(index);
                return;
            }

            if (RemainingChanceCount <= 0)
            {
                m_EventComponent.Fire(this, new ChessBoardOpenChessFieldFailureEventArgs());
                return;
            }

            this[index] = new NormalChessField(normalChessField.Color, true, false, -1);
            RemainingChanceCount--;
            m_CachedChangeList = new List<int>();
            m_CachedChangeList.Add(index);
            SendOpenNormalChessFieldMessage(index);
        }

        public void OpenBattleChessField(int index, bool win)
        {
            var data = this[index];
            var battleChessField = data as BattleChessField;

            if (battleChessField == null)
            {
                Log.Error("Chess field is not a battle one at index '{0}'", index);
                return;
            }

            if (battleChessField.IsOpened)
            {
                m_EventComponent.Fire(this, new ChessBoardOpenChessFieldFailureEventArgs());
                return;
            }

            if (RemainingChanceCount <= 0)
            {
                m_EventComponent.Fire(this, new ChessBoardOpenChessFieldFailureEventArgs());
                return;
            }

            if (m_OpenBattleChessFieldClosure != null)
            {
                Log.Error("Another battle chess field is being opened, whose index is {0}", m_OpenBattleChessFieldClosure.Index);
                return;
            }

            if (!win)
            {
                m_CachedChangeList = new List<int>();
            }
            else
            {
                m_CachedChangeList = OpenBattleChessField(battleChessField, index);
                RemainingChanceCount--;
            }
            m_OpenBattleChessFieldClosure = new OpenBattleChessFieldClosure(index, win);
            SendOpenBattleChessFieldMessage(index, win);
        }

        public void PerformQuickOpen()
        {
            HashSet<int> changeSet = new HashSet<int>();
            int index;
            while ((index = GetFreeFieldIndex()) >= 0)
            {
                // This may cause redundancy in changeSet, because some of the chess fields are likely to be changed and then reverted.
                changeSet.UnionWith(OpenFreeChessField(this[index] as NormalChessField, index));
            }

            m_CachedChangeList = new List<int>(changeSet);
            SendOpenNormalChessFieldMessage(-1);
        }

        public void Bomb()
        {
            // TODO: Show money popup, calculate money. After the user confirms, charge the money and perform the bomb.

            m_CachedChangeList = new List<int>();

            for (int i = 0; i < ChessBoardCount; ++i)
            {
                var chessField = this[i];
                var normalChessField = chessField as NormalChessField;
                var battleChessField = chessField as BattleChessField;

                if (normalChessField != null)
                {
                    if (!chessField.IsOpened)
                    {
                        this[i] = new NormalChessField(normalChessField.Color, true, false, -1);
                        m_CachedChangeList.Add(i);
                    }

                    continue;
                }

                if (battleChessField != null)
                {
                    if (battleChessField.IsOpened && battleChessField.ChildCount > 0)
                    {
                        this[i] = new BattleChessField(battleChessField.Color, true, battleChessField.RemainingCount, null);
                        m_CachedChangeList.Add(i);
                    }
                    continue;
                }
            }

            RemainingChanceCount = 0;
            SendBombMessage();
        }

        public event GameFrameworkAction OnChessBoardRefreshSuccess;

        public event GameFrameworkAction OnChessBoardRefreshFailure;

        public event GameFrameworkAction<IList<int>, ReceivedGeneralItemsViewData> OnChessFieldOpenSuccess;

        public event GameFrameworkAction OnChessFieldOpenFailure;

        public event GameFrameworkAction<IList<int>> OnBombSuccess;

        public event GameFrameworkAction OnBombFailure;

        public event GameFrameworkAction<int> OnGetEnemyDataSuccess;

        public event GameFrameworkAction<int> OnGetEnemyDataFailure;

        protected int GetLeftNeighbor(int index)
        {
            if (index < 0 || index >= ChessBoardCount || index % ChessBoardColCount == 0)
            {
                return -1;
            }

            return index - 1;
        }

        protected int GetRightNeighbor(int index)
        {
            if (index < 0 || index >= ChessBoardCount || index % ChessBoardColCount == ChessBoardColCount - 1)
            {
                return -1;
            }

            return index + 1;
        }

        protected int GetUpNeighbor(int index)
        {
            if (index < ChessBoardColCount || index >= ChessBoardCount)
            {
                return -1;
            }

            return index - ChessBoardColCount;
        }

        protected int GetDownNeighbor(int index)
        {
            if (index < 0 || index >= ChessBoardCount - ChessBoardColCount)
            {
                return -1;
            }

            return index + ChessBoardColCount;
        }

        protected IList<int> Get4Neighbors(int index)
        {
            List<int> ret = new List<int>();

            int left = GetLeftNeighbor(index);
            if (left >= 0) ret.Add(left);

            int right = GetRightNeighbor(index);
            if (right >= 0) ret.Add(right);

            int up = GetUpNeighbor(index);
            if (up >= 0) ret.Add(up);

            int down = GetDownNeighbor(index);
            if (down >= 0) ret.Add(down);

            return ret;
        }

        protected void ClearChessBoard()
        {
            m_FreeFieldIndices.Clear();
            m_ChessBoard.Clear();
            m_GoodsObtained.Clear();
            RemainingResetCount = 0;
            RemainingChanceCount = 0;
            ChessBoardColCount = 0;
            ChessBoardRowCount = 0;
        }

        protected void AddChessField(ChessField chessField)
        {
            m_ChessBoard.Add(null);
            this[m_ChessBoard.Count - 1] = chessField;
        }

        protected int GetFreeFieldIndex()
        {
            if (FreeFieldCount <= 0)
            {
                return -1;
            }

            var enumerator = m_FreeFieldIndices.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current;
        }

        protected abstract void SendOpenNormalChessFieldMessage(int index);

        protected abstract void SendOpenBattleChessFieldMessage(int index, bool battleWon);

        protected abstract void SendBombMessage();

        protected int ReparentNormalChessField(int index)
        {
            Queue<int> searchQueue = new Queue<int>();
            HashSet<int> searchedFlags = new HashSet<int>();
            searchQueue.Enqueue(index);
            var toReparent = this[index] as NormalChessField;

            while (searchQueue.Count > 0)
            {
                int cur = searchQueue.Dequeue();
                searchedFlags.Add(cur);

                var data = this[cur];
                var battleData = data as BattleChessField;
                if (battleData != null)
                {
                    if (!battleData.IsOpened || battleData.RemainingCount <= 0)
                    {
                        continue;
                    }

                    this[index] = new NormalChessField(toReparent.Color, false, true, cur);

                    var currentChildren = new List<int>();
                    for (int i = 0; i < battleData.ChildCount; ++i)
                    {
                        currentChildren.Add(battleData.GetChildId(i));
                    }
                    currentChildren.Add(index);

                    this[cur] = new BattleChessField(battleData.Color, true, battleData.RemainingCount, currentChildren);
                    return cur;
                }

                var normalData = data as NormalChessField;
                if (normalData != null && (normalData.IsOpened || index == cur))
                {
                    var neighbors = Get4Neighbors(cur);
                    for (int i = 0; i < neighbors.Count; ++i)
                    {
                        if (searchedFlags.Contains(neighbors[i]))
                        {
                            continue;
                        }

                        searchQueue.Enqueue(neighbors[i]);
                    }
                }
            }

            this[index] = new NormalChessField(toReparent.Color, false, false, -1);
            return -1;
        }

        protected IList<int> OpenFreeChessField(NormalChessField normalChessField, int index)
        {
            var parentIndex = normalChessField.ParentId;
            var freedList = FreeChessFieldsForIndex(index, parentIndex);
            var parentField = this[parentIndex] as BattleChessField;

            var childList = new List<int>();
            for (int i = 0; i < parentField.ChildCount; ++i)
            {
                var childIndex = parentField.GetChildId(i);
                if (childIndex == index)
                {
                    continue;
                }
                childList.Add(parentField.GetChildId(i));
            }

            for (int i = 0; i < freedList.Count; ++i)
            {
                childList.Add(freedList[i]);
            }

            this[index] = new NormalChessField(normalChessField.Color, true, false, -1);

            List<int> changeList = new List<int>();
            changeList.Add(parentIndex);
            changeList.Add(index);

            if (parentField.RemainingCount > 1)
            {
                changeList.AddRange(freedList);
                this[parentIndex] = new BattleChessField(parentField.Color, true, parentField.RemainingCount - 1, childList);
            }
            else
            {
                this[parentIndex] = new BattleChessField(parentField.Color, true, 0, null);

                var changedParents = new HashSet<int>();
                for (int i = 0; i < childList.Count; ++i)
                {
                    int changedParent = ReparentNormalChessField(childList[i]);
                    if (changedParent >= 0)
                    {
                        changedParents.Add(changedParent);
                    }
                    changeList.Add(childList[i]);
                }

                foreach (int i in changedParents)
                {
                    changeList.Add(i);
                }
            }

            return changeList;
        }

        protected IList<int> OpenBattleChessField(BattleChessField battleChessField, int index)
        {
            var freedList = FreeChessFieldsForIndex(index, index);
            this[index] = new BattleChessField(battleChessField.Color, true, battleChessField.RemainingCount, freedList);

            var changeList = new List<int>(freedList);
            changeList.Add(index);
            return changeList;
        }

        protected IList<int> FreeChessFieldsForIndex(int index, int parentIndex)
        {
            Queue<int> searchQueue = new Queue<int>();
            HashSet<int> searchedFlags = new HashSet<int>();
            searchQueue.Enqueue(index);
            List<int> changeList = new List<int>();

            while (searchQueue.Count > 0)
            {
                int cur = searchQueue.Dequeue();
                searchedFlags.Add(cur);

                var data = this[cur];

                if (cur != index && !(data is NormalChessField && data.IsOpened))
                {
                    continue;
                }

                IList<int> neighbors = Get4Neighbors(cur);

                for (int i = 0; i < neighbors.Count; ++i)
                {
                    int neighborIndex = neighbors[i];
                    if (searchedFlags.Contains(neighborIndex))
                    {
                        continue;
                    }

                    var neighborData = this[neighborIndex];
                    if (neighborData is BattleChessField)
                    {
                        continue;
                    }

                    var normalChessField = neighborData as NormalChessField;
                    if (!normalChessField.IsOpened && !normalChessField.IsFree)
                    {
                        this[neighborIndex] = new NormalChessField(normalChessField.Color, false, true, parentIndex);
                        changeList.Add(neighborIndex);
                        searchedFlags.Add(neighborIndex);
                        continue;
                    }

                    if (normalChessField.IsOpened)
                    {
                        searchQueue.Enqueue(neighbors[i]);
                        continue;
                    }
                }
            }

            return changeList;
        }

        protected virtual void OnChessBoardRefreshSuccessInternal(object o, GameEventArgs e)
        {
            if (OnChessBoardRefreshSuccess != null)
            {
                OnChessBoardRefreshSuccess();
            }
            ClearClosureData();
        }

        protected virtual void OnChessBoardRefreshFailureInternal(object o, GameEventArgs e)
        {
            if (OnChessBoardRefreshFailure != null)
            {
                OnChessBoardRefreshFailure();
            }

            ClearClosureData();
            ClearChessBoard();
        }

        protected virtual void OnChessFieldOpenSuccessInternal(object o, GameEventArgs e)
        {
            var ne = e as ChessBoardOpenChessFieldSuccessEventArgs;

            if (OnChessFieldOpenSuccess != null)
            {
                if (ne.Packet != null && ne.Packet.FieldId >= 0 && ne.ReceiveGoodsData != null)
                {
                    OnChessFieldOpenSuccess(m_CachedChangeList, ne.ReceiveGoodsData);
                }
                else
                {
                    OnChessFieldOpenSuccess(m_CachedChangeList, null);
                }
            }

            ClearClosureData();
            CheckComplete();
        }

        protected virtual void OnChessFieldOpenFailureInternal(object o, GameEventArgs e)
        {
            if (OnChessFieldOpenFailure != null)
            {
                OnChessFieldOpenFailure();
            }

            ClearClosureData();
            ClearChessBoard();
        }

        protected virtual void OnBombSuccessInternal(object o, GameEventArgs e)
        {
            if (OnBombSuccess != null)
            {
                OnBombSuccess(m_CachedChangeList);
            }

            ClearClosureData();
            CheckComplete();
        }

        protected virtual void OnBombFailureInternal(object o, GameEventArgs e)
        {
            if (OnBombFailure != null)
            {
                OnBombFailure();
            }

            ClearClosureData();
            //ClearChessBoard();
        }

        protected virtual void OnGetEnemyDataSuccessInternal(object o, GameEventArgs e)
        {
            var ne = e as ChessBoardGetEnemyDataSuccessEventArgs;
            if (OnGetEnemyDataSuccess != null)
            {
                OnGetEnemyDataSuccess(ne.ChessFieldIndex);
            }
        }

        protected virtual void OnGetEnemyDataFailureInternal(object o, GameEventArgs e)
        {
            var ne = e as ChessBoardGetEnemyDataFailureEventArgs;
            if (OnGetEnemyDataFailure != null)
            {
                OnGetEnemyDataFailure(ne.ChessFieldIndex);
            }
        }

        protected void LogClosureData()
        {
            var sb = new StringBuilder();
            sb.Append("[ChessManagerBase] change list: [");

            if (m_CachedChangeList == null)
            {
                sb.Append("null");
            }
            else
            {
                for (int i = 0; i < m_CachedChangeList.Count; ++i)
                {
                    if (i > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(m_CachedChangeList[i]);
                }
            }

            sb.AppendFormat("], battle field to open: {0}", m_OpenBattleChessFieldClosure == null ? -1 : m_OpenBattleChessFieldClosure.Index);
            Log.Info(sb.ToString());
        }

        private bool IsAvailbleFreeField(NormalChessField field)
        {
            return field != null && field.IsFree && !field.IsOpened;
        }

        private void ClearClosureData()
        {
            LogClosureData();
            m_CachedChangeList = null;
            m_OpenBattleChessFieldClosure = null;
        }

        private void CheckComplete()
        {
            if (RemainingChanceCount != 0 && IsComplete)
            {
                RemainingChanceCount = 0;
            }
        }
    }
}
