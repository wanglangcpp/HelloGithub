using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本准备器抽象类。
    /// </summary>
    public abstract class AbstractInstancePreparer
    {
        protected BaseInstanceLogic m_InstanceLogic;

        public event GameFrameworkAction OnShouldGoToWaiting;

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

        public abstract void StartInstance();

        public abstract void OnLoadSceneSuccess(UnityGameFramework.Runtime.LoadSceneSuccessEventArgs e);

        public abstract void OnOpenUIFormSuccess(UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs e);

        public abstract void OnLoadBehaviorSuccess(LoadBehaviorSuccessEventArgs e);

        protected void FireShouldGoToWaiting()
        {
            if (OnShouldGoToWaiting != null)
            {
                OnShouldGoToWaiting();
            }
        }

        protected void FireShouldGoToResult()
        {
            if (OnShouldGoToResult != null)
            {
                OnShouldGoToResult();
            }
        }
        public virtual void OnLoadFail()
        {

        }
    }
}
