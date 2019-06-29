using System;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ServerErrorEventArgs : GameEventArgs
    {
        public ServerErrorEventArgs(NetworkCustomErrorData data)
        {
            NetworkErrorData = data;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ServerError;
            }
        }

        public NetworkCustomErrorData NetworkErrorData
        {
            get;
            set;
        }
    }
}
