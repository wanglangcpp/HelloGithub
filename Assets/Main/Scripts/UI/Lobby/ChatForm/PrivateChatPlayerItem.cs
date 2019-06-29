using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class PrivateChatPlayerItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_VipLv = null;

        [SerializeField]
        private UILabel m_LevelLabel = null;

        [SerializeField]
        private UILabel m_ChatLineName = null;

        [SerializeField]
        private UISprite m_PlayerPortrait = null;

        [SerializeField]
        private GameObject m_Online = null;

        [SerializeField]
        private GameObject m_Offline = null;

        [SerializeField]
        private GameObject m_Select = null;

        [SerializeField]
        private GameObject m_Reminder = null;

        private PlayerChatData m_ChatPlayer = null;
        private Action<PlayerChatData, bool> m_SelectReturn = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        public int PlayerId
        {
            get
            {
                if (m_ChatPlayer == null)
                {
                    return 0;
                }
                return m_ChatPlayer.m_PlayerId;
            }
        }

        public void SetReminder(bool reminder)
        {
            m_Reminder.SetActive(reminder);
        }

        public void InitChatPlayer(PlayerChatData data, Action<PlayerChatData, bool> selectReturn)
        {
            m_ChatPlayer = data;
            m_SelectReturn = selectReturn;
            m_VipLv.text = data.VipLevel.ToString();
            m_ChatLineName.text = data.PlayerName;
            m_LevelLabel.text = data.Level.ToString();
            m_Online.SetActive(data.IsOnline);
            m_Offline.SetActive(!data.IsOnline);
            m_PlayerPortrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(data.PortraitType));
        }

        public bool Select
        {
            set
            {
                m_Select.SetActive(value);
                if (value)
                {
                    m_Reminder.SetActive(false);
                }
            }
        }

        public void OnClickSelect()
        {
            if (m_Select.activeSelf)
            {
                return;
            }
            m_Reminder.SetActive(false);
            if (m_SelectReturn != null)
            {
                m_SelectReturn(m_ChatPlayer, true);
            }
        }
    }
}
