using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        [SerializeField]
        protected InstanceResultData m_ResultData = null;

        private void PopulateMockedSuccessData()
        {
            var playerLevelUp = Random.Range(0, 3);
            var playerDeltaExp = Random.Range(5, 3000);
            var coinEarned = Random.Range(1000, 10000);
            m_ResultData.ItsPlayer.NewLevel = m_ResultData.ItsPlayer.OldLevel + playerLevelUp;
            m_ResultData.ItsPlayer.NewExp = m_ResultData.ItsPlayer.OldExp + playerDeltaExp;
            m_ResultData.ItsPlayer.NewCoin = m_ResultData.ItsPlayer.OldCoin + coinEarned;

            for (int i = 0; i < m_ResultData.Heroes.Count; ++i)
            {
                var heroLevelUp = Random.Range(0, 3);
                var heroDeltaExp = Random.Range(5, 3000);
                var hero = m_ResultData.Heroes[i];
                hero.NewLevel = hero.OldLevel + heroLevelUp;
                hero.NewExp = hero.OldExp + heroDeltaExp;
            }
        }

        protected virtual void PrepareSuccessData()
        {
            m_ResultData = new InstanceResultData();
            m_ResultData.Type = GameEntry.SceneLogic.BaseInstanceLogic.Type;
            for (int i = 0; i < m_ResultData.RequestsComplete.Length; ++i)
            {
                m_ResultData.RequestsComplete[i] = IsRequestComplete(i);
            }

            m_ResultData.CompleteRequestCount = RequestCompleteCount;

            m_ResultData.ItsPlayer.Name = GameEntry.Data.Player.Name;
            m_ResultData.ItsPlayer.OldLevel = GameEntry.Data.Player.Level;
            m_ResultData.ItsPlayer.OldExp = GameEntry.Data.Player.Exp;
            m_ResultData.ItsPlayer.OldCoin = GameEntry.Data.Player.Coin;
            m_ResultData.ItsPlayer.OldMeridianEnergy = GameEntry.Data.Player.MeridianEnergy;
            m_ResultData.ItsPlayer.PortraitId = GameEntry.Data.Player.PortraitType;

            var lobbyHeroes = GetHeroTeamData();

            for (int i = 0; i < lobbyHeroes.Count; ++i)
            {
                var lobbyHero = lobbyHeroes[i];
                var resultHero = new InstanceResultData.Hero();
                resultHero.Name = lobbyHero.Name;
                resultHero.PortraitSpriteName = lobbyHero.PortraitSpriteName;
                resultHero.Profession = lobbyHero.Profession;
                resultHero.ElementId = lobbyHero.ElementId;
                resultHero.OldLevel = lobbyHero.Level;
                resultHero.OldExp = lobbyHero.Exp;
                m_ResultData.Heroes.Add(resultHero);
            }
        }

        protected void PopulateSuccessData()
        {
            m_ResultData.ItsPlayer.NewLevel = GameEntry.Data.Player.Level;
            m_ResultData.ItsPlayer.NewExp = GameEntry.Data.Player.Exp;
            m_ResultData.ItsPlayer.NewCoin = GameEntry.Data.Player.Coin;
            m_ResultData.ItsPlayer.NewMeridianEnergy = GameEntry.Data.Player.MeridianEnergy;

            var lobbyHeroes = GetHeroTeamData();
            for (int i = 0; i < m_ResultData.Heroes.Count; ++i)
            {
                var hero = m_ResultData.Heroes[i];

                hero.NewLevel = lobbyHeroes[i].Level;
                hero.NewExp = lobbyHeroes[i].Exp;
            }
        }

        private List<LobbyHeroData> GetHeroTeamData()
        {
            var ret = new List<LobbyHeroData>();
            var heroTypeIds = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType;

            for (int i = 0; i < heroTypeIds.Count; ++i)
            {
                if (heroTypeIds[i] == 0)
                {
                    break;
                }

                LobbyHeroData lobbyHero = null;

                for (int j = 0; j < GameEntry.Data.LobbyHeros.Data.Count; ++j)
                {
                    var tempHero = GameEntry.Data.LobbyHeros.Data[j];
                    if (tempHero.Type == heroTypeIds[i])
                    {
                        lobbyHero = tempHero;
                        break;
                    }
                }

                if (lobbyHero == null)
                {
                    break;
                }

                ret.Add(lobbyHero);
            }

            return ret;
        }

        protected abstract class BaseSinglePlayerInstanceSuccess : AbstractInstanceSuccess
        {
            protected override UIFormBaseUserData PopulateData(GameEventArgs e)
            {
                var instanceLogic = (InstanceLogic as BaseSinglePlayerInstanceLogic);
                instanceLogic.PopulateSuccessData();
                return null;
            }

            protected override void StopInstance()
            {
                base.StopInstance();
                var instanceLogic = (InstanceLogic as BaseSinglePlayerInstanceLogic);
                instanceLogic.ShouldShowHud = false;
                instanceLogic.StopAllAIsAndProhibitFurtherUse();
                instanceLogic.PrepareSuccessData();
            }

            protected void MockData()
            {
                var instanceLogic = (InstanceLogic as BaseSinglePlayerInstanceLogic);
                instanceLogic.PopulateMockedSuccessData();
            }
        }

        protected abstract class BaseSinglePlayerInstanceFailure : AbstractInstanceFailure
        {
            public BaseSinglePlayerInstanceFailure(bool shouldOpenUI) : base(shouldOpenUI)
            {

            }
        }
    }
}
