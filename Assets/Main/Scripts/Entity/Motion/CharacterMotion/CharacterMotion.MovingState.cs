using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 移动状态。
        /// </summary>
        private class MovingState : StateBase
        {
            private bool justEntered = false;

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

            protected override void OnInit(IFsm<CharacterMotion> fsm)
            {
                base.OnInit(fsm);
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                base.OnEnter(fsm);

                fsm.Owner.Owner.PlayAnimation("Run");
                fsm.Owner.Owner.NavAgent.Resume();
                fsm.Owner.Owner.NavAgent.destination = fsm.Owner.MoveTargetPosition;

                justEntered = true;
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);

                justEntered = false;

                if (fsm.Owner.Owner.NavAgent.isActiveAndEnabled)
                {
                    fsm.Owner.Owner.NavAgent.Stop();
                }

                if (fsm.Owner.OnMovingEnd != null)
                {
                    fsm.Owner.OnMovingEnd();
                }
            }

            private bool ShouldChangeToStandingState(IFsm<CharacterMotion> fsm)
            {
                if (justEntered)
                {
                    return false;
                }

                var agent = fsm.Owner.Owner.NavAgent;

                if (agent.pathPending)
                {
                    return false;
                }

                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    return false;
                }

                return !agent.hasPath || agent.velocity.sqrMagnitude < 0.0001f;
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (ShouldChangeToStandingState(fsm))
                {
                    justEntered = false;
                    ChangeState<StandingState>(fsm);
                    return;
                }

                if (!justEntered)
                {
                    if (fsm.Owner.OnMovingUpdate != null)
                    {
                        fsm.Owner.OnMovingUpdate();
                    }
                }
                else
                {
                    justEntered = false;
                }
            }

            public override bool StartMove(IFsm<CharacterMotion> fsm)
            {
                fsm.Owner.Owner.NavAgent.destination = fsm.Owner.MoveTargetPosition;
                return true;
            }

            public override bool StopMove(IFsm<CharacterMotion> fsm)
            {
                ChangeState<StandingState>(fsm);

                return true;
            }

            public override void PerformSkill(IFsm<CharacterMotion> fsm, bool forcePerform)
            {
                ChangeState<SkillState>(fsm);
            }

            public override bool PerformStiffness(IFsm<CharacterMotion> fsm)
            {
                ChangeState<StiffnessState>(fsm);

                return true;
            }

            public override bool PerformHardHit(IFsm<CharacterMotion> fsm)
            {
                ChangeState<StiffnessState>(fsm);

                return true;
            }

            public override bool PerformBlownAway(IFsm<CharacterMotion> fsm)
            {
                ChangeState<FloatState>(fsm);

                return true;
            }

            public override bool PerformFloat(IFsm<CharacterMotion> fsm)
            {
                ChangeState<FloatState>(fsm);

                return true;
            }

            public override bool PerformStun(IFsm<CharacterMotion> fsm)
            {
                ChangeState<StunState>(fsm);
                return true;
            }

            public override bool PerformFreeze(IFsm<CharacterMotion> fsm)
            {
                ChangeState<FreezeState>(fsm);
                return true;
            }
        }
    }
}
