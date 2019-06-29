using System.Collections.Generic;

namespace Genesis.GameClient
{
    public abstract partial class BasePvpInstanceLogic : BaseInstanceLogic
    {
        private class OtherPlayer : Player
        {
            public OtherPlayer(int playerId, PlayerHeroesData heroesData, IDictionary<CampType, List<ITargetable>> campTargetableObjects, BasePvpInstanceLogic instanceLogic)
                : base(playerId, heroesData, campTargetableObjects, instanceLogic)
            {

            }

            protected override void OnWillSwitchHero()
            {
                GameEntry.Event.Fire(this, new SwitchHeroStartEventArgs(m_Id));
            }

            public override HeroCharacter GetCurrentHeroCharacter()
            {
                var oppHeroesData = (m_InstanceLogic as BasePvpInstanceLogic).m_OppHeroesData;
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
                GameEntry.Entity.ShowHero(heroData);
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
