using GameFramework;

namespace Genesis.GameClient
{
    public abstract partial class BaseInstanceLogic
    {
        protected AbstractInstanceResult m_Result = null;

        public bool HasResult
        {
            get
            {
                return m_Result != null;
            }
        }

        protected AbstractInstanceSuccess InitSuccessResult(InstanceSuccessReason reason, GameFrameworkAction onComplete)
        {
            var result = CreateSuccessResult();
            result.Init(this, reason, onComplete);
            return result;
        }

        protected AbstractInstanceFailure InitFailureResult(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
        {
            var result = CreateFailureResult(shouldOpenUI);
            result.Init(this, reason, onComplete);
            return result;
        }

        protected AbstractInstanceDraw InitDrawResult(InstanceDrawReason reason, GameFrameworkAction onComplete)
        {
            var result = CreateDrawResult();
            result.Init(this, reason, onComplete);
            return result;
        }

        protected abstract AbstractInstanceSuccess CreateSuccessResult();

        protected abstract AbstractInstanceFailure CreateFailureResult(bool shouldOpenUI);

        protected virtual AbstractInstanceDraw CreateDrawResult()
        {
            return null;
        }
    }
}
