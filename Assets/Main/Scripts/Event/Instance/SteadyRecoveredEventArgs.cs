using GameFramework.Event;

namespace Genesis.GameClient
{
    public class SteadyRecoveredEventArgs : GameEventArgs
    {
        public SteadyRecoveredEventArgs(Character character)
        {
            Character = character;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.SteadyRecovered;
            }
        }

        public Character Character
        {
            get;
            private set;
        }
    }
}
