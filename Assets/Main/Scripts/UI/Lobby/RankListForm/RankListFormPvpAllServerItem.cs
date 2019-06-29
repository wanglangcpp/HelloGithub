using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class RankListFormPvpAllServerItem : MonoBehaviour
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
        private PlayerPortrait m_Portrait = null;

        [SerializeField]
        private UISprite m_PortraitSprite = null;

        [SerializeField]
        private UISprite m_Grading = null;
        private UILabel m_GradingText = null;

        [SerializeField]
        private UILabel m_PlayerName, m_Integral, m_Level, m_Server;

        private PlayerData m_PlayerData = null;

        DRPvpRank[] PvpRankData = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
            PvpRankData = GameEntry.DataTable.GetDataTable<DRPvpRank>().GetAllDataRows();

            m_Server = transform.Find("PlayerPortrait/Server/Label").GetComponent<UILabel>();
            m_PlayerName = transform.Find("PlayerPortrait/Name/Name Label").GetComponent<UILabel>();
            m_Integral = transform.Find("PlayerPortrait/Integral/Integral Label").GetComponent<UILabel>();
            m_Level = transform.Find("PlayerPortrait/Level/Text").GetComponent<UILabel>();
            m_PortraitSprite = transform.Find("PlayerPortrait/Icon/Border/Portrait").GetComponent<UISprite>();
            m_Grading = transform.Find("PlayerPortrait/Grading").GetComponent<UISprite>();
            m_GradingText = m_Grading.transform.GetComponentInChildren<UILabel>();
        }

        public void Refresh(PBRankInfo data)
        {
            int rank = data.Ranking;
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

            m_PlayerData = new PlayerData();
            m_PlayerData.UpdateData(data.PlayerInfo);
            m_Portrait.SetPortrait(m_PlayerData);

            m_PlayerName.text = data.PlayerInfo.Name;
            m_Level.text = data.PlayerInfo.Level.ToString();
            m_Integral.text = data.Score.ToString();

            m_Server.text = GameEntry.Data.ServerNames.GetServerData(data.ServerId).Name;
            //m_Grading.spriteName = GetPvpGrading(data.Score,false);
            m_GradingText.text = GameEntry.Localization.GetString(GetPvpGrading(data.Score, true));
        }
        /// <summary>
        /// 获取积分对应的信息
        /// </summary>
        /// <param name="integral">积分</param>
        /// <param name="isname">需要的内容（true-段位名称/false-段位图标）</param>
        /// <returns></returns>
        public string GetPvpGrading(int integral, bool isname)
        {
            for (int i = 0; i < PvpRankData.Length; i++)
            {
                if (integral >= PvpRankData[i].MinIntegral && integral <= PvpRankData[i].MaxIntegral)
                {
                    if (isname)
                    {
                        return PvpRankData[i].GradingName;
                    }
                    else
                    {
                        return PvpRankData[i].GradingIcon;
                    }
                }
            }
            return null;
        }


        public void UpdateAnchors()
        {
            var rects = GetComponentsInChildren<UIRect>();
            for (int i = 0; i < rects.Length; i++)
            {
                rects[i].UpdateAnchors();
            }
        }

        // Called by NGUI via reflection.
        public void OnClickItem()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PlayerSummaryForm, new PlayerSummaryFormDisplayData { ShowPlayerData = m_PlayerData, EnableInvite = true });
        }
    }
}
