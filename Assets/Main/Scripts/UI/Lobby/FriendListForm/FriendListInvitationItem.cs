using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class FriendListInvitationItem : MonoBehaviour
    {
        public DateTime LastLoginTime { get; private set; }

        public int PlayerId { get; private set; }

        [SerializeField]
        private UILabel m_NameText = null;

        [SerializeField]
        private UISprite m_Portrait = null;

        [SerializeField]
        private UIButton m_InviteButton = null;

        [SerializeField]
        private UILabel m_InviteButtonText = null;

        [SerializeField]
        private UILabel m_LastLoginTime = null;

        [SerializeField]
        private UILabel m_VipLabel = null;

        [SerializeField]
        private UILabel m_LevelLabel = null;

        private PlayerData m_PlayerData = null;
        private bool m_EnableInvite;
        public bool EnableInvite
        {
            get
            {
                return m_EnableInvite;
            }
        }

        public void Refresh(PlayerData playerData, bool canInvite)
        {
            m_PlayerData = playerData;
            PlayerId = playerData.Id;
            m_NameText.text = playerData.Name;// GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", playerData.Name, playerData.Level);
            m_LevelLabel.text = playerData.Level.ToString();
            RefreshInviteStatus(canInvite);

            string onLineStatusKey = playerData.IsOnline ? "UI_TEXT_CHAT_ONLINE" : "UI_TEXT_CHAT_OFFLINE";
            m_LastLoginTime.text = GameEntry.Localization.GetString(onLineStatusKey);

            m_Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(playerData.PortraitType));
            m_Portrait.color = playerData.IsOnline ? Color.white : Color.gray;
            m_VipLabel.text = playerData.VipLevel.ToString();
        }

        public void RefreshInviteStatus(bool canInvite)
        {
            m_InviteButton.isEnabled = canInvite;
            m_EnableInvite = canInvite;
        }

        private void Awake()
        {
            m_InviteButtonText.text = GameEntry.Localization.GetString("UI_BUTTON_INVITATION");
        }

        public void OnClickWholeButton()
        {
            if(m_PlayerData.Id == GameEntry.Data.Player.Id)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData() { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_CLICKED_ITEM_IS_SELF") });
                return;
            }

            GameEntry.UI.OpenUIForm(UIFormId.PlayerSummaryForm, new PlayerSummaryFormDisplayData
            {
                ShowPlayerData = m_PlayerData,
                OnSendAddFriend = OnSendInviteSuccess,
                EnableInvite = m_EnableInvite
            });
        }

        public void OnClickInviteButton()
        {
            if (GameEntry.Data.Friends.Data.Count >= GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.MaxFriendCount, 50))
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_MAX_FRIEND_COUNT_REACHED") });
                return;
            }

            GameEntry.LobbyLogic.SendFriendRequest(PlayerId);
            RefreshInviteStatus(false);
        }

        private void OnSendInviteSuccess(int id)
        {
            if(PlayerId == id)
                RefreshInviteStatus(false);
        }
    }
}
