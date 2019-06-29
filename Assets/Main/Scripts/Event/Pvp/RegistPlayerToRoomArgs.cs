using GameFramework.Event;


namespace Genesis.GameClient
{
    internal class RegistPlayerToRoomArgs : GameEventArgs
    {
        public RegistPlayerToRoomArgs(int errorCode)
        {
            ErrorCode = errorCode;
        }

        public override int Id
        {
            get { return (int)(EventId.RegistPlayerToRoom); }
        }
        public int ErrorCode
        {
            get;
            set;
        }
    }
}
