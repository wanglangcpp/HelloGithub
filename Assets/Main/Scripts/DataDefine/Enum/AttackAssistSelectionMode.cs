namespace Genesis.GameClient
{
    /// <summary>
    /// 辅助瞄准选择目标的方式。
    /// </summary>
    public enum AttackAssistSelectionMode
    {
        /// <summary>
        /// 最小角度差。
        /// </summary>
        MinimumAngleDiff,

        /// <summary>
        /// 最小距离。
        /// </summary>
        MinimumDistance,

        /// <summary>
        /// 最小角度差（高级）。
        /// </summary>
        MinimumAngleDiffAdvanced,
    }
}
