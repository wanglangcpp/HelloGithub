using GameFramework.Event;
using GameFramework.Fsm;
using System;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        private partial class StateHasTeam
        {
            /// <summary>
            /// 冷却状态。
            /// </summary>
            private class StateCD : StateBase
            {
                protected override void OnEnter(IFsm<StateHasTeam> fsm)
                {
                    base.OnEnter(fsm);

                    if (m_OuterFsm.Owner.m_CDMaskDoorStatus != ActivityFoundryForm.CDMaskDoorStatus.Closed && m_OuterFsm.Owner.m_CDMaskDoorStatus != ActivityFoundryForm.CDMaskDoorStatus.Closing)
                    {
                        m_OuterFsm.Owner.CloseCDMaskDoor(true);
                    }
                }

                protected override void OnUpdate(IFsm<StateHasTeam> fsm, float elapseSeconds, float realElapseSeconds)
                {
                    base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                    if (!m_OuterFsm.Owner.IsCoolingDown)
                    {
                        ChangeState<StateFoundry>(fsm);
                        return;
                    }

                    if (!m_OuterFsm.Owner.IsCDMaskDoorAnimating)
                    {
                        RefreshCDText();
                    }
                }

                protected override void OnLeave(IFsm<StateHasTeam> fsm, bool isShutdown)
                {
                    ClearCDText();
                    base.OnLeave(fsm, isShutdown);
                }

                public override void OnPerformedFoundry(object sender, GameEventArgs e)
                {
                    base.OnPerformedFoundry(sender, e);

                    if (m_OuterFsm.Owner.IsComplete)
                    {
                        ChangeState<StateCompleting>(m_CachedFsm);
                    }
                    else if (m_OuterFsm.Owner.RewardLevel >= 0)
                    {
                        ChangeState<StateReward>(m_CachedFsm);
                    }
                    else if (!m_OuterFsm.Owner.IsCoolingDown)
                    {
                        ChangeState<StateFoundry>(m_CachedFsm);
                    }
                }

                private void RefreshCDText()
                {
                    TimeSpan coolDownTime = m_OuterFsm.Owner.m_SrcData.NextFoundryTime - GameEntry.Time.LobbyServerUtcTime;
                    if (coolDownTime >= TimeSpan.Zero)
                    {
                        m_OuterFsm.Owner.m_CDText.text = GameEntry.Localization.GetString("UI_TEXT_TIMENUMBER", coolDownTime.Minutes, coolDownTime.Seconds);
                    }
                    else
                    {
                        ClearCDText();
                    }
                }

                private void ClearCDText()
                {
                    m_OuterFsm.Owner.m_CDText.text = string.Empty;
                }
            }

        }
    }
}
