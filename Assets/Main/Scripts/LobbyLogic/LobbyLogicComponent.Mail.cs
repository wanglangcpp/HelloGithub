using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class LobbyLogicComponent
    {
        public void DeleteMail(int mailId)
        {
            var ids = new List<int>();
            ids.Add(mailId);
            DeleteMails(ids);
        }

        public void DeleteMails(List<int> mailIds)
        {
            if (mailIds.Count <= 0)
                return;

            var request = new CLDeleteMail();
            request.MailIds.AddRange(mailIds);
            GameEntry.Network.Send(request);
        }

        public void MarkMailAsRead(int mailId)
        {
            var ids = new List<int>();
            ids.Add(mailId);
            MarkMailsAsRead(ids);
        }

        public void MarkMailsAsRead(List<int> mailIds)
        {
            if (mailIds.Count <= 0)
                return;

            var request = new CLReadMail();
            request.MailIds.AddRange(mailIds);
            GameEntry.Network.Send(request);
        }

        public void ReceiveAllEmails()
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                //CLGetEmailList request = new CLGetEmailList();
                //GameEntry.Network.Send(request);
            }
            else
            {

            }
        }

        public void ReceiveEmailItems(params int[] emailIds)
        {
            if (!GameEntry.OfflineMode.OfflineModeEnabled)
            {
                //CLOpenEmail request = new CLOpenEmail();
                //request.OpenEmailIds.AddRange(emailIds);
                //GameEntry.Network.Send(request);
            }
            else
            {

            }
        }
    }
}
