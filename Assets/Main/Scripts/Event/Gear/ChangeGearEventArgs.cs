using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ChangeGearEventArgs : GameEventArgs
    {
        public ChangeGearEventArgs(LobbyHeroData putOnHero, LobbyHeroData takeOffHero)
        {
            PutOnHero = putOnHero;
            TakeOffHero = takeOffHero;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ChangeGear;
            }
        }

        public LobbyHeroData PutOnHero
        {
            get;
            private set;
        }

        public LobbyHeroData TakeOffHero
        {
            get;
            private set;
        }
    }
}
