using UnityEngine;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public class MailDetailInfo : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_MailTitleLabel = null;

        [SerializeField]
        private UILabel m_MailSenderLabel = null;

        [SerializeField]
        private UILabel m_MailInfoMessageLabel = null;

        [SerializeField]
        private UILabel m_RewardStatusLabel = null;

        [SerializeField]
        private GameObject m_PickButtonObject = null;

        [SerializeField]
        private GameObject m_DeleteButtonObject = null;

        [SerializeField]
        private List<GeneralItemView> m_AnnexItemList = null;

        private MailData m_MailData;
        public void SetMailDetailInfo(MailData mail)
        {
            m_MailData = mail;

            if (mail == null)
            {
                SetEmpty();
                return;
            }

            gameObject.SetActive(true);
            m_MailTitleLabel.text = mail.MailTitle;
            m_MailSenderLabel.text = GameEntry.Localization.GetString("UI_TEXT_EMAIL_SENDER", mail.Sender);

            m_MailInfoMessageLabel.text = mail.Messages;

            if (mail.Rewards.Data.Count > 0)
            {
                if (mail.IsAlreadRead)
                {
                    m_RewardStatusLabel.text = GameEntry.Localization.GetString("UI_BUTTON_RECEIVED");
                    m_PickButtonObject.SetActive(false);
                    m_DeleteButtonObject.SetActive(true);
                }
                else
                {
                    m_RewardStatusLabel.text = GameEntry.Localization.GetString("UI_TEXT_EMAIL_NOT_RECEIVED");
                    m_PickButtonObject.SetActive(true);
                    m_DeleteButtonObject.SetActive(false);
                }
            }
            else
            {
                m_PickButtonObject.SetActive(false);
                m_DeleteButtonObject.SetActive(true);
                m_RewardStatusLabel.text = GameEntry.Localization.GetString("UI_TEXT_EMAIL_NOTHING");
            }

            for (int i = 0; i < m_AnnexItemList.Count; i++)
            {
                if (i < mail.Rewards.Data.Count)
                {
                    var item = mail.Rewards.Data[i];
                    m_AnnexItemList[i].InitGeneralItem(item.Type, item.Count);
                    m_AnnexItemList[i].gameObject.SetActive(true);
                    m_AnnexItemList[i].transform.FindChild("Mask").gameObject.SetActive(mail.IsAlreadRead); //如果这里找不到，就是Prefab错了
                    m_AnnexItemList[i].transform.FindChild("Receive").gameObject.SetActive(mail.IsAlreadRead);
                }
                else
                {
                    m_AnnexItemList[i].gameObject.SetActive(false);
                }
            }
        }

        private void SetEmpty()
        {
            for (int i = 0; i < m_AnnexItemList.Count; i++)
                m_AnnexItemList[i].gameObject.SetActive(false);

            m_MailTitleLabel.text = string.Empty;
            m_MailSenderLabel.text = string.Empty;
            m_MailInfoMessageLabel.text = GameEntry.Localization.GetString("UI_TEXT_EMAIL_NOTICE_EMPTY");
            m_RewardStatusLabel.text = GameEntry.Localization.GetString("UI_TEXT_EMAIL_NOTHING");

            m_PickButtonObject.SetActive(false);
            m_DeleteButtonObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void OnPickRewardButtonClicked()
        {
            GameEntry.LobbyLogic.MarkMailAsRead(m_MailData.Id);
        }

        public void OnDeleteButtonClicked()
        {
            GameEntry.LobbyLogic.DeleteMail(m_MailData.Id);
        }
    }
}