using GameFramework;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public abstract class AbstractInstanceResult
    {
        private readonly bool m_ShouldOpenUI;

        private GameFrameworkAction m_OnComplete;

        private BaseInstanceLogic m_InstanceLogic;

        public AbstractInstanceResult(bool shouldOpenUI)
        {
            m_ShouldOpenUI = shouldOpenUI;
        }

        public abstract InstanceResultType Type { get; }

        protected BaseInstanceLogic InstanceLogic { get { return m_InstanceLogic; } }

        public void Start()
        {
            StopInstance();
            SendLeaveInstanceRequest();
        }

        public virtual void ShutDown()
        {

        }

        protected void InitInternal(BaseInstanceLogic instanceLogic, GameFrameworkAction onComplete)
        {
            m_InstanceLogic = instanceLogic;
            m_OnComplete = onComplete;
        }

        protected void OnReceiveLeaveInstanceResponse(object sender, GameEventArgs e)
        {
            if (!m_InstanceLogic.HasResult)
            {
                return;
            }

            if (m_ShouldOpenUI)
            {
                var data = PopulateData(e);
                ShowResultUI(data);
            }

            if (m_OnComplete != null)
            {
                m_OnComplete();
            }
        }

        protected virtual void StopInstance()
        {
            GameEntry.Input.MeHeroCharacter = null;
            GameEntry.Input.JoystickActivated = false;
            GameEntry.Input.SkillActivated = false;
            m_InstanceLogic.DisableAutoFight();
            GameEntry.SceneLogic.MeHeroCharacter.StopMove();
        }

        protected abstract void SendLeaveInstanceRequest();

        protected virtual UIFormBaseUserData PopulateData(GameEventArgs e)
        {
            return null;
        }

        protected abstract void ShowResultUI(UIFormBaseUserData userData);
    }

}
