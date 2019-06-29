using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class FriendData
    {
        private readonly static DateTime InvalidDateTime = new DateTime(1900, 1, 1, 0, 0, 0, 0);

        private PlayerData m_Player = new PlayerData();
        public PlayerData Player { get { return m_Player; } }

        /// <summary>
        /// 最后收到体力的时间
        /// </summary>
        private long m_LastReceiveEnergyTime;
        /// <summary>
        /// 最后赠送的体力时间
        /// </summary>
        private long m_LastGiveEnergyTime;
        /// <summary>
        /// 最后领取体力的时间
        /// </summary>
        private long m_LastClaimEnergyTime;

        private long m_LastLogoutTime;

        /// <summary>
        /// 当前玩家是否能向此玩家赠与能量。
        /// </summary>
        public bool CanGiveEnergy
        {
            get
            {
                var lastGiveTime = new DateTime(m_LastGiveEnergyTime, DateTimeKind.Utc);
                var refreshTime = TimeSpan.Parse(GameEntry.ServerConfig.GetString(Constant.ServerConfig.Friend.RefreshUtcTime));

                return !TimeUtility.IsSameDay(lastGiveTime, refreshTime);
            }
        }

        /// <summary>
        /// 当前玩家是否能领取此玩家赠与的能量。
        /// </summary>
        public bool CanReceiveEnergy
        {
            get
            {
                var lastReceiveTime = new DateTime(m_LastReceiveEnergyTime, DateTimeKind.Utc);
                if (lastReceiveTime <= InvalidDateTime) //根本没有收到过体力
                    return false;

                var lastClaimTime = new DateTime(m_LastClaimEnergyTime, DateTimeKind.Utc);
                var refreshTime = TimeSpan.Parse(GameEntry.ServerConfig.GetString(Constant.ServerConfig.Friend.RefreshUtcTime));
                return !TimeUtility.IsSameDay(lastClaimTime, refreshTime);
            }
        }

        /// <summary>
        /// 当前玩家上次登出时间
        /// </summary>
        public long LastLogoutTime
        {
            get
            {
                return m_LastLogoutTime;
            }
        }

        /// <summary>
        /// 更新好友数据。
        /// </summary>
        /// <param name="friendInfo">服务器下发的好友数据</param>
        public void UpdateFriendData(PBFriendInfo friendInfo)
        {
            m_Player.UpdateData(friendInfo.PlayerInfo);
            m_LastClaimEnergyTime = friendInfo.LastClaimEnergyTime;
            m_LastGiveEnergyTime = friendInfo.LastGiveEnergyTime;
            m_LastReceiveEnergyTime = friendInfo.LastReceiveEnergyTime;

            if (friendInfo.LastLogoutTime != null)
                m_LastLogoutTime = friendInfo.LastLogoutTime;
        }

        /// <summary>
        /// 更新好友数据
        /// </summary>
        /// <param name="lastClaimTime">最后一次的领取时间</param>
        /// <param name="lastGiveTime">最后一次的赠送时间</param>
        /// <param name="lastReceiveTime">最后一次收到体力的时间</param>
        public void UpdateFriendData(long lastClaimTime = 0, long lastGiveTime = 0, long lastReceiveTime = 0)
        {
            if (lastClaimTime > InvalidDateTime.Ticks)
                m_LastClaimEnergyTime = lastClaimTime;

            if (lastGiveTime > InvalidDateTime.Ticks)
                m_LastGiveEnergyTime = lastGiveTime;

            if (lastReceiveTime > InvalidDateTime.Ticks)
                m_LastGiveEnergyTime = lastReceiveTime;
        }

    }
}
