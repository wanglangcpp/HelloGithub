using GameFramework.Event;

namespace Genesis.GameClient
{
    public class NewGearStrengthenEventArgs : GameEventArgs
    {
        public NewGearStrengthenEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return (int)EventId.NewGearStrengthen;
            }
        }
    }
}
