using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 请求连接大厅服务器。
    /// </summary>
    public class RequestConnectLobbyServerEventArgs : GameEventArgs
    {
        public RequestConnectLobbyServerEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.RequestConnectLobbyServer;
            }
        }
    }
}
