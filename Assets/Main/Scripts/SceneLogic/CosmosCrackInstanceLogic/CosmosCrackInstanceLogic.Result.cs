using GameFramework;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public partial class CosmosCrackInstanceLogic
    {
        protected override AbstractInstanceSuccess CreateSuccessResult()
        {
            return new InstanceSuccess();
        }

        protected override AbstractInstanceFailure CreateFailureResult(bool shouldOpenUI)
        {
            return new InstanceFailure(shouldOpenUI);
        }

        private class InstanceSuccess : BaseSinglePlayerInstanceSuccess
        {
            protected override void SendLeaveInstanceRequest()
            {
                GameEntry.LobbyLogic.CosmosCrack.RequestLeaveInstance(InstanceLogic.InstanceId, true);
            }

            protected override void ShowResultUI(UIFormBaseUserData data)
            {
                var instanceLogic = (InstanceLogic as CosmosCrackInstanceLogic);
                GameEntry.UI.OpenUIForm(UIFormId.InstanceResultForm, instanceLogic.m_ResultData);
            }

            public override void Init(BaseInstanceLogic instanceLogic, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                GameEntry.Event.Subscribe(EventId.LeaveInstanceResponse, OnReceiveLeaveInstanceResponse);
            }

            public override void ShutDown()
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.LeaveInstanceResponse, OnReceiveLeaveInstanceResponse);
                }

                base.ShutDown();
            }

        }

        private class InstanceFailure : BaseSinglePlayerInstanceFailure
        {
            private InstanceFailureReason m_FailureReason = InstanceFailureReason.Unknown;

            public InstanceFailure(bool shouldOpenUI) : base(shouldOpenUI)
            {

            }

            public override void Init(BaseInstanceLogic instanceLogic, InstanceFailureReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                m_FailureReason = reason;
                GameEntry.Event.Subscribe(EventId.LeaveInstanceResponse, OnReceiveLeaveInstanceResponse);
            }

            public override void ShutDown()
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.LeaveInstanceResponse, OnReceiveLeaveInstanceResponse);
                }

                base.ShutDown();
            }

            protected override UIFormBaseUserData PopulateData(GameEventArgs e)
            {
                return null;
            }

            protected override void SendLeaveInstanceRequest()
            {
                if (!GameEntry.OfflineMode.OfflineModeEnabled)
                {
                    GameEntry.LobbyLogic.CosmosCrack.RequestLeaveInstance(InstanceLogic.InstanceId, false);
                }
                else
                {
                    OnReceiveLeaveInstanceResponse(null, null);
                }
            }

            protected override void ShowResultUI(UIFormBaseUserData data)
            {
                GameEntry.UI.OpenUIForm(UIFormId.CosmosCrackInstanceFailureForm, new InstanceFailureData { FailureReason = m_FailureReason });
            }
        }
    }
}
