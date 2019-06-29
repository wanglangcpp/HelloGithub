using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class HeroTeamForm
    {
        private const string EffectResourceName = "UI/effect_ui_heroteam_bench";

        private void OnShowEntitySuccess(object o, GameEventArgs e)
        {
            Log.Info("[HeroTeamForm OnShowEntitySuccess] Entrance.");
            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
            if (ne == null)
            {
                return;
            }

            if (!(ne.Entity.Logic is FakeCharacter))
            {
                return;
            }

            //Log.Info("[HeroTeamFrom OnShowEntitySuccess] Entity logic is FakeCharacter.");
            var newFakeCharacter = ne.Entity.Logic as FakeCharacter;
            int index = -1;

            for (int i = 0; i < m_HeroDisplays.Length; ++i)
            {
                if (newFakeCharacter.CachedTransform.parent == m_HeroDisplays[i].Platform)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
            {
                return;
            }

            //Log.Info("[HeroTeamFrom OnShowEntitySuccess] Valid index: {0}.", index);
            m_HeroDisplays[index].Character = newFakeCharacter;
            GameEntry.Waiting.StopWaiting(WaitingType.Default, GetType().Name);
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GetSerialId(), "", EffectResourceName, newFakeCharacter.Id));
            UpdateHeroDisplays();
            if (m_OldHeroTeam != null)
            {
                List<int> newHeroTeam = GetSelectedHeroIds(); 
                m_Strategy.RequestChangeHeroTeam(newHeroTeam, m_Strategy.HeroTeamInfoType.Value);
            }
        }

        private void OnHeroTeamDataChanged(object sender, GameEventArgs e)
        {
            m_Strategy.OnHeroTeamDataChanged();
        }

        private void OnLobbyHeroDataChanged(object sender, GameEventArgs e)
        {
            m_Strategy.OnLobbyHeroDataChanged();
        }
    }
}
