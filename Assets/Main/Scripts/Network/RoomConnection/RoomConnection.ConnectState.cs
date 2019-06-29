using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class RoomConnection
    {
        private class ConnectState : StateBase
        {
            private int m_CurTryTime = 0;
            private const int MaxTryTimes = 3;
            private const float WaitDuration = 5.0f;
            private bool m_NetworkConnected = false;

            protected override void OnEnter(IFsm<RoomConnection> fsm)
            {
                base.OnEnter(fsm);
                m_NetworkConnected = false;
                m_CurTryTime = 0;

                ConnectChannel();
            }

            protected override void OnLeave(IFsm<RoomConnection> fsm, bool isShutdown)
            {
                if (GameEntry.IsAvailable && GameEntry.Waiting != null)
                {
                    GameEntry.Waiting.ClearWaitingOfType(WaitingType.RoomBreak);
                    GameEntry.Waiting.StartWaiting(WaitingType.RoomBreak, Constant.Network.RoomNetworkChannelName);
                }
                base.OnLeave(fsm, isShutdown);
            }

            public override void OnNetworkConnected(IFsm<RoomConnection> fsm, object sender, UnityGameFramework.Runtime.NetworkConnectedEventArgs ne)
            {
                base.OnNetworkConnected(fsm, sender, ne);
                m_NetworkConnected = true;
                if (GameEntry.Data.Room.HasReconnected)
                {
                    ChangeState<ReconnectEnterRoomState>(fsm);
                }
                else
                {
                    ChangeState<NormalEnterRoom>(fsm);
                }
            }

            private void ConnectChannel()
            {
                m_CurTryTime++;

                if (m_CurTryTime > MaxTryTimes)
                {
                    TimerUtility.WaitSeconds(WaitDuration, delegate (object userData) { RestartGame(userData); });
                    return;
                }

                GameEntry.Network.InitNetworkChannel(Constant.Network.RoomNetworkChannelName);
                GameEntry.Network.ConnectNetworkChannel(Constant.Network.RoomNetworkChannelName);

                TimerUtility.WaitSeconds(WaitDuration, delegate (object userData) { ReconnectChannel(userData); });
            }

            private void RestartGame(object obj)
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.Waiting.StopWaiting(WaitingType.RoomBreak, Constant.Network.RoomNetworkChannelName);
                }

                GameEntry.UI.OpenUIForm(UIFormId.ReconnectionForm, new ReconnectionDisplayData
                {
                    Message = GameEntry.Localization.GetString("UI_TEXT_RESTART_SURE"),
                    ButtonMessage = GameEntry.Localization.GetString("UI_BUTTON_TEXT_SURE"),
                    OnClickConfirm = delegate (object userData) { GameEntry.Restart(); },
                });
            }

            private void ReconnectChannel(object obj)
            {
                if (m_NetworkConnected)
                {
                    return;
                }

                if (GameEntry.IsAvailable)
                {
                    GameEntry.Waiting.StopWaiting(WaitingType.RoomBreak, Constant.Network.RoomNetworkChannelName);
                }

                GameEntry.UI.OpenUIForm(UIFormId.ReconnectionForm, new ReconnectionDisplayData
                {
                    Message = GameEntry.Localization.GetString("UI_TEXT_RECONNECTION"),
                    ButtonMessage = GameEntry.Localization.GetString("UI_BUTTON_TEXT_RETRY"),
                    OnClickConfirm = delegate (object userData) { ConnectChannel(); },
                });
            }
        }
    }
}
