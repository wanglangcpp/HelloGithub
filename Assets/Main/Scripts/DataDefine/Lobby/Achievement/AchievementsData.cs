using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class AchievementsData : GenericData<AchievementData, PBAchievementInfo>
    {
        public List<AchievementData> GetShowAchievements()
        {
            List<AchievementData> achievements = new List<AchievementData>();
            for (int i = 0; i < Data.Count; i++)
            {
                var preAchievement = GetData(Data[i].PreAchievementId);
                if (!Data[i].IsClaimed && Data[i].PrePlayerLevel <= GameEntry.Data.Player.Level && (preAchievement == null || preAchievement.IsClaimed))
                {
                    achievements.Add(Data[i]);
                }
            }
            achievements.Sort(AchievementCompare);
            return achievements;
        }

        private int AchievementCompare(AchievementData a, AchievementData b)
        {
            if (a.IsCompleted && !b.IsCompleted)
            {
                return -1;
            }

            if (!a.IsCompleted && b.IsCompleted)
            {
                return 1;
            }

            return b.Weight.CompareTo(a.Weight);
        }

        public bool HasUnclaimedAchievement
        {
            get
            {
                for (int i = 0; i < Data.Count; i++)
                    if (Data[i].IsCompleted && Data[i].IsClaimed == false)
                        return true;

                return false;
            }
        }

    }
}
