using GameFramework;

namespace Genesis.GameClient
{
    public abstract class AbstractInstanceSuccess : AbstractInstanceResult
    {
        public InstanceSuccessReason Reason { get; private set; }

        public AbstractInstanceSuccess() : base(true)
        {

        }

        public override InstanceResultType Type
        {
            get
            {
                return InstanceResultType.Win;
            }
        }

        public virtual void Init(BaseInstanceLogic instanceLogic, InstanceSuccessReason reason, GameFrameworkAction onComplete)
        {
            Reason = reason;
            InitInternal(instanceLogic, onComplete);
        }
    }

}
