namespace Genesis.GameClient
{
    /// <summary>
    /// 副本逻辑类型。
    /// </summary>
    public enum InstanceLogicType
    {
        /// <summary>
        /// 非副本场景。
        /// </summary>
        NonInstance,

        /// <summary>
        /// 普通单人副本。
        /// </summary>
        SinglePlayer,

        /// <summary>
        /// 翻翻棋战斗副本。
        /// </summary>
        ChessBattle,

        /// <summary>
        /// 离线竞技战斗副本。
        /// </summary>
        ArenaBattle,

        /// <summary>
        /// 时空裂缝活动副本。
        /// </summary>
        CosmosCrack,

        /// <summary>
        /// 一对一在线副本。
        /// </summary>
        SinglePvp,

        /// <summary>
        /// 展示副本。
        /// </summary>
        Demo,

        /// <summary>
        /// 资源副本。
        /// </summary>
        ForResource,

        /// <summary>
        /// 模拟乱斗副本。
        /// </summary>
        MimicMelee,
    }
}
