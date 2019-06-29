using System;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BuildingMotion
    {
        private abstract class StateBase : FsmState<BuildingMotion>
        {
            protected abstract string AnimClipAlias
            {
                get;
            }

            protected AnimInfo m_AnimInfo = new AnimInfo();

            public DRBuildingAnimation GetAnimationDataRow(IFsm<BuildingMotion> fsm)
            {
                return fsm.Owner.AnimationDataRow;
            }

            public DRBuilding GetBuildingDataRow(IFsm<BuildingMotion> fsm)
            {
                return fsm.Owner.BuildingDataRow;
            }

            protected override void OnInit(IFsm<BuildingMotion> fsm)
            {
                base.OnInit(fsm);

                var dr = GetAnimationDataRow(fsm);
                if (dr == null)
                {
                    return;
                }

                var animInfo = new AnimInfo();
                if (InitAnims(fsm, dr, animInfo, AnimClipAlias))
                {
                    m_AnimInfo = animInfo;
                }
            }

            protected override void OnEnter(IFsm<BuildingMotion> fsm)
            {
                if (fsm.Owner.OnStateChanged != null)
                {
                    fsm.Owner.OnStateChanged();
                }

                base.OnEnter(fsm);
                //Log.Info("[BuildingMotion] {0} OnEnter", GetType().Name);
            }

            protected override void OnUpdate(IFsm<BuildingMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            }

            protected override void OnLeave(IFsm<BuildingMotion> fsm, bool isShutdown)
            {
                //Log.Info("[BuildingMotion] {0} OnLeave", GetType().Name);
                base.OnLeave(fsm, isShutdown);
            }

            public void PlayAliasedAnimation(IFsm<BuildingMotion> fsm, string animClipAlias)
            {
                var animInfo = new AnimInfo();
                if (InitAnims(fsm, GetAnimationDataRow(fsm), animInfo, animClipAlias))
                {
                    PlayAnimation(fsm, animInfo);
                }
            }

            public virtual void PerformSkill(IFsm<BuildingMotion> fsm)
            {
                CallNextPerformSkillOperationFailure(fsm);
            }

            public virtual bool PerformHPDamage(IFsm<BuildingMotion> fsm)
            {
                return false;
            }

            public virtual bool PerformGoDie(IFsm<BuildingMotion> fsm)
            {
                ChangeState<DeadState>(fsm);
                return true;
            }

            protected void CallNextPerformSkillOperationFailure(IFsm<BuildingMotion> fsm)
            {
                fsm.Owner.m_NextPerformSkillOperation.PerformSkillFailure();
                fsm.Owner.m_NextPerformSkillOperation = null;
            }

            protected static void PlayAnimation(IFsm<BuildingMotion> fsm, AnimInfo animInfo, bool crossFade = true)
            {
                if (!string.IsNullOrEmpty(animInfo.AnimClipName))
                {
                    if (crossFade)
                    {
                        fsm.Owner.Owner.CachedAnimation.CrossFade(animInfo.AnimClipName);
                    }
                    else
                    {
                        fsm.Owner.Owner.CachedAnimation.Play(animInfo.AnimClipName);
                    }
                }

                if (!string.IsNullOrEmpty(animInfo.ShooterAnimClipName) && fsm.Owner.Owner.ShooterAnimation != null)
                {
                    fsm.Owner.Owner.ShooterAnimation.Play(animInfo.ShooterAnimClipName);
                }
            }

            protected static void QueueAnimation(IFsm<BuildingMotion> fsm, AnimInfo animInfo)
            {
                if (!string.IsNullOrEmpty(animInfo.AnimClipName))
                {
                    fsm.Owner.Owner.CachedAnimation.CrossFadeQueued(animInfo.AnimClipName);
                }

                if (!string.IsNullOrEmpty(animInfo.ShooterAnimClipName) && fsm.Owner.Owner.ShooterAnimation != null)
                {
                    fsm.Owner.Owner.ShooterAnimation.CrossFadeQueued(animInfo.ShooterAnimClipName);
                }
            }

            private bool InitShooterAnim(IFsm<BuildingMotion> fsm, DRBuildingAnimation dr, AnimInfo animInfo, string animClipAlias)
            {
                var shooterAnimation = fsm.Owner.Owner.ShooterAnimation;
                if (dr == null || shooterAnimation == null)
                {
                    return true;
                }

                animInfo.ShooterAnimClipName = dr.GetShooterAnimationName(animClipAlias);
                if (string.IsNullOrEmpty(animInfo.ShooterAnimClipName))
                {
                    return true;
                }

                if (shooterAnimation[animInfo.ShooterAnimClipName] == null)
                {
                    Log.Error("Animation clip '{0}' not found int the animation component on bullet shooter of '{1}'.", animInfo.ShooterAnimClipName, fsm.Owner.Owner.Name);
                    return false;
                }

                animInfo.ShooterAnimClipLength = shooterAnimation[animInfo.ShooterAnimClipName].length;
                return true;
            }

            private bool InitBaseAnim(IFsm<BuildingMotion> fsm, DRBuildingAnimation dr, AnimInfo animInfo, string animClipAlias)
            {
                if (dr == null)
                {
                    return true;
                }

                animInfo.AnimClipName = dr.GetAnimationName(animClipAlias);
                if (string.IsNullOrEmpty(animInfo.AnimClipName))
                {
                    return true;
                }

                if (fsm.Owner.Owner.CachedAnimation[animInfo.AnimClipName] == null)
                {
                    Log.Error("Animation clip '{0}' not found in the animation component on '{1}'.", animInfo.AnimClipName, fsm.Owner.Owner.Name);
                    return false;
                }

                animInfo.AnimClipLength = fsm.Owner.Owner.CachedAnimation[animInfo.AnimClipName].length;
                return true;
            }

            protected bool InitAnims(IFsm<BuildingMotion> fsm, DRBuildingAnimation dr, AnimInfo animInfo, string animClipAlias)
            {
                return InitBaseAnim(fsm, dr, animInfo, animClipAlias) && InitShooterAnim(fsm, dr, animInfo, animClipAlias);
            }

            protected class AnimInfo
            {
                internal string AnimClipName = string.Empty;
                internal float AnimClipLength = 0f;
                internal string ShooterAnimClipName = string.Empty;
                internal float ShooterAnimClipLength = 0f;

                internal float AnimMaxLength
                {
                    get
                    {
                        return Mathf.Max(AnimClipLength, ShooterAnimClipLength);
                    }
                }

                internal bool IsEmpty
                {
                    get
                    {
                        return !string.IsNullOrEmpty(AnimClipName) && !string.IsNullOrEmpty(ShooterAnimClipName);
                    }
                }
            }
        }
    }
}
