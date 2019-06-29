using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class DailyQuestsData : GenericData<DailyQuestData, PBDailyQuestInfo>
    {
        [SerializeField]
        private int m_ClaimActivenessChestStatus;

        public int ClaimActivenessChestStatus
        {
            get
            {
                return m_ClaimActivenessChestStatus;
            }
            set
            {
                m_ClaimActivenessChestStatus = value;
            }
        }

        public bool HasUnclaimedChest
        {
            get
            {
                var dt = GameEntry.DataTable.GetDataTable<DRDailyQuestActiveness>();

                for (int i = 1; i < dt.Count; i++)
                {
                    var dr = dt.GetDataRow(i);
                    if (dr.Activeness <= GameEntry.Data.Player.ActivenessToken)
                        if (((ClaimActivenessChestStatus >> i) & 0x1) == 0)
                            return true;
                }

                return false;
            }
        }

        public bool HasUnclaimedQuest
        {
            get
            {
                var quests = GetShowDailyQuests();
                for (int i = 0; i < quests.Count; i++)
                    if (quests[i].IsCompleted && quests[i].IsClaimed == false)
                        return true;

                return false;
            }
        }

        public List<DailyQuestData> GetShowDailyQuests()
        {
            List<DailyQuestData> dailyQuests = new List<DailyQuestData>();
            for (int i = 0; i < Data.Count; i++)
            {
                if (Data[i].PrePlayerLevel <= GameEntry.Data.Player.Level)
                {
                    dailyQuests.Add(Data[i]);
                }
            }
            dailyQuests.Sort(DailyQuestCompare);
            return dailyQuests;
        }

        private int DailyQuestCompare(DailyQuestData a, DailyQuestData b)
        {
            if (a.IsClaimed && !b.IsClaimed)
            {
                return 1;
            }

            if (!a.IsClaimed && b.IsClaimed)
            {
                return -1;
            }

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
    }
}
