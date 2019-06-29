using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄图鉴卡牌。
    /// </summary>
    public class HeroAlbumCard : MonoBehaviour
    {
        private UIResourceReleaser m_ResourceReleaser = new UIResourceReleaser();

        public BaseLobbyHeroData HeroData
        {
            get
            {
                return m_CachedData;
            }
        }
        protected int m_HeroId = -1;

        public int HeroId
        {
            get
            {
                return m_HeroId;
            }
        }

        [SerializeField]
        protected UISprite m_ElementIcon = null;

        [SerializeField]
        protected UITexture m_Portrait = null;

        [SerializeField]
        protected UILabel m_HeroName = null;

        [SerializeField]
        private UIButton m_ComposeButton = null;

        [SerializeField]
        private UILabel m_ComposeButtonText = null;

        [SerializeField]
        private UILabel m_Might = null;

        [SerializeField]
        private UILabel m_PiecesLabel = null;

        [SerializeField]
        private UIProgressBar m_PiecesProgress = null;

        [SerializeField]
        private UISprite m_QualityBorder = null;

        [SerializeField]
        private UILabel m_QualityLevelLabel = null;

        [SerializeField]
        private Shader m_NormalShader = null;

        [SerializeField]
        private Shader m_GreyShader = null;

        [SerializeField]
        private GameObject m_Reminder = null;

        [SerializeField]
        private UIGrid m_OnlineStarParent = null;

        [SerializeField]
        private UIGrid m_OfflineStarParent = null;

        [SerializeField]
        protected UISprite[] m_OnlineStars = null;

        [SerializeField]
        protected UISprite[] m_OfflineStars = null;

        [SerializeField]
        protected UISprite m_QualityCornerIcon = null;

        [SerializeField]
        protected GameObject m_PvpTeamTag = null;

        [SerializeField]
        protected GameObject m_InstanceTeamTag = null;

        [SerializeField]
        private UILabel m_PvpTeamLabel = null;

        [SerializeField]
        private UILabel m_InstanceTeamLabel = null;


        private BaseLobbyHeroData m_CachedData = null;

        private bool m_CanCompose = false;
        private bool m_CanCultivate = false;

        public bool UnpossessedHero
        {
            get
            {
                if (m_CachedData == null)
                {
                    return false;
                }

                return m_CachedData is UnpossessedLobbyHeroData;
            }
        }

        public void RefreshData(BaseLobbyHeroData heroData)
        {
            m_HeroId = heroData.Type;
            DRHero heroDataRow = null;
            RefreshColorTint(heroData);
            m_ElementIcon.spriteName = UIUtility.GetElementSpriteName(heroData.ElementId);

            m_Portrait.LoadAsync(UIUtility.GetHeroBigPortraitTextureId(heroData.Type));
            bool heroInOfflineArenaTeam = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Arena).HeroType.Contains(heroData.Type);

            bool offlineArenaUnlocked = false;
            var activityDataRow = GameEntry.DataTable.GetDataTable<DRActivity>().GetDataRow((int)ActivityType.OfflineArena);
            if (activityDataRow != null)
            {
                offlineArenaUnlocked = GameEntry.Data.Player.Level >= activityDataRow.UnlockLevel;
            }

            m_PvpTeamTag.SetActive(offlineArenaUnlocked && heroInOfflineArenaTeam);
            m_PvpTeamLabel.text = GameEntry.Localization.GetString("UI_TEXT_TEAM_FOR_PVP");
            m_InstanceTeamTag.SetActive(heroInOfflineArenaTeam);
            m_InstanceTeamLabel.text = GameEntry.Localization.GetString("UI_TEXT_TEAM_FOR_INSTANCE");

            if (heroData is UnpossessedLobbyHeroData)
            {
                m_OnlineStarParent.gameObject.SetActive(false);
                m_OfflineStarParent.gameObject.SetActive(true);
                UIUtility.SetStarLevel(m_OfflineStars, heroData.StarLevel);
                m_OfflineStarParent.Reposition();
            }
            else
            {
                m_OnlineStarParent.gameObject.SetActive(true);
                m_OfflineStarParent.gameObject.SetActive(false);
                UIUtility.SetStarLevel(m_OnlineStars, heroData.StarLevel);
                m_OnlineStarParent.Reposition();
            }

            m_HeroName.text = heroData.Name;
            m_HeroName.color = ColorUtility.GetColorForQuality((int)heroData.Quality);
            m_PiecesLabel.gameObject.SetActive(heroData is UnpossessedLobbyHeroData);
            m_PiecesProgress.gameObject.SetActive(heroData is UnpossessedLobbyHeroData);
            if (heroData is UnpossessedLobbyHeroData)
            {
                m_CanCultivate = false;
                var unpossessedHeroData = heroData as UnpossessedLobbyHeroData;
                m_CanCompose = unpossessedHeroData.ComposedItemCount >= unpossessedHeroData.ComposeItemNeed;
                m_PiecesLabel.text = GameEntry.Localization.GetString("UI_TEXT_SLASH", unpossessedHeroData.ComposedItemCount, unpossessedHeroData.ComposeItemNeed);
                m_PiecesProgress.value = (float)unpossessedHeroData.ComposedItemCount / (float)unpossessedHeroData.ComposeItemNeed;
            }
            else if (heroData is LobbyHeroData)
            {
                heroDataRow = GameEntry.DataTable.GetDataTable<DRHero>().GetDataRow(heroData.Type);
                var item = GameEntry.Data.Items.GetData(heroDataRow.StarLevelUpItemId);
                int count = item == null ? 0 : item.Count;
                int starLevelUpItemCount = heroDataRow.StarLevelUpItemCounts[heroData.StarLevel - 1];
                m_CanCultivate = count >= starLevelUpItemCount && heroData.StarLevel < Constant.HeroStarLevelCount;
                ((LobbyHeroData)heroData).CanStarLevelUp = m_CanCultivate;
                m_CanCompose = false;
            }

            m_PiecesProgress.gameObject.SetActive(m_PiecesProgress.value < 1);

            m_Reminder.SetActive(m_CanCompose || m_CanCultivate);
            m_ComposeButton.gameObject.SetActive(m_CanCompose);
            m_Might.gameObject.SetActive(heroData is LobbyHeroData);
            m_Might.text = GameEntry.Localization.GetString("UI_TEXT_HERO_GS_TEXT", heroData.Might);
            m_Might.color = m_HeroName.color;
            m_CachedData = heroData;
            m_QualityBorder.spriteName = Constant.Quality.HeroSquareBorderSpriteNames[(int)heroData.Quality];
            m_QualityLevelLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", heroData.QualityLevel);
            m_QualityLevelLabel.color = Constant.Quality.QualityLevelColors[(int)heroData.Quality];
            m_QualityCornerIcon.spriteName = Constant.Quality.HeroBorderCornerSpriteNames[(int)heroData.Quality];
            m_QualityCornerIcon.gameObject.SetActive((heroData.QualityLevel > 0) && (heroData is LobbyHeroData));
        }

        // Called by NGUI via reflection
        public void OnClickMe()
        {
            if (m_CachedData == null)
            {
                return;
            }
            if (HeroAlbumForm.IsCanClickOpenHeroInfoFormBtn)
                HeroAlbumForm.IsCanClickOpenHeroInfoFormBtn = false;
            else
                return;
            if (m_CachedData is UnpossessedLobbyHeroData)
            {
                GameEntry.UI.OpenUIForm(UIFormId.HeroInfoForm_Unpossessed, new HeroInfoUnpossessedDisplayData
                {
                    UnpossessedHeroData = m_CachedData as UnpossessedLobbyHeroData,
                });
            }
            else
            {
                if (!GameEntry.OpenFunction.CheckHeroAlbumCard())
                {
                    return;
                }

                int index = -1;
                List<BaseLobbyHeroData> baseHeroData = new List<BaseLobbyHeroData>();
                var lobbyHeroes = GameEntry.Data.LobbyHeros.Data;

                lobbyHeroes.Sort(Comparer.CompareHeroes);
                for (int i = 0; i < lobbyHeroes.Count; i++)
                {
                    baseHeroData.Add(lobbyHeroes[i]);
                    if (lobbyHeroes[i].Type == m_CachedData.Type)
                    {
                        index = i;
                    }
                }

                if (index < 0)
                {
                    Log.Error("Oops, hero type ID '{0}' cannot be found.", m_CachedData.Type.ToString());
                    return;
                }

                GameEntry.UI.OpenUIForm(UIFormId.HeroInfoForm_Possessed, new HeroInfoDisplayData
                {
                    Scenario = HeroInfoScenario.Mine,
                    IndexInAllHeroes = index,
                    AllHeroes = baseHeroData,
                });
            }
        }

        public void OnClickComposeBtn()
        {
            if (HeroAlbumForm.IsCanClickOpenHeroInfoFormBtn)
                HeroAlbumForm.IsCanClickOpenHeroInfoFormBtn = false;
            else
                return;
            if (m_CanCompose)
            {
                GameEntry.LobbyLogic.ComposeHero(m_CachedData.Type);
            }
        }

        private void RefreshColorTint(BaseLobbyHeroData heroData)
        {
            var shader = heroData is UnpossessedLobbyHeroData ? m_GreyShader : m_NormalShader;

            m_Portrait.shader = shader;
        }

        #region MonoBehaviour

        private void Awake()
        {
            m_ComposeButtonText.text = GameEntry.Localization.GetString(m_ComposeButtonText.text);
        }

        private void Start()
        {
            m_ResourceReleaser.CollectWidgets(gameObject);
        }

        private void OnDestroy()
        {
            m_ResourceReleaser.ReleaseResources();
        }

        #endregion MonoBehaviour
    }
}
