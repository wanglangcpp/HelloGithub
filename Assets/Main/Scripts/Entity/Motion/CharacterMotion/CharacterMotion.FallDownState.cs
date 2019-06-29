using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 倒地状态。
        /// </summary>
        private class FallDownState : StateBase
        {
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

                fsm.Owner.Owner.PlayAnimation("FallDown");
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (fsm.CurrentStateTime >= fsm.Owner.Owner.Motion.FallDownTime)
                {
                    ChangeState<StandUpState>(fsm);
                }
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
