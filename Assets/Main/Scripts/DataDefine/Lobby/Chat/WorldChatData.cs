using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class WorldChatData : BaseChatData
    {
        public override void UpdateData(LCReceiveChat data)
        {
            base.UpdateData(data);
            m_Sender = new PlayerChatData();
            m_Sender.UpdateData(data.Sender);
            m_Receiver = null;
        }

        public override void UpdateData(LCSendChat data)
        {
            base.UpdateData(data);
            m_Receiver = new PlayerChatData();
            m_Receiver.UpdateData(GameEntry.Data.Player);
            m_Sender = null;
        }
    }
}
