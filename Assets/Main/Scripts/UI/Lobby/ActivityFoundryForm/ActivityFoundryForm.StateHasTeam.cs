using GameFramework.Event;
using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        /// <summary>
        /// 有队伍的状态。
        /// </summary>
        private partial class StateHasTeam : StateBase
        {
            private IFsm<StateHasTeam> m_InnerFsm = null;

            protected override void OnEnter(IFsm<ActivityFoundryForm> fsm)
            {
                base.OnEnter(fsm);

                GameEntry.Event.Subscribe(EventId.GearFoundryTeamPlayersChanged, OnTeamPlayersChanged);
                GameEntry.Event.Subscribe(EventId.GearFoundryLeftTeam, OnLeftTeam);
                GameEntry.Event.Subscribe(EventId.GearFoundryKickedFromTeam, OnKickedFromTeam);
                GameEntry.Event.Subscribe(EventId.GearFoundryPerformed, OnPerformedFoundry);
                GameEntry.Event.Subscribe(EventId.GearFoundryRewardClaimed, OnRewardClaimed);
                GameEntry.Event.Subscribe(EventId.GearFoundryReset, OnForceReset);

                fsm.Owner.ResetHasTeamDisplay();
                fsm.Owner.RefreshPlayers();
                fsm.Owner.RefreshKickButtons();

                m_InnerFsm = GameEntry.Fsm.CreateFsm(this,
                    new StateInit(),
                    new StateFoundry(),
                    new StateCD(),
                    new StateReward(),
                    new StateLevelUp(),
                    new StateCompleting(),
                    new StateCompleted());
                m_InnerFsm.Start<StateInit>();
            }

            protected override void OnLeave(IFsm<ActivityFoundryForm> fsm, bool isShutdown)
            {
                if (!GameEntry.IsAvailable)
                {
                    base.OnLeave(fsm, isShutdown);
                }

                GameEntry.Event.Unsubscribe(EventId.GearFoundryTeamPlayersChanged, OnTeamPlayersChanged);
                GameEntry.Event.Unsubscribe(EventId.GearFoundryLeftTeam, OnLeftTeam);
                GameEntry.Event.Unsubscribe(EventId.GearFoundryKickedFromTeam, OnKickedFromTeam);
                GameEntry.Event.Unsubscribe(EventId.GearFoundryPerformed, OnPerformedFoundry);
                GameEntry.Event.Unsubscribe(EventId.GearFoundryRewardClaimed, OnRewardClaimed);
                GameEntry.Event.Unsubscribe(EventId.GearFoundryReset, OnForceReset);

                fsm.Owner.m_CompletingAnim.gameObject.SetActive(false);

                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<ActivityFoundryForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            }

            public override void OnClickInvite(IFsm<ActivityFoundryForm> fsm)
            {
                if (GameEntry.Data.Friends.Data.Count <= 0)
                {
                    GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_NO_FRIENDS") });
                    return;
                }

                fsm.Owner.ShowFriends();
            }

            public override void OnClickFoundry(IFsm<ActivityFoundryForm> fsm)
            {
                GameEntry.LobbyLogic.GearFoundryPerform();
            }

            public override void OnClickSendLink(IFsm<ActivityFoundryForm> fsm)
            {
                // TODO: 相关功能未实现。
            }

            public override void OnClickClaimReward(IFsm<ActivityFoundryForm> fsm)
            {
                GameEntry.LobbyLogic.GearFoundryClaimReward(fsm.Owner.RewardLevel);
            }

            public override void OnClickLeave(IFsm<ActivityFoundryForm> fsm)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_CONFIRM_LEAVE_TEAM"),
                    OnClickConfirm = o => { GameEntry.LobbyLogic.GearFoundryLeaveTeam(); }
                });
            }

            public override void OnKickButton(IFsm<ActivityFoundryForm> fsm, int playerIndex)
            {
                var playerData = fsm.Owner.m_SrcData.Players.Data[playerIndex];
                var playerId = playerData.Id;
                var playerName = playerData.Name;

                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_FOUNDRY_KICK_MEMBER", playerName),
                    OnClickConfirm = o => { GameEntry.LobbyLogic.GearFoundryKickPlayer(playerId); },
                });
            }

            public override void OnClickCompletingAnimMask(IFsm<ActivityFoundryForm> fsm)
            {
                (m_InnerFsm.CurrentState as StateHasTeam.StateBase).OnClickCompletingAnimMask(m_InnerFsm);
            }

            private void OnTeamPlayersChanged(object sender, GameEventArgs e)
            {
                m_CachedFsm.Owner.RefreshPlayers();
                m_CachedFsm.Owner.RefreshKickButtons();
            }

            private void OnLeftTeam(object sender, GameEventArgs e)
            {
                DoLeaveTeam();
            }

            private void OnKickedFromTeam(object sender, GameEventArgs e)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_KICKED_FROM_TEAM") });
                DoLeaveTeam();
            }

            private void OnPerformedFoundry(object sender, GameEventArgs e)
            {
                (m_InnerFsm.CurrentState as StateHasTeam.StateBase).OnPerformedFoundry(sender, e);
            }

            private void OnRewardClaimed(object sender, GameEventArgs e)
            {
                (m_InnerFsm.CurrentState as StateHasTeam.StateBase).OnRewardClaimed(sender, e);
            }

            private void OnForceReset(object sender, GameEventArgs e)
            {
                (m_InnerFsm.CurrentState as StateHasTeam.StateBase).OnForceReset(m_InnerFsm);
            }

            private void DoLeaveTeam()
            {
                m_CachedFsm.Owner.HideFriends();
                ChangeState<StateHasTeamToNoTeam>(m_CachedFsm);
            }
        }
    }
}
