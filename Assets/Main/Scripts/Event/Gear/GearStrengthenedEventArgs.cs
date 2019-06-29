using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GearStrengthenedEventArgs : GameEventArgs
    {
        public GearStrengthenedEventArgs(GearData gear, bool success)
        {
            Gear = gear;
            SuccessStrengthened = success;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GearStrengthened;
            }
        }

        public GearData Gear
        {
            get;
            set;
        }

        public bool SuccessStrengthened
        {
            get;
            set;
        }
    }
}
