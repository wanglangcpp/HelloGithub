using GameFramework.Fsm;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备锻造活动界面。
    /// </summary>
    public partial class ActivityFoundryForm : NGUIForm
    {
        [Serializable]
        private class Member
        {
            [SerializeField]
            public Transform Root = null;

            [SerializeField]
            public UISprite Portrait = null;

            [SerializeField]
            public UILabel Name = null;

            [SerializeField]
            public UISprite NameBg = null;

            [SerializeField]
            public UILabel FoundryCount = null;

            [SerializeField]
            public UISprite FoundryCountBg = null;

            [SerializeField]
            public UIButton KickButton = null;

            [SerializeField]
            public Animation Animation = null;
        }

        [SerializeField]
        private Transform m_FriendsRoot = null;

        [SerializeField]
        private Member[] m_Members = null;

        [SerializeField]
        private TweenScale m_MatchTweener = null;

        [SerializeField]
        private UILabel m_CDText = null;

        [SerializeField]
        private UILabel m_ProgressText = null;

        [SerializeField]
        private Transform m_ButtonListNoTeam = null;

        [SerializeField]
        private Transform m_ButtonListHasTeam = null;

        [SerializeField]
        private Transform[] m_LevelIndicators = null;

        [SerializeField]
        private UISprite[] m_LevelIndicatorCDs = null;

        [SerializeField]
        private GameObject[] m_DecalRotation = null;

        [SerializeField]
        private Animation m_FireAnimation = null;

        [SerializeField]
        private Animation[] m_CDMaskDoorAnims = null;

        [SerializeField]
        private UIButton[] m_ClaimRewardButtons = null;

        [SerializeField]
        private UIButton m_FoundryButton = null;

        [SerializeField]
        private Animation m_CompletingAnim = null;

        [SerializeField]
        private Animation m_CDAnimation = null;

        [SerializeField]
        private GameObject m_ClickNote = null;

#pragma warning disable 0414

        [SerializeField]
        private UIButton m_CreateTeamButton = null;

        [SerializeField]
        private UIButton m_InviteButton = null;

        [SerializeField]
        private UIButton m_MatchRootButton = null;

        [SerializeField]
        private UIButton[] m_MatchButtons = null;

        [SerializeField]
        private UIButton m_RequestButton = null;

        [SerializeField]
        private UIButton m_SendLinkButton = null;

        [SerializeField]
        private UIButton m_LeaveButton = null;

#pragma warning restore 0414

        private GearFoundryData m_SrcData = null;
        private IFsm<ActivityFoundryForm> m_Fsm = null;

        private enum CDMaskDoorStatus
        {
            Open,
            Closed,
            Opening,
            Closing,
        }

        private CDMaskDoorStatus m_CDMaskDoorStatus = CDMaskDoorStatus.Closed;

        private const string InClipInName = "FoundryMelteHighIn";

        private const string InClipOutName = "FoundryMelterOut";

        private const string EffectFireClick = "EffectFireClick";

        private const string EffectFireOn = "EffectFireOn";

        private const string EffectFireBlock = "EffectFireBlock";

        private int m_EffectFireOnKey = 0;

        private int m_EffectFireBlock = 0;

        private bool m_HasOpenedDoor = false;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_SrcData = GameEntry.Data.GearFoundry;
            ResetMatchTweener();
            CloseCDMaskDoor(false);
            m_CompletingAnim.gameObject.SetActive(false);
            m_FireAnimation.Play();

            m_Fsm = GameEntry.Fsm.CreateFsm(this,
                new StateOpen(),
                new StateNoTeam(),
                new StateHasTeam(),
                new StateNoTeamToHasTeam(),
                new StateHasTeamToNoTeam());
            m_Fsm.Start<StateOpen>();
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_SrcData = null;
            HideFriends();
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            UpdateCDMaskDoorState();
        }

        protected override void OnPostOpen(object data)
        {
            base.OnPostOpen(data);

            if (m_HasOpenedDoor && m_EffectFireOnKey <= 0)
            {
                m_EffectFireOnKey = m_EffectsController.ShowEffect(EffectFireOn);
            }
            else if (m_EffectFireBlock <= 0)
            {
                m_EffectFireBlock = m_EffectsController.ShowEffect(EffectFireBlock);
            }
        }

        // Called by NGUI via reflection.
        public void OnClickCreateTeam()
        {
            (m_Fsm.CurrentState as StateBase).OnClickCreateTeam(m_Fsm);
        }

        // Called by NGUI via reflection.
        public void OnClickInvite()
        {
            (m_Fsm.CurrentState as StateBase).OnClickInvite(m_Fsm);
        }

        // Called by NGUI via reflection.
        public void OnClickMatch(UIIntKey intKey)
        {
            int level = intKey.Key;
            (m_Fsm.CurrentState as StateBase).OnClickMatch(m_Fsm, level);
        }

        // Called by NGUI via reflection.
        public void OnClickRequests()
        {
            (m_Fsm.CurrentState as StateBase).OnClickRequests(m_Fsm);
        }

        // Called by NGUI via reflection.
        public void OnClickFoundry()
        {
            (m_Fsm.CurrentState as StateBase).OnClickFoundry(m_Fsm);
        }

        // Called by NGUI via reflection.
        public void OnClickSendLink()
        {
            (m_Fsm.CurrentState as StateBase).OnClickSendLink(m_Fsm);
        }

        // Called by NGUI via reflection.
        public void OnClickClaimReward()
        {
            (m_Fsm.CurrentState as StateBase).OnClickClaimReward(m_Fsm);
        }

        // Called by NGUI via reflection.
        public void OnClickLeave()
        {
            (m_Fsm.CurrentState as StateBase).OnClickLeave(m_Fsm);
        }

        // Called by NGUI via reflection.
        public void OnKickButton(UIIntKey intKey)
        {
            (m_Fsm.CurrentState as StateBase).OnKickButton(m_Fsm, intKey.Key);
        }

        // Called by NGUI via reflection.
        public void OnClickCompletingAnimMask()
        {
            (m_Fsm.CurrentState as StateBase).OnClickCompletingAnimMask(m_Fsm);
        }

        protected override bool BackButtonIsAvailable
        {
            get
            {
                return !FriendsAreShowing;
            }
        }

        private void RefreshPlayers()
        {
            var players = m_SrcData.Players.Data;
            for (int i = 0; i < m_Members.Length; ++i)
            {
                var memberDisplay = m_Members[i];
                if (i >= players.Count)
                {
                    memberDisplay.FoundryCount.text = string.Empty;
                    memberDisplay.Name.text = string.Empty;
                    memberDisplay.FoundryCountBg.gameObject.SetActive(false);
                    memberDisplay.NameBg.gameObject.SetActive(false);
                    memberDisplay.Portrait.gameObject.SetActive(false);
                }
                else
                {
                    memberDisplay.FoundryCount.text = players[i].FoundryCount.ToString();
                    memberDisplay.Name.text = players[i].Name;
                    memberDisplay.FoundryCountBg.gameObject.SetActive(true);
                    memberDisplay.NameBg.gameObject.SetActive(true);
                    memberDisplay.Portrait.gameObject.SetActive(true);
                    // TODO: Set player portrait.
                }
            }
        }

        private void RefreshKickButtons()
        {
            var players = m_SrcData.Players.Data;
            var canKick = m_SrcData.AmILeader;
            for (int i = 1; i < players.Count; ++i)
            {
                m_Members[i].KickButton.isEnabled = canKick;
            }

            for (int i = players.Count; i < m_Members.Length; ++i)
            {
                m_Members[i].KickButton.isEnabled = false;
            }
        }

        private int RewardLevel
        {
            get
            {
                for (int i = 0; i < 3/*GameEntry.ServerConfig.GetInt(Constant.ServerConfig.GearFoundryLevelCount, 3)*/; ++i)
                {
                    if (m_SrcData.GetRewardFlagAtLevel(i))
                    {
                        return i;
                    }
                }

                return -1;
            }
        }

        private void RefreshProgress()
        {
            var progressData = m_SrcData.Progress;

            if (IsComplete)
            {
                SetLevelIndicatorCD(1.0f);
                m_ProgressText.text = GameEntry.Localization.GetString("UI_TEXT_FOUNDRY_COMPLETE");
                return;
            }

            int maxProgress = 5; // GameEntry.ServerConfig.GetInt(string.Format(Constant.ServerConfig.GearFoundryProgressCountFormat, progressData.CurrentLevel.ToString()), 5);
            m_ProgressText.text = GameEntry.Localization.GetString("UI_TEXT_SLASH",
                progressData.CurrentProgress.ToString(),
                maxProgress);
            SetLevelIndicatorCD(maxProgress > 0 ? (float)progressData.CurrentProgress / maxProgress : 0f);
        }

        private void SetLevelIndicatorCD(float amount)
        {
            for (int i = 0; i < m_LevelIndicators.Length; ++i)
            {
                if (m_LevelIndicators[i].gameObject.activeSelf)
                {
                    m_LevelIndicatorCDs[i].fillAmount = amount;
                }
                else
                {
                    m_LevelIndicatorCDs[i].fillAmount = 0;
                }
            }
        }

        private bool CanPerformFoundry
        {
            get
            {
                if (IsComplete || RewardLevel >= 0)
                {
                    return false;
                }

                return !IsCoolingDown;
            }
        }

        private bool IsCoolingDown
        {
            get
            {
                TimeSpan coolDownTime = m_SrcData.NextFoundryTime - GameEntry.Time.LobbyServerUtcTime;
                return coolDownTime >= TimeSpan.Zero;
            }
        }

        private bool IsComplete
        {
            get
            {
                return (m_SrcData.Progress.CurrentLevel >= 3/*GameEntry.ServerConfig.GetInt(Constant.ServerConfig.GearFoundryLevelCount, 3)*/);
            }
        }

        private void ShowFriends()
        {
            m_FriendsRoot.gameObject.SetActive(true);
        }

        private void HideFriends()
        {
            m_FriendsRoot.gameObject.SetActive(false);
        }

        private bool FriendsAreShowing
        {
            get
            {
                return m_FriendsRoot.gameObject.activeSelf;
            }
        }

        private void ResetMatchTweener()
        {
            m_MatchTweener.ResetToBeginning();
            m_MatchTweener.gameObject.SetActive(false);
        }

        private void ResetNoTeamDisplay()
        {
            m_ButtonListNoTeam.gameObject.SetActive(true);
            m_ButtonListHasTeam.gameObject.SetActive(false);

            m_FoundryButton.isEnabled = false;
            m_CDText.gameObject.SetActive(false);
            m_ProgressText.gameObject.SetActive(false);
            SetLevelIndicatorCD(0);
            ResetMatchTweener();

            for (int i = 0; i < m_LevelIndicators.Length; ++i)
            {
                m_LevelIndicators[i].gameObject.SetActive(i == 0);
                if (i == 0)
                {
                    var anim = m_LevelIndicators[i].GetComponent<Animation>();
                    anim.Stop();
                    anim.GetClip(InClipInName).SampleAnimation(anim.gameObject, anim.GetClip(InClipInName).length);
                }
            }

            for (int i = 0; i < m_Members.Length; ++i)
            {
                m_Members[i].Root.gameObject.SetActive(false);
            }

            for (int i = 0; i < m_ClaimRewardButtons.Length; ++i)
            {
                m_ClaimRewardButtons[i].gameObject.SetActive(false);
            }
        }

        private void ResetHasTeamDisplay()
        {
            m_ButtonListNoTeam.gameObject.SetActive(false);
            m_ButtonListHasTeam.gameObject.SetActive(true);

            m_FoundryButton.isEnabled = false;
            m_CDText.gameObject.SetActive(true);
            m_CDText.text = string.Empty;
            m_ProgressText.gameObject.SetActive(m_CDMaskDoorStatus == CDMaskDoorStatus.Closing || m_CDMaskDoorStatus == CDMaskDoorStatus.Closed);

            int bgLevel = RewardLevel >= 0 ? RewardLevel : m_SrcData.Progress.CurrentLevel;

            for (int i = 0; i < m_LevelIndicators.Length; ++i)
            {
                m_LevelIndicators[i].gameObject.SetActive(i == bgLevel);
            }

            if (bgLevel == m_LevelIndicators.Length)
            {
                m_LevelIndicators[m_LevelIndicators.Length - 1].gameObject.SetActive(true);
            }

            RefreshPlayers();
            for (int i = 0; i < m_Members.Length; ++i)
            {
                m_Members[i].Root.gameObject.SetActive(true);
                m_Members[i].Animation.enabled = false;
            }
            RefreshKickButtons();

            for (int i = 0; i < m_ClaimRewardButtons.Length; ++i)
            {
                m_ClaimRewardButtons[i].gameObject.SetActive(false);
            }

            RefreshProgress();
        }

        private void OpenCDMaskDoor(bool animated)
        {
            for (int i = 0; i < m_CDMaskDoorAnims.Length; ++i)
            {
                var anim = m_CDMaskDoorAnims[i];
                if (!animated)
                {
                    m_HasOpenedDoor = true;
                    m_EffectFireOnKey = m_EffectsController.ShowEffect(EffectFireOn);
                    anim.Stop();
                    anim.clip.SampleAnimation(anim.gameObject, 0f);
                }
                else
                {
                    anim[anim.clip.name].normalizedTime = 1f;
                    anim[anim.clip.name].speed = -1f;
                    anim.Play();
                }
            }
            m_FireAnimation.transform.parent.gameObject.SetActive(true);
            m_FireAnimation.GetComponent<UIWidget>().alpha = 1.0f;
            m_FireAnimation[m_FireAnimation.clip.name].normalizedTime = 0f;
            m_FireAnimation[m_FireAnimation.clip.name].speed = 1f;
            m_FireAnimation.Play(m_FireAnimation.clip.name);
            m_ClickNote.SetActive(true);
            m_ProgressText.gameObject.SetActive(false);
            m_CDMaskDoorStatus = animated ? CDMaskDoorStatus.Opening : CDMaskDoorStatus.Open;
            if (m_EffectFireBlock != 0)
            {
                m_EffectsController.DestroyEffect(m_EffectFireBlock);
                m_EffectFireBlock = -1;
            }
        }

        private void CloseCDMaskDoor(bool animated)
        {
            for (int i = 0; i < m_CDMaskDoorAnims.Length; ++i)
            {
                var anim = m_CDMaskDoorAnims[i];
                if (!animated)
                {
                    m_HasOpenedDoor = false;
                    if (m_EffectFireBlock <= 0)
                    {
                        m_EffectFireBlock = m_EffectsController.ShowEffect(EffectFireBlock);
                    }
                    anim.Stop();
                    anim.clip.SampleAnimation(anim.gameObject, 1f);
                }
                else
                {
                    anim[anim.clip.name].normalizedTime = 0f;
                    anim[anim.clip.name].speed = 1f;
                    anim.Play();
                }
            }
            m_ClickNote.SetActive(false);
            m_ProgressText.gameObject.SetActive(true);
            m_CDMaskDoorStatus = animated ? CDMaskDoorStatus.Closing : CDMaskDoorStatus.Closed;
            if (m_EffectFireOnKey != 0)
            {
                m_EffectsController.DestroyEffect(m_EffectFireOnKey);
            }
        }

        private bool IsCDMaskDoorAnimating
        {
            get
            {
                for (int i = 0; i < m_CDMaskDoorAnims.Length; ++i)
                {
                    var anim = m_CDMaskDoorAnims[i];
                    if (anim.isPlaying)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private void UpdateCDMaskDoorState()
        {
            if (IsCDMaskDoorAnimating)
            {
                return;
            }

            if (m_CDMaskDoorStatus == CDMaskDoorStatus.Opening)
            {
                m_CDMaskDoorStatus = CDMaskDoorStatus.Open;
                m_EffectFireOnKey = m_EffectsController.ShowEffect(EffectFireOn);
            }
            else if (m_CDMaskDoorStatus == CDMaskDoorStatus.Closing)
            {
                m_CDMaskDoorStatus = CDMaskDoorStatus.Closed;
                if (m_EffectFireBlock <= 0)
                {
                    m_EffectFireBlock = m_EffectsController.ShowEffect(EffectFireBlock);
                }
            }
        }
    }
}
