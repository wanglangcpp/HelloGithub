using UnityEngine;
using System.Collections;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ClaimOnlineRewardsEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.OnlineRewardsDataChange;
            }
        }

        public ClaimOnlineRewardsEventArgs()
        {

        }

    }
}

