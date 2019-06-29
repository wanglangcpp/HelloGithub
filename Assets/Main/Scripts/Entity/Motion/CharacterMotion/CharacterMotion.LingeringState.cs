using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        private class LingeringState : StateBase
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

            private const float FlatCorner = 180.0f;
            private float m_StartSpeed = 0.0f;
            private float m_StartStopingDistance = 0.0f;

            protected override void OnInit(IFsm<CharacterMotion> fsm)
            {
                base.OnInit(fsm);
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                base.OnEnter(fsm);
                var hasLeft = HasLingeringDirLeft(fsm);
                fsm.Owner.Owner.PlayAnimation(hasLeft ? "LingeringLeft" : "LingeringRight");
                fsm.Owner.Owner.NavAgent.Resume();
                fsm.Owner.Owner.NavAgent.destination = fsm.Owner.m_LingeringParams.TargetPosition;
                m_StartSpeed = fsm.Owner.Owner.NavAgent.speed;
                fsm.Owner.Owner.NavAgent.speed *= fsm.Owner.m_LingeringParams.SpeedFactor;
                m_StartStopingDistance = fsm.Owner.Owner.NavAgent.stoppingDistance;
                fsm.Owner.Owner.NavAgent.stoppingDistance = 0;
            }

            public bool HasLingeringDirLeft(IFsm<CharacterMotion> fsm)
            {
                bool hasLingerLeft = false;
                ICanHaveTarget canHaveTargetSelf = fsm.Owner.Owner as ICanHaveTarget;
                var targetObj = canHaveTargetSelf.Target as TargetableObject;
                var targetDir = targetObj.transform.position - fsm.Owner.m_LingeringParams.TargetPosition;

                float angleZ = Vector3.Angle(Vector3.forward, targetDir);
                float angleX = Vector3.Angle(Vector3.right, targetDir);

                if ((angleZ <= FlatCorner / 2) && (angleX <= FlatCorner / 2))
                {
                    hasLingerLeft = false;
                }
                else if ((angleZ <= FlatCorner / 2) && (angleX > FlatCorner / 2))
                {
                    hasLingerLeft = true;
                }
                else if ((angleZ > FlatCorner / 2) && (angleX <= FlatCorner / 2))
                {
                    hasLingerLeft = true;
                }
                else if ((angleZ > FlatCorner / 2) && (angleX > FlatCorner / 2))
                {
                    hasLingerLeft = false;
                }
                return hasLingerLeft;
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
                if (!fsm.Owner.Owner.NavAgent.isActiveAndEnabled)
                {
                    return;
                }
                fsm.Owner.Owner.NavAgent.speed = m_StartSpeed;
                fsm.Owner.Owner.NavAgent.Stop();
                fsm.Owner.Owner.NavAgent.stoppingDistance = m_StartStopingDistance;
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                var transform = fsm.Owner.Owner.CachedTransform;
                if (ShouldChangeToStandingState(fsm, transform))
                {
                    ChangeState<StandingState>(fsm);
                    return;
                }
            }

            private bool ShouldChangeToStandingState(IFsm<CharacterMotion> fsm, Transform transform)
            {
                var agent = fsm.Owner.Owner.NavAgent;
                ICanHaveTarget canHaveTargetSelf = fsm.Owner.Owner as ICanHaveTarget;

                // 目标丢失。
                if (!canHaveTargetSelf.HasTarget)
                {
                    return true;
                }

                var targetObj = canHaveTargetSelf.Target as TargetableObject;
                var character = fsm.Owner.Owner as Character;
                float distance = AIUtility.GetAttackDistance(targetObj, canHaveTargetSelf.Target);

                // 距离过远或角度过大。
                if (distance > fsm.Owner.m_LingeringParams.MaxDistance || AIUtility.GetFaceAngle(character, targetObj) > fsm.Owner.m_LingeringParams.AngleScope)
                {
                    return true;
                }

                if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                {
                    return false;
                }

                return !agent.hasPath || agent.velocity.sqrMagnitude < 0.0001f;
            }

            public override bool StartLingering(IFsm<CharacterMotion> fsm)
            {
                fsm.Owner.Owner.NavAgent.destination = fsm.Owner.m_LingeringParams.TargetPosition;

                return true;
            }

            public override bool StopLingering(IFsm<CharacterMotion> fsm)
            {
                ChangeState<StandingState>(fsm);

                return true;
            }

            public override bool StartMove(IFsm<CharacterMotion> fsm)
            {
                ChangeState<MovingState>(fsm);

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
