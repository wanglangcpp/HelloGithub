using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChessBoardGetEnemyDataFailureEventArgs : GameEventArgs
    {
        public ChessBoardGetEnemyDataFailureEventArgs(int chessFieldIndex)
        {
            ChessFieldIndex = chessFieldIndex;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardGetEnemyDataFailure;
            }
        }

        public int ChessFieldIndex { get; private set; }
    }
}
