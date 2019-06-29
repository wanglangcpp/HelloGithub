using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class GearMultiSelectedEventArgs : GameEventArgs
    {
        public GearMultiSelectedEventArgs(List<GearData> gears)
        {
            Gears = gears;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GearMultiSelected;
            }
        }

        public List<GearData> Gears
        {
            get;
            set;
        }
    }
}
