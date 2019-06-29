using GameFramework;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Genesis.GameClient
{
    public class LoginForm : NGUIForm
    {
        [SerializeField]
        private UIInput m_AccountName = null;

        [SerializeField]
        private UILabel m_AccountHint = null;

        [SerializeField]
        private UIInput m_Password = null;

        [SerializeField]
        private UILabel m_PasswordHint = null;

        [SerializeField]
        private UIButton m_NoticeButton = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Debug("LoginForm.OnOpen.hasSDK:" + SDKManager.HasSDK);
            if (SDKManager.HasSDK)
            {
                gameObject.transform.Find("Btn Bg").GetComponent<UIWidget>().gameObject.SetActive(false);
                SDKManager.Instance.helper.LoginSDK();
                BoxCollider bc = gameObject.GetComponent<BoxCollider>();
                if (null == bc)
                    bc = gameObject.AddComponent<BoxCollider>();
                bc.center = Vector3.zero;
                float fla = Screen.height / 768f;
                bc.size = new Vector3(Screen.width * fla, Screen.height * fla, 1);
                UIEventListener.Get(gameObject).onClick = (GameObject go) => {
                    SDKManager.Instance.helper.LoginSDK();
                };
            }

            m_NoticeButton.gameObject.SetActive(false);
            string accountName = GameEntry.Data.Account.AccountName;
            if (accountName != null)
            {
                m_AccountName.value = accountName;
            }

            m_AccountHint.gameObject.SetActive(m_AccountName.value.Length <= 0);
            m_PasswordHint.gameObject.SetActive(m_Password.value.Length <= 0);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        private Regex AccountRegex = new Regex(@"^[A-Za-z0-9._-]+$");

        public void OnClickLogin()
        {
            ProcedureLogin procedureLogin = GameEntry.Procedure.CurrentProcedure as ProcedureLogin;
            if (procedureLogin == null)
            {
                Log.Warning("Can not login in '{0}' procedure.", GameEntry.Procedure.CurrentProcedure.GetType().Name);
                return;
            }

            if (string.IsNullOrEmpty(m_AccountName.value) || !AccountRegex.IsMatch(m_AccountName.value))
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_BUTTON_ACCOUNT_NAME_IS_INVALID"));
            }
            else
            {
                procedureLogin.RequestLogin(m_AccountName.value, m_Password.value);
            }
        }

        public void OnClickNotice()
        {

        }

        public void OnAccountNameChange()
        {
            RefreshAccountHint();
        }

        public void OnPasswordChange()
        {
            RefreshPasswordHint();
        }

        private void RefreshAccountHint()
        {
            m_AccountHint.gameObject.SetActive(m_AccountName.value.Length <= 0);
        }

        private void RefreshPasswordHint()
        {
            m_PasswordHint.gameObject.SetActive(m_Password.value.Length <= 0);
        }
    }
}
