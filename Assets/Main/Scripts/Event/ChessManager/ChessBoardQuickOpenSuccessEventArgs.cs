using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChessBoardQuickOpenSuccessEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.ChessBoardQuickOpenSuccess;
            }
        }
    }
}
