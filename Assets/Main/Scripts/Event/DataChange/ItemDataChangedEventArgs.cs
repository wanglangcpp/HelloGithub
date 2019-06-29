using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ItemDataChangedEventArgs : GameEventArgs
    {
        public ItemDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ItemDataChanged;
            }
        }
    }
}
