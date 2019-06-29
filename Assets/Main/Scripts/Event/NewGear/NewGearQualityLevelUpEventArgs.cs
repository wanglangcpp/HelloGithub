using GameFramework.Event;

namespace Genesis.GameClient
{
    public class NewGearQualityLevelUpEventArgs : GameEventArgs
    {

        public NewGearQualityLevelUpEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return (int)EventId.NewGearQualityLevelUp;
            }
        }
    }
}
