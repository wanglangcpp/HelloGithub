using GameFramework.Event;


namespace Genesis.GameClient
{
    internal class SingleMatchSuccessArgs : GameEventArgs
    {
        public SingleMatchSuccessArgs(int roomId)
        {
            RoomId = roomId;
        }

        public override int Id
        {
            get { return (int)(EventId.SingleMatchSuccess); }
        }
        public int RoomId
        {
            get;
            set;
        }
    }
}

