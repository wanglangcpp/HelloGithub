using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// PVP竞技数据类。
    /// </summary>
    [Serializable]
    public class PvpArenaData
    {
        [SerializeField]
        private int m_ChallengeCount = 0;

        public int ChallengeCount
        {
            get
            {
                return m_ChallengeCount;
            }
        }

        [SerializeField]
        private int m_Rank = 0;

        public int Rank
        {
            get
            {
                return m_Rank;
            }
        }

        public int LastRank
        {
            get;
            set;
        }

        [SerializeField]
        private int m_Score = 0;

        public int Score
        {
            get
            {
                return m_Score;
            }
        }

        /// <summary>
        /// 赛季
        /// </summary>
        [SerializeField]
        private int m_Season = 0;

        public int Season
        {
            get
            {
                return m_Season;
            }
        }

        public int LastScore
        {
            get;
            set;
        }

        private bool m_FirstTimeUpdateData = true;

        public void UpdateData(int challengeCount, int rank, int score, int season)
        {
            if (m_FirstTimeUpdateData)
            {
                LastScore = score;
                LastRank = rank;
                m_FirstTimeUpdateData = false;
            }
            else
            {
                LastScore = Score;
                LastRank = Rank;
            }

            m_Season = season;
            m_ChallengeCount = challengeCount;
            m_Rank = rank;
            m_Score = score;
        }
    }
}
