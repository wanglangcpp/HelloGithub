using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class BaseChatData
    {
        [SerializeField]
        public ChatType m_Type = ChatType.None;

        [SerializeField]
        public string m_Message = string.Empty;

        [SerializeField]
        public bool m_IsRead = false;

        [SerializeField]
        public PlayerChatData m_Sender = null;

        [SerializeField]
        public PlayerChatData m_Receiver = null;

        [SerializeField]
        public long m_ChatTime;

        public virtual bool IsRead
        {
            get
            {
                return m_IsRead;
            }
            set
            {
                m_IsRead = value;
            }
        }

        public ChatType Type
        {
            get
            {
                return m_Type;
            }
        }

        public PlayerChatData Sender
        {
            get
            {
                return m_Sender;
            }
        }

        public PlayerChatData Receiver
        {
            get
            {
                return m_Receiver;
            }
        }

        public string Message
        {
            get
            {
                return m_Message;
            }
        }

        public DateTime ChatTime
        {
            get
            {
                return new DateTime(m_ChatTime);
            }
        }

        public virtual void UpdateData(LCReceiveChat data)
        {
            m_Type = (ChatType)data.Channel;
            m_Message = data.Message;
            m_ChatTime = data.Time;
        }

        public virtual void UpdateData(LCSendChat data)
        {
            m_Type = (ChatType)data.Channel;
            m_Message = data.Message;
            m_ChatTime = data.Time;
        }
    }
}
