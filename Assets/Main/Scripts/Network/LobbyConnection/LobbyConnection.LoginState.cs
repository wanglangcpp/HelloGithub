using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class LobbyConnection
    {
        private class LoginState : StateBase
        {
            private bool m_LoginSuccess = false;

            protected override void OnEnter(IFsm<LobbyConnection> fsm)
            {
                base.OnEnter(fsm);
                GameEntry.Event.Subscribe(EventId.LoginServer, OnLoginServer);

                m_LoginSuccess = false;

                GameEntry.Waiting.StartWaiting(WaitingType.Network, "LoginState");

                if (SDKManager.Instance.isSDKLogin)
                {
                    SDKManager.Instance.helper.LoginPlatformServer();
                }
                else
                {
                    CLLoginServer request = new CLLoginServer();
                    request.AccountName = GameEntry.Data.Account.AccountName;
                    request.LoginKey = GameEntry.Data.Account.LoginKey;
                    GameEntry.Network.Send(request);
                }
            }

            protected override void OnLeave(IFsm<LobbyConnection> fsm, bool isShutdown)
            {
                if (GameEntry.IsAvailable && GameEntry.Event != null)
                {
                    GameEntry.Event.Unsubscribe(EventId.LoginServer, OnLoginServer);
                }

                if (GameEntry.IsAvailable && GameEntry.Waiting != null)
                {
                    GameEntry.Waiting.StopWaiting(WaitingType.Network, "LoginState");
                }

                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<LobbyConnection> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (m_LoginSuccess)
                {
                    ChangeState<WaitForSignInState>(fsm);
                    GameEntry.Event.Fire(this, new ResponseConnectLobbyServerEventArgs());
                    return;
                }
            }

            private void OnLoginServer(object sender, GameEventArgs e)
            {
                LoginServerEventArgs ne = e as LoginServerEventArgs;
                if (!ne.Authorized)
                {
                    // TODO
                    return;
                }
                ServerData lastServer = null;
                ProcedureSelectServer procedureSelectServer = GameEntry.Procedure.CurrentProcedure as ProcedureSelectServer;
                if (null != procedureSelectServer)
                    lastServer = procedureSelectServer.GetLastServer();
                //if (null != lastServer)
                //    lastServer.isRestrict = ne.RestrictServer;
                if (ne.RestrictServer)//ne.RestrictServer
                {
                    //该服已经停止注册，请移驾新服
                    //GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_STOP_REG") });
                    GameEntry.Waiting.ClearWaitingOfType(WaitingType.Network);
                    GameEntry.UI.OpenUIForm(UIFormId.ReconnectionForm, new ReconnectionDisplayData
                    {
                        Message = GameEntry.Localization.GetString("UI_TEXT_STOP_REG"),
                        ButtonMessage = GameEntry.Localization.GetString("UI_BUTTON_TEXT_SURE"),
                        OnClickConfirm = delegate (object userData) { GameEntry.Restart(); },
                    });
                    return;
                }

                m_LoginSuccess = true;
                GameEntry.Data.Account.LastServerId = GameEntry.Data.Account.ServerData.Id;
                Log.Info("[LobbyConnection.LoginState OnLoginServer] new_account = {0}", ne.NewAccount);
                GameEntry.Data.AddOrUpdateTempData(Constant.TempData.NeedCreatePlayer, ne.NewAccount);
                GameEntry.Data.AddOrUpdateTempData(Constant.TempData.SignInShowId, 2);

                string recordData = string.Empty;
                recordData = GameEntry.Data.Account.ServerData.Id.ToString() + "," + GameEntry.Data.Account.ServerData.Name;
                SDKManager.Instance.helper.Record("ChooseServer", recordData);
            }
        }
    }
}
