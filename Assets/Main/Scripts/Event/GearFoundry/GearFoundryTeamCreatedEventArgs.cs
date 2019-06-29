using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GearFoundryTeamCreatedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get { return (int)EventId.GearFoundryTeamCreated; }
        }
    }
}
