using GameFramework.Event;

namespace Genesis.GameClient
{
    public class NearbyPlayerDataChangedEventArgs : GameEventArgs
    {
        public NearbyPlayerDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.NearbyPlayerDataChanged;
            }
        }
    }
}
