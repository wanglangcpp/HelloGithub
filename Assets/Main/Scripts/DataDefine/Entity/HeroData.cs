using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class HeroData : CharacterData, IMeleeHeroData
    {
        [SerializeField]
        private int m_HeroId;

        [SerializeField]
        private int m_StarLevel;

        [SerializeField]
        private bool m_IsMe;

        [SerializeField]
        private bool m_DebutOnShow;

        [SerializeField]
        private AltSkillData m_AltSkill;

        [SerializeField]
        private int[] m_SkillLevel;

        [SerializeField]
        private bool[] m_SkillIsLevelLocked;

        [SerializeField]
        private CoolDownTime[] m_SkillCD;

        [SerializeField]
        private int m_SwitchSkillLevel;

        [SerializeField]
        private CoolDownTime m_SwitchSkillCD;

        [SerializeField]
        private int m_Profession;

        [SerializeField]
        private int m_WeaponSuiteId;

        [SerializeField]
        private float m_RecoverHP;

        [SerializeField]
        private float m_ReducedHeroSwitchCDRate;

        [SerializeField]
        private float m_ReducedSkillCoolDownRate;

        [SerializeField]
        private float m_DodgeEnergy;

        [SerializeField]
        private float m_MaxDodgeEnergy;

        [SerializeField]
        private float m_DodgeEnergyRecoverySpeed;

        [SerializeField]
        private float m_CostPerDodge;

        [SerializeField]
        private float m_WaitTimeBeforeDodgeRecovery;

        [SerializeField]
        private int m_MeleeLevel = 0;

        [SerializeField]
        private int m_MeleeExpAtCurrentLevel = 0;

        [SerializeField]
        private int m_MeleeScore = 0;

        private DRHero m_CachedHeroDR = null;
        private List<DRSkillGroup> m_CachedSkillGroupDRs = new List<DRSkillGroup>();

        public HeroData(int entityId, bool isMe, CharacterDataModifierType modifierType = CharacterDataModifierType.Offline)
            : base(entityId, modifierType)
        {
            m_IsMe = isMe;

            m_SkillLevel = new int[Constant.TotalSkillGroupCount];
            m_SkillIsLevelLocked = new bool[Constant.TotalSkillGroupCount];
            m_SkillCD = new CoolDownTime[Constant.TotalSkillGroupCount];
            for (int i = 0; i < Constant.TotalSkillGroupCount; i++)
            {
                m_SkillCD[i] = new CoolDownTime();
            }

            m_SwitchSkillCD = new CoolDownTime();

            m_AltSkill = new AltSkillData();
        }

        public new HeroCharacter Entity
        {
            get
            {
                return base.Entity as HeroCharacter;
            }
        }

        public int HeroId
        {
            get
            {
                return m_HeroId;
            }
            set
            {
                m_HeroId = value;
            }
        }

        /// <summary>
        /// 角色星级。
        /// </summary>
        public int StarLevel
        {
            get
            {
                return m_StarLevel;
            }
            set
            {
                m_StarLevel = value;
            }
        }

        public bool IsMe
        {
            get
            {
                return m_IsMe;
            }
        }

        public AltSkillData AltSkill
        {
            get
            {
                return m_AltSkill;
            }
        }

        public bool DebutOnShow
        {
            get
            {
                return m_DebutOnShow;
            }
            set
            {
                m_DebutOnShow = value;
            }
        }

        /// <summary>
        /// 技能等级。
        /// </summary>
        public int[] SkillLevel
        {
            get
            {
                return m_SkillLevel;
            }
        }

        /// <summary>
        /// 复活。
        /// </summary>
        public void Revive()
        {
            HP = MaxHP;

            // 恢复霸体条的数值和状态。
            Steady.SteadyStatus = false;
            Steady.Steady = Steady.MaxSteady;

            for (int i = 0; i < m_SkillLevel.Length; i++)
            {
                SkillCD[i].SetReady();
            }

            SwitchSkillCD.SetReady();
            BuffPool.Clear();

            IsDead = false;
        }

        /// <summary>
        /// 获取技能等级。
        /// </summary>
        /// <param name="index">技能位置。</param>
        /// <returns>技能等级。</returns>
        public int GetSkillLevel(int index)
        {
            return m_SkillLevel[index];
        }

        /// <summary>
        /// 设置技能等级。
        /// </summary>
        /// <param name="index">技能位置。</param>
        /// <param name="level">技能等级。</param>
        public void SetSkillLevel(int index, int level)
        {
            m_SkillLevel[index] = level;
        }

        /// <summary>
        /// 获取技能是否未解锁。
        /// </summary>
        /// <param name="index">技能位置。</param>
        /// <returns>是否未解锁。</returns>
        public bool GetSkillIsLevelLocked(int index)
        {
            return m_SkillIsLevelLocked[index];
        }

        /// <summary>
        /// 技能冷却时间。
        /// </summary>
        public CoolDownTime[] SkillCD
        {
            get
            {
                return m_SkillCD;
            }
        }

        /// <summary>
        /// 获取技能冷却时间。
        /// </summary>
        /// <param name="index">技能位置。</param>
        /// <returns>技能冷却时间。</returns>
        public CoolDownTime GetSkillCoolDownTime(int index)
        {
            return m_SkillCD[index];
        }

        public int SwitchSkillLevel
        {
            get
            {
                return m_SwitchSkillLevel;
            }
            set
            {
                m_SwitchSkillLevel = value;
            }
        }

        public CoolDownTime SwitchSkillCD
        {
            get
            {
                return m_SwitchSkillCD;
            }
            set
            {
                m_SwitchSkillCD = value;
            }
        }

        public int Profession
        {
            get
            {
                return m_Profession;
            }
            set
            {
                m_Profession = value;
            }
        }

        public int WeaponSuiteId
        {
            get
            {
                return m_WeaponSuiteId;
            }
            set
            {
                m_WeaponSuiteId = value;
            }
        }

        public float RecoverHP
        {
            get
            {
                return m_RecoverHP;
            }

            set
            {
                m_RecoverHP = value;
            }
        }

        public float ReducedHeroSwitchCD
        {
            get
            {
                return m_ReducedHeroSwitchCDRate;
            }

            set
            {
                m_ReducedHeroSwitchCDRate = value;
            }
        }

        public float ReducedSkillCoolDown
        {
            get
            {
                return m_ReducedSkillCoolDownRate;
            }

            set
            {
                m_ReducedSkillCoolDownRate = value;
            }
        }

        public float DodgeEnergy
        {
            get
            {
                return m_DodgeEnergy;
            }
            set
            {
                m_DodgeEnergy = value;
            }
        }

        public float MaxDodgeEnergy
        {
            get
            {
                return m_MaxDodgeEnergy;
            }
            set
            {
                m_MaxDodgeEnergy = value;
            }
        }

        public float DodgeEnergyRatio
        {
            get
            {
                return m_MaxDodgeEnergy > 0f ? m_DodgeEnergy / m_MaxDodgeEnergy : 0f;
            }
        }

        public float DodgeEnergyRecoverySpeed
        {
            get
            {
                return m_DodgeEnergyRecoverySpeed;
            }
            set
            {
                m_DodgeEnergyRecoverySpeed = value;
            }
        }

        public float CostPerDodge
        {
            get
            {
                return m_CostPerDodge;
            }

            set
            {
                m_CostPerDodge = value;
            }
        }

        public float WaitTimeBeforeDodgeRecovery
        {
            get
            {
                return m_WaitTimeBeforeDodgeRecovery;
            }

            set
            {
                m_WaitTimeBeforeDodgeRecovery = value;
            }
        }

        public IList<SkillBadgesData> SkillsBadges
        {
            get;
            set;
        }

        public void UpdateData(PBRoomHeroInfo pb)
        {
            if (pb.HasAdditionalDamage)
            {
                AdditionalDamage = pb.AdditionalDamage;
            }

            if (pb.HasAntiCriticalHitProb)
            {
                AntiCriticalHitProb = pb.AntiCriticalHitProb;
            }

            // Ignore camp

            if (pb.HasCriticalHitProb)
            {
                CriticalHitProb = pb.CriticalHitProb;
            }

            if (pb.HasCriticalHitRate)
            {
                CriticalHitRate = pb.CriticalHitRate;
            }

            if (pb.HasDamageReductionRate)
            {
                DamageReductionRate = pb.DamageReductionRate;
            }

            if (pb.HasHP)
            {
                HP = pb.HP;
            }

            if (pb.HasMagicAtkHPAbsorbRate)
            {
                MagicAtkHPAbsorbRate = pb.MagicAtkHPAbsorbRate;
            }

            if (pb.HasMagicAtkReflectRate)
            {
                MagicAtkReflectRate = pb.MagicAtkReflectRate;
            }

            if (pb.HasMagicAttack)
            {
                MagicAttack = pb.MagicAttack;
            }

            if (pb.HasMagicDefense)
            {
                MagicDefense = pb.MagicDefense;
            }

            if (pb.HasMaxHP)
            {
                MaxHP = pb.MaxHP;
            }

            if (pb.HasOppMagicDfsReduceRate)
            {
                OppMagicDfsReduceRate = pb.OppMagicDfsReduceRate;
            }

            if (pb.HasOppPhysicalDfsReduceRate)
            {
                OppPhysicalDfsReduceRate = pb.OppPhysicalDfsReduceRate;
            }

            if (pb.HasPhysicalAtkHPAbsorbRate)
            {
                PhysicalAtkHPAbsorbRate = pb.PhysicalAtkHPAbsorbRate;
            }

            if (pb.HasPhysicalAtkReflectRate)
            {
                PhysicalAtkReflectRate = pb.PhysicalAtkReflectRate;
            }

            if (pb.HasPhysicalAttack)
            {
                PhysicalAttack = pb.PhysicalAttack;
            }

            if (pb.HasPhysicalDefense)
            {
                PhysicalDefense = pb.PhysicalDefense;
            }

            if (pb.HasSpeed)
            {
                Speed = pb.Speed;
            }

            // Skip weapon suite ID.

            if (pb.Transform != null)
            {
                UpdateTransform(pb.Transform);
            }

            if (pb.HasReducedSkillCoolDownRate)
            {
                ReducedSkillCoolDown = pb.ReducedSkillCoolDownRate;
            }

            if (pb.HasHeroSwitchCoolDownRate)
            {
                ReducedHeroSwitchCD = pb.HeroSwitchCoolDownRate;
            }

            if (pb.HasRecoverHP)
            {
                RecoverHP = pb.RecoverHP;
            }
        }

        public override void OnBuffPoolChanged(BuffData added, IList<BuffData> removed)
        {
            base.OnBuffPoolChanged(added, removed);
            if (Entity == null)
            {
                return;
            }
            Entity.OnShowHideWeaponBuffsChanged(added, removed);
        }

        /// <summary>
        /// 根据技能徽章数据替换技能数据并缓存需要的数据表行。
        /// </summary>
        public void ReplaceSkillsAndCacheDataRows()
        {
            var heroDataTable = GameEntry.DataTable.GetDataTable<DRHero>();

            m_CachedHeroDR = heroDataTable.GetDataRow(HeroId);
            if (m_CachedHeroDR == null)
            {
                Log.Error("Hero with ID {0} doesn't exist.", HeroId);
                return;
            }

            var dtSkillGroup = GameEntry.DataTable.GetDataTable<DRSkillGroup>();

            m_CachedSkillGroupDRs.Clear();
            for (int skillIndex = 0; skillIndex < Constant.TotalSkillGroupCount; ++skillIndex)
            {
                int skillGroupId = -1;
                if (skillIndex < Constant.SkillGroupCount)
                {
                    skillGroupId = m_CachedHeroDR.GetSkillGroupId(skillIndex);

                }
                else if (skillIndex == Constant.SwitchSkillIndex)
                {
                    skillGroupId = m_CachedHeroDR.SwitchSkillGroupId;
                }

                skillGroupId = GetReplacedSkillGroupIdBySpecificBadge(skillIndex, skillGroupId);
                DRSkillGroup drSkillGroup = dtSkillGroup.GetDataRow(skillGroupId);
                if (drSkillGroup == null)
                {
                    m_CachedSkillGroupDRs.Add(null);
                }
                else
                {
                    m_CachedSkillGroupDRs.Add(drSkillGroup);
                }
            }
        }

        /// <summary>
        /// 初始化技能解锁情况。
        /// </summary>
        public void InitSkillLevelLocks()
        {
            for (int i = 0; i < m_CachedSkillGroupDRs.Count; ++i)
            {
                m_SkillIsLevelLocked[i] = m_CachedSkillGroupDRs[i] == null || Level < m_CachedSkillGroupDRs[i].SkillUnlockLevel;
            }
        }

        /// <summary>
        /// 刷新闪避技能数据。
        /// </summary>
        public void RefreshDodgeSkillEnergyData()
        {
            DRSkillGroup drSkillGroup = GetCachedSkillGroupDataRow(Constant.DodgeSkillIndex);
            DRDodgeSkill drDodgeSkill = GameEntry.DataTable.GetDataTable<DRDodgeSkill>().GetDataRow(drSkillGroup.SkillId);
            if (drDodgeSkill == null)
            {
                Log.Warning("Dodge skill '{0}' not found in DodgeSkill data table.", drSkillGroup.SkillId.ToString());
                return;
            }

            MaxDodgeEnergy = drDodgeSkill.MaxEnergy;
            DodgeEnergyRecoverySpeed = drDodgeSkill.RecoverySpeed;
            CostPerDodge = drDodgeSkill.CostPerDodge;
            WaitTimeBeforeDodgeRecovery = drDodgeSkill.WaitTimeBeforeRecovery;

            var dodgeGenericBadges = SkillsBadges[Constant.DodgeSkillIndex].GenericBadges;
            for (int i = 0; i < dodgeGenericBadges.Count; ++i)
            {
                var drGenericBadge = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>().GetDataRow(dodgeGenericBadges[i].BadgeId);
                if (drGenericBadge == null || !drGenericBadge.IsDodge)
                {
                    continue;
                }

                MaxDodgeEnergy += drGenericBadge.Params[0];
                DodgeEnergyRecoverySpeed += drGenericBadge.Params[1];
            }

            DodgeEnergy = MaxDodgeEnergy;
        }

        /// <summary>
        /// 获取缓存的英雄数据表行。如果不存在将立即缓存。
        /// </summary>
        /// <returns>缓存的英雄数据表行</returns>
        public DRHero GetCachedHeroDataRow()
        {
            if (m_CachedHeroDR == null)
            {
                var heroDataTable = GameEntry.DataTable.GetDataTable<DRHero>();
                m_CachedHeroDR = heroDataTable.GetDataRow(HeroId);
            }
            return m_CachedHeroDR;
        }

        public DRSkillGroup GetCachedSkillGroupDataRow(int skillIndex)
        {
            return m_CachedSkillGroupDRs[skillIndex];
        }

        private int GetReplacedSkillGroupIdBySpecificBadge(int skillIndex, int skillGroupId)
        {
            int specificSkillBadgeId = SkillsBadges[skillIndex].SpecificBadge.BadgeId;
            if (specificSkillBadgeId <= 0)
            {
                return skillGroupId;
            }

            var drBadge = GameEntry.DataTable.GetDataTable<DRSpecificSkillBadge>().GetDataRow(specificSkillBadgeId);
            int replaceSkillGroupId = -1;
            if (drBadge != null)
            {
                replaceSkillGroupId = drBadge.ReplaceSkillGroupId;
            }

            if (replaceSkillGroupId > 0)
            {
                skillGroupId = replaceSkillGroupId;
            }

            return skillGroupId;
        }

        /// <summary>
        /// 乱斗过程中的等级。
        /// </summary>
        public int MeleeLevel
        {
            get
            {
                return m_MeleeLevel;
            }

            set
            {
                m_MeleeLevel = value;
            }
        }

        /// <summary>
        /// 乱斗过程中当前等级的经验值。
        /// </summary>
        public int MeleeExpAtCurrentLevel
        {
            get
            {
                return m_MeleeExpAtCurrentLevel;
            }

            set
            {
                m_MeleeExpAtCurrentLevel = value;
            }
        }

        /// <summary>
        /// 乱斗积分。
        /// </summary>
        public int MeleeScore
        {
            get
            {
                return m_MeleeScore;
            }

            set
            {
                m_MeleeScore = value;
            }
        }

        /// <summary>
        /// 增加乱斗经验值。
        /// </summary>
        /// <param name="deltaExp">经验增量。</param>
        public void AddMeleeExp(int deltaExp)
        {
            this.AddMeleeExp(deltaExp, OnMeleeLevelUp);
        }

        private void OnMeleeLevelUp()
        {
            var mimicMeleeBaseDataRow = GameEntry.DataTable.GetDataTable<DRMimicMeleeBase>().GetDataRow(MeleeLevel);
            var hpRatio = (float)HP / MaxHP;
            MaxHP = mimicMeleeBaseDataRow.MaxHPBase;
            HP = Mathf.RoundToInt(MaxHP * hpRatio);
            Steady.MaxSteady = mimicMeleeBaseDataRow.Steady;
            Steady.SteadyRecoverSpeed = mimicMeleeBaseDataRow.SteadyRecoverSpeed;
            PhysicalAttack = mimicMeleeBaseDataRow.PhysicalAttackBase;
            PhysicalDefense = mimicMeleeBaseDataRow.PhysicalDefenseBase;
            CriticalHitProb = mimicMeleeBaseDataRow.CriticalHitProb;
            CriticalHitRate = mimicMeleeBaseDataRow.CriticalHitRate;
            DamageRandomRate = mimicMeleeBaseDataRow.DamageRandomRate;
        }
    }
}
