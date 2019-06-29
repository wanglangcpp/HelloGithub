using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 收取好友发送的能量事件。
    /// </summary>
    public class EnergyReceivedFromFriendEventArgs : GameEventArgs
    {
        public EnergyReceivedFromFriendEventArgs(int friendPlayerId, int remainCount = 0)
        {
            FriendPlayerId = friendPlayerId;
            RemainCount = remainCount;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.EnergyReceivedFromFriend;
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
