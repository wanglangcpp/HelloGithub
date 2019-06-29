using LuaInterface;

namespace Genesis.GameClient
{
    /// <summary>
    /// Lua 组件接口。
    /// </summary>
    public interface ILuaComponent
    {
        /// <summary>
        /// 加载 Lua 脚本。
        /// </summary>
        /// <param name="scriptName">脚本名。</param>
        /// <param name="category">脚本类型。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadScript(string scriptName, LuaScriptCategory category, object userData = null);

        /// <summary>
        /// 检查是否加载了 Lua 脚本。
        /// </summary>
        /// <param name="scriptName">脚本名。</param>
        /// <returns>是否加载了 Lua 脚本。</returns>
        bool HasScript(string scriptName);

        /// <summary>
        /// 获取 Lua 脚本内容。
        /// </summary>
        /// <param name="scriptName">脚本名。</param>
        /// <returns>Lua 脚本内容。</returns>
        string GetScriptContent(string scriptName);

        /// <summary>
        /// 启动 Lua 虚拟机。
        /// </summary>
        void StartVM();

        /// <summary>
        /// 执行 Lua 脚本。
        /// </summary>
        /// <param name="scriptName">脚本名。</param>
        /// <returns>返回值。</returns>
        object[] DoScript(string scriptName);

        /// <summary>
        /// 执行 Lua 的 require 指令。
        /// </summary>
        /// <param name="scriptName">脚本名。</param>
        void Require(string scriptName);

        /// <summary>
        /// 执行 Lua 脚本。
        /// </summary>
        /// <param name="scriptContent">脚本内容。</param>
        /// <param name="chunkName">模块名。</param>
        /// <returns></returns>
        object[] DoScript(string scriptContent, string chunkName);

        /// <summary>
        /// 调用 Lua 脚本中的函数。
        /// </summary>
        /// <param name="funcName">函数名。</param>
        /// <param name="args">参数表。</param>
        /// <returns>返回值。</returns>
        object[] CallFunction(string funcName, params object[] args);

        /// <summary>
        /// 调用 Lua 垃圾回收。
        /// </summary>
        void GC();

        /// <summary>
        /// Update 事件。
        /// </summary>
        LuaBeatEvent UpdateEvent { get; }

        /// <summary>
        /// LateUpdate 事件。
        /// </summary>
        LuaBeatEvent LateUpdateEvent { get; }

        /// <summary>
        /// FixedUpdate 事件。
        /// </summary>
        LuaBeatEvent FixedUpdateEvent { get; }
    }
}
