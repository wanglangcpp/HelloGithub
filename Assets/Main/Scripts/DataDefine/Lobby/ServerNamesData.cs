using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    [Serializable]
    public class ServerNamesData
    {
        private List<ServerNameData> m_ServerNameList = new List<ServerNameData>();

        public ServerNameData GetServerData(int serverId)
        {
            for (int i = 0; i < m_ServerNameList.Count; i++)
            {
                if (m_ServerNameList[i].Id == serverId)
                {
                    return m_ServerNameList[i];
                }
            }

            return null;
        }

        public void UpdateData(ServerListData data)
        {
            m_ServerNameList.Clear();
            for (int i = 0; i < data.ServerList.Count; i++)
            {
                ServerNameData serverName = new ServerNameData();
                serverName.Id = data.ServerList[i].Id;
                serverName.Name = data.ServerList[i].Name;
                m_ServerNameList.Add(serverName);
            }
        }
    }
}
