namespace Genesis.GameClient
{
    /// <summary>
    /// Lua 脚本种类。
    /// </summary>
    public enum LuaScriptCategory
    {
        /// <summary>
        /// 基础脚本。由 ToLua 插件提供。
        /// </summary>
        Base,

        /// <summary>
        /// 定制脚本。由项目提供。
        /// </summary>
        Custom,
    }
}
