using GameFramework.Event;

namespace Genesis.GameClient
{
    public class EnergyGivenFromFriendEventArgs : GameEventArgs
    {
        public EnergyGivenFromFriendEventArgs(int friendPlayerId)
        {
            FriendPlayerId = friendPlayerId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.EnergyGivenFromFriend;
            }
        }

        public int FriendPlayerId
        {
            get;
            private set;
        }
    }
}
