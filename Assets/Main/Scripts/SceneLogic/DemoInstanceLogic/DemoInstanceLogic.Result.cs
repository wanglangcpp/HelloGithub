using System;

namespace Genesis.GameClient
{
    public partial class DemoInstanceLogic
    {
        protected override AbstractInstanceFailure CreateFailureResult(bool shouldOpenUI)
        {
            throw new NotSupportedException();
        }

        protected override AbstractInstanceSuccess CreateSuccessResult()
        {
            return new InstanceResultSuccess();
        }

        private class InstanceResultSuccess : AbstractInstanceSuccess
        {
            protected override void StopInstance()
            {
                base.StopInstance();
                (InstanceLogic as DemoInstanceLogic).StopAllAIsAndProhibitFurtherUse();
            }

            protected override void SendLeaveInstanceRequest()
            {
                OnReceiveLeaveInstanceResponse(this, null);
            }

            protected override void ShowResultUI(UIFormBaseUserData userData)
            {
                return;
            }
        }
    }
}
