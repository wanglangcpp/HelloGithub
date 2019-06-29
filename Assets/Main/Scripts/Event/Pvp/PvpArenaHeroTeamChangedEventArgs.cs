using GameFramework.Event;

namespace Genesis.GameClient
{
    internal class PvpArenaHeroTeamChangedEventArgs : GameEventArgs
    {
        public PvpArenaHeroTeamChangedEventArgs()
        {

        }

        public override int Id
        {
            get { return (int)(EventId.PvpArenaHeroTeamDataChanged); }
        }
    }
}
