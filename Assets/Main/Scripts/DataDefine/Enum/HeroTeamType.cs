namespace Genesis.GameClient
{
    /// <summary>
    /// 阵容类型。
    /// </summary>
    public enum HeroTeamType
    {
        /// <summary>
        /// 默认阵容（单人副本阵容）。
        /// </summary>
        Default = 0,

        /// <summary>
        /// 离线竞技阵容。
        /// </summary>
        Arena,

        /// <summary>
        /// 阵容类型数量，若要增加阵容类型，需加在此枚举之前！
        /// </summary>
        HeroTeamTypeCount

    }
}
