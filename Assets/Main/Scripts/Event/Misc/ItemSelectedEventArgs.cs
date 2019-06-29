using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ItemSelectedEventArgs : GameEventArgs
    {
        public ItemSelectedEventArgs(ItemData item)
        {
            Item = item;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ItemSelected;
            }
        }

        public ItemData Item
        {
            get;
            set;
        }
    }
}
