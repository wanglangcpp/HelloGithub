using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 待处理好友请求变更事件。
    /// </summary>
    public class PendingFriendRequestsDataChangedEventArgs : GameEventArgs
    {
        public PendingFriendRequestsDataChangedEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.PendingFriendRequestsDataChanged;
            }
        }
    }
}
