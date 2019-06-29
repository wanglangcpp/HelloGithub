using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChessBoardBombSuccessEventArgs : GameEventArgs
    {
        public ChessBoardBombSuccessEventArgs(LCBombChessBoard packet = null)
        {
            Packet = packet;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardBombSuccess;
            }
        }

        public LCBombChessBoard Packet { get; private set; }
    }
}
