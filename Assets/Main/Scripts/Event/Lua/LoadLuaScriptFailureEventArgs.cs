using GameFramework.Event;

namespace Genesis.GameClient
{
    /// <summary>
    /// 加载 Lua 脚本失败事件。
    /// </summary>
    public class LoadLuaScriptFailureEventArgs : GameEventArgs
    {
        public LoadLuaScriptFailureEventArgs(string scriptName, object userData)
        {
            ScriptName = scriptName;
            UserData = userData;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.LoadLuaScriptFailure;
            }
        }

        public string ScriptName { get; private set; }

        public object UserData { get; private set; }
    }
}
