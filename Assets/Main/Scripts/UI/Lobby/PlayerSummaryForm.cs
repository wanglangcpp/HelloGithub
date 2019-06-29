using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class PlayerSummaryForm : NGUIForm
    {
        [SerializeField]
        private UISprite m_PlayerIcon = null;

        [SerializeField]
        private UILabel m_VipLevelLbl = null;

        [SerializeField]
        private UILabel m_LevelLabel = null;

        [SerializeField]
        private UILabel m_PlayerNameLbl = null;

        [SerializeField]
        private UILabel m_PlayerDisplayIdLbl = null;

        [SerializeField]
        private UILabel m_MightLbl = null;

        [SerializeField]
        private UILabel m_PlayerIdLbl = null;

        [SerializeField]
        private UIButton m_AddFriendBtn = null;

        [SerializeField]
        private UIButton m_RemoveFriendBtn = null;

        [SerializeField]
        private UIButton m_BlackListBtn = null;

        [SerializeField]
        private UIButton m_ChatBtn = null;

        [SerializeField]
        private UILabel m_BlackListLbl = null;

        private PlayerData m_Player = null;

        private GameFrameworkAction<object> m_OnClickCloseReturn = null;
        private GameFrameworkAction<int> OnSendAddFriend = null;
        private GameFrameworkAction<int> OnRemoveFriend = null;
        private bool m_EnableInvite = false;

        public PlayerData FriendPlayerData { get; private set; }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.GetPlayerInfo, OnGetPlayerInfo);
            var myUserData = userData as PlayerSummaryFormDisplayData;

            if (myUserData == null)
            {
                Log.Error("User data is invalid.");
                return;
            }
            m_Player = myUserData.ShowPlayerData;
            m_OnClickCloseReturn = myUserData.OnClickCloseReturn;
            OnSendAddFriend = myUserData.OnSendAddFriend;
            OnRemoveFriend = myUserData.OnRemoveFriend;
            m_EnableInvite = myUserData.EnableInvite;

            RefreshData();
        }

        private void OnGetPlayerInfo(object sender, GameEventArgs e)
        {
            var data = e as GetPlayerInfoEventArgs;
            GameEntry.UI.OpenUIForm(UIFormId.OtherPlayerInfoForm, new OtherPlayerInfoDisplayData
            { PlayerData = data.PlayerData, Heroes = data.Heroes, HeroTeam = data.HeroTeam });
            CloseSelf(true);
        }

        protected override void OnClose(object userData)
        {
            if (m_OnClickCloseReturn != null)
            {
                m_OnClickCloseReturn(null);
            }
            GameEntry.Event.Unsubscribe(EventId.GetPlayerInfo, OnGetPlayerInfo);
            m_Player = null;
            base.OnClose(userData);
        }

        // Called by NGUI via reflection.
        public void OnClickPlayerInfoBtn()
        {
            if (m_Player.Id == GameEntry.Data.Player.Id)
            {
                GameEntry.UI.OpenUIForm(UIFormId.PlayerInfoForm, new PlayerInfoDisplayData { PlayerId = GameEntry.Data.Player.Id });
                CloseSelf(true);
            }
            else
            {
                GameEntry.Network.Send(new CLGetPlayerDetail() { PlayerId = m_Player.Id });
            }
        }

        // Called by NGUI via reflection.
        public void OnClickAddFriendBtn()
        {
            if (GameEntry.Data.Friends.Data.Count >= GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.MaxFriendCount, 50))
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_MAX_FRIEND_COUNT_REACHED") });
                return;
            }

            GameEntry.LobbyLogic.SendFriendRequest(m_Player.Id);

            if (OnSendAddFriend != null)
                OnSendAddFriend(m_Player.Id);

            CloseSelf(true);
        }

        public void OnClickBlackListBtn()
        {
            if (GameEntry.Data.Chat.IsChatBlackList(m_Player.Id))
            {
                GameEntry.Data.Chat.RemoveChatBlackList(m_Player.Id);
            }
            else
            {
                PlayerChatData playerData = new PlayerChatData();
                playerData.UpdateData(m_Player);
                GameEntry.Data.Chat.AddChatBlackList(playerData);
            }
            RefreshData();
        }

        public void OnClickRemoveFriendBtn()
        {
            GameEntry.LobbyLogic.RemoveFriend(m_Player.Id);

            if (OnRemoveFriend != null)
                OnRemoveFriend(m_Player.Id);

            CloseSelf(true);
        }

        // Called by NGUI via reflection.
        public void OnClickChatBtn()
        {
            if (GameEntry.Data.Chat.IsChatBlackList(m_Player.Id))
            {
                UIUtility.ShowToast(GameEntry.Localization.GetString("UI_TEXT_CHAT_SHIELDING_CAN_NOT_SEND_INFORMATION"));
                return;
            }

            if (GameEntry.UI.HasUIForm(UIFormId.ChatForm))
            {
                if (m_OnClickCloseReturn != null)
                {
                    m_OnClickCloseReturn(m_Player);
                }
                m_OnClickCloseReturn = null;
            }
            else
            {
                GameEntry.UI.OpenUIForm(UIFormId.ChatForm, new ChatDisplayData
                {
                    ChatType = ChatType.Private,
                    ChatPlayer = m_Player,
                });
            }
            CloseSelf(true);
        }

        private void RefreshData()
        {

            if (GameEntry.Data.Chat.IsChatBlackList(m_Player.Id))
            {
                m_BlackListLbl.text = GameEntry.Localization.GetString("UI_BUTTON_CANCEL_MASK");
            }
            else
            {
                m_BlackListLbl.text = GameEntry.Localization.GetString("UI_TEXT_ADD_MASK");
            }

            m_PlayerDisplayIdLbl.text = GameEntry.Localization.GetString("UI_TEXT_PLAYERID", m_Player.DisplayId.ToString());
            m_PlayerIcon.LoadAsync(UIUtility.GetPlayerPortraitIconId(m_Player.PortraitType));
            m_PlayerNameLbl.text = m_Player.Name;// GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", m_Player.Name, m_Player.Level);
            m_LevelLabel.text = m_Player.Level.ToString();
            m_VipLevelLbl.text = m_Player.VipLevel.ToString();
            m_MightLbl.text = m_Player.TeamMight.ToString();
            m_PlayerIdLbl.text = m_Player.DisplayId.ToString();

            bool isMyFriend = GameEntry.Data.Friends.CheckWhetherIsMyFriend(m_Player.Id);
            m_AddFriendBtn.gameObject.SetActive(!isMyFriend);
            m_AddFriendBtn.isEnabled = m_EnableInvite;
            m_RemoveFriendBtn.gameObject.SetActive(isMyFriend);
            m_ChatBtn.isEnabled = (m_Player.Id != GameEntry.Data.Player.Id);
            if (m_Player.Id == GameEntry.Data.Player.Id)
            {
                m_AddFriendBtn.isEnabled = false;
                m_RemoveFriendBtn.gameObject.SetActive(false);
                m_BlackListBtn.isEnabled = false;
            }
        }
    }
}
