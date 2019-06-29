using GameFramework.Event;

namespace Genesis.GameClient
{
    public class LobbyHeroDataChangedEventArgs : GameEventArgs
    {
        public LobbyHeroDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.LobbyHeroDataChanged;
            }
        }
    }
}
