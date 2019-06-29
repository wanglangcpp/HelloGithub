using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技奖励配置表。
    /// </summary>
    public class DRArenaReward : IDataRow
    {
        public class Reward
        {
            public Reward(int id, int count)
            {
                Id = id;
                Count = count;
            }

            public int Id
            {
                get; private set;
            }

            public int Count
            {
                get; private set;
            }
        }

        /// <summary>
        /// 离线竞技奖励类型。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 离线竞技开始名次
        /// </summary>
        public int StartRank { get; private set; }

        public List<Reward> Rewards = new List<Reward>();

        /// <summary>
        /// 奖励道具1类型。
        /// </summary>
        private int RewardItemType0 { get; set; }

        /// <summary>
        /// 奖励道具1数量。
        /// </summary>
        private int RewardItemCount0 { get; set; }

        /// <summary>
        /// 奖励道具2类型。
        /// </summary>
        private int RewardItemType1 { get; set; }

        /// <summary>
        /// 奖励道具2数量。
        /// </summary>
        private int RewardItemCount1 { get; set; }

        /// <summary>
        /// 奖励道具3类型。
        /// </summary>
        private int RewardItemType2 { get; set; }

        /// <summary>
        /// 奖励道具3数量。
        /// </summary>
        private int RewardItemCount2 { get; set; }

        /// <summary>
        /// 奖励道具4类型。
        /// </summary>
        private int RewardItemType3 { get; set; }

        /// <summary>
        /// 奖励道具4数量。
        /// </summary>
        private int RewardItemCount3 { get; set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            StartRank = int.Parse(text[index++]);
            RewardItemType0 = int.Parse(text[index++]);
            RewardItemCount0 = int.Parse(text[index++]);
            RewardItemType1 = int.Parse(text[index++]);
            RewardItemCount1 = int.Parse(text[index++]);
            RewardItemType2 = int.Parse(text[index++]);
            RewardItemCount2 = int.Parse(text[index++]);
            RewardItemType3 = int.Parse(text[index++]);
            RewardItemCount3 = int.Parse(text[index++]);

            Rewards.Clear();

            if (RewardItemType0 > 0 && RewardItemCount0 > 0)
                Rewards.Add(new Reward(RewardItemType0, RewardItemCount0));

            if (RewardItemType1 > 0 && RewardItemCount1 > 0)
                Rewards.Add(new Reward(RewardItemType1, RewardItemCount1));

            if (RewardItemType2 > 0 && RewardItemCount2 > 0)
                Rewards.Add(new Reward(RewardItemType2, RewardItemCount2));

            if (RewardItemType3 > 0 && RewardItemCount3 > 0)
                Rewards.Add(new Reward(RewardItemType3, RewardItemCount3));
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRArenaReward>();
        }
    }
}
