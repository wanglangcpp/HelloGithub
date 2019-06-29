using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class ClaimTaskRewardSuccessEventArgs : GameEventArgs
    {
        public override int Id
        {
            get
            {
                return (int)EventId.ClaimTaskRewardSuccess;
            }
        }
        public int TaskId;
    }
}

