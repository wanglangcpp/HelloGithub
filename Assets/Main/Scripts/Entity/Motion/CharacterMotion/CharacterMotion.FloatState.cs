using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 浮空状态。
        /// </summary>
        private class FloatState : StateBase
        {
            public override StateForImpactCalc StateForImpactCalc
            {
                get
                {
                    return StateForImpactCalc.Floating;
                }
            }

            public override CharacterMotionStateCategory StateForCharacterMotion
            {
                get
                {
                    return CharacterMotionStateCategory.Air;
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

                fsm.Owner.Owner.PlayAnimation(GetImpactAnimationAliasName(fsm.Owner.StiffnessImpactAnimType), true);
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (fsm.CurrentStateTime < fsm.Owner.FloatingTime)
                {
                    var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
                    var displacementUpdateVelocity = instanceLogic.GetPosSyncVelocity(m_DisplacementToUpdate.ToVector3(), fsm.Owner.FloatingTime);

                    fsm.Owner.Owner.NavAgent.nextPosition = fsm.Owner.Owner.CachedTransform.localPosition
                        + fsm.Owner.Owner.Motion.FloatingSpeed * elapseSeconds
                        + displacementUpdateVelocity * elapseSeconds;
                }
                else
                {
                    ChangeState<FloatFallingState>(fsm);
                }
            }

            public override bool PerformStiffness(IFsm<CharacterMotion> fsm)
            {
                ChangeState<AirStiffnessState>(fsm);

                return true;
            }

            public override bool PerformHardHit(IFsm<CharacterMotion> fsm)
            {
                ChangeState<FloatFallingState>(fsm);

                return true;
            }

            public override bool PerformBlownAway(IFsm<CharacterMotion> fsm)
            {
                ChangeState<FloatFallingState>(fsm);

                return true;
            }

            public override bool PerformFloat(IFsm<CharacterMotion> fsm)
            {
                ChangeState<AirStiffnessState>(fsm);
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
