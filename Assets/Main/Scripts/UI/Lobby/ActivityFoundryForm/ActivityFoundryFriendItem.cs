using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动中用于好友列表中的条目。
    /// </summary>
    public class ActivityFoundryFriendItem : MonoBehaviour
    {
        public int PlayerId { get; private set; }

        [SerializeField]
        private UILabel m_NameText = null;

        [SerializeField]
        private UILabel m_LevelText = null;

        [SerializeField]
        private UISprite m_Portrait = null;

        [SerializeField]
        private UIButton m_InviteButton = null;

        [SerializeField]
        private UILabel m_InviteButtonText = null;

        public void Refresh(PlayerData playerData, bool canInvite)
        {
            PlayerId = playerData.Id;
            m_NameText.text = playerData.Name;
            m_LevelText.text = playerData.Level.ToString();
            Refresh(canInvite);

            var dtIcon = GameEntry.DataTable.GetDataTable<DRIcon>();
            DRIcon drIcon = dtIcon.GetDataRow(playerData.PortraitType);
            if (drIcon != null)
            {
                m_Portrait.spriteName = drIcon.SpriteName;
            }
        }

        public void Refresh(bool canInvite)
        {
            m_InviteButton.isEnabled = canInvite;
        }

        // Called by NGUI via reflection.
        public void OnClickInviteButton()
        {
            GameEntry.LobbyLogic.GearFoundryInviteFriend(PlayerId);
        }

        private void Awake()
        {
            m_InviteButtonText.text = GameEntry.Localization.GetString("UI_BUTTON_INVITATION");
            GameEntry.Event.Subscribe(EventId.GearFoundryInvitationSent, OnInvitationSent);
        }

        private void OnDestroy()
        {
            if (GameEntry.IsAvailable)
            {
                GameEntry.Event.Unsubscribe(EventId.GearFoundryInvitationSent, OnInvitationSent);
            }
        }

        private void OnInvitationSent(object sender, GameEventArgs e)
        {
            var ne = e as GearFoundryInvitationSentEventArgs;
            if (ne.InviteePlayerId != PlayerId)
            {
                return;
            }

            Refresh(false);
        }
    }
}
