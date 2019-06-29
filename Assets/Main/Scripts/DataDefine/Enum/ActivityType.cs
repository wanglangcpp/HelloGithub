namespace Genesis.GameClient
{
    /// <summary>
    /// 活动类型。
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// 翻翻棋。
        /// </summary>
        TurnOverChess = 1,

        /// <summary>
        /// 装备锻造。
        /// </summary>
        GearFoundry = 2,

        /// <summary>
        /// 时空裂缝。
        /// </summary>
        CosmosCrack = 4,

        /// <summary>
        /// 离线竞技。
        /// </summary>
        OfflineArena = 5,

        /// <summary>
        /// 资源副本：金币。
        /// </summary>
        InstanceForResource_Coin = 6,

        /// <summary>
        /// 资源副本：经验。
        /// </summary>
        InstanceForResource_Exp = 7,

        /// <summary>
        /// 大乱斗。
        /// </summary>
        MimicMeleeInstance = 8,

        /// <summary>
        /// 单人竞技。
        /// </summary>
        SinglePVP = 9,

        /// <summary>
        /// Boss副本
        /// </summary>
        BossChallenge = 10,

        /// <summary>
        /// 风暴之塔副本
        /// </summary>
        StormTower=11,
    }
}
