using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 删除好友事件。
    /// </summary>
    public class FriendDeletedEventArgs : GameEventArgs
    {
        public FriendDeletedEventArgs(int playerId)
        {
            PlayerId = playerId;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.FriendDeleted;
            }
        }

        /// <summary>
        /// 删除好友的玩家编号。
        /// </summary>
        public int PlayerId { get; private set; }
    }
}
