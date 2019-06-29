using System;
using System.Collections.Generic;
using System.Linq;

namespace Genesis.GameClient
{
    /// <summary>
    /// 好友数据。实际上类似是一个好友管理器。
    /// </summary>
    [Serializable]
    public class FriendsData
    {
        /// <summary>
        /// Key:Player Id
        /// Value:Player data
        /// </summary>
        private Dictionary<int, FriendData> m_FriendsData = new Dictionary<int, FriendData>();

        /// <summary>
        /// 好友列表。
        /// </summary>
        public List<FriendData> Data
        {
            get
            {
                return m_FriendsData.Values.ToList();
            }
        }

        public int TodayGiveCount
        {
            get;
            private set;
            }

        public int TodayClaimCount
        {
            get;
            private set;
        }

        public void ModifyTodayGiveCount(int count)
        {
            TodayGiveCount = Math.Min(Math.Max(0, count), GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.DailyEnergyGiveTimes, 10));
        }

        public void ModifyTodyClaimCount(int count)
        {
            TodayClaimCount = Math.Min(Math.Max(0, count), GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.DailyEnergyClaimTimes, 10));
        }

        public void ModifyFriend(FriendData friend)
        {

        }

        /// <summary>
        /// 检查某玩家是否是自己的好友。
        /// </summary>
        /// <param name="playerId">玩家ID</param>
        /// <returns>True：是好友关系</returns>
        public bool CheckWhetherIsMyFriend(int playerId)
        {
            return m_FriendsData.ContainsKey(playerId);
        }

        /// <summary>
        /// 获取好友数据。
        /// </summary>
        /// <param name="friendId">好友的ID</param>
        /// <returns>好友的数据</returns>
        public FriendData GetFriendDataById(int friendId)
        {
            FriendData friend = null;
            m_FriendsData.TryGetValue(friendId, out friend);

            return friend;
        }

        /// <summary>
        /// 删除好友数据。
        /// </summary>
        public void ClearData()
        {
            m_FriendsData.Clear();
        }

        /// <summary>
        /// 刷新好友数据。会将原来的数据删除掉，覆盖成传入的FriendList。
        /// </summary>
        /// <param name="friendList">最新的好友数据</param>
        public void RefreshData(List<PBFriendInfo> friendList)
        {
            ClearData();
            AddOrModifyFriend(friendList);
        }

        /// <summary>
        /// 添加好友。多个。
        /// </summary>
        /// <param name="friendList">好友列表</param>
        public void AddOrModifyFriend(List<PBFriendInfo> friendList)
        {
            for (int i = 0; i < friendList.Count; i++)
                AddOrModifyFriend(friendList[i]);
        }

        /// <summary>
        /// 添加好友。单个。
        /// </summary>
        /// <param name="friendInfo">好友数据</param>
        public void AddOrModifyFriend(PBFriendInfo friendInfo)
        {
            var friend = new FriendData();
            friend.UpdateFriendData(friendInfo);
            m_FriendsData[friendInfo.PlayerInfo.Id] = friend;
        }

        public void ModifyFriend(PBFriendInfo friendInfo)
        {
            if(m_FriendsData.ContainsKey(friendInfo.PlayerInfo.Id))
            {
                AddOrModifyFriend(friendInfo);
            }
        }

        /// <summary>
        /// 移除好友数据。
        /// </summary>
        /// <param name="friendId">好友ID</param>
        public void RemoveFriend(int friendId)
        {
            m_FriendsData.Remove(friendId);
        }
    }
}
