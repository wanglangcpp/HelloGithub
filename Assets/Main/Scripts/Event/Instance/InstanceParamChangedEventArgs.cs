using GameFramework.Event;

namespace Genesis.GameClient
{
    public class InstanceParamChangedEventArgs : GameEventArgs
    {
        public InstanceParamChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.InstanceParamChanged;
            }
        }
    }
}
