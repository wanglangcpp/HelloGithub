using GameFramework.Event;

namespace Genesis.GameClient
{
    public class EnergyGivenToFriendEventArgs : GameEventArgs
    {
        public EnergyGivenToFriendEventArgs(int friendPlayerId, int remainCount = 0)
        {
            FriendPlayerId = friendPlayerId;
            RemainCount = remainCount;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.EnergyGivenToFriend;
            }
        }

        public int FriendPlayerId
        {
            get;
            private set;
        }

        public int RemainCount
        {
            get;
            private set;
        }
    }
}
