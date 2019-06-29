using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class RankListAllServerItem : MonoBehaviour
    {
        [Serializable]
        private class Rank
        {
            public Transform Self = null;
            public UILabel Num = null;
        }

        [SerializeField]
        private Rank[] m_Ranks = null;

        [SerializeField]
        private UILabel m_PlayerName = null;

        [SerializeField]
        private UILabel m_ServerName = null;

        [SerializeField]
        private UILabel m_Score = null;

        [SerializeField]
        private PlayerHeadView m_Portrait = null;

        public void Refresh(PBPlayerInfo data, int score, int rank, int serverId)
        {
            int activeRankIndex;
            if (1 <= rank && rank <= 3)
            {
                activeRankIndex = rank - 1;
            }
            else
            {
                activeRankIndex = m_Ranks.Length - 1;
            }

            for (int i = 0; i < m_Ranks.Length; ++i)
            {
                m_Ranks[i].Self.gameObject.SetActive(activeRankIndex == i);
            }

            var activeRank = m_Ranks[activeRankIndex];

            if (activeRank.Num != null)
            {
                activeRank.Num.text = rank.ToString();
            }

            m_PlayerName.text = data.Name;
            var serverData = GameEntry.Data.ServerNames.GetServerData(serverId);
            if (serverData != null)
            {
                m_ServerName.text = serverData.Name;
            }
            m_Score.text = GameEntry.Localization.GetString("UI_TEXT_PVP_INTEGRAL", score);
            m_Portrait.InitPlayerHead(data.PortraitType);
        }
    }
}
