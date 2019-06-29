using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 出场状态。
        /// </summary>
        private class EnteringState : StateBase
        {
            private float m_EnteringAnimLength = 0f;

            private const float StandingDuration = 0f;
            private bool m_HasCrossFadeToStanding = false;
            private const string EnteringAnimationName = "Entering1";
            private const float Entering1Duration = 0.5f;

            public override StateForImpactCalc StateForImpactCalc
            {
                get
                {
                    return StateForImpactCalc.Normal;
                }
            }

            public override CharacterMotionStateCategory StateForCharacterMotion
            {
                get
                {
                    return CharacterMotionStateCategory.Ground;
                }
            }

            public override bool DontTurnOnHit
            {
                get
                {
                    return false;
                }
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                base.OnEnter(fsm);
                fsm.Owner.Owner.ImpactCollider.enabled = false;
                PrepareAndPlayAnimation(fsm);
                PlayCameraShakingIfNeeded(fsm);
                m_HasCrossFadeToStanding = false;
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                fsm.Owner.Owner.ImpactCollider.enabled = true;
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (!m_HasCrossFadeToStanding && fsm.CurrentStateTime > m_EnteringAnimLength - Constant.DefaultAnimCrossFadeDuration)
                {
                    m_HasCrossFadeToStanding = true;
                    ChangeState<StandingState>(fsm);
                }
            }

            private void PrepareAndPlayAnimation(IFsm<CharacterMotion> fsm)
            {
                var data = fsm.Owner.Owner.Data as NpcCharacterData;
                if (data == null)
                {
                    m_EnteringAnimLength = fsm.Owner.Owner.PlayAnimation(EnteringAnimationName, false, true);
                }
                else
                {
                    m_EnteringAnimLength = fsm.Owner.Owner.PlayAnimation(data.Entity.BornEnteringAnimation, false, true);
                    if (data.Entity.BornEnteringAnimation == EnteringAnimationName)
                    {
                        m_EnteringAnimLength = Entering1Duration;
                    }
                }
            }

            private void PlayCameraShakingIfNeeded(IFsm<CharacterMotion> fsm)
            {
                var index = fsm.Owner.Owner.Data.CameraShakingIndexOnEnter;
                if (index >= 0 && GameEntry.IsAvailable)
                {
                    GameEntry.CameraShaking.PerformShaking(index);
                }
            }
        }
    }
}
