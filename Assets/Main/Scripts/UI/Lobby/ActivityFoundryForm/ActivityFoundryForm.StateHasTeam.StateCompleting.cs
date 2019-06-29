using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class ActivityFoundryForm
    {
        private partial class StateHasTeam
        {
            /// <summary>
            /// 通关状态。（播放通关动画等）
            /// </summary>
            private class StateCompleting : StateBase
            {
                private const float MaxDuration = 5f;

                private const string InwardClipName = "CleanranceIn";
                private const string OutwardClipName = "CleanranceOut";

                private bool m_HasStartedFadingOut = false;

                protected override void OnEnter(IFsm<StateHasTeam> fsm)
                {
                    base.OnEnter(fsm);
                    m_HasStartedFadingOut = false;
                    m_OuterFsm.Owner.CloseCDMaskDoor(false);
                    m_OuterFsm.Owner.m_CompletingAnim.gameObject.SetActive(true);
                    FadeIn(fsm);

                }

                protected override void OnLeave(IFsm<StateHasTeam> fsm, bool isShutdown)
                {
                    m_OuterFsm.Owner.m_CompletingAnim.gameObject.SetActive(false);
                    base.OnLeave(fsm, isShutdown);
                }

                protected override void OnUpdate(IFsm<StateHasTeam> fsm, float elapseSeconds, float realElapseSeconds)
                {
                    base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                    if (!m_HasStartedFadingOut && fsm.CurrentStateTime > MaxDuration)
                    {
                        FadeOut(fsm);
                    }

                    if (m_HasStartedFadingOut && !m_OuterFsm.Owner.m_CompletingAnim.isPlaying)
                    {
                        CheckStateChange(fsm);
                    }
                }

                public override void OnClickCompletingAnimMask(IFsm<StateHasTeam> fsm)
                {
                    if (m_OuterFsm.Owner.m_CompletingAnim.isPlaying)
                    {
                        return;
                    }

                    FadeOut(fsm);
                }

                private void CheckStateChange(IFsm<StateHasTeam> fsm)
                {
                    ChangeState<StateCompleted>(fsm);
                }

                private void FadeIn(IFsm<StateHasTeam> fsm)
                {
                    m_OuterFsm.Owner.m_CompletingAnim.Rewind(InwardClipName);
                    m_OuterFsm.Owner.m_CompletingAnim.Play(InwardClipName);
                }

                private void FadeOut(IFsm<StateHasTeam> fsm)
                {
                    m_HasStartedFadingOut = true;
                    m_OuterFsm.Owner.m_CompletingAnim.Rewind(OutwardClipName);
                    m_OuterFsm.Owner.m_CompletingAnim.Play(OutwardClipName);
                }
            }
        }
    }
}
