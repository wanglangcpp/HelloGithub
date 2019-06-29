
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ChatForm
    {
        [Serializable]
        private class ChatInput
        {
            [SerializeField]
            private UIInput m_ChatInput = null;

            private PlayerChatData m_ChatPlayerData = null;

            public PlayerChatData PrivateChatInputPlayer
            {
                get
                {
                    return m_ChatPlayerData;
                }
                set
                {
                    m_ChatPlayerData = value;
                }
            }

            public string GetInputValue()
            {
                return m_ChatInput.value;
            }

            public string GetInputStrAndClear()
            {
                string str = string.Empty;

                if (m_ChatInput != null)
                {
                    str = GetInputValue().Trim().Replace('`', '\'').Replace('[', '\u3010').Replace(']', '\u3011');
                    m_ChatInput.value = string.Empty;
                }
                return str;
            }

            public void ClearChatInputValue()
            {
                m_ChatInput.value = string.Empty;
            }

            public void ClearChatInput()
            {
                m_ChatInput.characterLimit = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Chat.MaxMessageLength, 30);
                m_ChatInput.defaultText = GameEntry.Localization.GetString("UI_TEXT_ENTER_THIRTY");
                PrivateChatInputPlayer = null;
            }
        }
    }
}
