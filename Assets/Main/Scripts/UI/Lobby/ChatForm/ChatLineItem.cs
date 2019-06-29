using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ChatLineItem : MonoBehaviour
    {
        public enum BackgroundType
        {
            System = 0,
            Player = 1,
        }
        [SerializeField]
        private UILabel m_VipLv = null;

        [SerializeField]
        private UILabel m_LevelLabel = null;

        [SerializeField]
        private UILabel m_ChatLineName = null;

        [SerializeField]
        private UILabel m_ChatContent = null;

        [SerializeField]
        private Color m_WordsColor = Color.black;

        [SerializeField]
        private UIButton m_ChatLineButton = null;

        [SerializeField]
        private UISprite m_PlayerPortrait = null;

        [SerializeField]
        private GameObject[] BackgroundObjs = null;

        [SerializeField]
        private UISprite m_ChatBg = null;

        [SerializeField]
        private UILabel m_ChatTime = null;

        [SerializeField]
        private GameObject m_LevelBorder = null;

        private BaseChatData m_ChatData = null;
        private Action<ChatLineItem> m_SelectReturn = null;
        private const int SystemPortraitIconId = 100003;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        public BaseChatData ChatData
        {
            get
            {
                return m_ChatData;
            }
        }

        public void InitChatLine(BaseChatData data, Action<ChatLineItem> selectReturn)
        {
            m_ChatTime.text = GameEntry.Localization.GetString("UI_TEXT_CHAT_SENDING_TIME", data.ChatTime.ToLocalTime());
            m_SelectReturn = selectReturn;
            m_ChatData = data;
            data.IsRead = true;
            if (m_ChatData.Type != ChatType.System)
            {
                var privateChata = data as PrivateChatData;
                PlayerChatData playerChatData = null;
                if (privateChata != null && privateChata.IsMe)
                {
                    playerChatData = privateChata.Sender;
                }
                else
                {
                    playerChatData = m_ChatData.Receiver == null || m_ChatData.Receiver.m_PlayerId <= 0 ? m_ChatData.Sender : m_ChatData.Receiver;
                }
                InitNormalChatLine(playerChatData);
            }
            else
            {
                m_VipLv.gameObject.SetActive(false);
                m_LevelBorder.SetActive(false);
                m_ChatLineName.text = GameEntry.Localization.GetString("UI_BUTTON_SYSTEMCHAT");
                m_PlayerPortrait.LoadAsync(SystemPortraitIconId);
            }
            m_ChatLineButton.isEnabled = (data.Type == ChatType.Private || data.Type == ChatType.World) && (m_ChatData.Sender != null && m_ChatData.Sender.m_PlayerId > 0);
            m_ChatContent.text = ColorUtility.AddColorToString(m_WordsColor, GameEntry.StringReplacement.GetString(m_ChatData.Message));
            SetBackground();
            if (m_ChatBg != null)
            {
                m_ChatBg.ResetAndUpdateAnchors();
            }
        }

        private void SetBackground()
        {
            if (BackgroundObjs.Length == 0)
            {
                return;
            }

            BackgroundObjs[(int)BackgroundType.System].SetActive(m_ChatData.Type == ChatType.System);
            BackgroundObjs[(int)BackgroundType.Player].SetActive(m_ChatData.Type != ChatType.System);
            BackgroundObjs[(int)BackgroundType.System].GetComponent<UISprite>().ResetAndUpdateAnchors();
            BackgroundObjs[(int)BackgroundType.Player].GetComponent<UISprite>().ResetAndUpdateAnchors();
        }

        private void InitNormalChatLine(PlayerChatData data)
        {
            if (data == null)
            {
                Log.Warning("PlayerChatData is Invalid!");
                return;
            }
            m_VipLv.gameObject.SetActive(true);
            m_LevelBorder.SetActive(true);
            m_VipLv.text = data.VipLevel.ToString();
            string name = data.PlayerName;
            int level = data.Level;
            m_ChatLineName.text = name;// GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", name, level.ToString());
            m_LevelLabel.text = level.ToString();
            m_PlayerPortrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(data.PortraitType));
        }

        public void OnClickPlayerChatLineItem()
        {
            if (m_SelectReturn != null)
            {
                m_SelectReturn(this);
            }
        }

        public void ClearData()
        {
            m_ChatContent.text = string.Empty;
        }
    }
}
