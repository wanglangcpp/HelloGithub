using GameFramework;

namespace Genesis.GameClient
{
    public abstract class AbstractInstanceDraw : AbstractInstanceResult
    {
        public InstanceDrawReason Reason { get; private set; }

        public AbstractInstanceDraw() : base(true)
        {

        }

        public override InstanceResultType Type
        {
            get
            {
                return InstanceResultType.Draw;
            }
        }

        public virtual void Init(BaseInstanceLogic instanceLogic, InstanceDrawReason reason, GameFrameworkAction onComplete)
        {
            Reason = reason;
            InitInternal(instanceLogic, onComplete);
        }
    }
}
