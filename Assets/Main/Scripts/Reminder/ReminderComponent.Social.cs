using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ReminderComponent : MonoBehaviour
    {
        private bool m_HasPendingFriendRequestsData = false;

        /// <summary>
        /// 是否有待处理的好友请求。
        /// </summary>
        public bool HasPendingFriendRequestsData
        {
            get
            {
                return m_HasPendingFriendRequestsData;
            }
        }

        private void RefreshPendingFriendRequestsReminder()
        {
            m_HasPendingFriendRequestsData = GameEntry.Data.FriendRequests.RequestList.Count > 0;
        }

        /// <summary>
        /// 是否有没看的私聊信息。
        /// </summary>
        public bool PrivateChatRequestsReminder
        {
            get
            {
                return GameEntry.Data.Chat.PrivateChatRequestsReminder;
            }
            set
            {
                AfterProcessingDataChange();
            }
        }

        /// <summary>
        /// 是否有能用的星图点。
        /// </summary>
        public bool HasMeridianEnergyReminder
        {
            get
            {
                return GameEntry.Data.Player.MeridianEnergy > 0 && GameEntry.Data.Player.Level >= GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Meridian.UnlockLevel, 3);
            }
        }

        /// <summary>
        /// 是否有免费抽奖次数。
        /// </summary>
        public bool HasFreeChancedTimes
        {
            get
            {
                for (int i = 0; i < (int)ChanceType.ChanceTypeCount; i++)
                {
                    var chanceData = GameEntry.Data.Chances.GetChanceData((ChanceType)i + 1);
                    if (i == 0)
                    {
                        if (chanceData.FreeChancedTimes < 1)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (chanceData.FreeChancedTimes < 3)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public bool EnableDailyLogin
        {
            get
            {
                return GameEntry.Data.DailyLogin.IsLoginedToday == false;
            }
        }

        public bool HasUnfinishedActivity
        {
            get
            {
                var activities = GameEntry.DataTable.GetDataTable<DRActivity>().GetAllDataRows();

                for (int i = 0; i < activities.Length; i++)
                    if (IsActivityUnfinished(activities[i]))
                        return true;

                return false;
            }
        }

        public bool HasUnclaimedQuest
        {
            get
            {
                if (GameEntry.Data.Player.Level < GameEntry.ServerConfig.GetInt(Constant.ServerConfig.DailyQuest.UnlockLevel, 10))
                    return false;

                return GameEntry.Data.Achievements.HasUnclaimedAchievement
                    || GameEntry.Data.DailyQuests.HasUnclaimedQuest
                    || GameEntry.Data.DailyQuests.HasUnclaimedChest; 
            }
        }

        private bool IsActivityUnfinished(DRActivity activity)
        {
            if (activity == null)
                return false;

            if (activity.ShouldDisplay == false)
                return false;

            if (GameEntry.Data.Player.Level < activity.UnlockLevel)
                return false;

            if (IsActivityActive(activity) == false)
                return false;

            return GetEnableCountByType((ActivityType)activity.Id) > 0;
        }

        private int GetEnableCountByType(ActivityType type)
        {
            int remainingPlayCount = 0, freePlayCount;
            switch (type)
            {
                case ActivityType.TurnOverChess:
                case ActivityType.GearFoundry:
                case ActivityType.CosmosCrack:
                    return 0;   // 上面这三个目前没开放
                case ActivityType.OfflineArena:
                    UIUtility.GetPlayCount_OfflineArena(out remainingPlayCount, out freePlayCount);
                    return remainingPlayCount;
                case ActivityType.InstanceForResource_Coin:
                    UIUtility.GetPlayCount_InstanceForCoinResource(out remainingPlayCount, out freePlayCount);
                    return remainingPlayCount;
                case ActivityType.InstanceForResource_Exp:
                    UIUtility.GetPlayCount_InstanceForExpResource(out remainingPlayCount, out freePlayCount);
                    return remainingPlayCount;
                default:
                    return 0;
            }
        }

        private bool IsActivityActive(DRActivity activity)
        {
            int periodCount = activity.StartTimes.Length;
            int dayOfWeekToday = (int)(GameEntry.Time.LobbyServerUtcTime.Date.DayOfWeek);
            bool activeToday = activity.ActiveOnWeekDays[dayOfWeekToday];

            // 全天开放。
            if (periodCount <= 0)
            {
                return activeToday;
            }

            for (int i = 0; i < periodCount; i++)
            {
                var startTime = activity.StartTimes[i];
                var endTime = activity.EndTimes[i];

                if (startTime <= endTime) // 未跨天。
                {
                    var realStartTime = GameEntry.Time.LobbyServerUtcTime.Date.Add(startTime);
                    var realEndTime = GameEntry.Time.LobbyServerUtcTime.Date.Add(endTime);
                    if (activeToday && realStartTime <= GameEntry.Time.LobbyServerUtcTime && GameEntry.Time.LobbyServerUtcTime <= realEndTime)
                    {
                        return true;
                    }
                }
                else // 跨天。
                {
                    int dayOfWeekPrevDay = (int)(GameEntry.Time.LobbyServerUtcTime.Date.DayOfWeek) - 1;
                    if (dayOfWeekPrevDay < 0) dayOfWeekPrevDay += 7;
                    bool activePrevDay = activity.ActiveOnWeekDays[dayOfWeekPrevDay];

                    if (GameEntry.Time.LobbyServerUtcTime.TimeOfDay >= startTime) // 仍在当天。
                    {
                        if (activeToday) return true;
                    }

                    if (GameEntry.Time.LobbyServerUtcTime.TimeOfDay <= endTime) // 已进入第二天。
                    {
                        if (activePrevDay) return true;
                    }
                }
            }

            return false;
        }
    }
}
