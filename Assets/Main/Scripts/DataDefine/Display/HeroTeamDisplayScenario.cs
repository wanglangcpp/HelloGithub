namespace Genesis.GameClient
{
    public enum HeroTeamDisplayScenario
    {
        /// <summary>
        /// From entrance in <see cref="Genesis.GameClient.MainForm"/>.
        /// </summary>
        Lobby,

        /// <summary>
        /// After selecting an instance to play.
        /// </summary>
        InstanceSelection,

        /// <summary>
        /// When preparing for a PVPAI battle in the chess activity.
        /// </summary>
        ChessBattle,

        /// <summary>
        /// When preparing for a PVPAI battle in the off-line arena.
        /// </summary>
        ArenaBattle,

        /// <summary>
        /// When viewing another player's hero team.
        /// </summary>
        OtherPlayer,

        /// <summary>
        /// When preparing for a Pvp battle.
        /// </summary>
        PvpArenaBattle,
    }
}
