using GameFramework.Event;

namespace Genesis.GameClient
{
    public class UpgradeSoulEventArgs : GameEventArgs
    {
        public UpgradeSoulEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.UpgradeHeroSoul;
            }
        }
    }
}
