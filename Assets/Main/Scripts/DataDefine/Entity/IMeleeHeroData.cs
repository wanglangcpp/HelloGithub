namespace Genesis.GameClient
{
    /// <summary>
    /// 乱斗用的英雄数据接口。
    /// </summary>
    public interface IMeleeHeroData
    {
        /// <summary>
        /// 乱斗中的等级。
        /// </summary>
        int MeleeLevel { get; set; }

        /// <summary>
        /// 乱斗中当前等级的经验。
        /// </summary>
        int MeleeExpAtCurrentLevel { get; set; }

        /// <summary>
        /// 乱斗中的积分。
        /// </summary>
        int MeleeScore { get; set; }
    }
}
