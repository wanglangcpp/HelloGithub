using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 翻翻棋彩色棋子数据。
    /// </summary>
    public class BattleChessField : ChessField
    {
        private List<int> m_ChildIds;

        public int ChildCount
        {
            get
            {
                return m_ChildIds == null ? 0 : m_ChildIds.Count;
            }
        }

        public IList<int> Children
        {
            get
            {
                return m_ChildIds.ToArray();
            }
        }

        public int GetChildId(int index)
        {
            return m_ChildIds[index];
        }

        public int RemainingCount { get; private set; }

        public BattleChessField(ChessFieldColor color, bool isOpened, int remainingCount, IList<int> childIds)
            : base(color, isOpened)
        {
            RemainingCount = remainingCount;

            if (childIds != null && childIds.Count > 0)
            {
                m_ChildIds = new List<int>(childIds);
            }
        }
    }
}
