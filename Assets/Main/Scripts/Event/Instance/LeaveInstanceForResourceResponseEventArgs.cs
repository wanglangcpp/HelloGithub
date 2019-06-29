using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离开资源副本事件。
    /// </summary>
    public class LeaveInstanceForResourceResponseEventArgs : GameEventArgs
    {
        public LeaveInstanceForResourceResponseEventArgs(RewardCollectionHelper helper, LCLeaveInstanceForResource packet)
        {
            Packet = packet;
            RewardCollectionHelper = helper;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.LeaveInstanceForResourceResponse;
            }
        }

        public LCLeaveInstanceForResource Packet
        {
            get;
            private set;
        }

        public RewardCollectionHelper RewardCollectionHelper
        {
            get;
            private set;
        }
    }
}
