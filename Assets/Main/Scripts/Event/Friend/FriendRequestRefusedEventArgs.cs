using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 拒绝好友请求成功事件。
    /// </summary>
    public class FriendRequestRefusedEventArgs : GameEventArgs
    {
        public FriendRequestRefusedEventArgs(int playerId)
        {
            PlayerId = playerId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.FriendRequestRefused;
            }
        }

        public int PlayerId { get; private set; }
    }
}
