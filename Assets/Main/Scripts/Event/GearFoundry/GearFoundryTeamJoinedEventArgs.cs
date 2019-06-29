using GameFramework.Event;

namespace Genesis.GameClient
{
    public class GearFoundryTeamJoinedEventArgs : GameEventArgs
    {
        public override int Id
        {
            get { return (int)EventId.GearFoundryTeamJoined; }
        }
    }
}
