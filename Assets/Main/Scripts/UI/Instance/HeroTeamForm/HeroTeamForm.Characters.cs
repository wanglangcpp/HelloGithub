using GameFramework;

namespace Genesis.GameClient
{
    public partial class HeroTeamForm
    {
        private void ShowFakeCharacter(int characterId, int heroId, int index)
        {
            Log.Info("[HeroTeamForm ShowFakeCharacter] characterId={0}, heroId={1}, index={2}.", characterId, heroId, index);
            GameEntry.Waiting.StartWaiting(WaitingType.Default, GetType().Name);
            FakeCharacter.Show(characterId, heroId, m_HeroDisplays[index].Platform);
        }

        private void SwapCharacters(int i, int j)
        {
            if (i < 0 || j < 0 || i == j)
            {
                return;
            }

            var characterI = m_HeroDisplays[i].Character;
            var characterJ = m_HeroDisplays[j].Character;

            if (!CharacterIsAvailable(i) || !CharacterIsAvailable(j))
            {
                Log.Error("Swapping 2 fake characters demands both of them be available.");
                return;
            }
            m_HeroDisplays[i].Character = characterJ;
            m_HeroDisplays[j].Character = characterI;

            characterJ.CachedTransform.parent = m_HeroDisplays[i].Platform;
            characterJ.ResetTransform();
            SetPortraitDisplayIndexInTeam(characterJ.Data.HeroId, i);

            characterI.CachedTransform.parent = m_HeroDisplays[j].Platform;
            characterI.ResetTransform();
            SetPortraitDisplayIndexInTeam(characterI.Data.HeroId, j);
        }

        private void RemoveFakeCharacter(int index)
        {
            Log.Info("[HeroTeamForm RemoveFakeCharacter] index={0}.", index);
            ResetPortraitIndexInTeam(m_HeroDisplays[index].Character.Data.HeroId);
            GameEntry.Entity.HideEntity(m_HeroDisplays[index].Character.Entity);
            m_HeroDisplays[index].Character = null;
        }

        private void ClearFakeCharacters()
        {
            Log.Info("[HeroTeamForm ClearFakeCharacters] Start.");
            for (int i = 0; i < m_HeroDisplays.Length; ++i)
            {
                var c = m_HeroDisplays[i].Character;

                if (c != null && c.IsAvailable)
                {
                    GameEntry.Entity.HideEntity(c.Entity);
                }

                m_HeroDisplays[i].Character = null;
            }
        }
    }
}
