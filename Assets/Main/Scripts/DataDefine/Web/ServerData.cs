using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class ServerData
    {
        public int Id = 0;
        public string Name = string.Empty;
        public int AreaId = 0;
        public ServerLoad Load = 0;
        public ServerFlag Flag = 0;
        public string GameHost = string.Empty;
        public int GamePort = 0;
        public bool isRestrict = false;

        public bool CheckFlag(ServerFlag flag)
        {
            return (Flag & flag) != 0;
        }
    }
}
