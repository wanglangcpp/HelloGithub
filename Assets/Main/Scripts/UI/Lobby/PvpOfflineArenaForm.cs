using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技副本准备界面。
    /// </summary>
    public class PvpOfflineArenaForm : BasePvpPrepareForm
    {
        private List<int> m_CachedHeroTeam = new List<int>();

        private OfflineArenaPrepareData m_OfflineArenaPrepareData = null;

        protected override void OnOpen(object userData)
        {
            m_CachedHeroTeam.Clear();
            m_CachedHeroTeam.AddRange(GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Arena).HeroType);
            base.OnOpen(userData);
            m_OfflineArenaPrepareData = userData as OfflineArenaPrepareData;
            if (m_OfflineArenaPrepareData == null)
            {
                Log.Error("User data is invalid.");
                return;
            }

            GameEntry.Event.Subscribe(EventId.HeroTeamDataChanged, OnHeroTeamDataChanged);
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.HeroTeamDataChanged, OnHeroTeamDataChanged);

            m_CachedHeroTeam.Clear();
            m_OfflineArenaPrepareData = null;
            base.OnClose(userData);
        }

        public override void OnClickChangeHeroButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData { Scenario = HeroTeamDisplayScenario.ArenaBattle });
        }

        protected override void EnterBattle()
        {
        }

        protected override void RefreshMyData()
        {
            base.RefreshMyData();
            RefreshMyHeroTeam();
        }

        protected override void RefreshOppData()
        {
            var oppData = GameEntry.Data.OfflineArenaOpponent;

            var player = oppData.Player;
            m_Opponent.Name.text = GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", player.Name, player.Level);
            m_Opponent.Might.text = player.TeamMight.ToString();
            m_OppPlayerId = oppData.Player.Id;
            m_Opponent.Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(player.PortraitType));

            var heroesData = oppData.Heroes;

            for (int i = 0; i < m_OppHeroes.Length; ++i)
            {
                if (i < heroesData.Data.Count)
                {
                    var heroDisplay = m_OppHeroes[i];

                    var lobbyHeroData = heroesData.Data[i];
                    heroDisplay.Root.SetActive(true);
                    heroDisplay.Portrait.spriteName = UIUtility.GetHeroPortraitSpriteName(lobbyHeroData.Type);
                    heroDisplay.HeroName.text = lobbyHeroData.Name;
                    m_OppHeroTeam.Add(lobbyHeroData.Type);
                }
                else
                {
                    var heroDisplay = m_OppHeroes[i];
                    heroDisplay.HeroName.text = string.Empty;
                    heroDisplay.Root.SetActive(false);
                }
            }
        }

        private void RefreshMyHeroTeam()
        {
            for (int i = 0; i < m_MyHeroes.Length; ++i)
            {
                var heroDisplay = m_MyHeroes[i];

                var heroTeamTypes = m_CachedHeroTeam;
                if (i < heroTeamTypes.Count && heroTeamTypes[i] > 0)
                {
                    heroDisplay.Root.SetActive(true);
                    var heroTypeId = heroTeamTypes[i];
                    heroDisplay.Portrait.spriteName = UIUtility.GetHeroPortraitSpriteName(heroTypeId);
                    heroDisplay.HeroName.text = GameEntry.Data.LobbyHeros.GetData(heroTypeId).Name;
                }
                else
                {
                    heroDisplay.HeroName.text = string.Empty;
                    heroDisplay.Root.SetActive(false);
                }
            }
        }

        private void OnHeroTeamDataChanged(object sender, GameEventArgs e)
        {
            m_CachedHeroTeam.Clear();
            m_CachedHeroTeam.AddRange(GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Arena).HeroType);
            RefreshMyHeroTeam();
        }

    }
}
