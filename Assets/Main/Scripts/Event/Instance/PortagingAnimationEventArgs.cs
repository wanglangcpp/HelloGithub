using GameFramework.Event;

namespace Genesis.GameClient
{
    public class PortagingAnimationEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.PortagingAnimation;
            }
        }
    }
}
