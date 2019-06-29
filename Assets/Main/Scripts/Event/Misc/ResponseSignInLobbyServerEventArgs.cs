using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 回应登入大厅服务器。
    /// </summary>
    public class ResponseSignInLobbyServerEventArgs : GameEventArgs
    {
        public ResponseSignInLobbyServerEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.ResponseSignInLobbyServer;
            }
        }
    }
}
