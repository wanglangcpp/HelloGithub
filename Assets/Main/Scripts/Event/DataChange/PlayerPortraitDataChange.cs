using GameFramework.Event;

namespace Genesis.GameClient
{
    public class PlayerPortraitDataChange : GameEventArgs
    {
        public PlayerPortraitDataChange()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChangePlayerPortrait;
            }
        }
    }
}
