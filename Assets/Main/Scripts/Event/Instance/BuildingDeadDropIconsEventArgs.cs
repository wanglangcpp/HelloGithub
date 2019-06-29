using GameFramework.Event;

namespace Genesis.GameClient
{
    public class BuildingDeadDropIconsEventArgs : GameEventArgs
    {
        public BuildingDeadDropIconsEventArgs(int dropCoins)
        {
            DropCoins = dropCoins;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.BuildingDeadDropIcons;
            }
        }

        public int DropCoins
        {
            get;
            private set;
        }
    }
}
