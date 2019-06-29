namespace Genesis.GameClient
{
    /// <summary>
    /// 名字板类型。
    /// </summary>
    public enum NameBoardMode
    {
        /// <summary>
        /// 仅显示名字。
        /// </summary>
        NameOnly,

        /// <summary>
        /// 仅显示血条。
        /// </summary>
        HPBarOnly,

        /// <summary>
        /// 显示名字和血条。
        /// </summary>
        NameAndHPBar,

        /// <summary>
        /// 根据自身规则来显示。
        /// </summary>
        ShowBySelf,
    }
}
