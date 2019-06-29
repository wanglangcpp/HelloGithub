using GameFramework;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class HeroTeamForm
    {
        private class StrategyOtherPlayer : StrategyBase
        {
            private List<int> m_HeroTeam = null;
            private List<LobbyHeroData> m_LobbyHeroes = null;

            public override void RequestChangeHeroTeam(List<int> heroTypeIds, HeroTeamType type)
            {
                throw new NotSupportedException();
            }

            public override void OnHeroTeamDataChanged()
            {
                throw new NotSupportedException();
            }

            public override bool CanChangeHeroTeam
            {
                get
                {
                    return false;
                }
            }

            public override void Init(HeroTeamForm form)
            {
                base.Init(form);
                var lobbyHeroesData = m_Form.m_UserData.Heroes;
                if (lobbyHeroesData == null)
                {
                    Log.Error("User data is invalid.");
                }

                m_LobbyHeroes = new List<LobbyHeroData>(lobbyHeroesData.Data);
                m_HeroTeam = new List<int>();
                for (int i = 0; i < m_LobbyHeroes.Count; ++i)
                {
                    m_HeroTeam.Add(m_LobbyHeroes[i].Type);
                }
            }

            public override IList<int> HeroTeam
            {
                get
                {
                    return m_HeroTeam;
                }
            }

            public override IList<LobbyHeroData> SortedLobbyHeroes
            {
                get
                {
                    return m_LobbyHeroes;
                }
            }

            public override void OnLobbyHeroDataChanged()
            {
                // Empty.
            }

            public override void OpenHeroInfoForm(int heroType)
            {
                int index = -1;
                for (int i = 0; i < m_LobbyHeroes.Count; ++i)
                {
                    if (m_LobbyHeroes[i].Type == heroType)
                    {
                        index = i;
                        break;
                    }
                }

                if (index < 0)
                {
                    Log.Error("Invalid hero type '{0}'", heroType);
                    return;
                }
            }
        }
    }
}
