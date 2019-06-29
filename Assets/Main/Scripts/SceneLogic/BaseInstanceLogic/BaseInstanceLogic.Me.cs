using System.Collections.Generic;

namespace Genesis.GameClient
{
    public abstract partial class BaseInstanceLogic
    {
        protected class Me : Player
        {
            public Me(int id, PlayerHeroesData heroesData, IDictionary<CampType, List<ITargetable>> campTargetableObjects, BaseInstanceLogic instanceLogic)
                : base(id, heroesData, campTargetableObjects, instanceLogic)
            {

            }

            public void OnReviveHeroes()
            {
                var heroes = m_HeroesData.GetHeroes();
                for (int i = 0; i < heroes.Length; i++)
                {
                    heroes[i].HP = heroes[i].MaxHP;
                    heroes[i].IsDead = false;
                }

                GameEntry.TimeScale.ResumeGame();
                TryAutoSwitchHero();
            }

            public override void RequestSwitchHero(int index, bool ignoreCD = false)
            {
                if (!GetCurrentHeroCharacter().CanSwitchHero)
                {
                    return;
                }
                m_InstanceLogic.RequestSwitchHero(index, ignoreCD);
            }

            protected override void ShowHero(HeroData heroData)
            {
                GameEntry.Entity.ShowMeHero(heroData);
            }

            protected override void NotifyHeroesReady()
            {
                GameEntry.Event.Fire(this, new InstanceMePreparedEventArgs());
            }

            protected override void OnWillSwitchHero()
            {
                GameEntry.Input.MeHeroCharacter = null;
                GameEntry.Input.JoystickActivated = false;
                GameEntry.Input.SkillActivated = false;
                GameEntry.Event.Fire(this, new SwitchHeroStartEventArgs(m_Id));
                m_InstanceLogic.CameraController.ResetTarget();
            }

            protected override void OnSwitchHeroComplete()
            {
                base.OnSwitchHeroComplete();
                GameEntry.Input.MeHeroCharacter = m_InstanceLogic.MeHeroCharacter;
                GameEntry.Input.JoystickActivated = true;
                GameEntry.Input.SkillActivated = true;
                GameEntry.Event.Fire(this, new SwitchHeroCompleteEventArgs(m_Id));
            }

            public override HeroCharacter GetCurrentHeroCharacter()
            {
                return m_InstanceLogic.MeHeroCharacter;
            }
        }
    }
}
