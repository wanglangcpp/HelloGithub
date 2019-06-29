using System;
using System.Collections.Generic;
using System.Linq;

namespace Genesis.GameClient
{
    [Serializable]
    public class FriendRequestsData
    {
        private Dictionary<int, PlayerData> m_RequestDataMap = new Dictionary<int, PlayerData>();

        public List<PlayerData> RequestList
        {
            get
            {
                return m_RequestDataMap.Values.ToList();
            }
        }

        public PlayerData GetPlayerDataInRequestListById(int playerId)
        {
            PlayerData player = null;
            m_RequestDataMap.TryGetValue(playerId, out player);

            return player;
        }

        public void ClearData()
        {
            m_RequestDataMap.Clear();
        }

        public void RefreshData(List<PBPlayerInfo> requestList)
        {
            ClearData();
            AddRequest(requestList);
        }

        public void AddRequest(List<PBPlayerInfo> requestList)
        {
            for (int i = 0; i < requestList.Count; i++)
                AddRequest(requestList[i]);
        }

        public void AddRequest(PBPlayerInfo request)
        {
            var player = new PlayerData();
            player.UpdateData(request);
            m_RequestDataMap[request.Id] = player;
        }

        public void RemoveRequest(int playerId)
        {
            m_RequestDataMap.Remove(playerId);
        }

    }
}
