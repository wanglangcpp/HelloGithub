namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void ExchangeEnergy()
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLExchangeEnergy());
            }
        }

        public void ExchangeCoin()
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                GameEntry.Network.Send(new CLExchangeCoin());
            }
        }
    }
}
