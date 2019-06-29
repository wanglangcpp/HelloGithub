using GameFramework;

namespace Genesis.GameClient
{
    public class PlayerSummaryFormDisplayData : UIFormBaseUserData
    {
        public PlayerData ShowPlayerData
        {
            get;
            set;
        }

        public GameFrameworkAction<object> OnClickCloseReturn = null;
        public GameFrameworkAction<int> OnSendAddFriend = null;
        public GameFrameworkAction<int> OnRemoveFriend = null;

        private bool m_EnableInvite = true;
        public bool EnableInvite { get { return m_EnableInvite; } set { m_EnableInvite = value; } }
    }
}
