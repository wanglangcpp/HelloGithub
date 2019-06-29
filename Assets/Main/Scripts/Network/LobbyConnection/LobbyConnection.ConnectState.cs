using GameFramework;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class LobbyConnection
    {
        private class ConnectState : StateBase
        {
            private int m_CurTryTime = 0;
            private const int MaxTryTimes = 3;
            private const float WaitDuration = 5.0f;
            private bool m_NetworkConnected = false;

            GameFramework.Network.INetworkChannel mNetWork;

            protected override void OnEnter(IFsm<LobbyConnection> fsm)
            {
                base.OnEnter(fsm);
                mNetWork = GameEntry.Network.GetNetworkChannel(Constant.Network.LobbyNetworkChannelName);
                if (fsm.Owner.IsConnect)
                {
                    m_NetworkConnected = false;
                    m_CurTryTime = 0;
                }
                ConnectChannel();
            }

            protected override void OnLeave(IFsm<LobbyConnection> fsm, bool isShutdown)
            {
                if (GameEntry.IsAvailable && GameEntry.Waiting != null)
                {
                    GameEntry.Waiting.StopWaiting(WaitingType.Network, "ConnectState");
                }
                base.OnLeave(fsm, isShutdown);
            }

            public override void OnNetworkConnected(IFsm<LobbyConnection> fsm, object sender, UnityGameFramework.Runtime.NetworkConnectedEventArgs ne)
            {
                base.OnNetworkConnected(fsm, sender, ne);
                m_NetworkConnected = true;
                ChangeState<WaitForLoginState>(fsm);
            }

            private void ConnectChannel()
            {
                m_CurTryTime++;

                GameEntry.Waiting.StartWaiting(WaitingType.Network, "ConnectState");
                if (m_CurTryTime > MaxTryTimes)
                {
                    TimerUtility.WaitSeconds(WaitDuration, delegate (object userData) { RestartGame(userData); });
                    return;
                }
                var lobby = GameEntry.Network.GetNetworkChannel(Constant.Network.LobbyNetworkChannelName);
                if(null != lobby)
                    lobby.Close();
                GameEntry.Network.InitNetworkChannel(Constant.Network.LobbyNetworkChannelName);
                ServerData serverData = GameEntry.Data.Account.ServerData;
                Log.Debug("SDKManager.serverData:" + serverData.GameHost + ":" + serverData.GamePort);
                GameEntry.Network.ConnectNetworkChannel(Constant.Network.LobbyNetworkChannelName, serverData.GameHost, serverData.GamePort);

                TimerUtility.WaitSeconds(WaitDuration, delegate (object userData) {
                    if (mNetWork == lobby && !lobby.Connected)
                        ReconnectChannel(userData); });
            }

            private void RestartGame(object obj)
            {
                if (m_NetworkConnected)
                {
                    return;
                }

                if (GameEntry.IsAvailable && GameEntry.Waiting != null)
                {
                    GameEntry.Waiting.StopWaiting(WaitingType.Network, "ConnectState");
                }

                GameEntry.UI.OpenUIForm(UIFormId.ReconnectionForm, new ReconnectionDisplayData
                {
                    Message = GameEntry.Localization.GetString("UI_TEXT_RESTART_SURE"),
                    ButtonMessage = GameEntry.Localization.GetString("UI_BUTTON_CONFIRM"),
                    OnClickConfirm = delegate (object userData) {
                            GameEntry.Restart(); },
                });
            }

            private void ReconnectChannel(object obj)
            {
                if (m_NetworkConnected)
                {
                    return;
                }

                if (GameEntry.IsAvailable && GameEntry.Waiting != null)
                {
                    GameEntry.Waiting.StopWaiting(WaitingType.Network, "ConnectState");
                }

                GameEntry.UI.OpenUIForm(UIFormId.ReconnectionForm, new ReconnectionDisplayData
                {
                    Message = GameEntry.Localization.GetString("UI_TEXT_RECONNECTION"),
                    ButtonMessage = GameEntry.Localization.GetString("UI_TEXT_RETRY"),
                    OnClickConfirm = delegate (object userData) {
                            ConnectChannel(); },
                });
            }
        }
    }
}
