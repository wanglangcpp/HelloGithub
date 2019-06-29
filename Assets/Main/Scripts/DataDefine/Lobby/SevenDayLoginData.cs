using UnityEngine;
using System.Collections;
using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class SevenDayLoginData
    {
        /// <summary>
        /// 七日登录类型（1-新手，2-循环）
        /// </summary>
        public int Type { get; private set; }
        /// <summary>
        /// 奖励领取状态
        /// </summary>
        public int RewardState { get; private set; }
        /// <summary>
        /// 登陆次数
        /// </summary>
        public int LoginCount { get; private set; }

        public int ClaimId { get; set; }

        private IDataTable<DRSevenDayLogin> dataTable;

        private Dictionary<int, DRSevenDayLogin> dicNoviceRewards;//新手七日登录
        private Dictionary<int, DRSevenDayLogin> dicLoopRewards;//循环七日登录

        private void InitData()
        {
            dicNoviceRewards = new Dictionary<int, DRSevenDayLogin>();
            dicLoopRewards = new Dictionary<int, DRSevenDayLogin>();
            dataTable = GameEntry.DataTable.GetDataTable<DRSevenDayLogin>();
            foreach (var item in dataTable.GetAllDataRows())
            {
                if (item.LoginType == 1)
                {
                    if (!dicNoviceRewards.ContainsKey(item.Id))
                    {
                        dicNoviceRewards.Add(item.Id, item);
                    }
                }
                else
                {
                    if (!dicLoopRewards.ContainsKey(item.Id))
                    {
                        dicLoopRewards.Add(item.Id, item);
                    }
                }
            }
        }

        /// <summary>
        ///  获取七日登录数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, DRSevenDayLogin> GetDatas()
        {
            if (dataTable == null)
            {
                InitData();
            }
            if (Type == 1)
            {
                return dicNoviceRewards;
            }
            else if (Type == 2)
            {
                return dicLoopRewards;
            }
            return null;
        }

        public void UpdataData(PBSevenDayLoginInfo info)
        {
            Type = info.Type;
            RewardState = info.RewardsState;
            LoginCount = info.LoginCount;
        }
    }
}
