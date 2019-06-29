using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 好友及相关内容界面。
    /// </summary>
    public partial class FriendListForm : NGUIForm
    {
        [SerializeField]
        private Tab m_FriendListTab = null;

        [SerializeField]
        private Tab m_RequestsTab = null;

        [SerializeField]
        private Tab m_RecommendationTab = null; //推荐和搜索页签

        //[SerializeField]
        //private Tab m_InvitationTab = null;

        private Tab[] m_Tabs = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(EventId.ReminderUpdated, OnReminderUpdated);
            GameEntry.Event.Subscribe(EventId.EnergyGivenToFriend, OnEnergyGivenToFriend);
            GameEntry.Event.Subscribe(EventId.EnergyReceivedFromFriend, OnEnergyReceivedFromFriend);
            InitTabs();
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable) base.OnClose(userData);
            DeinitTabs();
            GameEntry.Event.Unsubscribe(EventId.ReminderUpdated, OnReminderUpdated);
            GameEntry.Event.Unsubscribe(EventId.EnergyGivenToFriend, OnEnergyGivenToFriend);
            GameEntry.Event.Unsubscribe(EventId.EnergyReceivedFromFriend, OnEnergyReceivedFromFriend);
            base.OnClose(userData);
        }

        public void OnTab()
        {
            for (int i = 0; i < m_Tabs.Length; ++i)
                m_Tabs[i].Content.gameObject.SetActive(m_Tabs[i].TabToggle.value);
        }

        private void InitTabs()
        {
            m_Tabs = new Tab[] { m_FriendListTab, m_RequestsTab, m_RecommendationTab, /*m_InvitationTab*/ };

            for (int i = 0; i < m_Tabs.Length; ++i)
            {
                m_Tabs[i].RedDot.gameObject.SetActive(false);
            }

            OnTab();
            RefreshRequestTabReminder();
        }

        private void DeinitTabs()
        {
            // When this method is called, the toggles' GameObjects are inactive, so that their link in a group is broken.
            // Therefore, UIToggle.Set method is required to use here.
            for (int i = 0; i < m_Tabs.Length; ++i)
            {
                m_Tabs[i].TabToggle.Set(i == 0);
            }

            for (int i = 0; i < m_Tabs.Length; ++i)
            {
                m_Tabs[i].Content.gameObject.SetActive(false);
            }

            m_Tabs = null;
        }

        private void OnReminderUpdated(object sender, GameEventArgs e)
        {
            RefreshRequestTabReminder();
        }

        private void OnEnergyReceivedFromFriend(object sender, GameEventArgs e)
        {
            EnergyReceivedFromFriendEventArgs msg = e as EnergyReceivedFromFriendEventArgs;
            GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDLIST_ENERGY_RECEIVED_FROM_FRIEND", msg.RemainCount) });
        }

        private void OnEnergyGivenToFriend(object sender, GameEventArgs e)
        {
            EnergyGivenToFriendEventArgs msg = e as EnergyGivenToFriendEventArgs;
            GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_FRIENDLIST_ENERGY_GIVEN_TO_FRIEND", msg.RemainCount) });
        }

        private void RefreshRequestTabReminder()
        {
            m_RequestsTab.RedDot.gameObject.SetActive(GameEntry.Data.FriendRequests.RequestList.Count > 0);
        }

        internal class CustomPlayerData
        {
            public PlayerData Player = new PlayerData();
            public bool IsMyFriend = false;

            public CustomPlayerData()
            {
            }

            public CustomPlayerData(NearbyPlayerData nearbyPlayerData)
            {
                Player = nearbyPlayerData.Player;
                IsMyFriend = nearbyPlayerData.IsMyFriend;
            }
        }
    }
}
