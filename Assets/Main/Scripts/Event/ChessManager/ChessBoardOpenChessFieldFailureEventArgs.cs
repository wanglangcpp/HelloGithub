using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChessBoardOpenChessFieldFailureEventArgs : GameEventArgs
    {
        public ChessBoardOpenChessFieldFailureEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardOpenChessFieldFailure;
            }
        }
    }
}
