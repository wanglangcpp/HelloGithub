using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ChatBlackListItem : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_VipLv = null;

        [SerializeField]
        private UILabel m_LevelLabel = null;

        [SerializeField]
        private UILabel m_PlayerName = null;

        [SerializeField]
        private UILabel m_PlayerLevel = null;

        [SerializeField]
        private UISprite m_PlayerPortrait = null;

        private PlayerChatData m_PlayerData = null;
        private Action m_RemoveBlacItemReturn = null;

        private void Awake()
        {
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        public void RefreshBlackItem(PlayerChatData playerData, Action removeBlacItemReturn)
        {
            m_PlayerData = playerData;
            m_RemoveBlacItemReturn = removeBlacItemReturn;
            m_VipLv.text = playerData.VipLevel.ToString();
            m_PlayerName.text = playerData.PlayerName;
            m_LevelLabel.text = playerData.Level.ToString();
            m_PlayerLevel.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", playerData.Level);
            m_PlayerPortrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(playerData.PortraitType));
        }

        public void OnClickRemoveChatBlackList()
        {
            GameEntry.Data.Chat.RemoveChatBlackList(m_PlayerData);
            if (m_RemoveBlacItemReturn != null)
            {
                m_RemoveBlacItemReturn();
            }
        }
    }
}
