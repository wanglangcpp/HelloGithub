using GameFramework.Event;

namespace Genesis.GameClient
{
    public class HerostarLevelUpEventArgs : GameEventArgs
    {
        public HerostarLevelUpEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return (int)EventId.HerostarLevelUp;
            }
        }

    }
}
