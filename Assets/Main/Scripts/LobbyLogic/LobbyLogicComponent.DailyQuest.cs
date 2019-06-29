namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void DailyQuestClaimReward(int questId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLClaimDailyQuestReward { DailyQuestId = questId });
                return;
            }
        }

        public void ClaimActivenessChest(int chestId)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLClaimActivenessChest { ChestId = chestId });
                return;
            }
        }

        public void DoDailyLogin()
        {
            GameEntry.Network.Send(new CLClaimDailyLogin());
        }
    }
}
