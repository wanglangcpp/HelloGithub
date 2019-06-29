using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 回应连接大厅服务器。
    /// </summary>
    public class ResponseConnectLobbyServerEventArgs : GameEventArgs
    {
        public ResponseConnectLobbyServerEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ResponseConnectLobbyServer;
            }
        }
    }
}
