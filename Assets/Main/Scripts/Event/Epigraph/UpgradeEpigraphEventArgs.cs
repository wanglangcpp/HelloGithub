using GameFramework.Event;

namespace Genesis.GameClient
{
    public class UpgradeEpigraphEventArgs : GameEventArgs
    {
        public UpgradeEpigraphEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.UpgradeEpigraph;
            }
        }
    }
}
