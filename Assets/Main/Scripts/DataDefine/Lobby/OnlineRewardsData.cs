using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    public class OnlineRewardsData
    {
        /// <summary>
        /// 当前倒计时的奖励编号
        /// </summary>
        public int CountdownRewardsId { get { return m_CountdownRewardsId; } }
        private int m_CountdownRewardsId;
        /// <summary>
        /// 最后一次领奖时间
        /// </summary>
        public long LastClaimRewardsTime { get { return m_LastClaimRewardsTime; } }
        private long m_LastClaimRewardsTime;
        /// <summary>
        /// 累计在线时长
        /// </summary>
        public int CumulativeOnlineTime { get { return m_CumulativeOnlineTime; } }
        private int m_CumulativeOnlineTime;

        public void UpdataData(PBOnlineRewardInfo info)
        {
            m_CumulativeOnlineTime = info.OnlineSec;
            m_LastClaimRewardsTime = info.RewardTime;
            m_CountdownRewardsId = info.RewardId;
        }
    }
}

