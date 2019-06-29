using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动 -- 领奖事件。
    /// </summary>
    public class GearFoundryRewardClaimedEventArgs : GameEventArgs
    {
        public GearFoundryRewardClaimedEventArgs()
            : this(null, null, null)
        {

        }

        public GearFoundryRewardClaimedEventArgs(IList<ItemData> items, IList<GearData> gears, IList<SoulData> souls)
        {
            Items = items == null ? new List<ItemData>() : new List<ItemData>(items);
            Gears = gears == null ? new List<GearData>() : new List<GearData>(gears);
            Souls = souls == null ? new List<SoulData>() : new List<SoulData>(souls);
        }

        public override int Id
        {
            get { return (int)EventId.GearFoundryRewardClaimed; }
        }

        public List<ItemData> Items { get; private set; }
        public List<GearData> Gears { get; private set; }
        public List<SoulData> Souls { get; private set; }
    }
}
