using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 网络定值错误事件。
    /// </summary>
    public class NetworkCustomErrorEventArgs : GameEventArgs
    {
        public NetworkCustomErrorEventArgs(ServerErrorCode serverErrorCode)
        {
            ServerErrorCode = serverErrorCode;
        }

        public override int Id
        {
            get
            {
                return (int)(EventId.NetworkCustomError);
            }
        }

        /// <summary>
        /// 服务器错误代码。
        /// </summary>
        public ServerErrorCode ServerErrorCode
        {
            get;
            private set;
        }
    }
}
