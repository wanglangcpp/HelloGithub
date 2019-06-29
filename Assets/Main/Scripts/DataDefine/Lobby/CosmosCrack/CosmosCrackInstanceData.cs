using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 时空裂缝副本数据。
    /// </summary>
    [Serializable]
    public class CosmosCrackInstanceData
    {
        [SerializeField]
        private int m_InstanceId = 0;

        /// <summary>
        /// 获取时空裂隙副本编号。
        /// </summary>
        public int InstanceId { get { return m_InstanceId; } }

        [SerializeField]
        private int m_RewardLevel = 0;

        /// <summary>
        /// 获取奖励等级。
        /// </summary>
        public int RewardLevel { get { return m_RewardLevel; } }

        [SerializeField]
        private List<CosmosCrackInstanceRewardData> m_Rewards = new List<CosmosCrackInstanceRewardData>();

        /// <summary>
        /// 获取奖励数据。
        /// </summary>
        /// <returns>奖励数据。</returns>
        public IList<CosmosCrackInstanceRewardData> GetRewards()
        {
            return m_Rewards.ToArray();
        }

        /// <summary>
        /// 更新数据。
        /// </summary>
        /// <param name="pb">通信协议数据。</param>
        public void UpdateData(PBCosmosCrackInstanceInfo pb)
        {
            m_InstanceId = pb.InstanceType;
            m_RewardLevel = pb.RewardLevel;
            int oldRewardCount = m_Rewards.Count;

            for (int i = 0; i < pb.Rewards.Count; ++i)
            {
                var pbReward = pb.Rewards[i];

                if (i < oldRewardCount)
                {
                    m_Rewards[i].UpdateData(pbReward);
                }
                else
                {
                    var reward = new CosmosCrackInstanceRewardData();
                    reward.UpdateData(pbReward);
                    m_Rewards.Add(reward);
                }
            }
        }
    }
}
