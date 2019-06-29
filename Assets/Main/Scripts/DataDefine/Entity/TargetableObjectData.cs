using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class TargetableObjectData : EntityData, IBuffTargetData, IImpactDataProvider
    {
        [SerializeField]
        protected string m_Name;

        [SerializeField]
        protected int m_Level;

        [SerializeField]
        protected CampType m_Camp;

        [SerializeField]
        protected bool m_Dead;

        [SerializeField]
        protected int m_BaseMaxHP;

        [SerializeField]
        protected int m_MaxHP;

        [SerializeField]
        protected int m_HP;

        [SerializeField]
        protected int m_BasePhysicalAttack;

        [SerializeField]
        protected int m_PhysicalAttack;

        [SerializeField]
        protected int m_BasePhysicalDefense;

        [SerializeField]
        protected int m_PhysicalDefense;

        [SerializeField]
        protected int m_BaseMagicAttack;

        [SerializeField]
        protected int m_MagicAttack;

        [SerializeField]
        protected int m_BaseMagicDefense;

        [SerializeField]
        protected int m_MagicDefense;

        [SerializeField]
        protected float m_OppPhysicalDfsReduceRate;

        [SerializeField]
        protected float m_OppMagicDfsReduceRate;

        [SerializeField]
        private float m_PhysicalAtkHPAbsorbRate;

        [SerializeField]
        private float m_MagicAtkHPAbsorbRate;

        [SerializeField]
        private float m_PhysicalAtkReflectRate;

        [SerializeField]
        private float m_MagicAtkReflectRate;

        [SerializeField]
        private float m_DamageReductionRate;

        [SerializeField]
        private float m_CriticalHitProb;

        [SerializeField]
        private float m_CriticalHitRate;

        [SerializeField]
        private float m_AntiCriticalHitProb;

        [SerializeField]
        protected float m_DamageRandomRange;

        [SerializeField]
        protected int m_AdditionalDamage;

        [SerializeField]
        protected int m_MaterialType = -1;

        [SerializeField]
        protected bool m_AttackOwnerTarget = false;

        [SerializeField]
        protected bool m_DieWithOwner = false;

        [SerializeField]
        protected List<BuffData> m_BuffPoolData;

        [SerializeField]
        protected int m_ElementId = 0; // 默认为无属性。

        [SerializeField]
        protected int m_MinHP = 0;

        private Dictionary<long, int> m_BuffSerialIdsToColorChangeSerialIds = new Dictionary<long, int>();

        protected IBuffPool m_BuffPool = null;

        protected CharacterDataModifierType m_ModifierType;

        public TargetableObjectData(int entityId, CharacterDataModifierType modifierType = CharacterDataModifierType.Offline)
            : base(entityId)
        {
            m_Name = string.Format("[Entity {0}]", Id.ToString());
            m_BuffPoolData = new List<BuffData>();
            m_ModifierType = modifierType;
            if (m_BuffPool == null)
            {
                switch (m_ModifierType)
                {
                    case CharacterDataModifierType.Offline:
                        m_BuffPool = new OfflineBuffPool(this, GameEntry.DataTable.GetDataTable<DRBuff>(),
                            GameEntry.DataTable.GetDataTable<DRBuffReplace>(), Constant.Buff.MaxBuffPerBuffTarget, CheckCustomRuleForAddingBuff, CheckCustomRuleForRemovingBuffs);
                        break;
                    case CharacterDataModifierType.Online:
                        m_BuffPool = new OnlineBuffPool(this, GameEntry.DataTable.GetDataTable<DRBuff>(),
                            GameEntry.DataTable.GetDataTable<DRBuffReplace>(), Constant.Buff.MaxBuffPerBuffTarget, CheckCustomRuleForAddingBuff, CheckCustomRuleForRemovingBuffs);
                        break;
                }
            }
        }

        public new TargetableObject Entity
        {
            get
            {
                return base.Entity as TargetableObject;
            }
        }
        public CharacterDataModifierType ModifierType
        {
            get { return m_ModifierType; }
        }
        public IBuffPool BuffPool
        {
            set
            {
                m_BuffPool = value;
            }

            get
            {
                if (m_BuffPool == null)
                {
                    switch (m_ModifierType)
                    {
                        case CharacterDataModifierType.Offline:
                            m_BuffPool = new OfflineBuffPool(this, GameEntry.DataTable.GetDataTable<DRBuff>(),
                                GameEntry.DataTable.GetDataTable<DRBuffReplace>(), Constant.Buff.MaxBuffPerBuffTarget, CheckCustomRuleForAddingBuff, CheckCustomRuleForRemovingBuffs);
                            break;
                        case CharacterDataModifierType.Online:
                            m_BuffPool = new OnlineBuffPool(this, GameEntry.DataTable.GetDataTable<DRBuff>(),
                                GameEntry.DataTable.GetDataTable<DRBuffReplace>(), Constant.Buff.MaxBuffPerBuffTarget, CheckCustomRuleForAddingBuff, CheckCustomRuleForRemovingBuffs);
                            break;
                    }
                }
                return m_BuffPool;
            }
        }

        public IList<BuffData> Buffs
        {
            get
            {
                return m_BuffPoolData;
            }
        }

        /// <summary>
        /// 角色名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        /// <summary>
        /// 角色等级。
        /// </summary>
        public int Level
        {
            get
            {
                return m_Level;
            }
            set
            {
                m_Level = value;
            }
        }

        /// <summary>
        /// 角色阵营。
        /// </summary>
        public CampType Camp
        {
            get
            {
                return m_Camp;
            }
            set
            {
                m_Camp = value;
            }
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
            set
            {
                m_BaseMaxHP = value;
                RefreshMaxHP();
            }
        }

        /// <summary>
        /// 当前生命。
        /// </summary>
        public int HP
        {
            get
            {
                return m_HP;
            }
            set
            {
                m_HP = Mathf.Clamp(value, 0, MaxHP);
            }
        }

        /// <summary>
        /// 当前是否死亡。
        /// </summary>
        public bool IsDead
        {
            get
            {
                return m_Dead;
            }
            set
            {
                m_Dead = value;
            }
        }

        /// <summary>
        /// 用于伤害计算的状态。
        /// </summary>
        public abstract StateForImpactCalc StateForImpactCalc
        {
            get;
        }

        /// <summary>
        /// 生命百分比。以当前最大生命值作为分母。
        /// </summary>
        public float HPRatio
        {
            get
            {
                return m_MaxHP > 0 ? (float)m_HP / m_MaxHP : 0f;
            }
        }

        /// <summary>
        /// 生命百分比。以基础生命最大值作为分母。
        /// </summary>
        public float BaseHPRatio
        {
            get
            {
                return m_BaseMaxHP > 0 ? (float)m_HP / m_BaseMaxHP : 0f;
            }
        }

        /// <summary>
        /// 物理攻击。
        /// </summary>
        public int PhysicalAttack
        {
            get
            {
                return m_PhysicalAttack;
            }
            set
            {
                m_BasePhysicalAttack = value;
                RefreshPhysicalAttack();
            }
        }

        /// <summary>
        /// 物理防御。
        /// </summary>
        public int PhysicalDefense
        {
            get
            {
                return m_PhysicalDefense;
            }
            set
            {
                m_BasePhysicalDefense = value;
                RefreshPhysicalDefense();
            }
        }

        /// <summary>
        /// 法术攻击。
        /// </summary>
        public int MagicAttack
        {
            get
            {
                return m_MagicAttack;
            }
            set
            {
                m_BaseMagicAttack = value;
                RefreshMagicAttack();
            }
        }

        /// <summary>
        /// 法术防御。
        /// </summary>
        public int MagicDefense
        {
            get
            {
                return m_MagicDefense;
            }
            set
            {
                m_BaseMagicDefense = value;
                RefreshMagicDefense();
            }
        }

        /// <summary>
        /// 降低对方物理防御百分比。
        /// </summary>
        public float OppPhysicalDfsReduceRate
        {
            get
            {
                return m_OppPhysicalDfsReduceRate;
            }
            set
            {
                m_OppPhysicalDfsReduceRate = value;
            }
        }

        /// <summary>
        /// 降低对方法术防御百分比。
        /// </summary>
        public float OppMagicDfsReduceRate
        {
            get
            {
                return m_OppMagicDfsReduceRate;
            }
            set
            {
                m_OppMagicDfsReduceRate = value;
            }
        }

        /// <summary>
        /// 物理伤害吸血率。
        /// </summary>
        public float PhysicalAtkHPAbsorbRate
        {
            get
            {
                return m_PhysicalAtkHPAbsorbRate;
            }
            set
            {
                m_PhysicalAtkHPAbsorbRate = value;
            }
        }

        /// <summary>
        /// 法术伤害吸血率。
        /// </summary>
        public float MagicAtkHPAbsorbRate
        {
            get
            {
                return m_MagicAtkHPAbsorbRate;
            }
            set
            {
                m_MagicAtkHPAbsorbRate = value;
            }
        }

        /// <summary>
        /// 物理伤害反击率。
        /// </summary>
        public float PhysicalAtkReflectRate
        {
            get
            {
                return m_PhysicalAtkReflectRate;
            }
            set
            {
                m_PhysicalAtkReflectRate = value;
            }
        }

        /// <summary>
        /// 法术伤害反击率。
        /// </summary>
        public float MagicAtkReflectRate
        {
            get
            {
                return m_MagicAtkReflectRate;
            }
            set
            {
                m_MagicAtkReflectRate = value;
            }
        }

        /// <summary>
        /// 受击伤害减免率。
        /// </summary>
        public float DamageReductionRate
        {
            get
            {
                return m_DamageReductionRate;
            }
            set
            {
                m_DamageReductionRate = value;
            }
        }

        /// <summary>
        /// 暴击率。
        /// </summary>
        public float CriticalHitProb
        {
            get
            {
                return m_CriticalHitProb;
            }
            set
            {
                m_CriticalHitProb = value;
            }
        }

        /// <summary>
        /// 暴击伤害倍数。
        /// </summary>
        public float CriticalHitRate
        {
            get
            {
                return m_CriticalHitRate;
            }
            set
            {
                m_CriticalHitRate = value;
            }
        }

        /// <summary>
        /// 免除暴击率。
        /// </summary>
        public float AntiCriticalHitProb
        {
            get
            {
                return m_AntiCriticalHitProb;
            }
            set
            {
                m_AntiCriticalHitProb = value;
            }
        }

        /// <summary>
        /// 伤害浮动率。
        /// </summary>
        public float DamageRandomRate
        {
            get
            {
                return m_DamageRandomRange;
            }
            set
            {
                m_DamageRandomRange = value;
            }
        }

        /// <summary>
        /// 附加伤害。
        /// </summary>
        public int AdditionalDamage
        {
            get
            {
                return m_AdditionalDamage;
            }
            set
            {
                m_AdditionalDamage = value;
            }
        }

        /// <summary>
        /// 材质类型。
        /// </summary>
        public int MaterialType
        {
            get
            {
                return m_MaterialType;
            }
            set
            {
                m_MaterialType = value;
            }
        }

        /// <summary>
        /// 是否直接攻击所有者目标。
        /// </summary>
        public bool AttackOwnerTarget
        {
            get
            {
                return m_AttackOwnerTarget;
            }
            set
            {
                m_AttackOwnerTarget = value;
            }
        }

        /// <summary>
        /// 是否跟随所有者死亡。
        /// </summary>
        public bool DieWithOwner
        {
            get
            {
                return m_DieWithOwner;
            }
            set
            {
                m_DieWithOwner = value;
            }
        }

        public int ElementId
        {
            get
            {
                return m_ElementId;
            }
            set
            {
                m_ElementId = value;
            }
        }

        /// <summary>
        /// 最低血量。
        /// </summary>
        public int MinHP
        {
            get
            {
                return m_MinHP;
            }
            set
            {
                m_MinHP = value;
            }
        }

        public virtual void RefreshProperties()
        {
            RefreshMaxHP();
            RefreshPhysicalAttack();
            RefreshPhysicalDefense();
            RefreshMagicAttack();
            RefreshMagicDefense();
        }

        protected bool RefreshFloatProperty(IList<BuffType> buffTypes, float baseValue, ref float value)
        {
            float timesIncreased = 0f;
            float valueIncreased = 0;
            for (int i = 0; i < Buffs.Count; ++i)
            {
                var buff = Buffs[i];

                if (!buffTypes.Contains(buff.BuffType))
                {
                    continue;
                }

                switch (Mathf.RoundToInt(buff.Params[0]))
                {
                    case 1:
                        timesIncreased += buff.Params[1];
                        break;
                    case 2:
                        valueIncreased += buff.Params[1];
                        break;
                    default:
                        continue;
                }
            }

            float oldValue = value;
            value = Mathf.Max(0f, NumericalCalcUtility.CalcFloatProperty2(baseValue, valueIncreased, timesIncreased));
            return oldValue != value;
        }

        protected bool RefreshIntProperty(IList<BuffType> buffTypes, int baseValue, ref int value)
        {
            float timesIncreased = 0f;
            int valueIncreased = 0;
            for (int i = 0; i < Buffs.Count; ++i)
            {
                var buff = Buffs[i];

                if (!buffTypes.Contains(buff.BuffType))
                {
                    continue;
                }

                switch (Mathf.RoundToInt(buff.Params[0]))
                {
                    case 1:
                        timesIncreased += buff.Params[1];
                        break;
                    case 2:
                        valueIncreased += Mathf.RoundToInt(buff.Params[1]);
                        break;
                    default:
                        continue;
                }
            }

            float oldValue = value;
            value = Mathf.Max(0, NumericalCalcUtility.CalcIntProperty2(baseValue, valueIncreased, timesIncreased));
            return oldValue != value;
        }

        protected void RefreshMaxHP()
        {
            if (RefreshIntProperty(new BuffType[] { BuffType.ChangeMaxHP }, m_BaseMaxHP, ref m_MaxHP))
            {
                if (m_HP > m_MaxHP)
                {
                    m_HP = m_MaxHP;
                }

                GameEntry.Event.Fire(this, new CharacterPropertyChangeEventArgs(Id));
            }
        }

        protected void RefreshPhysicalAttack()
        {
            if (RefreshIntProperty(new BuffType[] { BuffType.ChangePhysicalAttack }, m_BasePhysicalAttack, ref m_PhysicalAttack))
            {
                GameEntry.Event.Fire(this, new CharacterPropertyChangeEventArgs(Id));
            }
        }

        protected void RefreshPhysicalDefense()
        {
            if (RefreshIntProperty(new BuffType[] { BuffType.ChangePhysicalDefense }, m_BasePhysicalDefense, ref m_PhysicalDefense))
            {
                GameEntry.Event.Fire(this, new CharacterPropertyChangeEventArgs(Id));
            }
        }

        protected void RefreshMagicAttack()
        {
            if (RefreshIntProperty(new BuffType[] { BuffType.ChangeMagicAttack }, m_BaseMagicAttack, ref m_MagicAttack))
            {
                GameEntry.Event.Fire(this, new CharacterPropertyChangeEventArgs(Id));
            }
        }

        protected void RefreshMagicDefense()
        {
            if (RefreshIntProperty(new BuffType[] { BuffType.ChangeMagicDefense }, m_BaseMagicDefense, ref m_MagicDefense))
            {
                GameEntry.Event.Fire(this, new CharacterPropertyChangeEventArgs(Id));
            }
        }

        #region IBuffTarget

        private List<BuffData> m_BuffAddeds = new List<BuffData>();
        private List<BuffData> m_BuffRemoveds = new List<BuffData>();

        public void UpdateBuff()
        {
            if (m_BuffAddeds.Count > 0)
            {
                for (int i = 0; i < m_BuffAddeds.Count; i++)
                {
                    OnBuffPoolChanged(m_BuffAddeds[i], null);
                }
                m_BuffAddeds.Clear();
            }

            if (m_BuffRemoveds.Count > 0)
            {
                OnBuffPoolChanged(null, m_BuffRemoveds);
                m_BuffRemoveds = new List<BuffData>();
            }
        }

        public virtual void OnBuffPoolChanged(BuffData added, IList<BuffData> removed)
        {
            ChangeBuffData(added);

            if (Entity == null)
            {
                if (added != null)
                {
                    m_BuffAddeds.Add(added);
                }

                if (removed != null && removed.Count > 0)
                {
                    m_BuffRemoveds.AddRange(removed);
                }
            }
            else
            {
                int oldMaxHP = MaxHP;
                RefreshProperties();
                int deltaMaxHP = MaxHP - oldMaxHP;
                LogBuffPool(added, removed);
                OnChangeMaxHPBuffAdded(added, deltaMaxHP);
                OnColorChangeBuffAdded(added);
                OnColorChangeBuffsRemoved(removed);
            }
        }

        private void ChangeBuffData(BuffData buffData)
        {
            if (buffData == null)
            {
                return;
            }

            var userDataDict = buffData.UserData as Dictionary<string, object>;
            object skillBadgesObj;
            if (userDataDict == null || !userDataDict.TryGetValue(Constant.Buff.UserData.OwnerSkillBadgesKey, out skillBadgesObj))
            {
                return;
            }

            var skillBadges = skillBadgesObj as SkillBadgesData;
            if (skillBadges == null)
            {
                return;
            }

            var genericBadges = skillBadges.GenericBadges;
            for (int i = 0; i < genericBadges.Count; ++i)
            {
                var badgeId = genericBadges[i].BadgeId;
                if (badgeId < 0)
                {
                    continue;
                }

                var drBadge = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>().GetDataRow(badgeId);
                if (drBadge == null)
                {
                    continue;
                }

                if (drBadge.ElementId == (int)HeroElementType.Fire && buffData.ElementId == (int)HeroElementType.Fire
                    || drBadge.ElementId == (int)HeroElementType.Thunder && buffData.ElementId == (int)HeroElementType.Thunder
                    || drBadge.ElementId == (int)HeroElementType.Light && buffData.ElementId == (int)HeroElementType.Light)
                {
                    buffData.IncreaseDuration(drBadge.Params[0]);
                }

                if (drBadge.ElementId == (int)HeroElementType.Thunder
                    && buffData.ElementId == (int)HeroElementType.Thunder
                    && buffData.BuffType == BuffType.ChangeSpeed)
                {
                    buffData.Params[1] *= (1 + drBadge.Params[1]);
                }
            }
        }

        public virtual void OnBuffHeartBeat(long serialId, BuffData buff)
        {
            if (Entity == null)
            {
                return;
            }

            if (buff.DRBuff.BuffType == (int)BuffType.PeriodicalImpact)
            {
                List<KeyValuePair<int, BuffType>> impactIdWithBuffConditions = new List<KeyValuePair<int, BuffType>>();
                for (int i = 0; i < buff.DRBuff.BuffParams.Length; ++i)
                {
                    var impactId = Mathf.RoundToInt(buff.DRBuff.BuffParams[i]);
                    if (impactId > 0)
                    {
                        impactIdWithBuffConditions.Add(new KeyValuePair<int, BuffType>(impactId, BuffType.Undefined));
                    }
                }

                ICampable buffOwner = null;
                UnityGameFramework.Runtime.Entity buffOwnerEntity = null;

                if (buff.HasOwner)
                {
                    buffOwnerEntity = GameEntry.Entity.GetEntity(buff.OwnerData.Id);
                }

                if (buffOwnerEntity != null)
                {
                    buffOwner = buffOwnerEntity.Logic as ICampable;
                }

                SkillBadgesData skillBadges = null;
                var userDataDict = buff.UserData as IDictionary<string, object>;
                if (userDataDict != null)
                {
                    object skillBadgesObj;
                    userDataDict.TryGetValue(Constant.Buff.UserData.OwnerSkillBadgesKey, out skillBadgesObj);
                    skillBadges = skillBadgesObj as SkillBadgesData;
                }

                GameEntry.Impact.PerformImpacts(buffOwner, buff.OwnerData, Entity, this, ImpactSourceType.Buff, impactIdWithBuffConditions.ToArray(),
                    null, null, null, skillBadges, null, new ImpactAuxData { CausingBuffId = buff.BuffId, CausingBuffData = buff });
            }
        }

        private void OnChangeMaxHPBuffAdded(BuffData added, int deltaMaxHP)
        {
            if (added != null && added.BuffType == BuffType.ChangeMaxHP)
            {
                if (Mathf.RoundToInt(added.Params[2]) > 0 && deltaMaxHP > 0)
                {
                    HP += deltaMaxHP;
                }
            }
        }

        private void OnColorChangeBuffAdded(BuffData added)
        {
            if (added == null || Entity == null)
            {
                return;
            }

            int colorChangeId = added.ColorChangeId;
            if (colorChangeId > 0)
            {
                int serialId = Entity.StartColorChange(colorChangeId);
                if (serialId >= 0)
                {
                    if (m_BuffSerialIdsToColorChangeSerialIds.ContainsKey(added.SerialId))
                    {
                        m_BuffSerialIdsToColorChangeSerialIds[added.SerialId] = serialId;
                    }
                    else
                    {
                        m_BuffSerialIdsToColorChangeSerialIds.Add(added.SerialId, serialId);
                    }
                }
            }
        }

        private void OnColorChangeBuffsRemoved(IList<BuffData> removed)
        {
            if (removed == null || Entity == null)
            {
                return;
            }

            for (int i = 0; i < removed.Count; ++i)
            {
                long buffSerialId = removed[i].SerialId;
                if (m_BuffSerialIdsToColorChangeSerialIds.ContainsKey(buffSerialId))
                {
                    int colorChangeSerialId = m_BuffSerialIdsToColorChangeSerialIds[buffSerialId];
                    Entity.StopColorChange(colorChangeSerialId);
                    m_BuffSerialIdsToColorChangeSerialIds.Remove(buffSerialId);
                }
            }
        }

        private bool CheckCustomRuleForAddingBuff(BuffData buffData)
        {
            // 如有无敌 Buff，禁止加入减益 Buff。
            if (buffData.GoodOrBad < 0)
            {
                var currentBuffs = m_BuffPool.Buffs;
                for (int i = 0; i < currentBuffs.Length; i++)
                {
                    if (currentBuffs[i].BuffType == BuffType.StateAndNumHarmFree)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private IList<BuffData> CheckCustomRuleForRemovingBuffs(BuffData added)
        {
            if (added == null)
            {
                return null;
            }
            List<BuffData> toRemove = new List<BuffData>();
            // 新增了无敌 Buff 则去掉所有减益 Buff。
            if (added.BuffType == BuffType.StateAndNumHarmFree)
            {
                var currentBuffs = m_BuffPool.Buffs;
                for (int i = 0; i < currentBuffs.Length; i++)
                {
                    if (currentBuffs[i].GoodOrBad < 0)
                    {
                        toRemove.Add(currentBuffs[i]);
                    }
                }
            }

            return toRemove;
        }

        [System.Diagnostics.Conditional("LOG_BUFF_POOL")]
        private void LogBuffPool(BuffData added, IList<BuffData> removed)
        {
            if (Entity == null)
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Entity Id: {0}\n", Id);

            if (added != null)
            {
                sb.AppendFormat("Added buff: {0}\n", added.BuffId);
            }

            if (removed != null)
            {
                sb.Append("Removed buff(s): [");
                for (int i = 0; i < removed.Count; ++i)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(removed[i].BuffId);
                }

                sb.Append("]\n");
            }

            var currentBuffs = m_BuffPool.Buffs;
            if (currentBuffs.Length > 0)
            {
                sb.Append("Current buff(s): [");
                for (int i = 0; i < currentBuffs.Length; ++i)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(currentBuffs[i].BuffId);
                }

                sb.Append("]\n");
            }

            Log.Info(sb.ToString());
        }

        #endregion IBuffTarget
    }
}
