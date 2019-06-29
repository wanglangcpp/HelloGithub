using UnityEngine;
using System.Collections;
using GameFramework;

namespace Genesis.GameClient
{
    public class UserAgreementSubFrom : NGUISubForm
    {
        [SerializeField]
        private UILabel m_UserAgreementText = null;

        [SerializeField]
        private UIToggle m_ReadingcheckBtn = null;

        [SerializeField]
        private UIButton m_AgreeBtn = null;

        protected internal override void OnOpen()
        {
            base.OnOpen();
            m_ReadingcheckBtn.value = true;
            string userAgreement = GameEntry.Localization.GetString("UI_AGREEMENT_CONTENT");
            m_UserAgreementText.text = GameEntry.StringReplacement.GetString(userAgreement);
        }

        public void InitUserAgreementPanel()
        {
            gameObject.SetActive(false);
        }

        public void OnClickReadingcheckBtn()
        {
            m_AgreeBtn.isEnabled = m_ReadingcheckBtn.value;
        }

        protected internal override void OnClose()
        {
            base.OnClose();
        }
    }
}