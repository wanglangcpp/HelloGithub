using GameFramework.Event;

namespace Genesis.GameClient
{

    public class NpcDeadDropIconsEventArgs : GameEventArgs
    {
        public NpcDeadDropIconsEventArgs(int dropCoins)
        {
            DropCoins = dropCoins;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.NpcDeadDropIcons;
            }
        }

        public int DropCoins
        {
            get;
            private set;
        }
    }
}
