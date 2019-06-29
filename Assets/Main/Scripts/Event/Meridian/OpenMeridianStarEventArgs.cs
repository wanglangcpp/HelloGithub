using GameFramework.Event;

namespace Genesis.GameClient
{
    public class OpenMeridianStarEventArgs : GameEventArgs
    {
        public OpenMeridianStarEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.OpenMeridianStar;
            }
        }
    }
}
