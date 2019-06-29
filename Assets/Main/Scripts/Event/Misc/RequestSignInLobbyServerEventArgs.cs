using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 请求登入大厅服务器。
    /// </summary>
    public class RequestSignInLobbyServerEventArgs : GameEventArgs
    {
        public RequestSignInLobbyServerEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.RequestSignInLobbyServer;
            }
        }
    }
}
