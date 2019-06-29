using GameFramework.Event;

namespace Genesis.GameClient
{
    public class MeridianChangedEventArgs : GameEventArgs
    {
        public MeridianChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.MeridianDataChanged;
            }
        }
    }
}
