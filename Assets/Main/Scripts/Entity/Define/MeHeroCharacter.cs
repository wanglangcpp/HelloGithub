using System;
using GameFramework.Event;
using UnityEngine;
using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// “我”的英雄实体。
    /// </summary>
    public class MeHeroCharacter : HeroCharacter
    {
        private GuideIndicatorEffect m_GuideIndicator = null;

        private bool m_HidingOrHidden = true;

        public ITimeLineInstance<Entity> TimeScaleInstance
        {
            get;
            set;
        }

        public float TimeScaleDurationTime
        {
            get;
            set;
        }

        public bool IsTryingToMove
        {
            get;
            private set;
        }

        public Vector3 MoveTargetDirection
        {
            get;
            private set;
        }

        /// <summary>
        /// 自动寻路状态
        /// </summary>
        public bool AutoMove = false;
       
        protected override bool ShouldLoadAI
        {
            get
            {
                return GameEntry.SceneLogic.IsInstance;
            }
        }

        public override bool IsDuringCommonCoolDown
        {
            get
            {
                return GameEntry.SceneLogic.BaseInstanceLogic.IsDuringCommonCoolDown;
            }
        }

        public override void StartCommonCoolDown(float coolDownTime)
        {
            GameEntry.SceneLogic.BaseInstanceLogic.StartCommonCoolDown(coolDownTime);
        }

        public override void StopCommonCoolDown()
        {
            GameEntry.SceneLogic.BaseInstanceLogic.StopCommonCoolDown();
        }

        public override void FastForwardCommonCoolDown(float amount)
        {
            GameEntry.SceneLogic.BaseInstanceLogic.FastForwardCommonCoolDown(amount);
        }

        //private void Awake()
        //{
        //    OnInit(null);
        //}

        protected override void OnShow(object userData)
        {
            m_HidingOrHidden = false;
            base.OnShow(userData);

            //Log.Info("[MeHeroCharacter OnShow] Name = {0}.", Name);

            GameEntry.Event.Subscribe(EventId.CanShowGuideIndicatorChanged, OnCanShowGuideIndicatorChanged);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);

            NavAgent.stoppingDistance = 0f;
            NavAgent.autoRepath = false;
            NavAgent.autoBraking = false;

            RefreshGuideIndicator();
        }

        protected override void OnHide(object userData)
        {
            //Log.Info("[MeHeroCharacter OnHide] Name = {0}.", Name);

            m_HidingOrHidden = true;
            if (GameEntry.IsAvailable && GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(EventId.CanShowGuideIndicatorChanged, OnCanShowGuideIndicatorChanged);
                GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            }

            m_GuideIndicator = null;
            base.OnHide(userData);
        }

        public void StartMove(Vector3 targetPosition, Vector3 targetDirection)
        {
            IsTryingToMove = true;
            AutoMove = false;
            MoveTargetDirection = targetDirection;

            if (!GameEntry.SceneLogic.BaseInstanceLogic.AutoFightIsEnabled)
            {
                Motion.StartMove(targetPosition);
            }

            Motion.SetSkillRushingDirInput(targetDirection);
        }
        /// <summary>
        /// 主城寻路
        /// </summary>
        /// <param name="targetPosition"></param>
        public void StartLoobyMove(Vector3 targetPosition)
        {
            IsTryingToMove = true;
            AutoMove = true;
            if (GameEntry.SceneLogic.NonInstanceLogic!=null)
            {
                Motion.StartMove(targetPosition);
            }
        }

        public void StopMove()
        {
            if (!GameEntry.SceneLogic.BaseInstanceLogic.AutoFightIsEnabled)
            {
                Motion.StopMove();
            }

            Motion.SetSkillRushingDirInput(Vector3.zero);
            MoveTargetDirection = Vector3.zero;
            IsTryingToMove = false;
            AutoMove = false;
        }

        /// <summary>
        /// 复活。
        /// </summary>
        public void Revive()
        {
            if (!IsDead)
            {
                Log.Warning("Cannot revive since I'm not dead.");
                return;
            }

            Data.Revive();
            Motion.Revive();
            AddBuff(Constant.Buff.ReviveBuffId, this.Data, OfflineBuffPool.GetNextSerialId(), null);
            GameEntry.Event.Fire(this, new ReviveEventArgs(this));
        }

        /// <summary>
        /// 激活自动战斗。
        /// </summary>
        public void EnableAutoFight()
        {
            if (Behavior.ExternalBehavior == null)
            {
                return;
            }

            if (GameEntry.IsAvailable && !GameEntry.SceneLogic.BaseInstanceLogic.ShouldProhibitAI)
            {
                Behavior.EnableBehavior();
            }
        }

        /// <summary>
        /// 停止自动战斗。
        /// </summary>
        public void DisableAutoFight()
        {
            if (Behavior.ExternalBehavior == null)
            {
                return;
            }

            Behavior.DisableBehavior();
        }

        protected override void OnLoadBehaviorSuccess(object sender, GameEventArgs e)
        {
            var ne = e as LoadBehaviorSuccessEventArgs;

            if (ne.Behavior != Behavior)
            {
                return;
            }

            if (GameEntry.SceneLogic.BaseInstanceLogic.AutoFightIsEnabled)
            {
                EnableAutoFight();
            }
        }

        protected override void OnMovingUpdate()
        {
            base.OnMovingUpdate();
            GameEntry.Event.FireNow(this, new MyHeroMovingUpdateEventArgs(Id, Motion.MoveTargetPosition, CachedTransform.rotation.eulerAngles.y, false));
        }

        protected override void OnMovingEnd()
        {
            base.OnMovingEnd();
            GameEntry.Event.FireNow(this, new MyHeroMovingUpdateEventArgs(Id, CachedTransform.position, CachedTransform.rotation.eulerAngles.y, true));
        }

        protected override void OnPerformSkillStart(int skillId, int skillIndex, bool isInCombo, bool isContinualTap)
        {
            base.OnPerformSkillStart(skillId, skillIndex, isInCombo, isContinualTap);

//             if (IsTryingToMove && isInCombo)
//             {
//                 CachedTransform.LookAt2D((CachedTransform.localPosition + MoveTargetDirection).ToVector2());
//             }

            GameEntry.Event.FireNow(this, new MyHeroPerformSkillStartEventArgs(Id, CachedTransform.position, CachedTransform.rotation.eulerAngles.y, skillId, skillIndex));
        }

        protected override void OnPerformSkillEnd(int skillId, SkillEndReasonType reason)
        {
            base.OnPerformSkillEnd(skillId, reason);
            GameEntry.Event.FireNow(this, new MyHeroPerformSkillEndEventArgs(Id, CachedTransform.position, CachedTransform.rotation.eulerAngles.y, skillId, reason));
            //Log.Info("Skill End: " + skillId.ToString() + ", Reason: " + reason.ToString());
        }

        protected override void OnSkillRushUpdate(int skillId, bool justEntered)
        {
            base.OnSkillRushUpdate(skillId, justEntered);
            GameEntry.Event.FireNow(this, new UpdateSkillRushingEventArgs(Id, skillId, NavAgent.nextPosition.ToVector2(), CachedTransform.localEulerAngles.y, justEntered));
        }

        private bool ShouldShowGuideIndicator
        {
            get
            {
                return GameEntry.SceneLogic.BaseInstanceLogic.CanShowGuideIndicator
                    && Motion != null && (Motion.Standing || Motion.Moving);
            }
        }

        private void OnCanShowGuideIndicatorChanged(object o, GameEventArgs e)
        {
            RefreshGuideIndicator();
        }

        private void OnShowEntitySuccess(object o, GameEventArgs e)
        {
            var ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
            var guideIndicator = ne.Entity.Logic as GuideIndicatorEffect;
            if (guideIndicator == null)
            {
                return;
            }

            if (m_GuideIndicator != null)
            {
                GameEntry.Entity.HideEntity(guideIndicator.Id);
            }
            else
            {
                m_GuideIndicator = guideIndicator;
            }

            if (!ShouldShowGuideIndicator)
            {
                HideGuideIndicator();
            }
        }

        protected override void OnStateChanged()
        {
            base.OnStateChanged();
            RefreshGuideIndicator();
        }

        private void RefreshGuideIndicator()
        {
            if (m_HidingOrHidden)
            {
                return;
            }

            if (ShouldShowGuideIndicator)
            {
                if (m_GuideIndicator == null)
                {
                    RequestShowGuideIndicator();
                }
            }
            else
            {
                if (m_GuideIndicator != null)
                {
                    HideGuideIndicator();
                }
            }
        }

        private void RequestShowGuideIndicator()
        {
            GameEntry.Entity.ShowGuideIndicator(Id);
        }

        private void HideGuideIndicator()
        {
            if (m_GuideIndicator != null)
            {
                GameEntry.Entity.HideEntity(m_GuideIndicator.Id);
                m_GuideIndicator = null;
            }
        }
    }
}
