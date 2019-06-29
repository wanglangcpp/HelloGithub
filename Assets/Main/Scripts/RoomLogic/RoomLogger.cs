using System.IO;
using System.Text;

namespace Genesis.GameClient
{
    public interface IRoomLogger
    {
        void Init(bool logToConsole, string logFilePath);

        void AddLog(string tag, string logFormat, params object[] logParams);

        void SaveLogFile();

        void Shutdown();
    }

    public static class RoomLoggerFactory
    {
        public static IRoomLogger Create()
        {
            if (UnityEngine.Debug.isDebugBuild)
            {
                return new RoomLoggerDebug();
            }

            return new RoomLoggerDummy();
        }

        private class RoomLoggerDebug : IRoomLogger
        {
            private bool m_LogToConsole = false;
            private string m_LogFilePath = string.Empty;
            private StringBuilder m_LogBuilder = new StringBuilder();

            public void Init(bool logToConsole, string logFilePath)
            {
                m_LogToConsole = logToConsole;
                m_LogFilePath = logFilePath;
                m_LogBuilder.AppendLine("========== Room Log ==========");
                m_LogBuilder.Append("PlayerId: ");
                m_LogBuilder.AppendLine(GameEntry.Data.Player.Id.ToString());
                m_LogBuilder.Append("PlayerName: ");
                m_LogBuilder.AppendLine(GameEntry.Data.Player.Name);
                m_LogBuilder.AppendLine();
                m_LogBuilder.AppendLine();
            }

            public void AddLog(string tag, string logFormat, params object[] logParams)
            {
                if (m_LogToConsole)
                {
                    GameFramework.Log.Info("[Room Logger][{0}] {1}", tag, string.Format(logFormat, logParams));
                }

                m_LogBuilder.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss.fff}] [{1}] {2}\n", GameEntry.Time.RoomServerUtcTime, tag, string.Format(logFormat, logParams));
            }

            public void SaveLogFile()
            {
                File.WriteAllText(m_LogFilePath, m_LogBuilder.ToString());
            }

            public void Shutdown()
            {
                m_LogBuilder.Length = 0;
            }
        }

        private class RoomLoggerDummy : IRoomLogger
        {
            public void Init(bool logToConsole, string logFilePath)
            {

            }

            public void AddLog(string tag, string logFormat, params object[] logParams)
            {

            }

            public void SaveLogFile()
            {

            }

            public void Shutdown()
            {

            }
        }
    }
}
