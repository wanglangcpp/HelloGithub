using UnityEngine;

namespace Genesis.GameClient
{
    // TODO: 此类目前用于显示千锤百炼活动的邀请。修改以适应更多情况的邀请。
    public class RequestListRequestItem : MonoBehaviour
    {
        public int PlayerId { get; private set; }

        [SerializeField]
        private UILabel m_LevelText = null;

        [SerializeField]
        private UISprite m_Portrait = null;

        [SerializeField]
        private UILabel m_ContentText = null;

        public object UserData { get; private set; }

        private void Awake()
        {
            var labels = GetComponentsInChildren<UILabel>(true);
            for (int i = 0; i < labels.Length; ++i)
            {
                labels[i].text = GameEntry.Localization.GetString(labels[i].text);
            }
        }

        public void Refresh(PlayerData player, object userData)
        {
            UserData = userData;

            PlayerId = player.Id;
            m_LevelText.text = player.Level.ToString();
            string activityName = GameEntry.Localization.GetString("UI_TITLE_NAME_ACTIVITYFOUNDRY");
            m_ContentText.text = GameEntry.Localization.GetString("UI_TEXT_ACTIVITY_INVITATION", player.Name, activityName);

            var dtIcon = GameEntry.DataTable.GetDataTable<DRIcon>();
            DRIcon drIcon = dtIcon.GetDataRow(player.PortraitType);
            if (drIcon != null)
            {
                m_Portrait.spriteName = drIcon.SpriteName;
            }
        }

        public void OnClickRefuseButton()
        {
            GameEntry.LobbyLogic.GearFoundryRespondToInvitation(false, PlayerId, (int)UserData);
        }

        public void OnClickAcceptButton()
        {
            GameEntry.LobbyLogic.GearFoundryRespondToInvitation(true, PlayerId, (int)UserData);
        }

        public void OnClickWholeButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.PlayerInfoForm, new PlayerInfoDisplayData { PlayerId = PlayerId });
        }
    }
}
