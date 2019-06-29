using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        private class HPDamageImpact : ImpactBase
        {
            private const int ColIndex_NormalImpactNumberTypeByMe = 0;
            private const int ColIndex_CriticalImpactNumberTypeByMe = 1;
            private const int ColIndex_RecoverHPNumberTypeByMe = 2;
            private const int ColIndex_SkillRecoverHPNumberTypeByMe = 3;
            private const int ColIndex_CounterHPNumberTypeByMe = 4;
            private const int ColIndex_NormalImpactNumberTypeByOther = 5;
            private const int ColIndex_CriticalImpactNumberTypeByOther = 6;
            private const int ColIndex_RecoverHPNumberTypeByOther = 7;
            private const int ColIndex_SkillRecoverHPNumberTypeByOther = 8;
            private const int ColIndex_CounterHPNumberTypeByOther = 9;
            private const int ColIndex_PhysicalAttackBase = 10;
            private const int ColIndex_PhysicalAttackRate = 11;
            private const int ColIndex_PhysicalAttackRateDeltaPerLevel = 12;
            private const int ColIndex_PhysicalAttackDamage = 13;
            private const int ColIndex_PhysicalAttackDamageDeltaPerLevel = 14;
            private const int ColIndex_MagicAttackBase = 15;
            private const int ColIndex_MagicAttackRate = 16;
            private const int ColIndex_MagicAttackDamage = 17;
            private const int ColIndex_AdditionalDamageRatio = 18;
            private const int ColIndex_AdditionalMaxHPDamageRatio = 19;
            private const int ColIndex_SkillRecoverHPRatio = 20;
            private const int ColIndex_SkillRecoverHPMaxCount = 21;

            public override int Type
            {
                get
                {
                    return (int)ImpactType.HPDamage;
                }
            }

            public override BaseApplyImpactData PerformImpact(BasePerformImpactData impactData)
            {
                OtherPerformImpactData data = impactData as OtherPerformImpactData;
                if (data == null)
                {
                    Log.Error("HPDamageImpact's PerformImpact's data is nulls.");
                    return null;
                }

                if (GameEntry.Impact.ShouldScreenHPDamage != null && GameEntry.Impact.ShouldScreenHPDamage(data.Target))
                {
                    return null;
                }

                var targetData = data.TargetData as TargetableObjectData;
                var target = data.Target as TargetableObject;
                if (target.HasNumHarmFreeBuff)
                {
                    GameEntry.Impact.CreateHudText(Constant.Buff.NumHarmFreeHudTextType, target.CachedTransform.position, GameEntry.Localization.GetString("HP_HARM_FREE"), target);
                    return null;
                }

                ImpactDamageData impactDamageData = CalculateImpactDamage(
                    skillIndex: impactData.SkillIndex,//技能编号
                    skillLevel: impactData.SkillLevel,//技能等级
                    skillBadges: impactData.SkillBadges,//技能徽章
                    originData: data.OriginData as IImpactDataProvider,//人物的基础属性
                    targetData: targetData,//目标的基础属性
                    auxData: data.AuxData,//?
                    physicalAttackBase: Mathf.RoundToInt(data.ImpactParams[ColIndex_PhysicalAttackBase]),//
                    physicalAttackRate: data.ImpactParams[ColIndex_PhysicalAttackRate],
                    physicalAttackRateDeltaPerLevel: data.ImpactParams[ColIndex_PhysicalAttackRateDeltaPerLevel],
                    physicalAttackDamage: Mathf.RoundToInt(data.ImpactParams[ColIndex_PhysicalAttackDamage]),
                    physicalAttackDamageDeltaPerLevel: Mathf.RoundToInt(data.ImpactParams[ColIndex_PhysicalAttackDamageDeltaPerLevel]),
                    magicAttackBase: Mathf.RoundToInt(data.ImpactParams[ColIndex_MagicAttackBase]),
                    magicAttackRate: data.ImpactParams[ColIndex_MagicAttackRate],
                    magicAttackDamage: Mathf.RoundToInt(data.ImpactParams[ColIndex_MagicAttackDamage]),
                    additionalDamageRatio: data.ImpactParams[ColIndex_AdditionalDamageRatio],
                    additionalMaxHPDamageRatio: data.ImpactParams[ColIndex_AdditionalMaxHPDamageRatio],
                    skillRecoverHPRatio: data.ImpactParams[ColIndex_SkillRecoverHPRatio],
                    skillRecoverHPMaxCount: Mathf.RoundToInt(data.ImpactParams[ColIndex_SkillRecoverHPMaxCount]),
                    sourceType: impactData.SourceType,
                    buffCondition: impactData.BuffCondition);

                var applyImpactData = BaseApplyImpactData.Create<ApplyHPDamageImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);
                applyImpactData.DamageHP = impactDamageData.DamageHP;
                applyImpactData.RecoverHP = impactDamageData.RecoverHP;
                applyImpactData.SkillRecoverHP = impactDamageData.SkillRecoverHP;
                applyImpactData.CounterHP = impactDamageData.CounterHP;
                applyImpactData.IsCritical = impactDamageData.IsCritical;

                return applyImpactData;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var data = impactData as ApplyHPDamageImpactData;
                if (data == null)
                {
                    Log.Error("ApplyHPDamageImpactData is invalid.");
                    return;
                }

                var targetData = impactData.TargetData as TargetableObjectData;
                if (targetData == null)
                {
                    Log.Error("Targetable object data is invalid.");
                    return;
                }

                var isMeAttackOpp = AIUtility.GetRelation(GameEntry.SceneLogic.BaseInstanceLogic.MyCamp, targetData.Camp) != RelationType.Friendly;

                if (targetData.MinHP > 0 && targetData.HP - data.DamageHP < targetData.MinHP)
                {
                    targetData.HP = targetData.MinHP;
                }
                else
                {
                    targetData.HP -= data.DamageHP;
                }

                if (targetData.HP < 0)
                {
                    targetData.HP = 0;
                }

                var target = targetData.Entity;
                if (target != null && target.Motion != null)
                {
                    if (data.DamageHP > 0)
                    {
                        target.Motion.PerformHPDamage(data.OriginData != null ? data.OriginData.Entity : null, data.SourceType);
                        int hudTextType = Mathf.RoundToInt(data.IsCritical
                            ? data.DataRow.ImpactParams[isMeAttackOpp ? ColIndex_CriticalImpactNumberTypeByMe : ColIndex_CriticalImpactNumberTypeByOther]
                            : data.DataRow.ImpactParams[isMeAttackOpp ? ColIndex_NormalImpactNumberTypeByMe : ColIndex_NormalImpactNumberTypeByOther]);
                        if (hudTextType >= 0)
                        {
                            GameEntry.Impact.CreateHudText(hudTextType, target.CachedTransform.position, data.DamageHP.ToString(), target);
                        }

                        if (data.OriginData != null)
                        {
                            GameEntry.SceneLogic.BaseInstanceLogic.Stat.RecordDamage(data.OriginData.Id, data.DamageHP);
                        }

                        if (target.NeedShowHPBarOnDamage)
                        {
                            GameEntry.Impact.CreateNameBoard(target, NameBoardMode.HPBarOnly);
                        }
                    }
                }
                else
                {
                    //Log.Error("Targetable object or motion '{0}' is invalid.", targetData.Id.ToString());
                }

                var originData = impactData.OriginData as TargetableObjectData;
                if (originData == null)
                {
                    return;
                }

                originData.HP += data.RecoverHP - data.CounterHP + data.SkillRecoverHP;
                if (originData.HP < 0)
                {
                    originData.HP = 0;
                }
                else if (originData.HP > originData.MaxHP)
                {
                    originData.HP = originData.MaxHP;
                }

                var origin = originData.Entity;
                if (origin != null && origin.Motion != null)
                {
                    if (data.RecoverHP > 0)
                    {
                        int hudTextType = Mathf.RoundToInt(data.DataRow.ImpactParams[isMeAttackOpp ? ColIndex_RecoverHPNumberTypeByMe : ColIndex_RecoverHPNumberTypeByOther]);
                        if (hudTextType >= 0)
                        {
                            GameEntry.Impact.CreateHudText(hudTextType, origin.CachedTransform.position, "+" + data.RecoverHP.ToString(), origin);
                        }
                    }

                    if (data.SkillRecoverHP > 0)
                    {
                        int hudTextType = Mathf.RoundToInt(data.DataRow.ImpactParams[isMeAttackOpp ? ColIndex_SkillRecoverHPNumberTypeByMe : ColIndex_SkillRecoverHPNumberTypeByOther]);
                        if (hudTextType >= 0)
                        {
                            GameEntry.Impact.CreateHudText(hudTextType, origin.CachedTransform.position, "+" + data.SkillRecoverHP.ToString(), origin);
                        }
                    }

                    if (data.CounterHP > 0)
                    {
                        origin.Motion.PerformHPDamage(data.TargetData.Entity, data.SourceType);
                        int hudTextType = Mathf.RoundToInt(data.DataRow.ImpactParams[isMeAttackOpp ? ColIndex_CounterHPNumberTypeByMe : ColIndex_CounterHPNumberTypeByOther]);
                        if (hudTextType >= 0)
                        {
                            GameEntry.Impact.CreateHudText(hudTextType, origin.CachedTransform.position, data.CounterHP.ToString(), origin);
                        }
                    }
                }
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var data = impactData as ApplyHPDamageImpactData;
                packet.HPDamageImpacts.Add(new PBHPDamageImpact
                {
                    ImpactId = data.DataRow.Id,
                    CounterHP = data.CounterHP,
                    DamageHP = data.DamageHP,
                    IsCritical = data.IsCritical,
                    RecoverHP = data.RecoverHP,
                    SkillRecoverHP = data.SkillRecoverHP,
                });
            }

            private ImpactDamageData CalculateImpactDamage(int skillIndex, int skillLevel, SkillBadgesData skillBadges, IImpactDataProvider originData, IImpactDataProvider targetData, ImpactAuxData auxData,
                int physicalAttackBase, float physicalAttackRate, float physicalAttackRateDeltaPerLevel, int physicalAttackDamage, int physicalAttackDamageDeltaPerLevel,
                int magicAttackBase, float magicAttackRate, int magicAttackDamage,
                float additionalDamageRatio, float additionalMaxHPDamageRatio, float skillRecoverHPRatio, int skillRecoverHPMaxCount, ImpactSourceType sourceType, BuffType buffCondition)
            {
                LogInfo("CalculateImpactDamage: skillIndex={0}, skillLevel={1}, 'skillBadges == null'={2}, physicalAttackBase={3}, physicalAttackRate={4}, physicalAttackRateDeltaPerLevel={5}, "
                    + "physicalAttackDamage={6}, physicalAttackDamageDeltaPerLevel={7}, magicAttackBase={8}, magicAttackRate={9}, magicAttackDamage={10}, additionalDamageRatio={11}, buffCondition={12}."
                    + "additionalMaxHPDamageRatio={12}, skillRecoverHPRatio={13}, skillRecoverHPMaxCount={14}, sourceType={15}.",
                    skillIndex.ToString(), skillLevel.ToString(), (skillBadges == null).ToString(), physicalAttackBase.ToString(), physicalAttackRate.ToString(), physicalAttackRateDeltaPerLevel.ToString(),
                    physicalAttackDamage.ToString(), physicalAttackDamageDeltaPerLevel.ToString(), magicAttackBase.ToString(), magicAttackRate.ToString(), magicAttackDamage.ToString(), additionalDamageRatio.ToString(),
                    additionalMaxHPDamageRatio.ToString(), skillRecoverHPRatio.ToString(), skillRecoverHPMaxCount.ToString(), sourceType.ToString(), buffCondition.ToString());

                if (originData == null)
                {
                    return new ImpactDamageData(0, 0, 0, 0, false);
                }

                // 受技能影响后的物理攻击力
                int physicalAttack = Mathf.Max(0, originData.PhysicalAttack + physicalAttackBase);
                // 受技能影响后的法术攻击力
                int magicAttack = Mathf.Max(0, originData.MagicAttack + magicAttackBase);

                SkillBadgesCausedData skillBadgesCausedData = GetSkillBadgesCausedData(skillBadges, sourceType, auxData.CausingBuffData, buffCondition);
                LogInfo("CalculateImpactDamage: genericSkillBadgesCausedData=[{0}].", skillBadgesCausedData.ToString());

                float temp = 0f;

                // 基础物理伤害
                temp = physicalAttack + targetData.PhysicalDefense * Mathf.Max(0f, (1f - originData.OppPhysicalDfsReduceRate - skillBadgesCausedData.OppPhysicalDfsReduceRate));
                float basePhysicalAttackDamage = 0f;
                if (temp > 0f)
                {
                    basePhysicalAttackDamage = physicalAttack / temp
                        * (physicalAttackRate + (skillLevel - 1) * physicalAttackRateDeltaPerLevel)
                        * (1 + skillBadgesCausedData.PhysicalAtkIncreaseRate)
                        * (1f + Random.Range(-originData.DamageRandomRate, originData.DamageRandomRate));
                    basePhysicalAttackDamage *= physicalAttack;
                    basePhysicalAttackDamage += physicalAttackDamage + (skillLevel - 1) * physicalAttackDamageDeltaPerLevel;
                    basePhysicalAttackDamage *= 1f - targetData.DamageReductionRate;
                    basePhysicalAttackDamage *= (1 + skillBadgesCausedData.ElementBuffPhysicalAtkDamageIncreaseRate + skillBadgesCausedData.SpecificBadgePhysicalAtkDamageIncreaseRate);
                    basePhysicalAttackDamage = Mathf.Max(0f, basePhysicalAttackDamage);
                    LogInfo("CalculateImpactDamage: basePhysicalAttackDamage={0}.", basePhysicalAttackDamage.ToString());
                }

                // 基础法术伤害
                temp = magicAttack + targetData.MagicDefense * (1f - originData.OppMagicDfsReduceRate);
                float baseMagicAttackDamage = 0f;
                if (temp > 0f)
                {
                    baseMagicAttackDamage = Mathf.Max(0f, (magicAttack / temp
                        * magicAttackRate
                        * (1f + Random.Range(-originData.DamageRandomRate, originData.DamageRandomRate))
                        * magicAttack + magicAttackDamage) * (1f - targetData.DamageReductionRate));
                    LogInfo("CalculateImpactDamage: baseMagicAttackDamage={0}.", baseMagicAttackDamage.ToString());
                }

                // 克制类属性效果。
                float elementRestrainedValue = GameEntry.DataTable.GetElementValue(originData.ElementId, targetData.ElementId);
                LogInfo("CalculateImpactDamage: elementRestrainedValue={0}.", elementRestrainedValue.ToString());

                // 基础伤害值
                float baseDamage = (basePhysicalAttackDamage + baseMagicAttackDamage) * (1 + elementRestrainedValue);
                LogInfo("CalculateImpactDamage: baseDamage={0}.", baseDamage.ToString());

                // 攻击方吸收血量
                int recoverHP = Mathf.RoundToInt((basePhysicalAttackDamage * originData.PhysicalAtkHPAbsorbRate + baseMagicAttackDamage * originData.MagicAtkHPAbsorbRate) * (1 + elementRestrainedValue));
                LogInfo("CalculateImpactDamage: recoverHP={0}.", recoverHP.ToString());

                // 技能吸血值
                int skillRecoverHP = 0;
                if (skillRecoverHPMaxCount < 0 || auxData.SkillRecoverHPCount <= skillRecoverHPMaxCount)
                {
                    skillRecoverHP = Mathf.RoundToInt(baseDamage * skillRecoverHPRatio * (1 + skillBadgesCausedData.ElementSkillRecoverHPRate));
                }

                if (skillRecoverHP > 0)
                {
                    ++auxData.SkillRecoverHPCount;
                }
                LogInfo("CalculateImpactDamage: skillRecoverHP={0}.", skillRecoverHP.ToString());

                // 防守方反击血量
                int counterHP = Mathf.RoundToInt((basePhysicalAttackDamage * targetData.PhysicalAtkReflectRate + baseMagicAttackDamage * targetData.MagicAtkReflectRate) * (1 - elementRestrainedValue));
                LogInfo("CalculateImpactDamage: counterHP={0}.", counterHP.ToString());

                // 处理暴击伤害
                float criticalHitProb = Mathf.Max(0f, originData.CriticalHitProb + skillBadgesCausedData.CriticalHitProb - targetData.AntiCriticalHitProb);
                LogInfo("CalculateImpactDamage: criticalHitProb={0}.", criticalHitProb.ToString());
                bool isCritical = Random.value < criticalHitProb;
                if (isCritical)
                {
                    baseDamage *= (1f + originData.CriticalHitRate + skillBadgesCausedData.CriticalHitRate);
                }
                LogInfo("CalculateImpactDamage: isCritical={0}.", isCritical.ToString());

                // 处理状态伤害
                baseDamage *= GetStateForImpactRatio(targetData.StateForImpactCalc);
                LogInfo("CalculateImpactDamage: baseDamage={0}.", baseDamage.ToString());

                // 总伤害
                int damageHP = Mathf.Max(1, Mathf.RoundToInt(baseDamage + originData.AdditionalDamage * additionalDamageRatio + targetData.MaxHP * additionalMaxHPDamageRatio));
                LogInfo("CalculateImpactDamage: damageHP={0}.", damageHP.ToString());

                return new ImpactDamageData(damageHP, recoverHP, skillRecoverHP, counterHP, isCritical);
            }

            private SkillBadgesCausedData GetSkillBadgesCausedData(SkillBadgesData skillBadges, ImpactSourceType sourceType, BuffData buffData, BuffType buffCondition)
            {
                var ret = new SkillBadgesCausedData();
                if (skillBadges == null)
                {
                    return ret;
                }

                int specificBadgeId = skillBadges.SpecificBadge.BadgeId;
                if (specificBadgeId > 0)
                {
                    var drSpecificBadge = GameEntry.DataTable.GetDataTable<DRSpecificSkillBadge>().GetDataRow(specificBadgeId);
                    if (drSpecificBadge != null && sourceType != ImpactSourceType.Buff)
                    {
                        ret.SpecificBadgePhysicalAtkDamageIncreaseRate += drSpecificBadge.DamageIncreaseRate;
                    }
                }

                HeroUtility.ForEachGenericSkillBadge(skillBadges, DealWithOneGenericSkillBadge, new ForEachGenericBadgeUserData
                {
                    SourceType = sourceType,
                    BuffData = buffData,
                    DataToOperateOn = ret,
                    BuffCondition = buffCondition,
                });

                return ret;
            }

            private void DealWithOneGenericSkillBadge(GenericBadgeData genericBadgeData, DRGenericSkillBadge drGenericBadge, object userData)
            {
                var myUserData = userData as ForEachGenericBadgeUserData;
                var dataToOperateOn = myUserData.DataToOperateOn;

                if (myUserData.SourceType != ImpactSourceType.Buff)
                {
                    dataToOperateOn.PhysicalAtkIncreaseRate += drGenericBadge.PhysicalAtkIncreaseRate;
                    dataToOperateOn.MagicAtkIncreaseRate += drGenericBadge.MagicAtkIncreaseRate;
                    dataToOperateOn.OppPhysicalDfsReduceRate += drGenericBadge.OppPhysicalDfsReduceRate;
                    dataToOperateOn.CriticalHitProb += drGenericBadge.CriticalHitProb;
                    dataToOperateOn.CriticalHitRate += drGenericBadge.CriticalHitRate;
                }

                if (drGenericBadge.ElementId == (int)HeroElementType.Fire
                    && myUserData.SourceType == ImpactSourceType.Buff
                    && myUserData.BuffData != null
                    && myUserData.BuffData.ElementId == (int)HeroElementType.Fire)
                {
                    dataToOperateOn.ElementBuffPhysicalAtkDamageIncreaseRate += drGenericBadge.Params[1];
                }
                else if (drGenericBadge.ElementId == (int)HeroElementType.Light
                    && myUserData.SourceType != ImpactSourceType.Buff
                    && myUserData.BuffCondition == BuffType.LightDebuff)
                {
                    dataToOperateOn.ElementBuffPhysicalAtkDamageIncreaseRate += drGenericBadge.Params[1];
                }
                else if (drGenericBadge.ElementId == (int)HeroElementType.Dark)
                {
                    dataToOperateOn.ElementSkillRecoverHPRate += drGenericBadge.Params[0];
                }
            }


            private float GetStateForImpactRatio(StateForImpactCalc state)
            {
                switch (state)
                {
                    case StateForImpactCalc.Normal:
                        return Constant.StateForImpactCalc.Normal;
                    case StateForImpactCalc.Floating:
                        return Constant.StateForImpactCalc.Floating;
                    case StateForImpactCalc.Stunned:
                        return Constant.StateForImpactCalc.Stunned;
                    case StateForImpactCalc.Frozen:
                        return Constant.StateForImpactCalc.Frozen;
                    default:
                        return Constant.StateForImpactCalc.Normal;
                }
            }

            [System.Diagnostics.Conditional("LOG_HP_DAMAGE_IMPACT")]
            private static void LogInfo(string format, params object[] args)
            {
                format = "[HPDamageImpact] " + format;
                Log.Info(format, args);
            }

            private class ForEachGenericBadgeUserData
            {
                public ImpactSourceType SourceType;
                public BuffData BuffData;
                public SkillBadgesCausedData DataToOperateOn;

                public BuffType BuffCondition { get; internal set; }
            }

            private class ImpactDamageData
            {
                public ImpactDamageData(int damageHP, int recoverHP, int skillRecoverHP, int counterHP, bool isCritical)
                {
                    DamageHP = damageHP;
                    RecoverHP = recoverHP;
                    SkillRecoverHP = skillRecoverHP;
                    CounterHP = counterHP;
                    IsCritical = isCritical;
                }

                public int DamageHP
                {
                    get;
                    private set;
                }

                public int RecoverHP
                {
                    get;
                    private set;
                }

                public int SkillRecoverHP
                {
                    get;
                    private set;
                }

                public int CounterHP
                {
                    get;
                    private set;
                }

                public bool IsCritical
                {
                    get;
                    private set;
                }
            }

            private class SkillBadgesCausedData
            {
                public float PhysicalAtkIncreaseRate;
                public float MagicAtkIncreaseRate;
                public float CriticalHitProb;
                public float CriticalHitRate;
                public float OppPhysicalDfsReduceRate;

                public float ElementBuffPhysicalAtkDamageIncreaseRate;
                public float ElementSkillRecoverHPRate;
                public float SpecificBadgePhysicalAtkDamageIncreaseRate;

                public override string ToString()
                {
                    return string.Format(
                        "PhysicalAtkIncreaseRate={0}, " +
                        "MagicAtkIncreaseRate={1}, " +
                        "CriticalHitProb={2}, " +
                        "CriticalHitRate={3}, " +
                        "OppPhysicalDfsReduceRate={4}, " +
                        "ElementBuffPhysicalAtkDamageIncreaseRate={5}, " +
                        "ElementSkillRecoverHPRate={6}, " +
                        "SpecificBadgeDamageIncreaseRate={7}.",
                        PhysicalAtkIncreaseRate.ToString(),
                        MagicAtkIncreaseRate.ToString(),
                        CriticalHitProb.ToString(),
                        CriticalHitRate.ToString(),
                        OppPhysicalDfsReduceRate.ToString(),
                        ElementBuffPhysicalAtkDamageIncreaseRate.ToString(),
                        ElementSkillRecoverHPRate.ToString(),
                        SpecificBadgePhysicalAtkDamageIncreaseRate.ToString());
                }
            }
        }
    }
}
