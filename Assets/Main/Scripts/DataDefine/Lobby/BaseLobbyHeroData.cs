using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class BaseLobbyHeroData
    {
        [SerializeField]
        protected int m_Type;

        [SerializeField]
        private int m_Level;

        [SerializeField]
        private int m_Exp;

        [SerializeField]
        private int m_StarLevel;

        [SerializeField]
        private int m_ConsciousnessLevel;

        [SerializeField]
        private int m_ElevationLevel;

        [SerializeField]
        private int m_Might;

        [SerializeField]
        private int m_TotalQualityLevel;

        [SerializeField]
        private List<int> m_SkillLevels = new List<int>();

        [SerializeField]
        private List<float> m_SkillExps = new List<float>();

        [SerializeField]
        protected List<SkillBadgesData> m_SkillBadges = new List<SkillBadgesData>();

        private Dictionary<AttributeType, float> m_CachedQualityLevelAttributes = new Dictionary<AttributeType, float>();
        private Dictionary<AttributeType, float> m_CachedNextQualityLevelAttributes = new Dictionary<AttributeType, float>();

        public BaseLobbyHeroData()
        {
            TotalQualityLevel = 1;
        }

        /// <summary>
        /// 当前品阶编号。
        /// </summary>
        public int QualityLevelId { get; private set; }

        /// <summary>
        /// 下一品阶编号。
        /// </summary>
        public int NextQualityLevelId { get; private set; }

        public int Type
        {
            get
            {
                return m_Type;
            }
            protected set
            {
                if (m_Type == value && m_HeroDataRow != null)
                {
                    return;
                }

                m_Type = value;

                var dataTable = GameEntry.DataTable.GetDataTable<DRHero>();
                m_HeroDataRow = dataTable.GetDataRow(m_Type);
                if (m_HeroDataRow == null)
                {
                    Log.Error("Cannot find hero '{0}'.", m_Type);
                }
            }
        }

        public int Level
        {
            get
            {
                return m_Level;
            }
            protected set
            {
                if (m_Level == value && m_HeroBaseDataRow != null)
                {
                    return;
                }

                m_Level = value;

                var dataTable = GameEntry.DataTable.GetDataTable<DRHeroBase>();
                m_HeroBaseDataRow = dataTable.GetDataRow(m_Level);
                if (m_HeroBaseDataRow == null)
                {
                    Log.Error("Cannot find hero level '{0}'.", m_Level);
                }
            }
        }

        public int Exp
        {
            get
            {
                return m_Exp;
            }
            protected set
            {
                m_Exp = value;
            }
        }

        public int StarLevel
        {
            get
            {
                return m_StarLevel;
            }
            protected set
            {
                m_StarLevel = value;
            }
        }

        public int Might
        {
            get
            {
                return m_Might;
            }
            protected set
            {
                m_Might = value;
            }
        }

        public int TotalQualityLevel
        {
            get
            {
                return m_TotalQualityLevel;
            }
            protected set
            {
                m_TotalQualityLevel = value;

                var dtQualityLevel = GameEntry.DataTable.GetDataTable<DRHeroQualityLevel>();
                var drs = dtQualityLevel.GetAllDataRows();
                bool found = false;
                for (int i = 0; i < drs.Length; ++i)
                {
                    if (Type != drs[i].HeroId)
                    {
                        continue;
                    }

                    if (value != drs[i].TotalQualityLevel)
                    {
                        continue;
                    }

                    found = true;
                    m_HeroQualityLevelRow = drs[i];
                    QualityLevel = drs[i].QualityLevel;
                    Quality = (QualityType)drs[i].Quality;
                    QualityLevelId = drs[i].Id;
                    m_CachedQualityLevelAttributes.Clear();
                    CacheQualityLevelAttribute(drs[i], m_CachedQualityLevelAttributes);

                    NextQualityLevelId = drs[i].NextId;
                    m_CachedNextQualityLevelAttributes.Clear();
                    if (NextQualityLevelId > 0)
                    {
                        CacheQualityLevelAttribute(dtQualityLevel[NextQualityLevelId], m_CachedNextQualityLevelAttributes);
                    }
                    break;
                }

                if (!found)
                {
                    //throw new ArgumentException(string.Format("Quality level info not found for total quality level '{0}' for hero '{1}'.", value.ToString(), Type.ToString()));
                }
            }
        }

        private static void CacheQualityLevelAttribute(DRHeroQualityLevel dr, Dictionary<AttributeType, float> attrDict)
        {
            var attrIds = dr.GetAttrIds();
            var attrVals = dr.GetAttrVals();
            for (int j = 0; j < attrIds.Count; ++j)
            {
                attrDict.Add((AttributeType)attrIds[j], attrVals[j]);
            }
        }

        /// <summary>
        /// 品质。
        /// </summary>
        public QualityType Quality { get; private set; }

        /// <summary>
        /// 品阶。
        /// </summary>
        public int QualityLevel { get; private set; }

        /// <summary>
        /// 获取当前总品阶下的属性值。
        /// </summary>
        /// <param name="attrType">属性类型。</param>
        /// <returns>属性值。</returns>
        public float GetQualityLevelAttribute(AttributeType attrType)
        {
            float ret;
            if (m_CachedQualityLevelAttributes.TryGetValue(attrType, out ret))
            {
                return ret;
            }

            return 0f;
        } 

        /// <summary>
        /// 获取下一总品阶下的属性值。
        /// </summary>
        /// <param name="attrType">属性类型。</param>
        /// <returns>属性值。</returns>
        public float GetNextQualityLevelAttribute(AttributeType attrType)
        {
            float ret;
            if (m_CachedNextQualityLevelAttributes.TryGetValue(attrType, out ret))
            {
                return ret;
            }

            return 0f;
        }

        public int[] SkillGroupIds
        {
            get
            {
                return HeroDataRow.SkillGroupIds;
            }
        }

        public int SwitchSkillGroupId
        {
            get
            {
                return HeroDataRow.SwitchSkillGroupId;
            }
        }

        public int IconId
        {
            get
            {
                return HeroDataRow.IconId;
            }
        }

        private DRHero m_HeroDataRow = null;
        protected DRHero HeroDataRow { get { return m_HeroDataRow; } }

        private DRHeroBase m_HeroBaseDataRow = null;
        protected DRHeroBase HeroBaseDataRow { get { return m_HeroBaseDataRow; } }

        protected DRIcon m_IconDataRow = null;

        protected DRHeroQualityLevel m_HeroQualityLevelRow = null;

        protected DRIcon IconDataRow
        {
            get
            {
                if (m_IconDataRow == null)
                {
                    var dataTable = GameEntry.DataTable.GetDataTable<DRIcon>();
                    m_IconDataRow = dataTable.GetDataRow(HeroDataRow.IconId);
                    if (m_IconDataRow == null)
                    {
                        Log.Error("Cannot find hero's icon '{0}'.", HeroDataRow.IconId);
                        return null;
                    }
                }

                return m_IconDataRow;
            }
        }

        #region Attribute tendencies

        public float MaxHPFactor
        {
            get
            {
                return HeroDataRow.MaxHPFactor;
            }
        }

        public float MaxHPFactorProgress
        {
            get
            {
                return NumericalCalcUtility.CalcProportion(Constant.Hero.MaxHPFactorMinVal, Constant.Hero.MaxHPFactorMaxVal, MaxHPFactor);
            }
        }

        public float PhysicalAttackFactor
        {
            get
            {
                return HeroDataRow.PhysicalAttackFactor;
            }
        }

        public float PhysicalAttackFactorProgress
        {
            get
            {
                return NumericalCalcUtility.CalcProportion(Constant.Hero.PhysicalAttackFactorMinVal, Constant.Hero.PhysicalAttackFactorMaxVal, PhysicalAttackFactor);
            }
        }

        public float PhysicalDefenseFactor
        {
            get
            {
                return HeroDataRow.PhysicalDefenseFactor;
            }
        }

        public float PhysicalDefenseFactorProgress
        {
            get
            {
                return NumericalCalcUtility.CalcProportion(Constant.Hero.PhysicalDefenseFactorMinVal, Constant.Hero.PhysicalDefenseFactorMaxVal, PhysicalDefenseFactor);
            }
        }

        public float MagicAttackFactor
        {
            get
            {
                return HeroDataRow.MagicAttackFactor;
            }
        }

        public float MagicAttackFactorProgress
        {
            get
            {
                return NumericalCalcUtility.CalcProportion(Constant.Hero.MagicAttackFactorMinVal, Constant.Hero.MagicAttackFactorMaxVal, MagicAttackFactor);
            }
        }

        public float MagicDefenseFactor
        {
            get
            {
                return HeroDataRow.MagicDefenseFactor;
            }
        }

        public float MagicDefenseFactorProgress
        {
            get
            {
                return NumericalCalcUtility.CalcProportion(Constant.Hero.MagicDefenseFactorMinVal, Constant.Hero.MagicDefenseFactorMaxVal, MagicDefenseFactor);
            }
        }

        #endregion Attribute tendencies

        #region Basic attributes

        public int CharacterId
        {
            get
            {
                return HeroDataRow.CharacterId;
            }
        }

        public string Name
        {
            get
            {
                return GameEntry.Localization.GetString(HeroDataRow.Name);
            }
        }

        public int Profession
        {
            get
            {
                return HeroDataRow.Profession;
            }
        }

        public int WeaponSuiteId
        {
            get
            {
                return HeroDataRow.DefaultWeaponSuiteId;
            }
        }

        public int ElementId
        {
            get
            {
                return HeroDataRow.ElementId;
            }
        }

        /// <summary>
        /// 基础生命值。
        /// </summary>
        public int MaxHPBase
        {
            get
            {
                return HeroBaseDataRow.MaxHPBase;
            }
        }

        /// <summary>
        /// 生命值。
        /// </summary>
        public int MaxHP
        {
            get;
            protected set;
        }

        public virtual int HP
        {
            get
            {
                return MaxHP;
            }
        }

        public int GetMaxHPAtStarLevel(int starLevel)
        {
            return Mathf.Max(1, Mathf.RoundToInt(MaxHPBase * MaxHPFactor * HeroDataRow.GetStarFactor(starLevel)));
        }

        public virtual float Steady
        {
            get
            {
                return HeroDataRow.Steady;
            }
        }

        public virtual float SteadyRecoverSpeed
        {
            get
            {
                return HeroDataRow.SteadyRecoverSpeed;
            }
        }

        public float Speed
        {
            get;
            protected set;
        }

        public int LevelUpExp
        {
            get
            {
                return HeroBaseDataRow.LevelUpExp;
            }
        }

        public float ExpProgress
        {
            get
            {
                return LevelUpExp > 0 ? (float)Exp / LevelUpExp : 0f;
            }
        }

        /// <summary>
        /// 英雄切换后的冷却时间缩减。
        /// </summary>
        public virtual float ReducedHeroSwitchCD
        {
            get
            {
                return 0f;
            }
        }

        /// <summary>
        /// 头像精灵图名称
        /// </summary>
        public string PortraitSpriteName
        {
            get
            {
                return IconDataRow.SpriteName;
            }
        }

        public string HeroDescription
        {
            get
            {
                return m_HeroDataRow.Description;
            }
        }

        /// <summary>
        /// 英雄战斗特点
        /// </summary>
        public string HeroFightCharacteristic
        {
            get
            {
                return m_HeroDataRow.FightCharacteristic;
            }
        }

        #endregion Basic attributes

        #region Attack plus

        /// <summary>
        /// 基础物理攻击。
        /// </summary>
        public int PhysicalAttackBase
        {
            get
            {
                return HeroBaseDataRow.PhysicalAttackBase;
            }
        }

        /// <summary>
        /// 物理攻击。
        /// </summary>
        public int PhysicalAttack
        {
            get;
            protected set;
        }

        public int GetPhysicalAttackAtStarLevel(int starLevel)
        {
            return Mathf.Max(0, Mathf.RoundToInt(PhysicalAttackBase * PhysicalAttackFactor * HeroDataRow.GetStarFactor(starLevel)));
        }

        /// <summary>
        /// 基础法术攻击。
        /// </summary>
        public int MagicAttackBase
        {
            get
            {
                return HeroBaseDataRow.MagicAttackBase;
            }
        }

        /// <summary>
        /// 法术攻击。
        /// </summary>
        public int MagicAttack
        {
            get;
            protected set;
        }

        public int GetMagicAttackAtStarLevel(int starLevel)
        {
            return Mathf.Max(0, Mathf.RoundToInt(MagicAttackBase * MagicAttackFactor * HeroDataRow.GetStarFactor(starLevel)));
        }

        /// <summary>
        /// 技能冷却时间缩减秒数。
        /// </summary>
        public virtual float ReducedSkillCoolDown
        {
            get
            {
                return 0f;
            }
        }

        /// <summary>
        /// 暴击率。
        /// </summary>
        public float CriticalHitProb
        {
            get;
            protected set;
        }

        /// <summary>
        /// 暴击伤害倍数。
        /// </summary>
        public float CriticalHitRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 物理伤害反击率。
        /// </summary>
        public float PhysicalAtkReflectRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 法术伤害反击率。
        /// </summary>
        public float MagicAtkReflectRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 降低对方物理防御百分比。
        /// </summary>
        public float OppPhysicalDfsReduceRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 降低对方法术防御百分比。
        /// </summary>
        public float OppMagicDfsReduceRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 附加伤害。
        /// </summary>
        public int AdditionalDamage
        {
            get;
            protected set;
        }

        #endregion Attack plus

        #region Defense plus

        /// <summary>
        /// 基础物理防御。
        /// </summary>
        public int PhysicalDefenseBase
        {
            get
            {
                return HeroBaseDataRow.PhysicalDefenseBase;
            }
        }

        /// <summary>
        /// 物理防御。
        /// </summary>
        public int PhysicalDefense
        {
            get;
            protected set;
        }

        public int GetPhysicalDefenseAtStarLevel(int starLevel)
        {
            return Mathf.Max(0, Mathf.RoundToInt(PhysicalDefenseBase * PhysicalDefenseFactor * HeroDataRow.GetStarFactor(starLevel)));
        }

        /// <summary>
        /// 基础法术防御。
        /// </summary>
        public int MagicDefenseBase
        {
            get
            {
                return HeroBaseDataRow.MagicDefenseBase;
            }
        }

        /// <summary>
        /// 法术防御。
        /// </summary>
        public int MagicDefense
        {
            get;
            protected set;
        }

        public int GetMagicDefenseAtStarLevel(int starLevel)
        {
            return Mathf.Max(0, Mathf.RoundToInt(MagicDefenseBase * MagicDefenseFactor * HeroDataRow.GetStarFactor(starLevel)));
        }

        /// <summary>
        /// 免除暴击率。
        /// </summary>
        public float AntiCriticalHitProb
        {
            get;
            protected set;
        }

        /// <summary>
        /// 物理伤害吸血率。
        /// </summary>
        public float PhysicalAtkHPAbsorbRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 法术伤害吸血率。
        /// </summary>
        public float MagicAtkHPAbsorbRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 受击伤害减免率。
        /// </summary>
        public float DamageReductionRate
        {
            get;
            protected set;
        }

        /// <summary>
        /// 待战回血。
        /// </summary>
        public float RecoverHP
        {
            get;
            protected set;
        }

        #endregion Defense plus

        /// <summary>
        /// 合成或升阶使用的道具（碎片）编号。
        /// </summary>
        public int StarLevelUpItemId
        {
            get
            {
                return HeroDataRow.StarLevelUpItemId;
            }
        }

        public int DefaultStarLevel
        {
            get
            {
                return HeroDataRow.DefaultStarLevel;
            }
        }

        public int DefaultMight
        {
            get
            {
                return HeroDataRow.DefaultMight;
            }
        }

        public int DefaultTotalQualityLevel
        {
            get
            {
                return HeroDataRow.DefaultTotalQualityLevel;
            }
        }

        public int[] WhereToGetIds
        {
            get
            {
                return HeroDataRow.WhereToGetIds;
            }
        }

        public string DetailDescription
        {
            get
            {
                return HeroDataRow.DetailDescription;
            }
        }

        public float CDAfterChangeHero
        {
            get
            {
                return HeroDataRow.CDAfterChangeHero;
            }
        }

        public int PiecesPerHero
        {
            get
            {
                return HeroDataRow.PiecesPerHero;
            }
        }

        public List<int> SkillLevels
        {
            get
            {
                return m_SkillLevels;
            }
            protected set
            {
                m_SkillLevels = value;
            }
        }

        public List<float> SkillExps
        {
            get
            {
                return m_SkillExps;
            }
            protected set
            {
                m_SkillExps = value;
            }
        }

        /// <summary>
        /// 获得技能等级。
        /// </summary>
        /// <param name="skillIndex"></param>
        /// <returns></returns>
        public int GetSkillLevel(int skillIndex)
        {
            return m_SkillLevels[skillIndex];
        }

        /// <summary>
        /// 获得技能经验。
        /// </summary>
        /// <param name="skillIndex"></param>
        /// <returns></returns>
        public float GetSkillExp(int skillIndex)
        {
            return m_SkillExps[skillIndex];
        }

        public IList<SkillBadgesData> GetAllSkillBadges()
        {
            return m_SkillBadges;
        }


        public SkillBadgesData GetSkillBadge(int skillIndex)
        {
            if (skillIndex < 0 || skillIndex >= m_SkillBadges.Count)
            {
                return null;
            }

            return m_SkillBadges[skillIndex];
        }

        private static IList<SkillBadgesData> s_EmptySkillBadges = null;

        public static IList<SkillBadgesData> GetEmptySkillBadges()
        {
            if (s_EmptySkillBadges != null)
            {
                return s_EmptySkillBadges;
            }

            s_EmptySkillBadges = new List<SkillBadgesData>();
            for (int i = 0; i < Constant.TotalSkillGroupCount; i++)
            {
                var skillBadgesData = new SkillBadgesData();
                var pb = new PBHeroSkillBadgesInfo { HasGenericBadgeIds = true, SpecificBadgeId = -1 };

                for (int j = 0; j < Constant.Hero.MaxBadgeSlotCountPerSkill; j++)
                {
                    pb.GenericBadgeIds.Add(-1);
                }

                skillBadgesData.UpdateData(pb);
                s_EmptySkillBadges.Add(skillBadgesData);
            }

            return s_EmptySkillBadges;
        }
    }
}
