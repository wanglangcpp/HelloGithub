using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载 Lua 脚本成功事件。
    /// </summary>
    public class LoadLuaScriptSuccessEventArgs : GameEventArgs
    {
        public LoadLuaScriptSuccessEventArgs(string scriptName, string scriptContent, object userData)
        {
            ScriptName = scriptName;
            ScriptContent = scriptContent ?? string.Empty;
            UserData = userData;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.LoadLuaScriptSuccess;
            }
        }

        public string ScriptName { get; private set; }

        public string ScriptContent { get; private set; }

        public object UserData { get; private set; }
    }
}
