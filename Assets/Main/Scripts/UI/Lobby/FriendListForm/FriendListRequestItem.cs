using UnityEngine;

namespace Genesis.GameClient
{
    public class FriendListRequestItem : MonoBehaviour
    {
        public int PlayerId { get; private set; }

        [SerializeField]
        private UILabel m_NameText = null;

        [SerializeField]
        private UISprite m_Portrait = null;

        [SerializeField]
        private UILabel m_FriendStatusLabel = null;

        [SerializeField]
        private UILabel m_VipLabel = null;

        [SerializeField]
        private UILabel m_LevelLabel = null;

        private PlayerData m_PlayerData = null;
        private void Awake()
        {
            var labels = GetComponentsInChildren<UILabel>(true);
            for (int i = 0; i < labels.Length; ++i)
            {
                labels[i].text = GameEntry.Localization.GetString(labels[i].text);
            }
        }

        public void Refresh(PlayerData player)
        {
            m_PlayerData = player;
            PlayerId = player.Id;
            m_NameText.text = player.Name;// GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", player.Name, player.Level);
            m_LevelLabel.text = player.Level.ToString();

            m_Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(player.PortraitType));
            m_Portrait.color = player.IsOnline ? Color.white : Color.gray;

            string onlineStatusLabelKey = player.IsOnline ? "UI_TEXT_CHAT_ONLINE" : "UI_TEXT_CHAT_OFFLINE";
            m_FriendStatusLabel.text = GameEntry.Localization.GetString(onlineStatusLabelKey);
            m_VipLabel.text = player.VipLevel.ToString();
        }

        public void OnClickRefuseButton()
        {
            GameEntry.LobbyLogic.RefuseFriendRequest(PlayerId);
        }

        public void OnClickAcceptButton()
        {
            if (GameEntry.Data.Friends.Data.Count >= GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.MaxFriendCount, 50))
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_MAX_FRIEND_COUNT_REACHED") });
                return;
            }

            AcceptFriend();
        }

        public void AcceptFriend()
        {
            GameEntry.LobbyLogic.AcceptFriendRequest(PlayerId);
        }

        public void OnClickWholeButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PlayerSummaryForm, new PlayerSummaryFormDisplayData { ShowPlayerData = m_PlayerData });
        }
    }
}
