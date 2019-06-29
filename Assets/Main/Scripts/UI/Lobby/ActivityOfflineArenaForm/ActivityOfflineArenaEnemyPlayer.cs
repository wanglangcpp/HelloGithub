using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ActivityOfflineArenaEnemyPlayer : MonoBehaviour
    {
        [Serializable]
        private class BattleAvatar
        {
            [SerializeField]
            private GameObject m_Root = null;
            
            [SerializeField]
            private UISprite m_IconSprite = null;

            [SerializeField]
            private UISprite m_ElementSprite = null;

            [SerializeField]
            private UISprite m_QualitySprite = null;

            public void SetAvatarData(LobbyHeroData avatarData)
            {
                int avatarId = avatarData.Type;
                m_IconSprite.LoadAsync(UIUtility.GetHeroProtraitIconId(avatarId));
                int avatarElementId = UIUtility.GetHeroElement(avatarId);
                m_ElementSprite.spriteName = UIUtility.GetElementSpriteName(avatarElementId);
                m_QualitySprite.spriteName = Constant.Quality.HeroBorderSpriteNames[(int)avatarData.Quality];

                m_Root.SetActive(true);
            }

            public void SetEmpty()
            {
                m_Root.SetActive(false);
            }
        }

        [SerializeField]
        private GameObject m_FirstRankingObject = null;

        [SerializeField]
        private GameObject m_SecondRankingObject = null;

        [SerializeField]
        private GameObject m_ThirdRankingObject = null;

        [SerializeField]
        private PlayerPortrait m_PlayerPortrait = null;

        [SerializeField]
        private UILabel m_RankingLabel = null;

        [SerializeField]
        private UILabel m_DefendMightLabel = null;

        [SerializeField]
        private List<BattleAvatar> m_BattleAvatars = null;

        [SerializeField]
        private GameObject m_ButtonObject = null;

        [SerializeField]
        private GameObject m_DefendLabelObject = null;

        [SerializeField]
        private UIButton m_ChallengeButton = null;

        private OfflineArenaPlayerData m_EnemyData = null;

        protected void Start()
        {
            // 初始化Label Text
            UIUtility.ReplaceDictionaryTextForLabels(m_ButtonObject);
            UIUtility.ReplaceDictionaryTextForLabels(m_DefendLabelObject);
            var collider = m_PlayerPortrait.gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(100, 100);
        }

        public void SetEnemyData(OfflineArenaPlayerData enemyData)
        {
            m_EnemyData = enemyData;

            SetRankLabel(enemyData.Rank);
            m_PlayerPortrait.SetPortrait(enemyData.Player);
            m_DefendMightLabel.text = CalculateDefendMight(enemyData.Heroes).ToString();

            for (int i = 0; i < m_BattleAvatars.Count; i++)
            {
                if (i < enemyData.Heroes.Data.Count)
                    m_BattleAvatars[i].SetAvatarData(enemyData.Heroes.Data[i]);
                else
                    m_BattleAvatars[i].SetEmpty();
            }

            m_ChallengeButton.isEnabled = GameEntry.Data.OfflineArena.IsEnableChallenge;

            UIEventListener.Get(m_PlayerPortrait.gameObject).onClick = OnPortraitClick;
        }

        private void OnPortraitClick(GameObject go)
        {
            GameEntry.UI.OpenUIForm(UIFormId.PlayerSummaryForm, new PlayerSummaryFormDisplayData { ShowPlayerData = m_EnemyData.Player, EnableInvite = true });
        }

        public void OnFightClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.OfflineMatchForm, new OfflineMatchFormDisplayData() { EnemyPlayerData = m_EnemyData });
        }

        private int CalculateDefendMight(LobbyHeroesData heroes)
        {
            int defendMight = 0;

            for (int i = 0; i < heroes.Data.Count; i++)
                defendMight += heroes.Data[i].Might;

            return defendMight;
        }

        private void SetRankLabel(int rank)
        {
            if (rank > 3)
            {
                m_FirstRankingObject.SetActive(false);
                m_SecondRankingObject.SetActive(false);
                m_ThirdRankingObject.SetActive(false);
                m_RankingLabel.gameObject.SetActive(true);
                m_RankingLabel.text = rank.ToString();
            }
            else if (rank == 3)
            {
                m_FirstRankingObject.SetActive(false);
                m_SecondRankingObject.SetActive(false);
                m_ThirdRankingObject.SetActive(true);
                m_RankingLabel.gameObject.SetActive(false);
            }
            else if (rank == 2)
            {
                m_FirstRankingObject.SetActive(false);
                m_SecondRankingObject.SetActive(true);
                m_ThirdRankingObject.SetActive(false);
                m_RankingLabel.gameObject.SetActive(false);
            }
            else if (rank == 1)
            {
                m_FirstRankingObject.SetActive(true);
                m_SecondRankingObject.SetActive(false);
                m_ThirdRankingObject.SetActive(false);
                m_RankingLabel.gameObject.SetActive(false);
            }
        }
    }
}