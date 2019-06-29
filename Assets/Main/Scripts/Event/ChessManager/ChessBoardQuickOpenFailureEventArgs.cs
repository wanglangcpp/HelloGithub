using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChessBoardQuickOpenFailureEventArgs : GameEventArgs
    {
        public ChessBoardQuickOpenFailureEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardQuickOpenFailure;
            }
        }
    }
}
