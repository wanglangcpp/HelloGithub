using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CharacterMotion : TargetableObjectMotion
    {
        private IFsm<CharacterMotion> m_Fsm = null;
        private ReplaceSkillInfo m_ReplaceSkillInfo = null;

        public event GameFrameworkAction OnStateChanged;

        public event GameFrameworkAction OnMovingUpdate;

        public event GameFrameworkAction OnMovingEnd;

        public event GameFrameworkAction<int, bool> OnSkillRushUpdate;

        private const string FsmDataKey_DontCrossFade = "DontCrossFade";

        public new Character Owner
        {
            get
            {
                return m_Owner as Character;
            }
        }

        public string CurrentStateName
        {
            get
            {
                return m_Fsm.CurrentState.GetType().Name;
            }
        }

        public float CurrentStateTime
        {
            get
            {
                return m_Fsm.CurrentStateTime;
            }
        }

        public Vector3 MoveTargetPosition
        {
            get;
            private set;
        }

        public bool IsLingering
        {
            get
            {
                return m_Fsm.CurrentState is LingeringState;
            }
        }

        public bool Moving
        {
            get
            {
                return m_Fsm.CurrentState is MovingState;
            }
        }

        public bool Standing
        {
            get
            {
                return m_Fsm.CurrentState is StandingState;
            }
        }

        public bool Skilling
        {
            get
            {
                return m_Fsm.CurrentState is SkillState;
            }
        }

        public bool Entering
        {
            get
            {
                return m_Fsm.CurrentState is EnteringState;
            }
        }

        public bool IsOnGround
        {
            get
            {
                return ((StateBase)m_Fsm.CurrentState).StateForCharacterMotion == CharacterMotionStateCategory.Ground;
            }
        }

        public bool IsInAir
        {
            get
            {
                return ((StateBase)m_Fsm.CurrentState).StateForCharacterMotion == CharacterMotionStateCategory.Air;
            }
        }

        public bool DontTurnOnHit
        {
            get
            {
                return ((StateBase)m_Fsm.CurrentState).DontTurnOnHit;
            }
        }

        public bool IsDuringComboSkill
        {
            get
            {
                var skillState = m_Fsm.CurrentState as SkillState;
                return (skillState != null) && skillState.CurrentSkillIsInCombo;
            }
        }

        public bool FallingDown
        {
            get
            {
                return m_Fsm.CurrentState is FallDownState;
            }
        }

        public bool StandingUp
        {
            get
            {
                return m_Fsm.CurrentState is StandUpState;
            }
        }

        public bool Dead
        {
            get
            {
                return m_Fsm.CurrentState is DeadState;
            }
        }

        /// <summary>
        /// 死亡后实体保留时间是否已到。
        /// </summary>
        public bool DeadKeepTimeIsReached
        {
            get
            {
                var deadState = (m_Fsm.CurrentState as DeadState);
                if (deadState == null)
                {
                    return false;
                }

                return m_Fsm.CurrentStateTime >= Owner.DeadKeepTime;
            }
        }

        public int PerformSkillId
        {
            get;
            private set;
        }

        public bool PerformSkillIsInCombo
        {
            get;
            private set;
        }

        private bool PerformSkillIsContinualTap
        {
            get;
            set;
        }

        private int m_PerformingChargeSkillId = int.MinValue;

        public ReplaceSkillInfo ReplaceSkillInfo
        {
            get
            {
                return m_ReplaceSkillInfo;
            }
        }

        public float StiffnessStartTime
        {
            get;
            private set;
        }

        public float StiffnessTime
        {
            get;
            private set;
        }

        public float RepulseStartTime
        {
            get;
            private set;
        }

        public Vector3 RepulseSpeed
        {
            get;
            private set;
        }

        public float RepulseTime
        {
            get;
            private set;
        }

        public ImpactAnimationType StiffnessImpactAnimType
        {
            get;
            private set;
        }

        public ImpactAnimationType FloatFallingAnimType
        {
            get;
            private set;
        }

        public float FallDownTime
        {
            get;
            private set;
        }

        public Vector3 FloatingSpeed
        {
            get;
            private set;
        }

        public Vector3 FloatingFallingSpeed
        {
            get;
            private set;
        }

        public float FloatingTime
        {
            get;
            private set;
        }

        public float FloatFallingTime
        {
            get;
            private set;
        }

        public float StunTime
        {
            get;
            private set;
        }

        public float FreezeTime
        {
            get;
            private set;
        }

        public CharacterMotionStateCategory FreezeStateCategory
        {
            get;
            private set;
        }

        public Entity ImpactSourceEntity
        {
            get;
            private set;
        }

        public ImpactSourceType ImpactSourceType
        {
            get;
            private set;
        }

        public Entity DeadlyImpactSourceEntity
        {
            get;
            set;
        }

        public ImpactSourceType DeadlyImpactSourceType
        {
            get;
            private set;
        }

        public float FakeDeathRecoverHPRatio
        {
            get;
            set;
        }

        public int PerformSkillIndex { get; private set; }

        public void Shutdown()
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;
        }

        /// <summary>
        /// 复活。
        /// </summary>
        public void Revive()
        {
            (m_Fsm.CurrentState as StateBase).Revive(m_Fsm);
        }

        /// <summary>
        /// 进入出场状态并播放动画。
        /// </summary>
        public void PlayEntering()
        {
            (m_Fsm.CurrentState as StateBase).PlayEntering(m_Fsm);
        }

        public bool StartMove(Vector3 targetPosition)
        {
            MoveTargetPosition = targetPosition;
            return (m_Fsm.CurrentState as StateBase).StartMove(m_Fsm);
        }

        public bool StopMove()
        {
            return (m_Fsm.CurrentState as StateBase).StopMove(m_Fsm);
        }

        public bool BreakSkills()
        {
            PerformSkillId = 0;
            PerformSkillIsInCombo = false;
            PerformSkillIsContinualTap = false;
            PerformSkillIndex = -1;

            ReplaceSkillInfo.Reset();

            return (m_Fsm.CurrentState as StateBase).BreakSkills(m_Fsm);
        }

        public bool BreakCurrentSkill(SkillEndReasonType reason)
        {
            return (m_Fsm.CurrentState as StateBase).BreakCurrentSkill(m_Fsm, reason);
        }

        public void UpdateGradualPosSync(float duration, float elaspeSeconds)
        {
            (m_Fsm.CurrentState as StateBase).UpdateGradualPosSync(m_Fsm, duration, elaspeSeconds);
        }

        public bool TryFastForwardSkill(int skillId, float targetTime)
        {
            return (m_Fsm.CurrentState as StateBase).TryFastForwardSkill(m_Fsm, skillId, targetTime);
        }

        public bool TryUpdateSkillRushing(int skillId, Vector2 position, float rotation)
        {
            return (m_Fsm.CurrentState as StateBase).TryUpdateSkillRushing(m_Fsm, skillId, position, rotation);
        }

        public override PerformSkillOperation PerformSkill(int skillId, int skillIndex, bool isInCombo, bool isContinualTap, bool isCharge, bool forcePerform, PerformSkillType performType)
        {
            var ownerHero = m_Owner as HeroCharacter;

            bool continualTapping = CheckContinualTapSkill(skillId, isContinualTap);
            if (ownerHero != null && ownerHero.IsDuringCommonCoolDown && !continualTapping)
            {
                return null;
            }

            // 如果已经有另一个释放技能操作，则放弃当前输入。
            if (m_NextPerformSkillOperation != null)
            {
                return null;
            }

            if (isCharge && !JudgeEnablePerformChargeSkill(performType, skillId))
                return null;

            PerformSkillId = skillId;
            PerformSkillIsInCombo = isInCombo;
            PerformSkillIsContinualTap = isContinualTap;
            PerformSkillIndex = ownerHero == null ? -1 : skillIndex;
            PerformSkillOperation op = null;

            if (!continualTapping)
            {
                if (!isCharge)
                {
                    op = CreateNormalSkillOp(skillId, isInCombo, isContinualTap, forcePerform);
                    m_NextPerformSkillOperation = op;
                }
                else
                {
                    op = CreateChargeSkillOp(skillId, isInCombo, isContinualTap, forcePerform, performType, op);
                }
            }
            else
            {
                op = m_CurrentPerformSkillOperation;
            }

            (m_Fsm.CurrentState as StateBase).PerformSkill(m_Fsm, forcePerform);

            return op;
        }

        public void OnShownChargeSkill()
        {
            m_PerformingChargeSkillId = int.MinValue;
        }

        private bool JudgeEnablePerformChargeSkill(PerformSkillType performType, int skillId)
        {
            // 这些状态下是不允许释放技能的状态。
            var currentState = m_Fsm.CurrentState;
            if (currentState is FreezeState
                || currentState is DeadState
                || currentState is FakeDeadState
                || currentState is AirStiffnessState
                || currentState is FallDownState
                || currentState is FloatFallingState
                || currentState is FloatState
                || currentState is StiffnessState
                || currentState is StunState)
            {
                GameEntry.Event.FireNow(this, new PerformChargeSkillEventArgs(PerformChargeSkillEventArgs.OperateType.Hide, skillId));
                return false;
            }

            // 当前有另一个技能待释放，则忽略本次
            if (m_NextPerformSkillOperation != null)
            {
                GameEntry.Event.FireNow(this, new PerformChargeSkillEventArgs(PerformChargeSkillEventArgs.OperateType.Hide, skillId));
                return false;
            }

            // 如果操作类型是Press，那么允许操作
            if (performType == PerformSkillType.Press && m_PerformingChargeSkillId == int.MinValue)
            {
                m_PerformingChargeSkillId = skillId;
                GameEntry.Event.FireNow(this, new PerformChargeSkillEventArgs(PerformChargeSkillEventArgs.OperateType.Show, skillId));
                return true;
            }

            // 如果当前没有释放技能，但是输入类型是Release也忽略
            if (m_CurrentPerformSkillOperation == null)
            {
                GameEntry.Event.FireNow(this, new PerformChargeSkillEventArgs(PerformChargeSkillEventArgs.OperateType.Hide, skillId));
                return false;
            }
            else
            {
                if (m_PerformingChargeSkillId == int.MinValue)
                {
                    GameEntry.Event.FireNow(this, new PerformChargeSkillEventArgs(PerformChargeSkillEventArgs.OperateType.Hide, skillId));
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private PerformSkillOperation CreateNormalSkillOp(int skillId, bool isInCombo, bool isContinualTap, bool forcePerform)
        {
            PerformSkillOperation op = new PerformSkillOperation(new PerformSkillOperationData
            {
                SkillId = skillId,
                IsInCombo = isInCombo,
                IsContinualTap = isContinualTap,
                ForcePerform = forcePerform,
                SkillIndex = PerformSkillIndex,
            }, Owner.Id);

            return op;
        }

        private PerformSkillOperation CreateChargeSkillOp(int skillId, bool isInCombo, bool isContinualTap, bool forcePerform, PerformSkillType performType, PerformSkillOperation op)
        {
            if (performType == PerformSkillType.Press)
            {
                op = new PerformSkillOperation(new PerformSkillOperationData
                {
                    SkillId = skillId,
                    IsInCombo = isInCombo,
                    IsContinualTap = isContinualTap,
                    ForcePerform = forcePerform,
                    SkillIndex = PerformSkillIndex,
                }, Owner.Id);

                op.IsChargeSkill = true;
                //蓄力中
                op.IsCharging = true;
                m_NextPerformSkillOperation = op;
            }
            else if (performType == PerformSkillType.Release)
            {
                //释放技能
                if (m_CurrentPerformSkillOperation != null && m_CurrentPerformSkillOperation.Data.SkillId == skillId)
                {
                    op = m_CurrentPerformSkillOperation;
                    op.IsCharging = false;
                }
            }

            return op;
        }

        public override bool PerformHPDamage(Entity impactSourceEntity, ImpactSourceType impactSourceType)
        {
            ImpactSourceEntity = impactSourceEntity;
            ImpactSourceType = impactSourceType;
            return (m_Fsm.CurrentState as StateBase).PerformHPDamage(m_Fsm);
        }

        public override bool PerformGoDie()
        {
            return (m_Fsm.CurrentState as StateBase).PerformGoDie(m_Fsm);
        }

        public bool PerformStiffness(ApplyStiffnessImpactData ad)
        {
            Owner.CheckFaceTo(-ad.RepulseDirection);

            PrepareStiffnessParams(ad.StiffTime, ad.RepulseStartTime, ad.RepulseVelocity, ad.RepulseTime, ad.ImpactAnimationType);
            FloatFallingAnimType = ad.FallingAnimationType;
            FloatFallingTime = ad.FloatFallingTime;
            FallDownTime = ad.DownTime;
            FloatingFallingSpeed = ad.FloatFallingVelocity;
            ReplaceSkillInfo.RecordStateImpact();

            return (m_Fsm.CurrentState as StateBase).PerformStiffness(m_Fsm);
        }

        public bool PerformHardHit(ApplyHardHitImpactData ad)
        {
            Owner.CheckFaceTo(-ad.RepulseDirection);
            FloatFallingAnimType = ad.FallingAnimationType;
            FloatFallingTime = ad.FloatFallingTime;
            FallDownTime = ad.DownTime;
            FloatingFallingSpeed = ad.FloatFallingVelocity;
            PrepareStiffnessParams(ad.StiffTime, ad.RepulseStartTime, ad.RepulseVelocity, ad.RepulseTime, ad.ImpactAnimationType);
            ReplaceSkillInfo.RecordStateImpact();

            return (m_Fsm.CurrentState as StateBase).PerformHardHit(m_Fsm);
        }

        public bool PerformFloat(ApplyFloatImpactData ad)
        {
            Owner.CheckFaceTo(-ad.RepulseDirection);
            FloatFallingAnimType = ad.FallingAnimationType;
            FloatingSpeed = ad.FloatVelocity;
            FloatingFallingSpeed = ad.FloatFallingVelocity;
            FloatingTime = ad.FloatingTime;
            FloatFallingTime = ad.FloatFallingTime;
            FallDownTime = ad.DownTime;
            PrepareStiffnessParams(ad.StiffTime, ad.RepulseStartTime, ad.RepulseVelocity, ad.RepulseTime, ad.RaiseAnimationType);
            ReplaceSkillInfo.RecordStateImpact();

            return (m_Fsm.CurrentState as StateBase).PerformFloat(m_Fsm);
        }

        public bool PerformBlownAway(ApplyBlownAwayImpactData ad)
        {
            Owner.CheckFaceTo(-ad.RepulseDirection);
            StiffnessImpactAnimType = ad.RaiseAnimationType;
            FloatFallingAnimType = ad.FallingAnimationType;
            FloatingSpeed = ad.FloatVelocity;
            FloatingFallingSpeed = ad.FloatFallingVelocity;
            FloatingTime = ad.FloatTime;
            FloatFallingTime = ad.FloatFallingTime;
            FallDownTime = ad.DownTime;
            ReplaceSkillInfo.RecordStateImpact();

            return (m_Fsm.CurrentState as StateBase).PerformBlownAway(m_Fsm);
        }

        public bool PerformStun(ApplyStunImpactData ad)
        {
            StunTime = ad.StunTime;
            ReplaceSkillInfo.RecordStateImpact();

            return (m_Fsm.CurrentState as StateBase).PerformStun(m_Fsm);
        }

        public bool PerformFreeze(ApplyFreezeImpactData ad)
        {
            StateBase stateBase = m_Fsm.CurrentState as StateBase;
            FreezeTime = ad.FreezeTime;
            FallDownTime = ad.DownTime;
            FreezeStateCategory = stateBase.StateForCharacterMotion;
            ReplaceSkillInfo.RecordStateImpact();

            return stateBase.PerformFreeze(m_Fsm);
        }

        public bool StartLingering(CharacterLingeringParams lingeringParams)
        {
            m_LingeringParams = lingeringParams;
            Log.Info("[CharacterMotion StartLingering] Params: targetPosition={0}, currentPosition={1}", m_LingeringParams.TargetPosition, Owner.CachedTransform.position);
            return (m_Fsm.CurrentState as StateBase).StartLingering(m_Fsm);
        }

        public bool StopLingering()
        {
            return (m_Fsm.CurrentState as StateBase).StopLingering(m_Fsm);
        }

        public bool CheckContinualTapSkill(int skillId, bool isContinualTap)
        {
            return Skilling && skillId == PerformSkillId && isContinualTap && PerformSkillIsContinualTap;
        }

        private void PrepareStiffnessParams(float stiffnessTime, float repulseStartTime, Vector3 repulseSpeed, float repulseTime, ImpactAnimationType animType)
        {
            StiffnessStartTime = Time.time;
            StiffnessTime = stiffnessTime;
            RepulseStartTime = repulseStartTime;
            RepulseSpeed = repulseSpeed;
            RepulseTime = repulseTime;
            StiffnessImpactAnimType = animType;
        }

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();

            m_Fsm = GameEntry.Fsm.CreateFsm(string.Format("Entity{0}", Owner.Id.ToString()), this,
                new StandingState(),
                new MovingState(),
                new SkillState(),
                new StiffnessState(),
                new AirStiffnessState(),
                new FallDownState(),
                new StandUpState(),
                new FloatState(),
                new FloatFallingState(),
                new StunState(),
                new FreezeState(),
                new FakeDeadState(),
                new DeadState(),
                new EnteringState(),
                new LingeringState());
            m_Fsm.Start<StandingState>();

            m_ReplaceSkillInfo = new ReplaceSkillInfo();
        }

        #endregion MonoBehaviour
    }
}
