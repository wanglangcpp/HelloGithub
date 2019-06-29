using GameFramework.Event;

namespace Genesis.GameClient
{
    public class InstanceProgressDataChangedEventArgs : GameEventArgs
    {
        public InstanceProgressDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.InstanceProgressDataChanged;
            }
        }
    }
}
