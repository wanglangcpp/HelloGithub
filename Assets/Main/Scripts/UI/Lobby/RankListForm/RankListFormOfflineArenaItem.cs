using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class RankListFormOfflineArenaItem : MonoBehaviour
    {
        [Serializable]
        private class Rank
        {
            public Transform Self = null;
            public UILabel Num = null;
        }

        [Serializable]
        private class Hero
        {
            public Transform Self = null;
            public UISprite Portrait = null;
            public UISprite Element = null;
        }

        [SerializeField]
        private Rank[] m_Ranks = null;

        [SerializeField]
        private UILabel m_OppoMight = null;

        [SerializeField]
        private Hero[] m_Heroes = null;

        [SerializeField]
        private PlayerPortrait m_Portrait = null;

        private List<int> m_HeroTeam = new List<int>();
        private PlayerData m_PlayerData = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        public void Refresh(PBRankInfo data)
        {
            m_PlayerData = new PlayerData();
            m_PlayerData.UpdateData(data.PlayerInfo);
            int rank = data.Ranking;
            int activeRankIndex;
            if (data.HasArenaMight)
            {
                m_OppoMight.text = data.ArenaMight.ToString();
            }
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
            m_Portrait.SetPortrait(m_PlayerData);
            var heroes = data.ArenaHeroTypes;
            m_HeroTeam.Clear();
            for (int i = 0; i < m_Heroes.Length; ++i)
            {
                if (i < heroes.Count && heroes[i] > 0)
                {
                    m_Heroes[i].Self.gameObject.SetActive(true);
                    int heroTypeId = heroes[i];
                    m_Heroes[i].Portrait.LoadAsync(UIUtility.GetHeroProtraitIconId(heroTypeId));
                    int heroElementId = UIUtility.GetHeroElement(heroTypeId);
                    m_Heroes[i].Element.spriteName = UIUtility.GetElementSpriteName(heroElementId);
                    m_HeroTeam.Add(heroes[i]);
                }
                else
                {
                    m_Heroes[i].Self.gameObject.SetActive(false);
                }
            }
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
