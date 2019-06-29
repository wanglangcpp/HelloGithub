using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Genesis.GameClient
{
    public class ProcedureLogin : ProcedureBase
    {
        private ProcedureConfig.ProcedureLoginConfig m_Config = null;
        private bool m_LoginOK = false;
        private UIForm m_LoginForm = null;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_Config = GameEntry.ClientConfig.ProcedureConfig.LoginConfig;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.UIBackground.HideDefault();
            GameEntry.UIBackground.ShowDefault();
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnRequestLoginResponse);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.WebRequestFailure, OnRequestLoginError);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
            m_LoginOK = false;
            GameEntry.UI.OpenUIForm(UIFormId.Login);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.WebRequestSuccess, OnRequestLoginResponse);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.WebRequestFailure, OnRequestLoginError);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.OpenUIFormSuccess, OnOpenUIFormSuccess);
            }

            if (GameEntry.IsAvailable && GameEntry.UI != null && m_LoginForm != null)
            {
                GameEntry.UI.CloseUIForm(m_LoginForm);
                m_LoginForm = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_LoginOK)
            {
                ChangeState<ProcedureSelectServer>(procedureOwner);
            }
        }

        public void RequestLogin(string accountName, string password)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("AccountName", WebUtility.EscapeString(accountName));
            wwwForm.AddField("Password", WebUtility.EscapeString(password));

            string publisher = GameEntry.Data.GetTempData<string>(Constant.TempData.Publisher);
            if (SDKManager.HasConfig) {
                m_Config.LoginUri = SDKManager.Instance.SDKData.LoginURL;
                GameEntry.BuildInfo.LoginUri = SDKManager.Instance.SDKData.LoginURLOut;
            }
            GameEntry.WebRequest.AddWebRequest(string.IsNullOrEmpty(publisher) ? m_Config.LoginUri : GameEntry.BuildInfo.LoginUri, wwwForm);
        }

        private void OnRequestLoginResponse(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = e as WebRequestSuccessEventArgs;
            LoginData loginData = Utility.Json.ToObject<LoginData>(ne.GetWebResponseBytes());
            if (loginData.AuthorizedCode != 0)
            {
                Log.Warning("Login failed with error code '{0}'.", loginData.AuthorizedCode);
                return;
            }

            bool needShow = false;
            if (GameEntry.Data.Account.AccountName != loginData.AccountName)
            {
                needShow = true;
            }

            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.NeedShowAccountAgreement, needShow);

            GameEntry.Data.Account.AccountName = loginData.AccountName;
            GameEntry.Data.Account.LoginKey = loginData.LoginKey;
            if (SDKManager.HasConfig) {
                SDKManager.Instance.TalkingData.SetAccount(GameEntry.Data.Account.AccountName);
            }
            m_LoginOK = true;
        }

        private void OnRequestLoginError(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = e as WebRequestFailureEventArgs;
            OnError("Request login failed with error message '{0}'.", ne.ErrorMessage);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = e as OpenUIFormSuccessEventArgs;
            if (ne.UIForm.Logic is LoginForm)
            {
                m_LoginForm = ne.UIForm;
            }
        }

        public void ChangeLoginOK(bool isLoginOK) {
            m_LoginOK = isLoginOK;
        }
    }
}
