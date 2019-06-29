using LuaInterface;

namespace Genesis.GameClient
{
    public class CustomLuaResLoader : LuaFileUtils
    {
        private ILuaComponent m_LuaComponent;

        public CustomLuaResLoader(ILuaComponent luaComponent) : base()
        {
            m_LuaComponent = luaComponent;
        }

        public override byte[] ReadFile(string fileName)
        {
            // Remove ".lua" suffix.
            if (fileName.ToLower().EndsWith(".lua"))
            {
                fileName = fileName.Substring(0, fileName.Length - 4);
            }

            if (!m_LuaComponent.HasScript(fileName))
            {
                return null;
            }

            var content = m_LuaComponent.GetScriptContent(fileName);
            return System.Text.Encoding.UTF8.GetBytes(content);
        }
    }
}
