using GameFramework.Event;

namespace Genesis.GameClient
{
    public class UseHeroExpItemEventArgs : GameEventArgs
    {
        public UseHeroExpItemEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.UseHeroExpItem;
            }
        }
    }
}
