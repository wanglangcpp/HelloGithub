using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GearUpgradedEventArgs : GameEventArgs
    {
        public GearUpgradedEventArgs(GearData gear)
        {
            Gear = gear;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GearUpgraded;
            }
        }

        public GearData Gear
        {
            get;
            set;
        }
    }
}
