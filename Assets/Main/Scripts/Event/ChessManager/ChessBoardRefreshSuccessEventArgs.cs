using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChessBoardRefreshSuccessEventArgs : GameEventArgs
    {
        public ChessBoardRefreshSuccessEventArgs(LCGetChessBoard packet = null)
        {
            Packet = packet;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardRefreshSuccess;
            }
        }

        public LCGetChessBoard Packet { get; private set; }
    }
}
