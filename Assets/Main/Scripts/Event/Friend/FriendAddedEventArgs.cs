using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 好友添加成功事件。
    /// </summary>
    public class FriendAddedEventArgs : GameEventArgs
    {
        public FriendAddedEventArgs(int playerId)
        {
            PlayerId = playerId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.FriendAdded;
            }
        }

        public int PlayerId { get; private set; }
    }
}
