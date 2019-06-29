using GameFramework.Event;

namespace Genesis.GameClient
{
    public partial class MeridianForm : NGUIForm
    {

        private int m_CurrentMeridianIndex = -1;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.MeridianDataChanged, OnMeridianDataChanged);
            GameEntry.Event.Subscribe(EventId.OpenMeridianStar, OnMeridianOpen);
            InitMeridian();
        }

        protected override void OnResume()
        {
            base.OnResume();
            m_EffectId = -1;
            ShowAstrolabeEffect();
        }

        protected override void OnPostOpen(object data)
        {
            base.OnPostOpen(data);
            m_EffectId = -1;
            ShowAstrolabeEffect();
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.MeridianDataChanged, OnMeridianDataChanged);
            GameEntry.Event.Unsubscribe(EventId.OpenMeridianStar, OnMeridianOpen);
            m_MeridianInsufficientSubForm.SetActive(false);
            base.OnClose(userData);
        }

        private void OnMeridianDataChanged(object sender, GameEventArgs e)
        {
            InitMeridian();
        }

        private void OnMeridianOpen(object sender, GameEventArgs e)
        {
            RefreshPageList();
            RefreshAstrolabe(false);
            RefreshReward();
        }

        private void InitMeridian()
        {
            RefreshPageList();
            RefreshAstrolabe(true);
            RefreshReward();
        }
    }
}
