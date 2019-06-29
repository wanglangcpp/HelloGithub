using GameFramework.Event;

namespace Genesis.GameClient
{
    public class InstanceStartTimeChangedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.InstanceStartTimeChanged;
            }
        }
    }
}
