namespace Genesis.GameClient
{
    public static class ChessManagerFactory
    {
        public static IChessManager Create()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return new ChessManagerOffline();
            }

            return new ChessManagerOnline();
        }
    }
}
