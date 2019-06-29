using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GearComposeCompleteEventArgs : GameEventArgs
    {
        public GearComposeCompleteEventArgs(GearData newGear)
        {
            NewGear = newGear;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GearComposeComplete;
            }
        }

        public GearData NewGear
        {
            get;
            set;
        }
    }
}
