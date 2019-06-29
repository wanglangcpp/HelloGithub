using GameFramework.Event;

namespace Genesis.GameClient
{
    public class RoomDataChangedEventArgs : GameEventArgs
    {
        public RoomDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.RoomDataChanged;
            }
        }
    }
}
