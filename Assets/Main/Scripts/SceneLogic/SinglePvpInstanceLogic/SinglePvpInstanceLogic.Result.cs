using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class SinglePvpInstanceLogic
    {
        private const float ResultShowUIDelay = 0.1f;

        protected override AbstractInstanceFailure CreateFailureResult(bool shouldOpenUI)
        {
            return new InstanceFailure(shouldOpenUI);
        }

        protected override AbstractInstanceSuccess CreateSuccessResult()
        {
            return new InstanceSuccess();
        }

        protected override AbstractInstanceDraw CreateDrawResult()
        {
            return new InstanceDraw();
        }

        private class InstanceDraw : AbstractInstanceDraw
        {
            private ITimerUtilityHelper m_Timer;

            public override void Init(BaseInstanceLogic instanceLogic, InstanceDrawReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                GameEntry.Event.Subscribe(EventId.RoomClosed, OnReceiveLeaveInstanceResponse);
            }

            public override void ShutDown()
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.RoomClosed, OnReceiveLeaveInstanceResponse);
                }

                if (m_Timer as MonoBehaviour != null)
                {
                    m_Timer.Interrupt();
                }

                base.ShutDown();
            }

            protected override void ShowResultUI(UIFormBaseUserData userData)
            {
                m_Timer = TimerUtility.WaitSeconds(ResultShowUIDelay, delegate (object o)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.ActivitySinglePvpResultForm, userData as UIFormBaseUserData);
                });
            }

            protected override void SendLeaveInstanceRequest()
            {
                OnReceiveLeaveInstanceResponse(null, null);
            }
        }

        private class InstanceSuccess : AbstractInstanceSuccess
        {
            private ITimerUtilityHelper m_Timer;

            public override void Init(BaseInstanceLogic instanceLogic, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);

                if (Reason == InstanceSuccessReason.AbandonedByOpponent)
                {
                    //对手放弃比赛
                    GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_OPPONENT_GIVES_UP") });
                }

                GameEntry.Event.Subscribe(EventId.RoomClosed, OnReceiveLeaveInstanceResponse);
            }

            public override void ShutDown()
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.RoomClosed, OnReceiveLeaveInstanceResponse);
                }

                if (m_Timer as MonoBehaviour != null)
                {
                    m_Timer.Interrupt();
                }

                base.ShutDown();
            }

            protected override void SendLeaveInstanceRequest()
            {
                OnReceiveLeaveInstanceResponse(null, null);
            }

            protected override void ShowResultUI(UIFormBaseUserData userData)
            {
                m_Timer = TimerUtility.WaitSeconds(ResultShowUIDelay, delegate (object o)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.ActivitySinglePvpResultForm, userData as UIFormBaseUserData);
                });
            }
        }

        private class InstanceFailure : AbstractInstanceFailure
        {
            private ITimerUtilityHelper m_Timer;

            private InstanceFailureReason m_FailureReason = InstanceFailureReason.Unknown;

            public InstanceFailure(bool shouldOpenUI) : base(shouldOpenUI)
            {

            }

            public override void Init(BaseInstanceLogic instanceLogic, InstanceFailureReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                m_FailureReason = reason;
                GameEntry.Event.Subscribe(EventId.RoomClosed, OnReceiveLeaveInstanceResponse);
            }

            public override void ShutDown()
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Event.Unsubscribe(EventId.RoomClosed, OnReceiveLeaveInstanceResponse);
                }

                if (m_Timer as MonoBehaviour != null)
                {
                    m_Timer.Interrupt();
                }

                base.ShutDown();
            }

            protected override void SendLeaveInstanceRequest()
            {
                if (Reason == InstanceFailureReason.AbandonedByUser)
                {
                    GameEntry.Network.Send(new CRGiveUpBattle { });
                }
                else
                {
                    OnReceiveLeaveInstanceResponse(null, null);
                }
            }

            protected override UIFormBaseUserData PopulateData(GameEventArgs e)
            {
                return new InstanceFailureData { FailureReason = m_FailureReason };
            }

            protected override void ShowResultUI(UIFormBaseUserData userData)
            {
                m_Timer = TimerUtility.WaitSeconds(ResultShowUIDelay, delegate (object o)
                {
                    //GameEntry.UI.OpenUIForm(UIFormId.ActivitySinglePvpFailureForm, userData);
                    GameEntry.UI.OpenUIForm(UIFormId.ActivitySinglePvpResultForm, userData as UIFormBaseUserData);
                });
            }
        }
    }
}
