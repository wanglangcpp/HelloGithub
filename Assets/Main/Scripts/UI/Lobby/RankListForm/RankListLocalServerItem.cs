using System;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class RankListLocalServerItem : MonoBehaviour
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
        private UILabel m_LevelLabel = null;

        [SerializeField]
        private UILabel m_Might = null;

        [SerializeField]
        private UILabel m_ScoreDesc = null;

        [SerializeField]
        private UILabel m_Score = null;

        [SerializeField]
        private PlayerHeadView m_Portrait = null;

        private PlayerData m_PlayerData = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        public void Refresh(PBRankInfo data)
        {
            int activeRankIndex;
            int rank = data.Ranking;
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

            m_PlayerName.text = data.PlayerInfo.Name;// GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", data.PlayerInfo.Name, data.PlayerInfo.Level);
            m_LevelLabel.text = data.PlayerInfo.Level.ToString();
            m_Might.text = data.PlayerInfo.Might.ToString();
            m_ScoreDesc.text = GameEntry.Localization.GetString("UI_TEXT_NOTICE_THE_TOTAL_FORCE");
            m_Score.text = data.Score.ToString();
            m_Portrait.InitPlayerHead(data.PlayerInfo.PortraitType);
            m_PlayerData = new PlayerData();
            m_PlayerData.UpdateData(data.PlayerInfo);
        }

        // Called by NGUI via reflection.
        public void OnClickItem()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PlayerSummaryForm, new PlayerSummaryFormDisplayData { ShowPlayerData = m_PlayerData, EnableInvite = true });
        }

        public void UpdateAnchors()
        {
            var rects = GetComponentsInChildren<UIRect>();
            for (int i = 0; i < rects.Length; i++)
            {
                rects[i].UpdateAnchors();
            }
        }
    }
}
