using System;
using System.Collections.Generic;
using System.Linq;

namespace Genesis.GameClient
{
    [Serializable]
    public class MailsData
    {
        private Dictionary<int, MailData> m_MailsData = new Dictionary<int, MailData>();

        public List<MailData> Data
        {
            get
            {
                return m_MailsData.Values.ToList();
            }
        }

        public MailData GetMailById(int mailId)
        {
            MailData mail = null;
            m_MailsData.TryGetValue(mailId, out mail);

            return mail;
        }

        public void ClearData()
        {
            m_MailsData.Clear();
        }

        public void RefreshMails(List<PBMailInfo> mailList)
        {
            ClearData();
            AddOrModifyMail(mailList);
        }

        public void AddOrModifyMail(List<PBMailInfo> mailList)
        {
            for (int i = 0; i < mailList.Count; i++)
                AddOrModifyMail(mailList[i]);
        }

        public void AddOrModifyMail(PBMailInfo mailInfo)
        {
            var mail = new MailData();
            mail.UpdateData(mailInfo);
            m_MailsData[mailInfo.Id] = mail;
        }

        public void RemoveMail(int mailId)
        {
            m_MailsData.Remove(mailId);
        }

        public bool HasUnreadMail()
        {
            for (int i = 0; i < Data.Count; i++)
                if (!Data[i].IsAlreadRead)
                    return true;

            return false;
        }

        public void RefreshMailStatus(int id, bool isRead)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                if (Data[i].Id == id)
                {
                    Data[i].IsAlreadRead = isRead;
                    break;
                }
            }
        }
    }
}
