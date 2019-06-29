using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class RankListFormPvpLocalServerItem : MonoBehaviour
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
        private UISprite PortraitSprite = null;

        [SerializeField]
        private UILabel m_Name, m_Might, m_Level;

        private PlayerData m_PlayerData = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);

            m_Name = transform.Find("PlayerPortrait/Name/Name Label").GetComponent<UILabel>();
            m_Might = transform.Find("PlayerPortrait/Might/Might Label").GetComponent<UILabel>();
            m_Level = transform.Find("PlayerPortrait/Level/Text").GetComponent<UILabel>();
            PortraitSprite = transform.Find("PlayerPortrait/Icon/Border/Portrait").GetComponent<UISprite>();
        }

        public void Refresh(RankListPvpData data)
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

            PortraitSprite.LoadAsync(UIUtility.GetPlayerPortraitIconId(data.Portrait));
            m_Name.text = data.Name;
            m_Might.text = data.Might.ToString();
            m_Level.text = data.Level.ToString();
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
            //GameEntry.UI.OpenUIForm(UIFormId.PlayerSummaryForm, new PlayerSummaryFormDisplayData { ShowPlayerData = m_PlayerData, EnableInvite = true });
        }
    }
}
