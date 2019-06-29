using System;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        private abstract class StateBase : FsmState<CharacterMotion>
        {
            // 在特定状态下用来以非突变地方式同步角色位置。
            protected Vector2 m_DisplacementToUpdate;

            public abstract StateForImpactCalc StateForImpactCalc
            {
                get;
            }

            public abstract CharacterMotionStateCategory StateForCharacterMotion
            {
                get;
            }

            public abstract bool DontTurnOnHit
            {
                get;
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                //if (fsm.Owner.Owner is NpcCharacter) Log.Info("[{0} OnEnter]", GetType().Name);
                if (fsm.Owner.OnStateChanged != null)
                {
                    fsm.Owner.OnStateChanged();
                }

                if (fsm.Owner.Owner.TransformToUpdate == null)
                {
                    m_DisplacementToUpdate = Vector2.zero;
                }
                else
                {
                    m_DisplacementToUpdate = (fsm.Owner.Owner.TransformToUpdate.Position - fsm.Owner.Owner.CachedTransform.localPosition).ToVector2();
                }

                fsm.Owner.Owner.Data.SetStateForImpactCalc(StateForImpactCalc);
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                m_DisplacementToUpdate = Vector2.zero;
                //if (fsm.Owner.Owner is NpcCharacter) Log.Info("[{0} OnLeave] isShutdown = {1}.", GetType().Name, isShutdown.ToString());
            }

            public virtual bool StartMove(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool StopMove(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool StartLingering(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool StopLingering(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual void PerformSkill(IFsm<CharacterMotion> fsm, bool forcePerform)
            {
                if (forcePerform)
                {
                    ChangeState<SkillState>(fsm);
                }
                else
                {
                    fsm.Owner.CallNextPerformSkillOperationFailure();
                }
            }

            public virtual bool PerformHPDamage(IFsm<CharacterMotion> fsm)
            {
                var character = fsm.Owner.Owner;
                if (character.IsDead)
                {
                    return false;
                }
                if (character.Data.HP <= 0)
                {
                    var buffData = character.GetBuffByType(BuffType.FakeDeath);
                    if (buffData != null)
                    {
                        fsm.Owner.FakeDeathRecoverHPRatio = buffData.Params[0];
                        ChangeState<FakeDeadState>(fsm);
                    }
                    else
                    {
                        character.TryGoDie();
                    }
                }

                return true;
            }

            public virtual bool PerformGoDie(IFsm<CharacterMotion> fsm)
            {
                ChangeState<DeadState>(fsm);
                return true;
            }

            public virtual bool PerformStiffness(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool PerformHardHit(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool PerformFloat(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool PerformBlownAway(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool PerformStun(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool PerformFreeze(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool BreakSkills(IFsm<CharacterMotion> fsm)
            {
                return true;
            }
            public virtual bool BreakCurrentSkill(IFsm<CharacterMotion> fsm, SkillEndReasonType reason)
            {
                return true;
            }

            public virtual bool DeadKeepTimeIsReached(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool PlayEntering(IFsm<CharacterMotion> fsm)
            {
                return false;
            }

            public virtual bool TryFastForwardSkill(IFsm<CharacterMotion> fsm, int skillId, float targetTime)
            {
                return false;
            }

            public virtual bool TryUpdateSkillRushing(IFsm<CharacterMotion> fsm, int skillId, Vector2 position, float rotation)
            {
                return false;
            }

            protected string GetImpactAnimationAliasName(ImpactAnimationType animType)
            {
                switch (animType)
                {
                    case ImpactAnimationType.HitAnimation:
                        return "Hit";
                    case ImpactAnimationType.HitStrongAnimation:
                        return "HitStrong";
                    case ImpactAnimationType.StandingAnimation:
                        return "Stand";
                    case ImpactAnimationType.RotateAnimation:
                        return "Rotate";
                    case ImpactAnimationType.HitAirAnimation:
                        return "HitAir";
                    case ImpactAnimationType.HitAirAnimationStrong:
                        return "HitAirStrong";
                    case ImpactAnimationType.FloatAnimation:
                        return "Float";
                    case ImpactAnimationType.FallingAnimation:
                        return "Falling";
                    case ImpactAnimationType.BlownAwayAnimation:
                        return "BlownAway";
                    case ImpactAnimationType.BlownAwayFallingAnimtion:
                        return "BlownAwayFalling";
                    default:
                        return "Stand";
                }
            }

            public virtual void UpdateGradualPosSync(IFsm<CharacterMotion> fsm, float duration, float elaspeSeconds)
            {

            }

            public virtual void Revive(IFsm<CharacterMotion> fsm)
            {
                Log.Warning("Cannot revive in state '{0}'.", GetType().ToString());
            }
        }
    }
}
