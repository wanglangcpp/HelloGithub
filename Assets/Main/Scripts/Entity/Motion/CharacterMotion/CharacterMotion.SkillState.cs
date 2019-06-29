using GameFramework;
using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        /// <summary>
        /// 技能状态。
        /// </summary>
        private class SkillState : StateBase
        {
            private ITimeLineInstance<Entity> m_TimeLineInstance = null;
            private bool m_CurrentSkillIsInCombo = false;
            private bool m_CurrentSkillIsContinualTap = false;
            private int m_CurrentSkillId = -1;
            private bool m_WasUpdatingRushingLastFrame = false;
            private bool m_ForcePerform = false;

            private RushEnv m_RushEnv = null;

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

            private SkillRushingParams GetRushingParams(IFsm<CharacterMotion> fsm)
            {
                return fsm.Owner.SkillRushingParams;
            }

            public bool CurrentSkillIsInCombo
            {
                get
                {
                    return m_CurrentSkillIsInCombo;
                }
            }

            public override bool TryFastForwardSkill(IFsm<CharacterMotion> fsm, int skillId, float targetTime)
            {
                if (m_TimeLineInstance == null || !m_TimeLineInstance.IsActive || m_TimeLineInstance.Id != skillId && m_TimeLineInstance.CurrentTime >= targetTime)
                {
                    return false;
                }

                HeroCharacter heroCharacter = fsm.Owner.Owner as HeroCharacter;
                if (heroCharacter == null)
                {
                    return false;
                }

                float deltaTime = targetTime - m_TimeLineInstance.CurrentTime;
                m_TimeLineInstance.FastForward(deltaTime, false);
                var userDataDict = m_TimeLineInstance.UserData as Dictionary<string, object>;
                userDataDict[Constant.TimeLineFastForwardTillKey] = targetTime;

                return true;
            }

            public override bool TryUpdateSkillRushing(IFsm<CharacterMotion> fsm, int skillId, Vector2 position, float rotation)
            {
                if (m_TimeLineInstance == null || !m_TimeLineInstance.IsActive || m_TimeLineInstance.Id != skillId)
                {
                    return false;
                }

                if (!fsm.Owner.IsDuringSkillRushing || m_RushEnv == null)
                {
                    return false;
                }

                m_RushEnv.TargetRotation = rotation;

                var entityPosition = fsm.Owner.Owner.CachedTransform.localPosition;
                fsm.Owner.SkillRushingDirInput = (position.ToVector3(entityPosition.y) - entityPosition);

                if (fsm.Owner.SkillRushingDirInput.magnitude < .01f)
                {
                    fsm.Owner.SkillRushingDirInput = Vector3.zero;
                }
                else
                {
                    fsm.Owner.SkillRushingDirInput = fsm.Owner.SkillRushingDirInput.normalized;
                }

                return true;
            }

            protected override void OnEnter(IFsm<CharacterMotion> fsm)
            {
                base.OnEnter(fsm);
                m_RushEnv = null;
                m_ForcePerform = false;
                InternalPerformSkill(fsm, fsm.Owner.PerformSkillId, fsm.Owner.PerformSkillIsInCombo, fsm.Owner.PerformSkillIsContinualTap, fsm.Owner.PerformSkillIndex);
            }

            protected override void OnLeave(IFsm<CharacterMotion> fsm, bool isShutdown)
            {
                base.OnLeave(fsm, isShutdown);
                BreakSkills(fsm);
                m_RushEnv = null;
                fsm.Owner.StopSkillRushing();
                fsm.Owner.m_CurrentPerformSkillOperation = null;

                if (fsm.Owner.m_NextPerformSkillOperation != null)
                {
                    fsm.Owner.CallNextPerformSkillOperationFailure();
                }
            }

            protected override void OnUpdate(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (m_TimeLineInstance == null)
                {
                    return;
                }

                Character character = m_TimeLineInstance.Owner as Character;
                if (character == null || character.Motion == null)
                {
                    Log.Warning(character == null ? "Character is null." : "CharacterMotion is null.");
                    return;
                }

                if (m_TimeLineInstance.IsActive)
                {
                    UpdateSkillRushing(fsm, elapseSeconds, realElapseSeconds);
                    return;
                }

                if (!fsm.Owner.PerformSkillIsInCombo && m_CurrentSkillIsInCombo || m_ForcePerform)
                {
                    m_ForcePerform = false;
                    InternalPerformSkill(fsm, fsm.Owner.PerformSkillId, fsm.Owner.PerformSkillIsInCombo, fsm.Owner.PerformSkillIsContinualTap, fsm.Owner.PerformSkillIndex);
                    return;
                }

                if (character.Motion.ReplaceSkillInfo.NeedReplaceSkill())
                {
                    InternalPerformSkill(fsm, fsm.Owner.PerformSkillId, fsm.Owner.PerformSkillIsInCombo, fsm.Owner.PerformSkillIsContinualTap, fsm.Owner.PerformSkillIndex);
                    return;
                }

                if (character.Motion.ReplaceSkillInfo.NeedRestartSkill())
                {
                    InternalPerformSkill(fsm, fsm.Owner.ReplaceSkillInfo.ReplacedSkillId, fsm.Owner.PerformSkillIsInCombo, fsm.Owner.PerformSkillIsContinualTap, fsm.Owner.PerformSkillIndex);
                    return;
                }

                if (!m_TimeLineInstance.IsBroken)
                {
                    fsm.Owner.CallCurrentPerformSkillOperationEnd(m_CurrentSkillId, SkillEndReasonType.Finish);
                }

                if (fsm.Owner.m_CurrentPerformSkillOperation.IsChargeSkill)
                {
                    // 这里要通知BattleForm当前的蓄力技已经释放完。由于捕获Press和Release两种情况的时候，
                    // TimeLine会改变Button的状态，因此会发生冲突，通知BattleForm就是为了统一管理防止冲突
                    GameEntry.Event.FireNow(this, new PerformChargeSkillEventArgs(PerformChargeSkillEventArgs.OperateType.Hide, int.MinValue));
                }

                m_TimeLineInstance = null;
                ChangeState<StandingState>(fsm);
            }

            public override bool StartMove(IFsm<CharacterMotion> fsm)
            {
                if (!m_TimeLineInstance.IsActive)
                {
                    return false;
                }

                m_TimeLineInstance.FireEvent(this, (int)EntityTimeLineEvent.Move, new EntityTimeLineEventUserData
                {
                    OnBreakTimeLineSuccess = delegate (object userData)
                    {
                        fsm.Owner.CallCurrentPerformSkillOperationEnd(m_CurrentSkillId, SkillEndReasonType.BreakByMove);
                        ChangeState<MovingState>(fsm);
                    },
                });

                return true;
            }

            public override void PerformSkill(IFsm<CharacterMotion> fsm, bool forcePerform)
            {
                if (!fsm.Owner.PerformSkillIsInCombo && m_CurrentSkillIsInCombo)
                {
                    m_ForcePerform = forcePerform;
                    BreakCurrentTimeLine(fsm, false, SkillEndReasonType.BreakBySkill);
                    return;
                }

                if (!m_TimeLineInstance.IsActive)
                {
                    return;
                }

                // 正在释放连续点击技能时再度收到同一连续点击技能的输入。
                if (m_CurrentSkillId == fsm.Owner.PerformSkillId && m_CurrentSkillIsContinualTap && fsm.Owner.PerformSkillIsContinualTap)
                {
                    m_TimeLineInstance.FireEvent(fsm.Owner, (int)EntityTimeLineEvent.ContinualTapSkill);
                    return;
                }

                // 强制释放技能。
                if (forcePerform)
                {
                    m_ForcePerform = true;
                    BreakCurrentTimeLine(fsm, false, SkillEndReasonType.BreakBySkill);
                    return;
                }

                bool breakSuccessful = false;
                m_TimeLineInstance.FireEventNow(fsm.Owner, (int)EntityTimeLineEvent.Skill, new EntityTimeLineEventUserData
                {
                    OnBreakTimeLineSuccess = delegate (object userData) { breakSuccessful = true; },
                });

                if (breakSuccessful)
                {
                    fsm.Owner.CallCurrentPerformSkillOperationEnd(m_CurrentSkillId, SkillEndReasonType.BreakBySkill);
                }

                // 如果是蓄力技能，并且没有处于蓄力状态，那么通知TimeLine，执行相应的技能
                if (fsm.Owner.m_CurrentPerformSkillOperation.IsChargeSkill && fsm.Owner.m_CurrentPerformSkillOperation.IsCharging == false)
                {
                    //这里一定要FireEventNow吗？
                    m_TimeLineInstance.FireEventNow(fsm.Owner, (int)EntityTimeLineEvent.PerformChargeSkill);
                }

                fsm.Owner.Owner.Motion.ReplaceSkillInfo.RecordEnable(fsm.Owner.PerformSkillId);
            }

            public override bool PerformStiffness(IFsm<CharacterMotion> fsm)
            {
                if (m_TimeLineInstance.IsActive)
                {
                    m_TimeLineInstance.FireEvent(this, (int)EntityTimeLineEvent.StateImpact, new EntityTimeLineEventUserData
                    {
                        OnBreakTimeLineSuccess = delegate (object userData)
                        {
                            fsm.Owner.CallCurrentPerformSkillOperationEnd(m_CurrentSkillId, SkillEndReasonType.BreakByImpact);
                            ChangeState<StiffnessState>(fsm);
                        },
                    });
                }

                return true;
            }

            public override bool PerformHardHit(IFsm<CharacterMotion> fsm)
            {
                if (m_TimeLineInstance.IsActive)
                {
                    m_TimeLineInstance.FireEvent(this, (int)EntityTimeLineEvent.StateImpact, new EntityTimeLineEventUserData
                    {
                        OnBreakTimeLineSuccess = delegate (object userData)
                        {
                            fsm.Owner.CallCurrentPerformSkillOperationEnd(m_CurrentSkillId, SkillEndReasonType.BreakByImpact);
                            ChangeState<StiffnessState>(fsm);
                        },
                    });
                }

                return true;
            }

            public override bool PerformBlownAway(IFsm<CharacterMotion> fsm)
            {
                if (m_TimeLineInstance.IsActive)
                {
                    m_TimeLineInstance.FireEvent(this, (int)EntityTimeLineEvent.StateImpact, new EntityTimeLineEventUserData
                    {
                        OnBreakTimeLineSuccess = delegate (object userData)
                        {
                            fsm.Owner.CallCurrentPerformSkillOperationEnd(m_CurrentSkillId, SkillEndReasonType.BreakByImpact);
                            ChangeState<FloatState>(fsm);
                        },
                    });
                }

                return true;
            }

            public override bool PerformFloat(IFsm<CharacterMotion> fsm)
            {
                if (m_TimeLineInstance.IsActive)
                {
                    m_TimeLineInstance.FireEvent(this, (int)EntityTimeLineEvent.StateImpact, new EntityTimeLineEventUserData
                    {
                        OnBreakTimeLineSuccess = delegate (object userData)
                        {
                            fsm.Owner.CallCurrentPerformSkillOperationEnd(m_CurrentSkillId, SkillEndReasonType.BreakByImpact);
                            ChangeState<FloatState>(fsm);
                        },
                    });
                }

                return true;
            }

            public override bool PerformStun(IFsm<CharacterMotion> fsm)
            {
                if (m_TimeLineInstance.IsActive)
                {
                    m_TimeLineInstance.FireEvent(this, (int)EntityTimeLineEvent.StateImpact, new EntityTimeLineEventUserData
                    {
                        OnBreakTimeLineSuccess = delegate (object userData)
                        {
                            fsm.Owner.CallCurrentPerformSkillOperationEnd(m_CurrentSkillId, SkillEndReasonType.BreakByImpact);
                            ChangeState<StunState>(fsm);
                        },
                    });
                }

                return true;
            }

            public override bool PerformFreeze(IFsm<CharacterMotion> fsm)
            {
                if (m_TimeLineInstance.IsActive)
                {
                    m_TimeLineInstance.FireEvent(this, (int)EntityTimeLineEvent.StateImpact, new EntityTimeLineEventUserData
                    {
                        OnBreakTimeLineSuccess = delegate (object userData)
                        {
                            fsm.Owner.CallCurrentPerformSkillOperationEnd(m_CurrentSkillId, SkillEndReasonType.BreakByImpact);
                            ChangeState<FreezeState>(fsm);
                        },
                    });
                }

                return true;
            }

            public override bool BreakSkills(IFsm<CharacterMotion> fsm)
            {
                BreakCurrentTimeLine(fsm, true, SkillEndReasonType.Other);
                m_CurrentSkillId = -1;
                m_CurrentSkillIsContinualTap = false;
                m_CurrentSkillIsInCombo = false;
                return true;
            }

            public override bool BreakCurrentSkill(IFsm<CharacterMotion> fsm, SkillEndReasonType reason)
            {
                BreakCurrentTimeLine(fsm, false, reason);
                return true;
            }

            private void BreakCurrentTimeLine(IFsm<CharacterMotion> fsm, bool instant, SkillEndReasonType reason)
            {
                fsm.Owner.BreakCurrentTimeLine(m_TimeLineInstance, instant, m_CurrentSkillId, reason);
            }

            public override void UpdateGradualPosSync(IFsm<CharacterMotion> fsm, float duration, float elaspeSeconds)
            {
                fsm.Owner.Owner.NavAgent.nextPosition += GameEntry.SceneLogic.BaseInstanceLogic.GetPosSyncVelocity(m_DisplacementToUpdate, duration) * elaspeSeconds;
            }

            private void InternalPerformSkill(IFsm<CharacterMotion> fsm, int skillId, bool isInCombo, bool isContinualTap, int skillIndex)
            {
                fsm.Owner.ReplaceSkillInfo.Reset();
                var userDataDict = new Dictionary<string, object>();
                userDataDict.Add(Constant.TimeLineSkillIndexKey, skillIndex);
                userDataDict.Add(Constant.TimeLineSkillLevelKey, fsm.Owner.Owner.GetSkillLevel(skillIndex));
                m_TimeLineInstance = GameEntry.TimeLine.Entity.CreateTimeLineInstance(fsm.Owner.Owner, skillId, userDataDict);

                if (m_TimeLineInstance == null)
                {
                    Log.Warning("Can not create entity time line instance '{0}'.", fsm.Owner.PerformSkillId.ToString());
                    fsm.Owner.CallNextPerformSkillOperationFailure();
                    return;
                }

                m_CurrentSkillId = skillId;
                m_CurrentSkillIsInCombo = isInCombo;
                m_CurrentSkillIsContinualTap = isContinualTap;

                fsm.Owner.CallCurrentPerformSkillOperationStart(skillId, skillIndex, isInCombo, isContinualTap);
            }

            private void UpdateSkillRushing(IFsm<CharacterMotion> fsm, float elapseSeconds, float realElapseSeconds)
            {
                if (fsm.Owner.IsDuringSkillRushing)
                {
                    if (!m_WasUpdatingRushingLastFrame)
                    {
                        m_RushEnv = new RushEnv();
                    }

                    var transform = fsm.Owner.Owner.CachedTransform;
                    var rushingParams = GetRushingParams(fsm);

                    UpdateSkillRushingRotation(fsm, elapseSeconds, transform);

                    var dir = CalcRushingMoveDir(fsm, transform, rushingParams);
                    float speed = CalcRushingMoveSpeed(fsm, rushingParams);
                    fsm.Owner.Owner.NavAgent.nextPosition = transform.localPosition + dir * speed * elapseSeconds;

                    if (fsm.Owner.SkillRushingDirInput != Vector3.zero)
                    {
                        m_RushEnv.LastNonZeroInput = fsm.Owner.SkillRushingDirInput;
                    }

                    if (fsm.Owner.OnSkillRushUpdate != null)
                    {
                        fsm.Owner.OnSkillRushUpdate(m_CurrentSkillId, !m_WasUpdatingRushingLastFrame);
                    }
                }
                else
                {
                    if (m_WasUpdatingRushingLastFrame)
                    {
                        m_RushEnv = null;
                    }
                }

                m_WasUpdatingRushingLastFrame = fsm.Owner.IsDuringSkillRushing;
            }

            private static float CalcRushingMoveSpeed(IFsm<CharacterMotion> fsm, SkillRushingParams rushingParams)
            {
                if (rushingParams.AcceptDirInput && rushingParams.MoveOnlyOnDirInput && fsm.Owner.SkillRushingDirInput == Vector3.zero)
                {
                    return 0f;
                }

                return fsm.Owner.Owner.Data.Speed * rushingParams.SpeedFactor;
            }

            private Vector3 CalcRushingMoveDir(IFsm<CharacterMotion> fsm, Transform transform, SkillRushingParams rushingParams)
            {
                if (!rushingParams.AcceptDirInput)
                {
                    return transform.forward;
                }

                if (rushingParams.ForbidRotate)
                {
                    if (rushingParams.MoveOnlyOnDirInput)
                    {
                        return fsm.Owner.SkillRushingDirInput;
                    }
                    else
                    {
                        return fsm.Owner.SkillRushingDirInput == Vector3.zero ? m_RushEnv.LastNonZeroInput : fsm.Owner.SkillRushingDirInput;
                    }
                }

                return transform.forward;
            }

            private void UpdateSkillRushingRotation(IFsm<CharacterMotion> fsm, float elapseSeconds, Transform transform)
            {
                var skillRushingParams = GetRushingParams(fsm);

                if (skillRushingParams.ForbidRotate)
                {
                    if (m_RushEnv.TargetRotation != null)
                    {
                        fsm.Owner.Owner.CachedTransform.localRotation = Quaternion.Euler(0f, m_RushEnv.TargetRotation.Value, 0f);
                    }

                    return;
                }

                if (!skillRushingParams.AcceptDirInput || skillRushingParams.ForbidRotate || fsm.Owner.SkillRushingDirInput == Vector3.zero)
                {
                    return;
                }

                Vector3 inputDir;
                if (m_RushEnv.TargetRotation != null)
                {
                    inputDir = Quaternion.Euler(0f, m_RushEnv.TargetRotation.Value, 0f) * Vector3.forward;
                }
                else
                {
                    inputDir = fsm.Owner.SkillRushingDirInput;
                }

                float angleToRotate = Vector3.Angle(transform.forward, inputDir);

                if (angleToRotate == 0f)
                {
                    return;
                }

                float rotationAmount = skillRushingParams.AngularSpeed * elapseSeconds;

                if (skillRushingParams.AngularSpeed <= 0f || rotationAmount >= angleToRotate)
                {
                    fsm.Owner.Owner.CachedTransform.LookAt2D((fsm.Owner.Owner.CachedTransform.position + inputDir).ToVector2());
                }
                else
                {
                    var from = Quaternion.LookRotation(transform.forward, transform.up);
                    var to = Quaternion.LookRotation(inputDir, transform.up);
                    var targetThisFrame = Quaternion.Lerp(from, to, rotationAmount / angleToRotate);
                    fsm.Owner.Owner.CachedTransform.localRotation = Quaternion.Euler(0f, targetThisFrame.eulerAngles.y, 0f);
                }
            }
        }

        private class RushEnv
        {
            public float? TargetRotation = null;
            public Vector3 LastNonZeroInput = Vector3.zero;
        }
    }
}
