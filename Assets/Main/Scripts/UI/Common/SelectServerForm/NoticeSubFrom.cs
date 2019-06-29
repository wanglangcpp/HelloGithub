using UnityEngine;
using System.Collections;

namespace Genesis.GameClient
{
    public class NoticeSubFrom : NGUISubForm
    {
        [SerializeField]
        private UILabel m_GameNoticeText = null;

        protected internal override void OnOpen()
        {
            string noticeText = GameEntry.Localization.GetString("UI_NOTICE_CONTENT");
            m_GameNoticeText.text = GameEntry.StringReplacement.GetString(noticeText);
            base.OnOpen();
        }

        public void InitNoticePanel()
        {
            gameObject.SetActive(false);
        }

        public void RefreshNotice()
        {

        }

        protected internal override void OnClose()
        {
            base.OnClose();
        }
    }
}