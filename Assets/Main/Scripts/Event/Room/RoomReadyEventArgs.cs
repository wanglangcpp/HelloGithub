using GameFramework.Event;

namespace Genesis.GameClient
{
    public class RoomReadyEventArgs : GameEventArgs
    {
        public RoomReadyEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.RoomReady;
            }
        }
    }
}
