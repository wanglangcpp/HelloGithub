using GameFramework.Event;

namespace Genesis.GameClient
{
    public class FriendRequestSentEventArgs : GameEventArgs
    {
        public FriendRequestSentEventArgs()
        {
        }

        public override int Id
        {
            get
            {
                return (int)EventId.FriendRequestSent;
            }
        }
    }
}
