using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技领取活跃度奖励成功事件。
    /// </summary>
    public class OfflineArenaLivenessRewardClaimedEventArgs : GameEventArgs
    {
        public OfflineArenaLivenessRewardClaimedEventArgs(RewardCollectionHelper rewards)
        {
            Rewards = rewards;
        }
       
        public override int Id
        {
            get { return (int)(EventId.OfflineArenaLivenessRewardClaimed); }
        }

        public RewardCollectionHelper Rewards
        {
            get;
            private set;
        }
    }
}
