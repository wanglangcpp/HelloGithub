using GameFramework;

namespace Genesis.GameClient
{
    public abstract class AbstractInstanceFailure : AbstractInstanceResult
    {
        public InstanceFailureReason Reason { get; private set; }

        public AbstractInstanceFailure(bool shouldOpenUI) : base(shouldOpenUI)
        {

        }

        public override InstanceResultType Type
        {
            get
            {
                return InstanceResultType.Lose;
            }
        }

        public virtual void Init(BaseInstanceLogic instanceLogic, InstanceFailureReason reason, GameFrameworkAction onComplete)
        {
            Reason = reason;
            InitInternal(instanceLogic, onComplete);
        }
    }
}
