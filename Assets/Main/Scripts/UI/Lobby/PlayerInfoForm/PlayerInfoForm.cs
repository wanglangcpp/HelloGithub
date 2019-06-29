using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家详细信息界面。
    /// </summary>
    public partial class PlayerInfoForm : NGUIForm
    {
        private enum TabType
        {
            PlayerInfo = 0,
            Setup = 1,
        }

        [SerializeField]
        private UIToggle[] m_TabToggleList = null;

        [SerializeField]
        private GameObject m_SelfInfoPlate = null;

        [SerializeField]
        private GameObject m_PlayerInfoPlate = null;

        [SerializeField]
        private GameObject m_SetUpInfoPlate = null;

        [SerializeField]
        private GameObject m_MusicPlate = null;

        [SerializeField]
        private GameObject m_SoundPlate = null;

        [SerializeField]
        private SelfInfo m_SelfInfo = null;

        [SerializeField]
        private PlayerInfo m_PlayerInfo = null;

        [SerializeField]
        private SetupInfo m_SetupInfo = null;

        private bool m_ShowSelfInfo = false;
        private bool m_MyFriend = false;
        private PBPlayerInfo m_Player = null;
        private PlayerInfoDisplayData m_UserData = null;
        private List<PBLobbyHeroInfo> m_HeroTeam = null;
        private bool m_HasGetPlayerDetail = false;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_UserData = userData as PlayerInfoDisplayData;
            if (m_UserData == null)
            {
                Log.Error("User data is invalid.");
                return;
            }

            m_TabToggleList[(int)TabType.Setup].gameObject.SetActive(GameEntry.Data.Player.Id == m_UserData.PlayerId);
            m_HasGetPlayerDetail = false;
            GameEntry.Event.Subscribe(EventId.FriendRequestSent, OnFriendRequestSent);
            GameEntry.Event.Subscribe(EventId.FriendDeleted, OnFriendDeleted);
            GameEntry.Event.Subscribe(EventId.PlayerDataChanged, OnPlayerPortraitDataChanged);
            m_TabToggleList[0].value = true;
            OnTabTypeChanged(0, true);
        }

        protected override void OnClose(object userData)
        {
            if (!GameEntry.IsAvailable) return;
            GameEntry.Event.Unsubscribe(EventId.PlayerDataChanged, OnPlayerPortraitDataChanged);
            GameEntry.Event.Unsubscribe(EventId.FriendRequestSent, OnFriendRequestSent);
            GameEntry.Event.Unsubscribe(EventId.FriendDeleted, OnFriendDeleted);
            m_UserData = null;
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (GameEntry.Data.Player.Id == m_UserData.PlayerId && m_SelfInfoPlate.gameObject.activeSelf)
            {
                m_SelfInfo.UpdateEnergyRecoveryTime(GameEntry.PlayerEnergy.EnergyRecoveryLeftTime);
            }
        }

        private void OnRefreshSetUp()
        {
            m_SetUpInfoPlate.SetActive(true);
            m_SetupInfo.InitData();
        }

        private void OnRefreshPlayerInfo()
        {
            if (GameEntry.Data.Player.Id == m_UserData.PlayerId)
            {
                m_PlayerInfoPlate.SetActive(false);
                m_SelfInfoPlate.SetActive(true);
                m_ShowSelfInfo = true;
                OnRefreshForm();
            }
            else
            {
                m_ShowSelfInfo = false;
                m_PlayerInfoPlate.SetActive(true);
                m_SelfInfoPlate.SetActive(false);
                m_PlayerInfo.HideAll();

                if (m_HasGetPlayerDetail)
                {
                    OnRefreshForm();
                }
                else
                {
                    GameEntry.LobbyLogic.GetPlayerInfo(m_UserData.PlayerId);
                }
            }
        }

        private void OnFriendRequestSent(object sender, GameEventArgs e)
        {
            CloseSelf();
        }

        private void OnFriendDeleted(object sender, GameEventArgs e)
        {
            CloseSelf();
        }

        private void OnRefreshForm()
        {
            if (m_ShowSelfInfo)
            {
                m_SelfInfo.InitSelfInfoPanel();
            }
            else
            {
                m_PlayerInfo.InitPlayerInfoPanel(m_MyFriend, m_Player);
            }
        }

        private void OnPlayerPortraitDataChanged(object sender, GameEventArgs e)
        {
            OnRefreshForm();
        }

        public void OnTabTypeChanged(int typeKey, bool selected)
        {
            if (selected)
            {
                m_SelfInfoPlate.SetActive(false);
                m_PlayerInfoPlate.SetActive(false);
                m_SetUpInfoPlate.SetActive(false);
                switch ((TabType)typeKey)
                {
                    case TabType.PlayerInfo:
                        OnRefreshPlayerInfo();
                        break;
                    case TabType.Setup:
                        OnRefreshSetUp();
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnMusicIsUnmutedChanged(bool unmuted)
        {
            m_MusicPlate.SetActive(unmuted);
            GameEntry.Sound.MuteMusic(!unmuted);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            GameEntry.Sound.SetMusicVolume(volume);
        }

        public void OnSoundIsUnmutedChanged(bool unmuted)
        {
            m_SoundPlate.SetActive(unmuted);
            GameEntry.Sound.MuteSound(!unmuted);
        }

        public void OnSoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetSoundVolume(volume);
        }

        public void OnQualityLevelChanged(int qualityLevel, bool selected)
        {
            if (selected)
            {
                SetQualityLevel(qualityLevel);
            }
        }

        private void SetQualityLevel(int qualityLevel)
        {
            int currentQualityLevel = GameEntry.Setting.GetInt(Constant.Setting.QualityLevel, QualitySettings.GetQualityLevel());
            if (qualityLevel != currentQualityLevel)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_CONFIRM_QUALITY_LEVEL"),
                    ConfirmText = GameEntry.Localization.GetString("UI_BUTTON_SYSTEM_CONFIRM"),
                    CancelText = GameEntry.Localization.GetString("UI_BUTTON_CANCEL"),
                    OnClickConfirm = ConfirmQualityLevel,
                    OnClickCancel = CancelQualityLevel,
                    UserData = qualityLevel,
                });
            }
        }

        private void ConfirmQualityLevel(object userData)
        {
            GameEntry.Setting.SetInt(Constant.Setting.QualityLevel, (int)userData);
            GameEntry.Setting.Save();
            GameEntry.Restart();
        }

        private void CancelQualityLevel(object userData)
        {
            m_SetupInfo.ResetQuality();
        }

        #region OnClick Event

        public void OnClickAddFriend()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }

            if (GameEntry.Data.Friends.Data.Count >= GameEntry.ServerConfig.GetInt(Constant.ServerConfig.Friend.MaxFriendCount, 50))
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_MAX_FRIEND_COUNT_REACHED") });
                return;
            }

            GameEntry.LobbyLogic.SendFriendRequest(m_Player.Id);
        }

        public void OnClickDeleteFriend()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }

            GameEntry.LobbyLogic.RemoveFriend(m_Player.Id);
        }

        public void OnClickChatPersonal()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }

            //             var chatDisplayData = new ChatDisplayData
            //             {
            //                 ChatType = ChatType.Private,
            //                 ChatPlayer = m_Player,
            //             };
            // 
            //             if (GameEntry.UI.GetUIForm(UIFormId.ChatForm) == null)
            //             {
            //                 GameEntry.UI.OpenUIForm(UIFormId.ChatForm, chatDisplayData);
            //             }
            //             else
            //             {
            //                 GameEntry.UI.RefocusUIForm(GameEntry.UI.GetUIForm(UIFormId.ChatForm), chatDisplayData);
            //             }
        }

        public void OnClickTeamButton()
        {
            if (GameEntry.Data.Player.Id == m_UserData.PlayerId)
            {
                Log.Error("Not supported.");
                return;
            }

            var lobbyHeroesData = new LobbyHeroesData();
            lobbyHeroesData.AddData(m_HeroTeam);
            GameEntry.UI.OpenUIForm(UIFormId.HeroTeamForm, new HeroTeamDisplayData { Scenario = HeroTeamDisplayScenario.OtherPlayer, Heroes = lobbyHeroesData });
        }

        public void OnClickHeadPortraitButton()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ReplacePlayerPortraitForm);
        }

        public void OnClickResetSettingButton()
        {
            int defaultQualityLevel = (int)GameEntry.ClientConfig.GetDefaultQualityLevel();
            SetQualityLevel(defaultQualityLevel);
        }

        public void OnClickRestartGame()
        {
            
            if (SDKManager.HasSDK&& SDKManager.Instance.SDKData.HadLogout())
            {
                SDKManager.Instance.helper.Logout();
            }
            else
            {
                GameEntry.Restart();
            }
        }

        #endregion OnClick Event
    }
}
