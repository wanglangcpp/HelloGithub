using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class AchievementData : IGenericData<AchievementData, PBAchievementInfo>
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
        private int m_PreAchievementId;

        [SerializeField]
        private int m_Weight;

        public int ProgressCount { get { return m_ProgressCount; } set { m_ProgressCount = value; } }

        public bool IsCompleted { get { return m_IsCompleted; } set { m_IsCompleted = value; } }

        public bool IsClaimed { get { return m_IsClaimed; } set { m_IsClaimed = value; } }

        public int PrePlayerLevel { get { return m_PrePlayerLevel; } set { m_PrePlayerLevel = value; } }

        public int PreAchievementId { get { return m_PreAchievementId; } set { m_PreAchievementId = value; } }

        public int Weight { get { return m_Weight; } set { m_Weight = value; } }

        public int Key
        {
            get
            {
                return m_Id;
            }
        }

        public void UpdateData(PBAchievementInfo data)
        {
            m_Id = data.AchievementId;
            m_ProgressCount = data.ProgressCount;
            m_IsCompleted = data.IsCompleted;
            m_IsClaimed = data.IsClaimed;
            var dtAchivenement = GameEntry.DataTable.GetDataTable<DRAchievement>().GetDataRow(m_Id);
            if (dtAchivenement == null)
            {
                return;
            }

            m_PrePlayerLevel = dtAchivenement.PrePlayerLevel;
            m_PreAchievementId = dtAchivenement.PreAchievementId;
            m_Weight = dtAchivenement.Weight;
        }
    }
}
