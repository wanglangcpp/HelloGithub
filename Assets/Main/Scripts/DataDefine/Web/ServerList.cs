using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable]
    public class ServerListData
    {
        public List<ServerData> ServerList = new List<ServerData>();

        public ServerData GetServerData(int serverId)
        {
            for (int i = 0; i < ServerList.Count; i++)
            {
                if (ServerList[i].Id == serverId)
                {
                    return ServerList[i];
                }
            }

            return null;
        }

        public ServerData[] GetServerDataByArea(int areaId)
        {
            List<ServerData> servers = new List<ServerData>();
            for (int i = 0; i < ServerList.Count; i++)
            {
                if (ServerList[i].AreaId == areaId)
                {
                    servers.Add(ServerList[i]);
                }
            }

            return servers.ToArray();
        }

        public ServerData[] GetServerDataByFlag(ServerFlag flag)
        {
            List<ServerData> servers = new List<ServerData>();
            for (int i = 0; i < ServerList.Count; i++)
            {
                if (ServerList[i].CheckFlag(flag))
                {
                    servers.Add(ServerList[i]);
                }
            }

            return servers.ToArray();
        }
    }
}
