using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ReviveHeroesEventArgs : GameEventArgs
    {
        public ReviveHeroesEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ReviveHeroes;
            }
        }
    }
}
