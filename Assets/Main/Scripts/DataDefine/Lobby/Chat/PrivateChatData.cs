using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class PrivateChatData : BaseChatData
    {
        [SerializeField]
        public bool m_IsMe = false;

        public bool IsMe
        {
            get
            {
                return m_IsMe;
            }
            set
            {
                m_IsMe = value;
            }
        }

        public override bool IsRead
        {
            get
            {
                return base.IsRead;
            }

            set
            {
                bool lastValue = base.IsRead;
                base.IsRead = value;
                if (lastValue != value)
                {
                    GameEntry.Data.Chat.WriteLocalPrivateChatData();
                    GameEntry.Event.Fire(this, new ReminderUpdatedEventArgs());
                }
            }
        }

        public void UpdateSender(PlayerData data)
        {
            m_Sender = new PlayerChatData();
            m_Sender.UpdateData(data);
        }

        public override void UpdateData(LCSendChat data)
        {
            base.UpdateData(data);
            m_Receiver = new PlayerChatData();
            m_Receiver.UpdateData(data.Receiver);
            m_Sender = null;          
        }

        public override void UpdateData(LCReceiveChat data)
        {
            base.UpdateData(data);
            m_Sender = new PlayerChatData();
            m_Sender.UpdateData(data.Sender);
            m_Receiver = null;          
        }
    }
}
