using System;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class FriendListFriendItem : MonoBehaviour
    {
        public DateTime LastLogoutTime { get; private set; }

        [SerializeField]
        private UILabel m_NameText = null;

        [SerializeField]
        private UISprite m_Portrait = null;

        [SerializeField]
        private UILabel m_LastLoginTime = null;

        [SerializeField]
        private UIButton m_GiveEnergyButton = null;

        [SerializeField]
        private UIButton m_ReceiveEnergyButton = null;

        [SerializeField]
        private UILabel m_VipLabel = null;

        [SerializeField]
        private float m_RefreshInterval = 15f;

        [SerializeField]
        private UILabel m_LevelLabel = null;

        private float m_LastRefreshTime = 0f;

        public PlayerData FriendPlayerData { get; private set; }

        private bool m_EnableSendEnergy;
        private bool m_EnableClaimEnergy;
        public bool EnableSendEnergy
        {
            get
            {
                return m_EnableSendEnergy;
            }
        }

        public bool EnableClaimEnergy
        {
            get
            {
                return m_EnableClaimEnergy;
            }
        }

        public void Refresh(FriendData friend)
        {
            FriendPlayerData = friend.Player;
            LastLogoutTime = new DateTime(friend.LastLogoutTime, DateTimeKind.Utc);
            m_NameText.text = FriendPlayerData.Name;// GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", FriendPlayerData.Name, FriendPlayerData.Level);
            m_LevelLabel.text = FriendPlayerData.Level.ToString();
            RefreshCanGiveEnergy(friend.CanGiveEnergy);
            RefreshCanReceiveEnergy(friend.CanReceiveEnergy);
            RefreshLastLoginTime();

            m_Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(friend.Player.PortraitType));
            m_Portrait.color = friend.Player.IsOnline ? Color.white : Color.gray;
            m_VipLabel.text = friend.Player.VipLevel.ToString();
        }

        public void RefreshFromEntityData()
        {
            var friend = GameEntry.Data.Friends.GetFriendDataById(FriendPlayerData.Id);
            Refresh(friend);
        }

        public void RefreshCanGiveEnergy(bool canGiveEnergy)
        {
            m_GiveEnergyButton.isEnabled = canGiveEnergy;
            m_EnableSendEnergy = canGiveEnergy;
        }

        public void RefreshCanReceiveEnergy(bool canReceiveEnergy)
        {
            m_ReceiveEnergyButton.isEnabled = canReceiveEnergy;
            m_EnableClaimEnergy = canReceiveEnergy;
        }

        private void Awake()
        {
            m_NameText.text = string.Empty;
            m_LastLoginTime.text = string.Empty;
            UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        }

        protected void Update()
        {
            if (FriendPlayerData.IsOnline)
                return;

            if (Time.realtimeSinceStartup - m_LastRefreshTime > m_RefreshInterval)
            {
                m_LastRefreshTime = Time.realtimeSinceStartup;
                RefreshLastLoginTime();
            }
        }

        public void OnClickWholeButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PlayerSummaryForm, new PlayerSummaryFormDisplayData { ShowPlayerData = FriendPlayerData });
        }

        public void OnClickGiveEnergyButton()
        {
            int maxCount = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.DailyEnergyGiveTimes, 10) - GameEntry.Data.Friends.TodayGiveCount;
            if (maxCount <= 0)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_GIVE_PHYSICAL_STRENGTH_LIMIT") });
                return;
            }

            GiveEnergyToFriend();
        }

        public void OnClickReceiveEnergyButton()
        {
            int maxCount = GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.DailyEnergyClaimTimes, 10) - GameEntry.Data.Friends.TodayClaimCount;
            if (maxCount <= 0)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDS_UPPER_LIMIT_OF_PHYSICAL_STRENGTH") });
                return;
            }

            PickEnergyFromFriend();
        }

        public void GiveEnergyToFriend()
        {
            GameEntry.LobbyLogic.GiveEnergyToFriend(FriendPlayerData.Id);
        }

        public void PickEnergyFromFriend()
        {
            GameEntry.LobbyLogic.ReceiveEnergyFromFriend(FriendPlayerData.Id);
        }

        private void RefreshLastLoginTime()
        {
            if (FriendPlayerData.IsOnline)
                m_LastLoginTime.text = GameEntry.Localization.GetString("UI_TEXT_CHAT_ONLINE");
            else
                m_LastLoginTime.text = UIUtility.GetLastLoginTimeString(GameEntry.Time.LobbyServerUtcTime - LastLogoutTime);
        }
    }
}
