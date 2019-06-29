namespace Genesis.GameClient
{
    /// <summary>
    /// 角色进入空中状态时对 Buff 的处理方式。
    /// </summary>
    public enum GoToAirBuffActionType
    {
        /// <summary>
        /// 不处理。
        /// </summary>
        DoNothing = 0,

        /// <summary>
        /// 隐藏特效，落地后恢复。
        /// </summary>
        HideEffect = 1,
    }
}
