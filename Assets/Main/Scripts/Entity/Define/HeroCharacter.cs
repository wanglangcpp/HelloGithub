using BehaviorDesigner.Runtime;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    /// <summary>
    /// 英雄角色实体。
    /// </summary>
    public class HeroCharacter : Character, ICanHaveTarget
    {
        [SerializeField]
        protected HeroData m_HeroData = null;

        [SerializeField]
        protected List<Weapon> m_Weapons = null;

        private int? m_WeaponCount = null;
        private int m_LoadedWeaponCount = 0;
        private float m_WaitTimeBeforeRecovery = 0f;

        private const int MinBuffParamIndexForHideWeapons = 5;

        public new HeroData Data
        {
            get
            {
                return m_HeroData;
            }
        }

        public override bool NeedShowHPBarOnDamage
        {
            get
            {
                return !Data.IsMe;
            }
        }

        public BehaviorTree Behavior
        {
            get;
            protected set;
        }

        public ITargetable Target
        {
            get;
            set;
        }

        public bool HasTarget
        {
            get
            {
                return AIUtility.TargetCanBeSelected(Target);
            }
        }

        public bool CanSwitchHero
        {
            get
            {
                return Motion.IsDuringComboSkill || Motion.Moving || Motion.Standing || Motion.FallingDown || Motion.StandingUp || Motion.Dead;
            }
        }

        public int WeaponCount
        {
            get
            {
                if (m_WeaponCount.HasValue)
                {
                    return m_WeaponCount.Value;
                }

                IDataTable<DRWeaponSuite> dtWeaponSuite = GameEntry.DataTable.GetDataTable<DRWeaponSuite>();
                DRWeaponSuite drWeaponSuite = dtWeaponSuite.GetDataRow(Data.WeaponSuiteId);
                if (drWeaponSuite == null)
                {
                    Log.Warning("Can not find weapon suite '{0}'.", Data.WeaponSuiteId.ToString());
                    return 0;
                }

                int count = 0;
                for (int i = 0; i < Constant.MaxWeaponCountInSuite; i++)
                {
                    if (!drWeaponSuite.IsWeaponAvailable(i))
                    {
                        continue;
                    }

                    count++;
                }

                m_WeaponCount = count;
                return count;
            }
        }

        public override float DeadKeepTime
        {
            get
            {
                if (!GameEntry.IsAvailable)
                {
                    return 0f;
                }

                if (GameEntry.SceneLogic.InstanceLogicType == InstanceLogicType.MimicMelee)
                {
                    return GameEntry.SceneLogic.MimicMeleeInstanceLogic.ReviveWaitTime;
                }

                return Data.GetCachedHeroDataRow().DeadDurationBeforeAutoSwitch;
            }
        }

        public override int SteadyBuffId
        {
            get
            {
                return Data.GetCachedHeroDataRow().SteadyBuffId;
            }
        }

        public override int GetSkillLevel(int skillIndex)
        {
            if (skillIndex < 0 || skillIndex >= Data.SkillLevel.Length)
            {
                return 1;
            }

            return Data.SkillLevel[skillIndex];
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            if (m_Weapons == null)
            {
                m_Weapons = new List<Weapon>();
            }
        }

        protected override void OnShow(object userData)
        {
            m_HeroData = userData as HeroData;
            if (m_HeroData == null)
            {
                Log.Error("Hero data is invalid.");
                return;
            }

            base.OnShow(userData);

            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(UnityGameFramework.Runtime.EventId.ShowEntityFailure, OnShowEntityFailure);
            GameEntry.Event.Subscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Subscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);

            if (GameEntry.SceneLogic.InstanceLogicType != InstanceLogicType.NonInstance)
            {
                GameEntry.Entity.AddLock(this);
            }

            m_LoadedWeaponCount = 0;
            if (WeaponCount > 0)
            {
                AttachWeapons();
            }
            else
            {
                CheckAllWeaponsShown();
            }

            NavAgent.avoidancePriority = Data.GetCachedHeroDataRow().AvoidancePriority;

            if (m_HeroData.DebutOnShow)
            {
                PerformSwitchSkill();
                m_HeroData.DebutOnShow = false;
            }

            InitBehavior();
        }

        protected override void OnHide(object userData)
        {
            if (!GameEntry.IsAvailable) return;

            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntitySuccess, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(UnityGameFramework.Runtime.EventId.ShowEntityFailure, OnShowEntityFailure);
            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorSuccess, OnLoadBehaviorSuccess);
            GameEntry.Event.Unsubscribe(EventId.LoadBehaviorFailure, OnLoadBehaviorFailure);

            DeinitBehavior();
            LeaveAltSkills();

            base.OnHide(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (!GameEntry.SceneLogic.BaseInstanceLogic.IsRunning)
            {
                return;
            }

            if (Data.AltSkill.Enabled && !Data.AltSkill.IsForever)
            {
                Data.AltSkill.LeftTime -= realElapseSeconds;
                if (Data.AltSkill.LeftTime <= 0f)
                {
                    LeaveAltSkills();
                }
            }

            CoolDownTime[] skillCD = Data.SkillCD;
            for (int i = 0; i < skillCD.Length; i++)
            {
                skillCD[i].Update(elapseSeconds, realElapseSeconds);
            }

            CoolDownTime[] altSkillCD = Data.AltSkill.SkillCD;
            for (int i = 0; i < altSkillCD.Length; i++)
            {
                altSkillCD[i].Update(elapseSeconds, realElapseSeconds);
            }

            UpdateDodgeEnergy(elapseSeconds);

            if (CheckCreateArrowPrompt())
            {
                GameEntry.Impact.CreateArrowPrompt(this);
            }
        }

        private bool CheckCreateArrowPrompt()
        {
            var instance = GameEntry.SceneLogic.BaseInstanceLogic;

            if (this is MeHeroCharacter || Data.Camp == instance.MeHeroCharacter.Camp)
            {
                return false;
            }

            Vector3 viewVec = Camera.main.WorldToViewportPoint(transform.position);
            return !((viewVec.x < 1 && viewVec.x > 0 && viewVec.y < 1 && viewVec.y > 0) || IsDead);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);

            Weapon weapon = childEntity as Weapon;
            if (weapon != null)
            {
                m_Weapons.Add(weapon);
            }
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);

            Weapon weapon = childEntity as Weapon;
            if (weapon != null)
            {
                m_Weapons.Remove(weapon);
            }
        }

        /// <summary>
        /// 释放英雄切换技能。
        /// </summary>
        public void PerformSwitchSkill()
        {
            PerformNormalSkillOnIndex(Constant.SwitchSkillIndex, PerformSkillType.Click, true);
        }

        /// <summary>
        /// 释放当前英雄指定按钮位置的技能。
        /// </summary>
        /// <param name="skillIndex">技能索引。</param>
        /// <param name="performType">释放技能方式。</param>
        /// <param name="dontCD">不计算冷却。</param>
        /// <returns>请求释放技能的操作。</returns>
        public PerformSkillOperation PerformSkillOnIndex(int skillIndex, PerformSkillType performType)
        {
            if (Data.AltSkill.Enabled)
            {
                return PerformAltSkillOnIndex(skillIndex, performType);
            }
            else
            {
                return PerformNormalSkillOnIndex(skillIndex, performType, false);
            }
        }

        /// <summary>
        /// 是否在公共冷却中。
        /// </summary>
        public virtual bool IsDuringCommonCoolDown
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 开始公共冷却。
        /// </summary>
        /// <param name="coolDownTime">冷却时间。</param>
        public virtual void StartCommonCoolDown(float coolDownTime)
        {

        }

        /// <summary>
        /// 停止公共冷却。
        /// </summary>
        public virtual void StopCommonCoolDown()
        {

        }

        /// <summary>
        /// 快进公共冷却。
        /// </summary>
        /// <param name="amount">快进时间量。</param>
        public virtual void FastForwardCommonCoolDown(float amount)
        {

        }

        /// <summary>
        /// 检查指定索引的技能是否为连续点击技能。
        /// </summary>
        /// <param name="skillIndex">技能索引。</param>
        /// <returns>是否为连续点击技能。</returns>
        public bool CheckContinualTapSkill(int skillIndex)
        {
            int skillId;
            bool isInCombo;
            bool isContinualTap;
            bool isCharge;

            if (!CheckNormalSkillAvailability(skillIndex, out skillId, out isInCombo, out isContinualTap, out isCharge))
            {
                return false;
            }

            if (Motion == null)
            {
                return false;
            }

            return Motion.CheckContinualTapSkill(skillId, isContinualTap);
        }

        public bool CheckNormalSkillAvailability(int skillIndex, out int skillId, out bool isInCombo, out bool isContinualTap, out bool isCharge)
        {
            skillId = -1;
            isInCombo = false;
            isContinualTap = false;
            isCharge = false;

            if (skillIndex >= Constant.TotalSkillGroupCount)
            {
                return false;
            }

            int skillLevel = Data.GetSkillLevel(skillIndex);
            if (skillLevel <= 0)
            {
                return false;
            }

            var drSkillGroup = Data.GetCachedSkillGroupDataRow(skillIndex);
            if (drSkillGroup == null)
            {
                return false;
            }

            skillId = drSkillGroup.SkillId;
            isInCombo = drSkillGroup.IsInCombo;
            isContinualTap = drSkillGroup.IsContinualTap;
            isCharge = drSkillGroup.IsCharge;
            return true;
        }

        private PerformSkillOperation PerformNormalSkillOnIndex(int skillIndex, PerformSkillType performType, bool dontCD)
        {
            int skillId;
            bool isInCombo;
            bool isContinualTap;
            bool isCharge;

            if (!CheckNormalSkillAvailability(skillIndex, out skillId, out isInCombo, out isContinualTap, out isCharge))
            {
                return null;
            }

            if (!isCharge && performType != PerformSkillType.Click || isCharge && performType == PerformSkillType.Click)
            {
                return null;
            }

            if (!Data.GetSkillCoolDownTime(skillIndex).IsReady && !isCharge && !Motion.CheckContinualTapSkill(skillId, isContinualTap))
            {
                return null;
            }

            if (skillIndex == Constant.DodgeSkillIndex && Data.DodgeEnergy < Data.CostPerDodge)
            {
                return null;
            }

            // 检查是否需要替换技能
            if (Motion.ReplaceSkillInfo.CanReplaceSkill(skillId))
            {
                skillId = Motion.ReplaceSkillInfo.ReplacementSkillId;
            }

            var dtSkill = GameEntry.DataTable.GetDataTable<DRSkill>();
            DRSkill drSkill = dtSkill.GetDataRow(skillId);
            if (drSkill == null)
            {
                Log.Warning("Can not load skill '{0}' from data table.", skillId.ToString());
                return null;
            }

            var performSkillOperation = Motion.PerformSkill(skillId, skillIndex, isInCombo, isContinualTap, isCharge, false, performType);
            var cdTime = Data.GetSkillCoolDownTime(skillIndex);

            if (!dontCD)
            {
                cdTime.Reset(Mathf.Max(0f, drSkill.CoolDownTime - Data.ReducedSkillCoolDown - GetReducedSkillCoolDownByGenericBadges(skillIndex)), true);
            }

            GameFrameworkAction onPerformFailure = delegate ()
            {
                cdTime.SetReady();
                cdTime.IsPaused = false;
            };

            GameFrameworkAction onPerformStart = delegate ()
            {
                if (skillIndex == Constant.DodgeSkillIndex)
                {
                    Data.DodgeEnergy -= Data.CostPerDodge;
                    m_WaitTimeBeforeRecovery = Data.WaitTimeBeforeDodgeRecovery;
                }
            };

            GameFrameworkAction onPerformEnd = delegate ()
            {
                cdTime.IsPaused = false;
            };

            DealWithPerformSkillOperation(performSkillOperation, onPerformFailure, onPerformStart, onPerformEnd);
            return performSkillOperation;
        }

        private float GetReducedSkillCoolDownByGenericBadges(int skillIndex)
        {
            if (Data.SkillsBadges == null || skillIndex < 0) return 0f;
            var genericBadges = Data.SkillsBadges[skillIndex].GenericBadges;

            var ret = 0f;
            for (int i = 0; i < genericBadges.Count; i++)
            {
                if (genericBadges[i].BadgeId <= 0)
                {
                    continue;
                }

                var drGenericBadge = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>().GetDataRow(genericBadges[i].BadgeId);
                if (drGenericBadge == null)
                {
                    continue;
                }

                ret += drGenericBadge.ReducedSkillCoolDown;
            }

            return ret;
        }

        private PerformSkillOperation PerformAltSkillOnIndex(int skillIndex, PerformSkillType performType)
        {
            if (skillIndex >= Constant.SkillGroupCount)
            {
                return null;
            }

            IDataTable<DRAltSkill> dtAltSkill = GameEntry.DataTable.GetDataTable<DRAltSkill>();
            DRAltSkill dataRowAltSkill = dtAltSkill.GetDataRow(Data.AltSkill.SkillId);
            if (dataRowAltSkill == null)
            {
                Log.Warning("Can not load alt skill '{0}' from data table.", Data.AltSkill.SkillId.ToString());
                return null;
            }

            // 禁用此技能
            if (!dataRowAltSkill.GetAltSkillGroupEnabled(skillIndex))
            {
                return null;
            }

            int skillGroupId = dataRowAltSkill.GetAltSkillGroupId(skillIndex);
            if (skillGroupId < 0)
            {
                return PerformNormalSkillOnIndex(skillIndex, performType, false);
            }

            if (!Data.AltSkill.GetSkillCoolDownTime(skillIndex).IsReady)
            {
                return null;
            }

            IDataTable<DRSkillGroup> dtSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();
            IDataTable<DRSkill> dtSkill = GameEntry.DataTable.GetDataTable<DRSkill>();

            DRSkillGroup dataRowSkillGroup = dtSkillGroup.GetDataRow(skillGroupId);
            if (dataRowSkillGroup == null)
            {
                Log.Warning("Can not load skill group '{0}' from data table.", skillGroupId.ToString());
                return null;
            }

            if (Data.AltSkill.SkillLevel <= 0)
            {
                return null;
            }

            int skillId = dataRowSkillGroup.SkillId;
            bool isInCombo = dataRowSkillGroup.IsInCombo;

            // 检查是否需要替换技能
            if (Motion.ReplaceSkillInfo.CanReplaceSkill(skillId))
            {
                skillId = Motion.ReplaceSkillInfo.ReplacementSkillId;
            }

            DRSkill dataRowSkill = dtSkill.GetDataRow(skillId);
            if (dataRowSkill == null)
            {
                Log.Warning("Can not load skill '{0}' from data table.", skillId.ToString());
                return null;
            }

            var performSkillOperation = Motion.PerformSkill(skillId, skillIndex, isInCombo, false, false, false, performType);
            var cdTime = Data.AltSkill.GetSkillCoolDownTime(skillIndex);
            cdTime.Reset(Mathf.Min(0f, dataRowSkill.CoolDownTime - Data.ReducedSkillCoolDown), true);

            GameFrameworkAction onPerformFailure = delegate ()
            {
                cdTime.SetReady();
                cdTime.IsPaused = false;
            };

            GameFrameworkAction onPerformEnd = delegate ()
            {
                cdTime.IsPaused = false;
            };

            DealWithPerformSkillOperation(performSkillOperation, onPerformFailure, null, onPerformEnd);
            return performSkillOperation;
        }

        public void EnterAltSkills(int altSkillId, int altSkillLevel, float? keepTime)
        {
            Data.AltSkill.SkillId = altSkillId;
            Data.AltSkill.SkillLevel = altSkillLevel;
            Data.AltSkill.LeftTime = keepTime.HasValue ? keepTime.Value : -1f;

            GameEntry.Event.Fire(this, new EnterAltSkillEventArgs(altSkillId, altSkillLevel));
        }

        public void LeaveAltSkills()
        {
            Data.AltSkill.SkillId = 0;
            Data.AltSkill.SkillLevel = 0;
            Data.AltSkill.LeftTime = 0f;

            GameEntry.Event.Fire(this, new LeaveAltSkillEventArgs());
        }

        public Weapon GetWeapon(int weaponIndex)
        {
            for (int i = 0; i < m_Weapons.Count; i++)
            {
                if (m_Weapons[i].Data.WeaponIndexInSuite == weaponIndex)
                {
                    return m_Weapons[i];
                }
            }

            return null;
        }

        public bool ShowWeapon(int weaponIndex)
        {
            for (int i = 0; i < m_Weapons.Count; i++)
            {
                if (m_Weapons[i].Data.WeaponIndexInSuite == weaponIndex)
                {
                    m_Weapons[i].gameObject.SetActive(true);
                    return true;
                }
            }

            return false;
        }

        public bool HideWeapon(int weaponIndex)
        {
            for (int i = 0; i < m_Weapons.Count; i++)
            {
                if (m_Weapons[i].Data.WeaponIndexInSuite == weaponIndex)
                {
                    m_Weapons[i].gameObject.SetActive(false);
                    return true;
                }
            }

            return false;
        }

        public override float PlayAnimation(string animationAliasName, bool needRewind = false, bool dontCrossFade = false, bool queued = false)
        {
            PlayWeaponAnimation(animationAliasName, needRewind, dontCrossFade, queued);
            return base.PlayAnimation(animationAliasName, needRewind, dontCrossFade, queued);
        }

        public void PlayWeaponAnimation(string animationAliasName, bool needRewind = false, bool dontCrossFade = false, bool queued = false)
        {
            if (string.IsNullOrEmpty(animationAliasName))
            {
                Log.Warning("Weapon animation alias name is invalid.");
                return;
            }

            for (int i = 0; i < m_Weapons.Count; i++)
            {
                Weapon weapon = m_Weapons[i];
                if (!weapon.gameObject.activeSelf)
                {
                    continue;
                }

                string animationName = weapon.AnimationDataRow.GetAnimationName(animationAliasName);
                if (string.IsNullOrEmpty(animationName))
                {
                    continue;
                }

                AnimationState animationState = weapon.CachedAnimation[animationName];
                if (animationState == null)
                {
                    Log.Warning("Can not find weapon animation '{0}' for weapon '{1}'.", animationName, weapon.Data.WeaponId.ToString());
                    continue;
                }

                if (needRewind)
                {
                    weapon.CachedAnimation.Rewind(animationName);
                }

                if (dontCrossFade)
                {
                    if (queued)
                    {
                        weapon.CachedAnimation.PlayQueued(animationName);
                    }
                    else
                    {
                        weapon.CachedAnimation.Play(animationName);
                    }
                }
                else
                {
                    if (queued)
                    {
                        weapon.CachedAnimation.CrossFadeQueued(animationName);
                    }
                    else
                    {
                        weapon.CachedAnimation.CrossFade(animationName);
                    }
                }
            }
        }

        public void PlayWeaponAnimationQueued(string animationAliasName, bool needRewind = false, bool dontCrossFade = false)
        {
            if (string.IsNullOrEmpty(animationAliasName))
            {
                Log.Warning("Weapon animation alias name is invalid.");
                return;
            }

            for (int i = 0; i < m_Weapons.Count; i++)
            {
                Weapon weapon = m_Weapons[i];
                if (!weapon.gameObject.activeSelf)
                {
                    continue;
                }

                string animationName = weapon.AnimationDataRow.GetAnimationName(animationAliasName);
                if (string.IsNullOrEmpty(animationName))
                {
                    continue;
                }

                AnimationState animationState = weapon.CachedAnimation[animationName];
                if (animationState == null)
                {
                    Log.Warning("Can not find weapon animation '{0}' for weapon '{1}'.", animationName, weapon.Data.WeaponId.ToString());
                    continue;
                }

                if (needRewind)
                {
                    weapon.CachedAnimation.Rewind(animationName);
                }

                if (dontCrossFade)
                {
                    weapon.CachedAnimation.PlayQueued(animationName);
                }
                else
                {
                    weapon.CachedAnimation.CrossFadeQueued(animationName);
                }
            }
        }

        private static void DealWithPerformSkillOperation(PerformSkillOperation performSkillOperation,
            GameFrameworkAction onPerformFailure, GameFrameworkAction onPerformStart, GameFrameworkAction onPerformEnd)
        {
            if (performSkillOperation == null || performSkillOperation.State == PerformSkillOperationState.PerformFailure)
            {
                onPerformFailure();
            }
            else if (performSkillOperation.State == PerformSkillOperationState.PerformEnd)
            {
                onPerformStart();
                onPerformEnd();
            }
            else if (performSkillOperation.State == PerformSkillOperationState.Performing)
            {
                onPerformStart();
                performSkillOperation.OnPerformSkillEnd += onPerformEnd;
            }
            else
            {
                performSkillOperation.OnPerformSkillStart += onPerformStart;
                performSkillOperation.OnPerformSkillEnd += onPerformEnd;
                performSkillOperation.OnPerformSkillFailure += onPerformFailure;
            }
        }

        private void UpdateDodgeEnergy(float elapseSeconds)
        {
            m_WaitTimeBeforeRecovery -= elapseSeconds;
            if (m_WaitTimeBeforeRecovery < 0f)
            {
                m_WaitTimeBeforeRecovery = 0f;
            }

            if (m_WaitTimeBeforeRecovery > 0f)
            {
                return;
            }

            Data.DodgeEnergy += Data.DodgeEnergyRecoverySpeed * elapseSeconds;
            if (Data.DodgeEnergy >= Data.MaxDodgeEnergy)
            {
                Data.DodgeEnergy = Data.MaxDodgeEnergy;
            }
        }

        protected virtual bool ShouldLoadAI
        {
            get
            {
                return false;
            }
        }

        protected void InitBehavior()
        {
            if (Behavior != null)
            {
                Log.Error("Behavior already exists.");
                return;
            }

            if (!ShouldLoadAI)
            {
                return;
            }

            Behavior = gameObject.AddComponent<BehaviorTree>();
            Behavior.StartWhenEnabled = false;
            Behavior.PauseWhenDisabled = false;
            Behavior.ExternalBehavior = null;
            GameEntry.Behavior.LoadBehavior(Behavior, Data.GetCachedHeroDataRow().AIBehavior);
        }

        protected void DeinitBehavior()
        {
            if (Behavior == null)
            {
                return;
            }

            Behavior.DisableBehavior();
            GameEntry.Behavior.UnloadBehavior(Behavior.ExternalBehavior);
            Destroy(Behavior);
            Behavior = null;
        }

        public void OnShowHideWeaponBuffsChanged(BuffData added, IList<BuffData> removed)
        {
            if (added != null && added.BuffType == BuffType.ShowOrHideWeapons)
            {
                ShowOrHideWeaponsOnBuffAdded(added);
            }

            if (removed != null)
            {
                for (int k = 0; k < removed.Count; ++k)
                {
                    var removedBuffData = removed[k];
                    if (removedBuffData.BuffType != BuffType.ShowOrHideWeapons)
                    {
                        continue;
                    }

                    ShowOrHideWeaponsOnBuffRemoved(removedBuffData);
                }
            }
        }

        private void ShowOrHideWeaponsOnBuffRemoved(BuffData buffData)
        {
            for (int i = 0; i < MinBuffParamIndexForHideWeapons; ++i)
            {
                int showIndex = Mathf.RoundToInt(buffData.Params[i]);
                if (showIndex >= 0)
                {
                    ShowWeapon(showIndex);
                }
            }

            for (int i = MinBuffParamIndexForHideWeapons; i < DRBuff.BuffParamCount; ++i)
            {
                int hideIndex = Mathf.RoundToInt(buffData.Params[i]);
                if (hideIndex >= 0)
                {
                    HideWeapon(hideIndex);
                }
            }
        }

        private void ShowOrHideWeaponsOnBuffAdded(BuffData buffData)
        {
            for (int i = 0; i < MinBuffParamIndexForHideWeapons; ++i)
            {
                int hideIndex = Mathf.RoundToInt(buffData.Params[i]);
                if (hideIndex >= 0)
                {
                    HideWeapon(hideIndex);
                }
            }

            for (int i = MinBuffParamIndexForHideWeapons; i < DRBuff.BuffParamCount; ++i)
            {
                int showIndex = Mathf.RoundToInt(buffData.Params[i]);
                if (showIndex >= 0)
                {
                    ShowWeapon(showIndex);
                }
            }
        }

        private void AttachWeapons()
        {
            IDataTable<DRWeaponSuite> dtWeaponSuite = GameEntry.DataTable.GetDataTable<DRWeaponSuite>();
            DRWeaponSuite drWeaponSuite = dtWeaponSuite.GetDataRow(Data.WeaponSuiteId);
            if (drWeaponSuite == null)
            {
                Log.Warning("Can not find weapon suite '{0}'.", Data.WeaponSuiteId.ToString());
                return;
            }

            for (int i = 0; i < Constant.MaxWeaponCountInSuite; i++)
            {
                if (!drWeaponSuite.IsWeaponAvailable(i))
                {
                    continue;
                }

                GameEntry.Entity.ShowWeapon(new WeaponData(GameEntry.Entity.GetSerialId(),
                    drWeaponSuite.GetWeaponId(i),
                    drWeaponSuite.Id,
                    i,
                    drWeaponSuite.GetAttachPointPath(i),
                    drWeaponSuite.GetVisibleByDefault(i),
                    GameEntry.SceneLogic.IsInstance ? WeaponData.WeaponShowType.Normal : WeaponData.WeaponShowType.Lobby,
                    Id));
            }
        }

        protected virtual void OnLoadBehaviorSuccess(object sender, GameEventArgs e)
        {

        }

        protected virtual void OnLoadBehaviorFailure(object sender, GameEventArgs e)
        {
            LoadBehaviorFailureEventArgs ne = e as LoadBehaviorFailureEventArgs;
            if (ne.Behavior != Behavior)
            {
                return;
            }

            Log.Warning("Can not load behavior '{0}' from '{1}' with error message '{2}'.", ne.BehaviorName, ne.BehaviorAssetName, ne.ErrorMessage);
        }

        protected override void OnStateChanged()
        {
            base.OnStateChanged();

            if (IsDead)
            {
                StopCommonCoolDown();
            }
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.ShowEntitySuccessEventArgs ne = e as UnityGameFramework.Runtime.ShowEntitySuccessEventArgs;
            if (ne.EntityLogicType != typeof(Weapon))
            {
                return;
            }

            Weapon weapon = ne.Entity.Logic as Weapon;
            if (weapon == null)
            {
                return;
            }

            if (weapon.Owner == this)
            {
                m_LoadedWeaponCount++;
                CheckAllWeaponsShown();
            }
        }

        private void CheckAllWeaponsShown()
        {
            if (m_LoadedWeaponCount >= WeaponCount)
            {
                GameEntry.Event.Fire(this, new ShowWeaponsCompleteEventArgs(Data));
            }
        }

        private void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            UnityGameFramework.Runtime.ShowEntityFailureEventArgs ne = e as UnityGameFramework.Runtime.ShowEntityFailureEventArgs;
            if (ne.EntityLogicType != typeof(Weapon))
            {
                return;
            }

            WeaponData weaponData = ne.UserData as WeaponData;
            if (weaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }

            if (weaponData.OwnerId == Id)
            {
                Log.Warning("Cannot show weapon for hero '{0}'", Name);
                m_LoadedWeaponCount++;
                CheckAllWeaponsShown();
            }
        }
    }
}
