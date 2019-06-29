using GameFramework.Event;

namespace Genesis.GameClient
{
    public class CreatePlayerEventArgs : GameEventArgs
    {
        public CreatePlayerEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.CreatePlayer;
            }
        }
    }
}
