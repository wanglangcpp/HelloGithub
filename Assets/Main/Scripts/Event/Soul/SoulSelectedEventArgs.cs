using GameFramework.Event;

namespace Genesis.GameClient
{
    public class SoulSelectedEventArgs : GameEventArgs
    {
        public SoulSelectedEventArgs(SoulData soul)
        {
            Soul = soul;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.SoulSelected;
            }
        }

        public SoulData Soul
        {
            get;
            set;
        }
    }
}
