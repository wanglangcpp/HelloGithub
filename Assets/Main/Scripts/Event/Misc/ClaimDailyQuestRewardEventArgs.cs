using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 每日任务完成并领取奖励事件。
    /// </summary>
    public class ClaimDailyQuestRewardEventArgs : GameEventArgs
    {
        public ClaimDailyQuestRewardEventArgs(ReceivedGeneralItemsViewData data)
        {
            ReceivedItemsView = data;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ClaimDailyQuestReward;
            }
        }

        public ReceivedGeneralItemsViewData ReceivedItemsView { get; private set; }
    }
}
