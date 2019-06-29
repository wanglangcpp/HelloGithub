using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 系统消息的滚动类型
    /// </summary>
    public enum SystemChatType
    {
        // 右下角
        SYSTEM = 1,
        // 滚动
        SCROLL = 2,
        // 切出
        CUTOUT = 3,
        // 正常
        NOMAL = 4,
        //弹窗
        ALERT = 5,
    }
    [Serializable]
    public class SystemChatData : BaseChatData
    {
        [SerializeField]
        private bool m_NeedScroll = false;

        [SerializeField]
        private int m_Priority = 0;

        public int SystemNotifyType { get; private set; }

        public int ContextId { get; private set; }

        public List<string> OtherContext { get; private set; }

        public bool NeedScroll
        {
            get
            {
                return m_NeedScroll;
            }
        }

        public int Priority
        {
            get
            {
                return m_Priority;
            }
        }

        public override void UpdateData(LCReceiveChat data)
        {
            base.UpdateData(data);
            m_Type = ChatType.System;
        }

        public void UpdateData(WorldChatData data)
        {
            m_Type = ChatType.System;
            m_Sender = null;
            m_Message = data.Message;
        }

        public void UpdateData(LCPushSystemAnnouncement data)
        {
            m_Type = ChatType.System;
            m_Sender = null;
            if (data.Sender != null)
            {
                m_Sender = new PlayerChatData();
                m_Sender.UpdateData(data.Sender);
            }
            List<string> strParams = new List<string>();
            if (data.Sender != null)
            {
                strParams.Add(ColorUtility.AddColorToString(GameEntry.ClientConfig.ClientColorConfig.PlayerNameColor, data.Sender.Name));
            }
            for (int i = 0; i < data.Params.Count; i++)
            {
                strParams.Add(GameEntry.StringReplacement.GetString(data.Params[i]));
            }
            DRAnnouncement announcedmentData = GameEntry.DataTable.GetDataTable<DRAnnouncement>().GetDataRow(data.AnnouncementId);
            if (announcedmentData == null)
            {
                Log.Error("ChatsData Wrong Announcement Data Id.");
                return;
            }
            m_Message = GameEntry.Localization.GetString(announcedmentData.AnnouncementMessage, strParams.ToArray());
            m_NeedScroll = announcedmentData.NeedScroll;
            m_Priority = announcedmentData.Priority;
        }

        public void UpdateData(LCSystemNotify data)
        {
            m_Type = ChatType.System;
            SystemNotifyType = data.contextType;
            m_Message = data.context;
            ContextId = data.contextId;
            OtherContext = data.otherInfo;
        }
    }
}
