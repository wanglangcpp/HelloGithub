namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void AchivementClaimReward(int achievementId)
        {
             if (!GameEntry.OfflineMode.OfflineModeEnabled)
             {
                 GameEntry.Network.Send(new CLClaimAchievementReward { AchievementId = achievementId });
                 return;
             }           
        }
    }
}
