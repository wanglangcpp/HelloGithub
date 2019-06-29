using GameFramework;
using GameFramework.Event;

namespace Genesis.GameClient
{
    public partial class ArenaBattleInstanceLogic
    {
        protected override AbstractInstanceSuccess CreateSuccessResult()
        {
            return new InstanceSuccess();
        }

        protected override AbstractInstanceFailure CreateFailureResult(bool shouldOpenUI)
        {
            return new InstanceFailure(shouldOpenUI);
        }

        private class InstanceSuccess : PvpaiInstanceSuccess
        {
            private ArenaBattleInstanceLogic m_ArenaBattleInstanceLogic = null;

            public override void Init(BaseInstanceLogic instanceLogic, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                m_ArenaBattleInstanceLogic = instanceLogic as ArenaBattleInstanceLogic;
                GameEntry.Event.Subscribe(EventId.OfflineArenaBattleResultDataObtained, OnReceiveLeaveInstanceResponse);
            }

            public override void ShutDown()
            {
                GameEntry.Event.Unsubscribe(EventId.OfflineArenaBattleResultDataObtained, OnReceiveLeaveInstanceResponse);
                base.ShutDown();
            }

            protected override UIFormBaseUserData PopulateData(GameEventArgs e)
            {
                return new ArenaBattleResultDisplayData { EventArgs = e as OfflineArenaBattleResultDataObtainedEventArgs };
            }

            protected override void SendLeaveInstanceRequest()
            {
                if (m_ArenaBattleInstanceLogic == null)
                {
                    Log.Error("m_ArenaBattleInstanceLogic is null!");
                    return;
                }
                GameEntry.LobbyLogic.LeaveArenaBattle(m_ArenaBattleInstanceLogic.OppPlayerId, true);
            }

            protected override void ShowResultUI(UIFormBaseUserData data)
            {
                GameEntry.UI.OpenUIForm(UIFormId.ArenaBattleResultForm, data);
            }
        }

        private class InstanceFailure : PvpaiInstanceFailure
        {
            private ArenaBattleInstanceLogic m_ArenaBattleInstanceLogic = null;

            public InstanceFailure(bool shouldOpenUI) : base(shouldOpenUI)
            {

            }

            public override void Init(BaseInstanceLogic instanceLogic, InstanceFailureReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                m_ArenaBattleInstanceLogic = instanceLogic as ArenaBattleInstanceLogic;
                GameEntry.Event.Subscribe(EventId.OfflineArenaBattleResultDataObtained, OnReceiveLeaveInstanceResponse);
            }

            public override void ShutDown()
            {
                GameEntry.Event.Unsubscribe(EventId.OfflineArenaBattleResultDataObtained, OnReceiveLeaveInstanceResponse);
                base.ShutDown();
            }

            protected override UIFormBaseUserData PopulateData(GameEventArgs e)
            {
                return new ArenaBattleFailureDisplayData { EventArgs = e as OfflineArenaBattleResultDataObtainedEventArgs };
            }

            protected override void SendLeaveInstanceRequest()
            {
                if (m_ArenaBattleInstanceLogic == null)
                {
                    return;
                }
                GameEntry.LobbyLogic.LeaveArenaBattle(m_ArenaBattleInstanceLogic.OppPlayerId, false);
            }

            protected override void ShowResultUI(UIFormBaseUserData data)
            {
                GameEntry.UI.OpenUIForm(UIFormId.ArenaBattleFailureForm, data);
            }
        }
    }
}
