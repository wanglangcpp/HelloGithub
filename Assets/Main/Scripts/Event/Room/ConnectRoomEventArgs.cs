using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ConnectRoomEventArgs : GameEventArgs
    {
        public ConnectRoomEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ConnectRoom;
            }
        }
    }
}
