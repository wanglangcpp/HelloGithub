using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChessBoardBombFailureEventArgs : GameEventArgs
    {
        public ChessBoardBombFailureEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardBombFailure;
            }
        }
    }
}
