using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class MailData : IGenericData<MailData, PBMailInfo>
    {
        private int m_Id;
        private string m_Title;
        private string m_Sender;
        private DateTime m_SendTime;
        private bool m_IsAlreadRead;
        private ItemsData m_Rewards;
        private int m_Priority;
        private string m_Messages;

        /// <summary>
        /// 邮件ID。
        /// </summary>
        public int Id { get { return m_Id; } }

        /// <summary>
        /// 邮件标题。
        /// </summary>
        public string MailTitle { get { return m_Title; } }

        /// <summary>
        /// 邮件发送者。
        /// </summary>
        public string Sender { get { return m_Sender; } }

        /// <summary>
        /// 邮件的发送时间。
        /// </summary>
        public DateTime SendTime { get { return m_SendTime; } }

        /// <summary>
        /// 是否已读。读取了邮件并且领取了附件算作已读（没有附件的话读取了就算已读）。
        /// </summary>
        public bool IsAlreadRead { get { return m_IsAlreadRead; } set { m_IsAlreadRead = value;} }

        /// <summary>
        /// 是否靠前显示
        /// </summary>
        public int Priority { get { return m_Priority; } }

        /// <summary>
        /// 邮件的消息
        /// </summary>
        public string Messages { get { return m_Messages; } }

        /// <summary>
        /// 附件奖励
        /// </summary>
        public ItemsData Rewards { get { return m_Rewards; } }

        public int Key { get { return m_Id; } }

        public void UpdateData(PBMailInfo mailInfo)
        {
            m_Id = mailInfo.Id;
            m_Title = mailInfo.Title;
            m_Sender = mailInfo.Sender;
            m_SendTime = new DateTime(mailInfo.GenerateTime, DateTimeKind.Utc);
            m_IsAlreadRead = mailInfo.IsRead;
            m_Priority = mailInfo.Priority;
            m_Messages = mailInfo.Message;
            m_Rewards = new ItemsData();
            m_Rewards.ClearAndAddData(mailInfo.ItemInfo);
        }

        public void RefreshStatus(bool isAlreadRead)
        {
            m_IsAlreadRead = isAlreadRead;
        }
    }
}