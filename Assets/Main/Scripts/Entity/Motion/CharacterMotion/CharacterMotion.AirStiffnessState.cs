using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 空中硬直状态。
        /// </summary>
        private class AirStiffnessState : StateBase
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

                float time = Time.time;

                float repulseStartTime = fsm.Owner.Owner.Motion.StiffnessStartTime + fsm.Owner.Owner.Motion.RepulseStartTime;
                float repulseDuration = fsm.Owner.Owner.Motion.RepulseTime;
                if (repulseDuration > 0f && time >= repulseStartTime && time < repulseStartTime + repulseDuration)
                {
                    var instanceLogic = GameEntry.SceneLogic.BaseInstanceLogic;
                    var displacementUpdateVelocity = instanceLogic.GetPosSyncVelocity(m_DisplacementToUpdate.ToVector3(), repulseDuration);

                    fsm.Owner.Owner.NavAgent.nextPosition = fsm.Owner.Owner.CachedTransform.localPosition
                        + fsm.Owner.Owner.Motion.RepulseSpeed * elapseSeconds
                        + displacementUpdateVelocity * elapseSeconds;
                }

                if (time >= fsm.Owner.Owner.Motion.StiffnessStartTime + fsm.Owner.Owner.Motion.StiffnessTime)
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
