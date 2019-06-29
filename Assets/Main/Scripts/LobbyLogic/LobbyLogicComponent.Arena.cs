using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void RefreshOfflineArena(bool isPlayerRefresh)
        {
            var request = new CLGetArenaPlayerAndTeamInfo();
            request.Refresh = isPlayerRefresh;

            GameEntry.Network.Send(request);
        }

        public void PickOfflineArenaReward()
        {
            GameEntry.Network.Send(new CLClaimArenaReward());
        }

        public void BuyAdditionalArenaCount()
        {
            GameEntry.Network.Send(new CLBuyAdditionalArenaCount());
        }
    }
}