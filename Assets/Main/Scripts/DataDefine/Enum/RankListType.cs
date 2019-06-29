namespace Genesis.GameClient
{
    /// <summary>
    /// 排行榜类型。
    /// </summary>
    public enum RankListType
    {
        /// <summary>
        /// 未指定。
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 最大战斗力排行榜。
        /// </summary>
        TotalMight,

        /// <summary>
        /// 最大战斗力排行榜。
        /// </summary>
        OfflineArena,

        /// <summary>
        /// 本服排行榜。
        /// </summary>
        PvpLocalServer = 3,

        /// <summary>
        /// 全服排行榜。
        /// </summary>
        PvpAllServer = 4,

        /// <summary>
        /// PVP排行榜
        /// </summary>
        PvpMain,
        /// <summary>
        /// 等级排行榜
        /// </summary>
        LevelTab,
    }
}
