using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ReminderComponent : MonoBehaviour
    {
        private void OnPendingFriendRequestsDataChanged(object sender, GameEventArgs e)
        {
            RefreshPendingFriendRequestsReminder();
            AfterProcessingDataChange();
        }

        private void OnPlayerDataChanged(object sender, GameEventArgs e)
        {
            if (GameEntry.Data.Player.Level != m_CachedPlayerLevel || GameEntry.Data.Player.Coin != m_CachedPlayerCoin)
            {
                m_CachedPlayerLevel = GameEntry.Data.Player.Level;
                m_CachedPlayerCoin = GameEntry.Data.Player.Coin;
            }
        }

        private void OnLobbyHeroDataChanged(object sender, GameEventArgs e)
        {
            AfterProcessingDataChange();
        }

        private void OnGearDataChanged(object sender, GameEventArgs e)
        {
            AfterProcessingDataChange();
        }

        private void OnItemDataChanged(object sender, GameEventArgs e)
        {
            AfterProcessingDataChange();
        }

        private void OnChanceDataChanged(object sender, GameEventArgs e)
        {
            AfterProcessingDataChange();
        }
    }
}
