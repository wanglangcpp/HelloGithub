using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 提醒器组件。用于图形界面上显示的新消息提醒。
    /// </summary>
    public partial class ReminderComponent : MonoBehaviour
    {
        private int m_CachedPlayerLevel = -1;
        private int m_CachedPlayerCoin = -1;

        private void Start()
        {
            GameEntry.Event.Subscribe(EventId.PlayerDataChanged, OnPlayerDataChanged);
            GameEntry.Event.Subscribe(EventId.LobbyHeroDataChanged, OnLobbyHeroDataChanged);
            GameEntry.Event.Subscribe(EventId.ItemDataChanged, OnItemDataChanged);
            GameEntry.Event.Subscribe(EventId.PendingFriendRequestsDataChanged, OnPendingFriendRequestsDataChanged);
            GameEntry.Event.Subscribe(EventId.ChanceDataChanged, OnChanceDataChanged);
        }

        private void AfterProcessingDataChange()
        {
            GameEntry.Event.Fire(this, new ReminderUpdatedEventArgs());
        }
    }
}
