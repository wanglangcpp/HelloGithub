using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GearSelectedEventArgs : GameEventArgs
    {
        public GearSelectedEventArgs(GearData gear, int index = 0)
        {
            Gear = gear;
            SlotIndex = index;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GearSelected;
            }
        }

        public GearData Gear
        {
            get;
            set;
        }

        public int SlotIndex
        {
            get;
            set;
        }
    }
}
