using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 登录游戏服务器的某个准备项完成事件。
    /// </summary>
    public class SignInPrepareEventArgs : GameEventArgs
    {
        public SignInPrepareEventArgs()
        {

        }

        public override int Id
        {
            get
            {
                return (int)EventId.SignInPrepare;
            }
        }
    }
}
