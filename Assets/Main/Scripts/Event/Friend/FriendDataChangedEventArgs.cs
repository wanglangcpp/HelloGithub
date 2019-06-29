using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 好友列表数据更改事件。
    /// </summary>
    public class FriendDataChangedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 更改方式枚举。
        /// </summary>
        public enum ChangeMode
        {
            /// <summary>
            /// 默认或未指定。
            /// </summary>
            Default,

            /// <summary>
            /// 朋友列表刷新。
            /// </summary>
            ListRefreshed,

            /// <summary>
            /// 系统时间进入新的一天。
            /// </summary>
            NewDay,

            /// <summary>
            /// 增加了一个好友。
            /// </summary>
            OneFriendAdded,

            /// <summary>
            /// 删除了一个好友。
            /// </summary>
            OneFriendRemoved,
        }

        public FriendDataChangedEventArgs(ChangeMode changeMode = ChangeMode.Default)
        {
            ItsChangeMode = changeMode;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.FriendDataChanged;
            }
        }

        /// <summary>
        /// 更改方式。
        /// </summary>
        public ChangeMode ItsChangeMode { get; private set; }
    }
}
