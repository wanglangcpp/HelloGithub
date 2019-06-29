using GameFramework.Event;

namespace Genesis.GameClient
{
    public class LoginServerEventArgs : GameEventArgs
    {
        public LoginServerEventArgs(bool authorized, bool newAccount,bool restrictServer)
        {
            Authorized = authorized;
            NewAccount = newAccount;
            RestrictServer = restrictServer;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.LoginServer;
            }
        }

        public bool Authorized
        {
            get;
            private set;
        }

        public bool NewAccount
        {
            get;
            private set;
        }

        public bool RestrictServer
        {
            get;
            private set;
        }
    }
}
