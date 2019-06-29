using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class BasePvpaiInstanceLogic
    {
        private class Opponent : Player
        {
            public Opponent(int id, PlayerHeroesData heroesData, IDictionary<CampType, List<ITargetable>> campTargetableObjects, BasePvpaiInstanceLogic instanceLogic)
                : base(id, heroesData, campTargetableObjects, instanceLogic)
            {

            }

            protected override void OnWillSwitchHero()
            {
                GameEntry.Event.Fire(this, new SwitchHeroStartEventArgs(GameEntry.Data.Player.Id));
            }

            public override HeroCharacter GetCurrentHeroCharacter()
            {
                var oppHeroesData = (m_InstanceLogic as BasePvpaiInstanceLogic).OppHeroesData;
                var entity = GameEntry.Entity.GetEntity(oppHeroesData.CurrentHeroData.Id);
                if (entity == null)
                {
                    return null;
                }

                var heroCharacter = entity.Logic as HeroCharacter;
                return heroCharacter;
            }

            protected override void ShowHero(HeroData heroData)
            {
                GameEntry.Entity.ShowPureAIHero(heroData);
            }

            protected override void OnSwitchHeroComplete()
            {
                base.OnSwitchHeroComplete();
                GameEntry.Event.Fire(this, new SwitchHeroCompleteEventArgs(m_Id));
            }

            protected override void NotifyHeroesReady()
            {
                GameEntry.Event.Fire(this, new InstanceOppPreparedEventArgs(m_Id));
            }
        }
    }
}
