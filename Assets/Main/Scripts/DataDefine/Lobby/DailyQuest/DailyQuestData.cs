using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class DailyQuestData : IGenericData<DailyQuestData, PBDailyQuestInfo>
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private int m_ProgressCount;

        [SerializeField]
        private bool m_IsCompleted;

        [SerializeField]
        private bool m_IsClaimed;

        [SerializeField]
        private int m_PrePlayerLevel;        

        [SerializeField]
        private int m_Weight;

        public int Key
        {
            get
            {
                return m_Id;
            }
        }

        public int ProgressCount { get { return m_ProgressCount; } set { m_ProgressCount = value; } }

        public bool IsCompleted { get { return m_IsCompleted; } set { m_IsCompleted = value; } }

        public bool IsClaimed { get { return m_IsClaimed; } set { m_IsClaimed = value; } }

        public int PrePlayerLevel { get { return m_PrePlayerLevel; } set { m_PrePlayerLevel = value; } }

        public int Weight { get { return m_Weight; } set { m_Weight = value; } }

        public void UpdateData(PBDailyQuestInfo data)
        {
            m_Id = data.DailyQuestId;
            m_ProgressCount = data.ProgressCount;
            m_IsCompleted = data.IsCompleted;
            m_IsClaimed = data.IsClaimed;

            var dtDailyQuest = GameEntry.DataTable.GetDataTable<DRDailyQuest>().GetDataRow(m_Id);
            if (dtDailyQuest == null)
            {
                return;
            }

            m_PrePlayerLevel = dtDailyQuest.PrePlayerLevel;
            m_Weight = dtDailyQuest.Weight;
        }

        public void ResetDailyQuest()
        {
            m_ProgressCount = 0;
            m_IsCompleted = false;
            m_IsClaimed = false;
        }
    }
}
