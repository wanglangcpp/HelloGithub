using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroTeamForm
    {
        private abstract class StrategyBase
        {
            protected HeroTeamForm m_Form = null;

            public virtual bool CanChangeHeroTeam
            {
                get
                {
                    return true;
                }
            }

            public virtual void Init(HeroTeamForm form)
            {
                m_Form = form;
            }

            public void Shutdown()
            {
                m_Form = null;
            }

            public virtual GameObject PortraitTemplate
            {
                get
                {
                    return m_Form.m_PortraitTemplateNormal;
                }
            }

            public virtual IList<int> HeroTeam
            {
                get
                {
                    return GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType;
                }
            }

            public virtual IList<LobbyHeroData> SortedLobbyHeroes
            {
                get
                {
                    return GameEntry.Data.LobbyHeros.Data;
                }
            }

            public virtual void SetPortraitForSelectionData(IList<int> selectedHeroes, int heroId, HeroPortraitForSelection script, bool isOnline = true)
            {
                script.HeroId = heroId;
                script.Portrait.LoadAsync(UIUtility.GetHeroBigPortraitTextureId(heroId));
                script.IndexInTeam = !isOnline ? -1 : selectedHeroes.IndexOf(heroId);
                script.ElementIcon.spriteName = UIUtility.GetElementSpriteName(UIUtility.GetHeroElement(heroId));
                var hero = GameEntry.Data.LobbyHeros.GetData(heroId);
                script.IsOnline = isOnline;
                if (hero == null)
                {
                    return;
                }
                UIUtility.SetStarLevel(script.Stars, hero.StarLevel);
                script.QualityLevelLabel.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", hero.QualityLevel);
                script.QualityIcon.spriteName = Constant.Quality.HeroSquareBorderSpriteNames[(int)hero.Quality];
                script.QualityCornerIcon.spriteName = Constant.Quality.HeroBorderCornerSpriteNames[(int)hero.Quality];
                script.QualityCornerIcon.gameObject.SetActive(hero.QualityLevel > 0);
                script.QualityLevelLabel.color = Constant.Quality.QualityLevelColors[(int)hero.Quality];
            }

            public virtual void OnHeroTeamDataChanged()
            {
                m_Form.ShowMightChangeForm(m_Form.m_OldMight, GameEntry.Data.Player.TeamMight);
            }

            public virtual void OnHeroDisplaysUpdated()
            {

            }

            public virtual void OnLobbyHeroDataChanged()
            {
                m_Form.UpdateHeroDisplays();
            }

            public virtual void RequestChangeHeroTeam(List<int> heroTypeIds, HeroTeamType type)
            {
                if (heroTypeIds.Count <= 0)
                {
                    return;
                }
                IsCanClickChangeHeroTeamBtn = false;
                Log.Debug("-----------------------------------Requset Change Hero Team........{0}", IsCanClickChangeHeroTeamBtn);
                GameEntry.LobbyLogic.ChangeHeroTeam(heroTypeIds, type);
            }

            public virtual HeroTeamType? HeroTeamInfoType
            {
                get
                {
                    return HeroTeamType.Default;
                }
            }

            public virtual void OpenHeroInfoForm(int heroType)
            {
                var allHeroes = UIUtility.GetLobbyHeroesIncludingUnpossessed();

                int indexInAllHeroes = -1;
                for (int i = 0; i < allHeroes.Count; ++i)
                {
                    if (allHeroes[i].Type == heroType)
                    {
                        indexInAllHeroes = i;
                        break;
                    }
                }

                if (indexInAllHeroes < 0)
                {
                    Log.Error("Oops, hero type '{0}' not found.", heroType);
                    return;
                }
            }
        }

        private static StrategyBase CreateStrategy(HeroTeamDisplayScenario scenario)
        {
            switch (scenario)
            {
                case HeroTeamDisplayScenario.Lobby:
                    return new StrategyLobby();
                //                 case HeroTeamDisplayScenario.InstanceSelection:
                //                     return new StrategyInstanceSelection();
                //                 case HeroTeamDisplayScenario.ChessBattle:
                //                     return new StrategyChessBattle();
                case HeroTeamDisplayScenario.ArenaBattle:
                    return new StrategyArenaBattle();
                //                 case HeroTeamDisplayScenario.OtherPlayer:
                //                     return new StrategyOtherPlayer();
                //                 case HeroTeamDisplayScenario.PvpArenaBattle:
                //                     return new StrategyPvpArenaBattle();
                default:
                    Log.Error("Scenario {0} not supported.", scenario);
                    return null;
            }
        }
    }
}
