using GameFramework;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本等待器的默认实现。
    /// </summary>
    /// <remarks>无需实现 <see cref="OnDraw"/>, <see cref="OnLose"/>, <see cref="OnWin"/> 方法，因为在 <see cref="Init"/> 时就会进入下一状态。</remarks>
    public class DefaultInstanceWaiter : AbstractInstanceWaiter
    {
        public override void OnDraw(InstanceDrawReason reason, GameFrameworkAction onComplete)
        {
            throw new NotSupportedException();
        }

        public override void OnLose(InstanceFailureReason reason, bool shouldOpenUI, GameFrameworkAction onComplete)
        {
            throw new NotSupportedException();
        }

        public override void OnWin(InstanceSuccessReason reason, GameFrameworkAction onComplete)
        {
            throw new NotSupportedException();
        }

        public override void Init(BaseInstanceLogic instanceLogic)
        {
            base.Init(instanceLogic);
            FireShouldGoToRunning();
        }
    }
}
