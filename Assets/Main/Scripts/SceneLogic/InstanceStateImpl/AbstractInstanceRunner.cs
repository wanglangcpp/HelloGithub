using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本运行器抽象类。
    /// </summary>
    public abstract class AbstractInstanceRunner
    {
        protected BaseInstanceLogic m_InstanceLogic;

        public event GameFrameworkAction OnShouldGoToResult;

        public virtual void Init(BaseInstanceLogic instanceLogic)
        {
            m_InstanceLogic = instanceLogic;
        }

        public virtual void Shutdown(bool isExternalShutdown)
        {
            m_InstanceLogic = null;
        }

        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        public abstract void OnWin(InstanceSuccessReason reason, GameFrameworkAction onComplete);

        public abstract void OnLose(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete);

        public abstract void OnDraw(InstanceDrawReason reason, GameFrameworkAction onComplete);

        protected void FireShouldGoToResult()
        {
            if (OnShouldGoToResult != null)
            {
                OnShouldGoToResult();
            }
        }
    }
}
