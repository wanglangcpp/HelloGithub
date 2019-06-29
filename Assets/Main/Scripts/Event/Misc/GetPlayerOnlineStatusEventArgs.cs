using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class GetPlayerOnlineStatusEventArgs : GameEventArgs
    {
        public GetPlayerOnlineStatusEventArgs(List<PBOnlineStatus> onlineStatus)
        {
            OnlineStatus = onlineStatus;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.GetPlayerOnlineStatus;
            }
        }

        public List<PBOnlineStatus> OnlineStatus = null;
    }
}
