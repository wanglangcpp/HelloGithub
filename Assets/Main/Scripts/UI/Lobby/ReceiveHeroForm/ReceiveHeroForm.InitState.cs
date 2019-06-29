using GameFramework.Event;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ReceiveHeroForm
    {
        private class InitState : StateBase
        {
            private IFsm<ReceiveHeroForm> m_Fsm = null;

            protected override void OnEnter(IFsm<ReceiveHeroForm> fsm)
            {
                base.OnEnter(fsm);
                m_Fsm = fsm;
                GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
                ShowFakeCharacter(fsm);
                ShowCongrAnimation(fsm);
            }

            protected override void OnLeave(IFsm<ReceiveHeroForm> fsm, bool isShutdown)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
                base.OnLeave(fsm, isShutdown);
            }

            public override void GoToNextStep(IFsm<ReceiveHeroForm> fsm)
            {
                ChangeState<DebutState>(fsm);
            }

            public override void SkipAnimation(IFsm<ReceiveHeroForm> fsm)
            {

            }

            private void ShowCongrAnimation(IFsm<ReceiveHeroForm> fsm)
            {
                fsm.Owner.m_CongrContent.gameObject.SetActive(true);
                fsm.Owner.m_HeroInfo.gameObject.SetActive(false);
                fsm.Owner.m_CongrContent.text = GameEntry.Localization.GetString("UI_TEXT_RECEIVEHERO_NOTICE");
                var anim = fsm.Owner.m_CongrAnimation;
                anim.Rewind();
                anim[anim.clip.name].time = 0;
                anim[anim.clip.name].speed = 1;
                anim.Sample();
                anim.Play();
            }

            private void ShowFakeCharacter(IFsm<ReceiveHeroForm> fsm)
            {
                var heroData = m_Fsm.Owner.HeroData;
                FakeCharacter.Show(heroData.CharacterId, heroData.Id, m_Fsm.Owner.m_PlatformRoot, actionOnShow: FakeCharacterData.ActionOnShow.DebutForReceive);
            }

            private void OnShowEntitySuccess(object o, GameEventArgs e)
            {
                var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
                if (ne == null)
                {
                    return;
                }
                if (!(ne.Entity.Logic is FakeCharacter))
                {
                    return;
                }
                var newFakeCharacter = ne.Entity.Logic as FakeCharacter;
                if (newFakeCharacter.CachedTransform.parent != m_Fsm.Owner.m_PlatformRoot)
                {
                    return;
                }
                m_Fsm.Owner.m_Character = newFakeCharacter;
                GoToNextStep(m_Fsm);
            }
        }
    }
}
