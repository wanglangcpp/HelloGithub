using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 每日签到配置表。
    /// </summary>
    public class DRDailyLogin : IDataRow
    {
        private const int BoxRewardCount = 3;
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 所属月份。
        /// </summary>
        public int Month { get; private set; }

        /// <summary>
        /// 领奖的天数(1开始)。
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// 奖励类型。
        /// </summary>
        public int RewardType { get; private set; }

        /// <summary>
        /// 奖励数量。
        /// </summary>
        public int RewardCount { get; private set; }

        /// <summary>
        /// 翻倍需要的VIP等级(-1是无限制)。
        /// </summary>
        public int NeedVipLevel { get; private set; }
        /// <summary>
        /// 累计奖励
        /// </summary>
        public List<PBItemInfo> BoxRewards { get { return m_BoxRewards; } }
        private List<PBItemInfo> m_BoxRewards = new List<PBItemInfo>();

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            RewardType = int.Parse(text[index++]);
            RewardCount = int.Parse(text[index++]);
            NeedVipLevel = int.Parse(text[index++]);
            for (int i = 0; i < BoxRewardCount; i++)
            {
                int type = int.Parse(text[index++]);
                int count = int.Parse(text[index++]);
                if (type > 0)
                {
                    m_BoxRewards.Add(new PBItemInfo() { Type = type, Count = count });
                }
            }

        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRDailyLogin>();
        }
    }
}
