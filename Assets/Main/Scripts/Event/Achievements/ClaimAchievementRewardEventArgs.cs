using GameFramework.Event;

namespace Genesis.GameClient
{
    public class ClaimAchievementRewardEventArgs : GameEventArgs
    {
        public ClaimAchievementRewardEventArgs(int achievementId, ReceivedGeneralItemsViewData data)
        {
            AchievementId = achievementId;
            ReceivedItemsView = data;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.ClaimAchievementReward;
            }
        }

        public int AchievementId { get; private set; }

        public ReceivedGeneralItemsViewData ReceivedItemsView { get; private set; }
    }
}
