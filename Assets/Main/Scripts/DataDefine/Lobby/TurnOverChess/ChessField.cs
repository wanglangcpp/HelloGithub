namespace Genesis.GameClient
{
    /// <summary>
    /// 翻翻棋棋子数据基类。
    /// </summary>
    public abstract class ChessField
    {
        public ChessFieldColor Color { get; private set; }

        public bool IsOpened { get; private set; }

        public ChessField(ChessFieldColor color, bool isOpened)
        {
            Color = color;
            IsOpened = isOpened;
        }
    }
}
