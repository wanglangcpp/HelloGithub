using GameFramework.Event;

namespace Genesis.GameClient
{
    public class PlayerDataChangedEventArgs : GameEventArgs
    {
        public PlayerDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.PlayerDataChanged;
            }
        }
    }
}
