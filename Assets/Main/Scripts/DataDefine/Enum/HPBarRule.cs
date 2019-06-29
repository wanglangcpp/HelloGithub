namespace Genesis.GameClient
{
    /// <summary>
    /// 血条显示规则。
    /// </summary>
    public enum HPBarDisplayRule
    {
        /// <summary>
        /// 不显示。
        /// </summary>
        DontDisplay = 0,

        /// <summary>
        /// 受击时显示。
        /// </summary>
        DisplayOnImpact = 1,

        /// <summary>
        /// 始终显示。
        /// </summary>
        AlwaysDisplay = 2,
    }
}
