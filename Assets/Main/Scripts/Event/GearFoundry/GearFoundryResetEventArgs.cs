using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GearFoundryResetEventArgs : GameEventArgs
    {
        public override int Id
        {
            get { return (int)EventId.GearFoundryReset; }
        }
    }
}
