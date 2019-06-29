namespace Genesis.GameClient
{
    /// <summary>
    /// 装备属性新值掩码。
    /// </summary>
    public enum GearAttributeNewValueMask
    {
        /// <summary>
        /// 默认。
        /// </summary>
        Default = 0,

        /// <summary>
        /// 下一等级。
        /// </summary>
        LevelPlusOne = 1,

        /// <summary>
        /// 下一强化等级。
        /// </summary>
        StrengthLevelPlusOne = 2,
    }
}
