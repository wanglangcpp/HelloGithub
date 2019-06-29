using GameFramework.Event;

namespace Genesis.GameClient
{
    public class SteadyBreakedEventArgs : GameEventArgs
    {
        public SteadyBreakedEventArgs(Character character)
        {
            Character = character;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.SteadyBreaked;
            }
        }

        public Character Character
        {
            get;
            private set;
        }
    }
}
