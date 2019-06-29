using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技数据类。
    /// </summary>
    [Serializable]
    public class OfflineArenaData
    {
        /// <summary>
        /// 当前自己的排行名次
        /// </summary>
        public int CurrentRank
        {
            get;
            private set;
        }

        /// <summary>
        /// 今天已经挑战的次数
        /// </summary>
        public int TodayChallengedCount
        {
            get;
            private set;
        }

        public int TodayBoughtCount
        {
            get
            {
                return GameEntry.Data.VipsData.GetData((int)VipPrivilegeType.BuyAdditionalArena).UsedVipPrivilegeCount;
            }
        }

        /// <summary>
        /// 今天已经刷新的次数
        /// </summary>
        public int TodayRefreshedCount
        {
            get;
            private set;
        }

        public bool HasReward
        {
            get;
            private set;
        }

        public int RewardId
        {
            get;
            private set;
        }

        public List<PBArenaReportInfo> ReportInfo
        {
            get
            {
                return m_ReportInfo;
            }
        }

        private int m_FreeRefreshTime = int.MinValue;
        public int FreeRefreshTime
        {
            get
            {
                if (m_FreeRefreshTime < 0)
                {
                    m_FreeRefreshTime = 0;
                    var arenaCostConfig = GameEntry.DataTable.GetDataTable<DRArenaCost>();
                    var costs = arenaCostConfig.GetAllDataRows();
                    for (int i = 0; i < costs.Length; i++)
                    {
                        if (costs[i].RefreshCostCoin == 0)
                            m_FreeRefreshTime++;
                    }
                }

                return m_FreeRefreshTime;
            }
        }

        private int m_CachedVipLevel = int.MinValue;
        private DRVip m_CachedVipConfig = null;

        public bool IsEnableChallenge
        {
            get
            {
                if (m_CachedVipLevel != GameEntry.Data.Player.VipLevel)
                {
                    var vipConfig = GameEntry.DataTable.GetDataTable<DRVip>();
                    m_CachedVipConfig = vipConfig.GetDataRow(GameEntry.Data.Player.VipLevel);

                    m_CachedVipLevel = GameEntry.Data.Player.VipLevel;
                }

                return m_CachedVipConfig.FreeArenaCount + TodayBoughtCount > TodayChallengedCount;
            }
        }

        private List<PBArenaReportInfo> m_ReportInfo;

        private OfflineArenaPlayersData m_Enermies = new OfflineArenaPlayersData();
        public OfflineArenaPlayersData Enermies
        {
            get { return m_Enermies; }
            set { m_Enermies = value; }
        }

        public int GetRefreshCostCoin()
        {
            if (TodayRefreshedCount < FreeRefreshTime)
                return 0;

            var arenaCostConfig = GameEntry.DataTable.GetDataTable<DRArenaCost>();
            var drCost = arenaCostConfig.GetDataRow(TodayRefreshedCount);
            if (drCost == null)
            {
                if (arenaCostConfig.MaxIdDataRow == null)
                {
                    Log.Error("Error arena cost configuration, can not get data where id ='{0}'", TodayRefreshedCount);
                    return int.MaxValue;
                }
                drCost = arenaCostConfig.MaxIdDataRow;
            }

            return drCost.RefreshCostCoin;
        }

        public void UpdateEnermies(LCGetArenaPlayerAndTeamInfo enermyInfo)
        {
            m_Enermies.ClearAndAddData(enermyInfo.PlayerAndTeamInfo);

            m_Enermies.Data.Sort((e1, e2) => { return e1.Rank.CompareTo(e2.Rank); });

            if (enermyInfo.HasRefreshArenaCount)
                TodayRefreshedCount = enermyInfo.RefreshArenaCount;
        }

        public void UpdateArenaData(PBArenaInfo arenaInfo)
        {
            if (arenaInfo.HasMyRank)
                CurrentRank = arenaInfo.MyRank;

            TodayChallengedCount = arenaInfo.PlayArenaCount;
            m_ReportInfo = arenaInfo.ArenaReportInfos;

            TodayRefreshedCount = arenaInfo.RefreshArenaCount;

            if(arenaInfo.HasRewardType)
            {
                HasReward = true;
                RewardId = arenaInfo.RewardType;
            }
            else
            {
                HasReward = false;
            }
        }

        public void UpdateArenaRewardData(int rewardId, bool hasReward)
        {
            HasReward = hasReward;
            RewardId = rewardId;
        }

        public void RefreshChallangeCount(int count)
        {
            TodayChallengedCount = count;
        }

        public void RefreshRank(int newRank)
        {
            CurrentRank = newRank;
        }

        public void ResetArenaData(LCResetArena newData)
        {
            GameEntry.Data.VipsData.GetData((int)VipPrivilegeType.BuyAdditionalArena).UsedVipPrivilegeCount = newData.BuyAdditionalArenaCount;
            TodayChallengedCount = newData.PlayArenaCount;
            TodayRefreshedCount = newData.RefreshArenaCount;
        }
    }
}
