using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GearFoundryTeamMatchingFailedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get { return (int)EventId.GearFoundryTeamMatchingFailed; }
        }
    }
}
