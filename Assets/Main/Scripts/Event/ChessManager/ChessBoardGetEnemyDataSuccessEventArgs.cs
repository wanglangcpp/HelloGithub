using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChessBoardGetEnemyDataSuccessEventArgs : GameEventArgs
    {
        public ChessBoardGetEnemyDataSuccessEventArgs(int chessFieldIndex)
        {
            ChessFieldIndex = chessFieldIndex;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardGetEnemyDataSuccess;
            }
        }

        public int ChessFieldIndex { get; private set; }
    }
}
