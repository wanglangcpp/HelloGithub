using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChessBoardRefreshFailureEventArgs : GameEventArgs
    {
        public ChessBoardRefreshFailureEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardRefreshFailure;
            }
        }
    }
}
