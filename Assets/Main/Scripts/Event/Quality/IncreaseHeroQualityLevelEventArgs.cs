using GameFramework.Event;

namespace Genesis.GameClient
{
    public class IncreaseHeroQualityLevelEventArgs : GameEventArgs
    {
        public IncreaseHeroQualityLevelEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.IncreaseHeroQualityLevel;
            }
        }
    }
}
