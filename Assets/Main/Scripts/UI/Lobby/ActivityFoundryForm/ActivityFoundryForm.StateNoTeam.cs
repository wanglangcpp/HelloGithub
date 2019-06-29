using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        /// <summary>
        /// 无队伍的状态。
        /// </summary>
        private class StateNoTeam : StateBase
        {
            protected override void OnEnter(IFsm<ActivityFoundryForm> fsm)
            {
                base.OnEnter(fsm);
                GameEntry.Event.Subscribe(EventId.GearFoundryTeamCreated, OnTeamCreated);
                GameEntry.Event.Subscribe(EventId.GearFoundryTeamJoined, OnTeamJoined);
                GameEntry.Event.Subscribe(EventId.GearFoundryTeamMatchingFailed, OnTeamMatchingFailed);

                var owner = fsm.Owner;
                owner.ResetNoTeamDisplay();
                owner.m_ButtonListNoTeam.gameObject.GetComponent<Animation>().clip.SampleAnimation(owner.m_ButtonListNoTeam.gameObject, 1f);
            }

            protected override void OnLeave(IFsm<ActivityFoundryForm> fsm, bool isShutdown)
            {
                if (!GameEntry.IsAvailable) return;
                GameEntry.Event.Unsubscribe(EventId.GearFoundryTeamCreated, OnTeamCreated);
                GameEntry.Event.Unsubscribe(EventId.GearFoundryTeamJoined, OnTeamJoined);
                GameEntry.Event.Unsubscribe(EventId.GearFoundryTeamMatchingFailed, OnTeamMatchingFailed);
                base.OnLeave(fsm, isShutdown);
            }

            public override void OnClickCreateTeam(IFsm<ActivityFoundryForm> fsm)
            {
                GameEntry.LobbyLogic.GearFoundryCreateTeam();
            }

            public override void OnClickMatch(IFsm<ActivityFoundryForm> fsm, int level)
            {
                GameEntry.LobbyLogic.GearFoundryMatchTeam(level);
            }

            public override void OnClickRequests(IFsm<ActivityFoundryForm> fsm)
            {
                GameEntry.UI.OpenUIForm(UIFormId.RequestListForm);
            }

            private void OnTeamMatchingFailed(object sender, GameEventArgs e)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_FOUNDRY_TEAM_MATCH_FAIL"),
                    ConfirmText = GameEntry.Localization.GetString("UI_BUTTON_CREATETEAM"),
                    OnClickConfirm = o => { GameEntry.LobbyLogic.GearFoundryCreateTeam(); },
                });
            }

            private void OnTeamCreated(object o, GameEventArgs e)
            {
                ChangeState<StateNoTeamToHasTeam>(m_CachedFsm);
            }

            private void OnTeamJoined(object sender, GameEventArgs e)
            {
                ChangeState<StateNoTeamToHasTeam>(m_CachedFsm);
            }
        }

    }
}
