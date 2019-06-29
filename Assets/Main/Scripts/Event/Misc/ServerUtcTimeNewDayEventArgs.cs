using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ServerUtcTimeNewDayEventArgs : GameEventArgs
    {
        public ServerUtcTimeNewDayEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ServerUtcTimeNewDay;
            }
        }
    }
}
