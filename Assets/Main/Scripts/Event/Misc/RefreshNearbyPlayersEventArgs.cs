using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    ///
    /// </summary>
    public class RefreshNearbyPlayersEventArgs : GameEventArgs
    {
        public RefreshNearbyPlayersEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.RefreshNearbyPlayers;
            }
        }
    }
}
