using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GotPlayerEnergyEventArgs : GameEventArgs
    {
        public GotPlayerEnergyEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.GotPlayerEnergy;
            }
        }
    }
}
