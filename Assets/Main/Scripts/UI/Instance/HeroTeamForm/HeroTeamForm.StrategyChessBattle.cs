using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroTeamForm
    {
        private class StrategyChessBattle : StrategyBase
        {
            public override GameObject PortraitTemplate
            {
                get
                {
                    return m_Form.m_PortraitTemplateChessBattle;
                }
            }

            public override IList<int> HeroTeam
            {
                get
                {
                    return GameEntry.Data.ChessBattleMe.HeroTeam.HeroType;
                }
            }

            public override IList<LobbyHeroData> SortedLobbyHeroes
            {
                get
                {
                    var copy = new List<LobbyHeroData>(GameEntry.Data.LobbyHeros.Data);
                    copy.Sort(CompareLobbyHeroes);
                    return copy;
                }
            }

            public override void SetPortraitForSelectionData(IList<int> selectedHeroes, int heroId, HeroPortraitForSelection script, bool isOnline)
            {
                base.SetPortraitForSelectionData(selectedHeroes, heroId, script);
                var chessBattleScript = script as HeroPortraitForSelectionInChessBattle;
                var status = GameEntry.Data.ChessBattleMe.HeroesStatus.GetData(heroId);
                var lobbyHero = GameEntry.Data.LobbyHeros.GetData(heroId);
                chessBattleScript.CurHPRatio = (status == null ? 1f : (lobbyHero.MaxHP > 0 ? (float)status.CurHP / lobbyHero.MaxHP : 0f));
            }

            public override void RequestChangeHeroTeam(List<int> heroTypeIds, HeroTeamType type)
            {
                if (heroTypeIds.Count <= 0)
                {
                    //    m_Form.ShowMightChangeForm(m_Form.m_OldHeroTeam, heroTypeIds);
                    return;
                }

                GameEntry.LobbyLogic.ChangeChessBattleHeroTeam(heroTypeIds);
            }

            private bool DisplayedTeamIsQualified
            {
                get
                {
                    if (!m_Form.CharacterIsAvailable(0))
                    {
                        return false;
                    }

                    var heroData = m_Form.m_HeroDisplays[0].Character.Data;
                    int heroTypeId = heroData.HeroId;

                    var portraits = m_Form.m_HeroProtraits;

                    var indexInPortrait = -1;
                    for (int i = 0; i < portraits.Count; ++i)
                    {
                        if (portraits[i].HeroId == heroTypeId)
                        {
                            indexInPortrait = i;
                        }
                    }

                    if (indexInPortrait < 0)
                    {
                        Log.Warning("Something odd must have happened.");
                        return false;
                    }

                    // 只要队长还有剩余血量，则认为阵容是合格的。
                    return (portraits[indexInPortrait] as HeroPortraitForSelectionInChessBattle).CurHPRatio > 0f;
                }
            }

            private static int CompareLobbyHeroes(LobbyHeroData a, LobbyHeroData b)
            {
                var chessBattleMe = GameEntry.Data.ChessBattleMe;

                var aIsAlive = chessBattleMe.HeroIsAlive(a.Type);
                var bIsAlive = chessBattleMe.HeroIsAlive(b.Type);

                if (aIsAlive && !bIsAlive)
                {
                    return -1;
                }

                if (bIsAlive && !aIsAlive)
                {
                    return 1;
                }

                return b.Level.CompareTo(a.Level);
            }
        }
    }
}
