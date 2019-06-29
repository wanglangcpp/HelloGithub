using UnityEngine;
using System;

namespace Genesis.GameClient
{
    public class MailItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_MailUnReadIcon = null;

        [SerializeField]
        private GameObject m_MailReadIcon = null;

        [SerializeField]
        private GameObject m_AnnexSymbolObject = null;

        [SerializeField]
        private UILabel m_MailDateLabel = null;

        [SerializeField]
        private UILabel m_MailTitleLabel = null;

        [SerializeField]
        private GameObject m_SelectedObject = null;

        [SerializeField]
        private GameObject m_AnnexBorder = null;

        [SerializeField]
        private GameObject m_ReceiveBorder = null;

        [SerializeField]
        private UILabel m_AnnexText = null;

        [SerializeField]
        private UILabel m_AlreadyReceiveText = null;

        private Action<MailItem> OnMailClick = null;
        public MailData MailItemData { get; private set; }

        public void SetMailData(MailData mail, Action<MailItem> onMailClick)
        {
            MailItemData = mail;
            OnMailClick = onMailClick;

            m_MailReadIcon.SetActive(mail.IsAlreadRead);
            m_MailUnReadIcon.SetActive(!mail.IsAlreadRead);

            m_AnnexText.text = GameEntry.Localization.GetString("UI_TEXT_EMAIL_ANNEX");
            m_AlreadyReceiveText.text = GameEntry.Localization.GetString("UI_BUTTON_ACHIEVEMENT_RECEIVE");

            if (mail.Rewards.Data.Count > 0)
            {
                m_AnnexSymbolObject.SetActive(true);
                if (mail.IsAlreadRead)
                {
                    m_ReceiveBorder.SetActive(true);
                    m_AnnexBorder.SetActive(false);
                }
                else
                {
                    m_ReceiveBorder.SetActive(false);
                    m_AnnexBorder.SetActive(true);

                    if (mail.IsAlreadRead)
                        m_AnnexBorder.GetComponent<UISprite>().color = Color.grey;
                    else
                        m_AnnexBorder.GetComponent<UISprite>().color = Color.white;
                }
            }
            else
            {
                m_AnnexSymbolObject.SetActive(false);
            }
            m_MailDateLabel.text = mail.SendTime.ToString("yyyy-MM-dd");
            m_MailTitleLabel.text = mail.MailTitle;
        }

        public void OnMailClicked()
        {
            if (OnMailClick != null)
                OnMailClick.Invoke(this);

            if (!MailItemData.IsAlreadRead && (MailItemData.Rewards.Data == null || MailItemData.Rewards.Data.Count <= 0))
                GameEntry.LobbyLogic.MarkMailAsRead(MailItemData.Id);
        }

        public void SetItemSelectStatus(bool isSelected)
        {
            m_SelectedObject.SetActive(isSelected);
        }
    }
}