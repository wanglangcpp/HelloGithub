namespace Genesis.GameClient
{
    /// <summary>
    /// 翻翻棋灰色棋子。
    /// </summary>
    public class NormalChessField : ChessField
    {
        public int ParentId { get; private set; }

        public bool IsFree { get; private set; }

        public NormalChessField(ChessFieldColor color, bool isOpened, bool isFree, int parentId)
            : base(color, isOpened)
        {
            IsFree = isFree;
            ParentId = parentId;
        }
    }
}
