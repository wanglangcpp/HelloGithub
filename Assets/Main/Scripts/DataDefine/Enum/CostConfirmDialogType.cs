namespace Genesis.GameClient
{
    /// <summary>
    /// 消费相关界面
    /// </summary>
    public enum CostConfirmDialogType
    {
        /// <summary>
        /// 金币购买。
        /// </summary>
        Undefine = 0,

        /// <summary>
        /// 金币购买。
        /// </summary>
        Coin = 1,

        /// <summary>
        /// 体力购买。
        /// </summary>
        Energy = 2,

        /// <summary>
        /// 竞技场购买次数。
        /// </summary>
        ArenaBattleCount = 3,

        /// <summary>
        /// 其他。
        /// </summary>
        Other,
    }
}
