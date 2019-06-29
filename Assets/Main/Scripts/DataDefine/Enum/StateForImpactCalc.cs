namespace Genesis.GameClient
{
    /// <summary>
    /// 用于伤害计算的状态类型。
    /// </summary>
    public enum StateForImpactCalc
    {
        /// <summary>
        /// 普通状态。
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 浮空状态。
        /// </summary>
        Floating,

        /// <summary>
        /// 眩晕状态。
        /// </summary>
        Stunned,

        /// <summary>
        /// 冰冻状态。
        /// </summary>
        Frozen
    }
}
