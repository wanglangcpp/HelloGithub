
namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void RefreshRankList(RankListType rankType)
        {
            if (rankType == RankListType.Unspecified)
            {
                return;
            }
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                CLRefreshRankList msg = new CLRefreshRankList();
                msg.RankType = (int)rankType;
                GameEntry.Network.Send(msg);
            }
        }
    }
}
