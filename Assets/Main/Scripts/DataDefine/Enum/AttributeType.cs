namespace Genesis.GameClient
{
    /// <summary>
    /// 属性类型。
    /// </summary>
    public enum AttributeType
    {
        /// <summary>
        /// 未指定。
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// 血量上限。
        /// </summary>
        MaxHP = 1,

        /// <summary>
        /// 物理攻击。
        /// </summary>
        PhysicalAttack = 2,

        /// <summary>
        /// 法术攻击。
        /// </summary>
        MagicAttack = 3,

        /// <summary>
        /// 物理防御。
        /// </summary>
        PhysicalDefense = 4,

        /// <summary>
        /// 法术防御。
        /// </summary>
        MagicDefense = 5,

        /// <summary>
        /// 物理攻击加成。
        /// </summary>
        PhysicalAtkIncreaseRate = 6,

        /// <summary>
        /// 法术攻击加成。
        /// </summary>
        MagicAtkIncreaseRate = 7,

        /// <summary>
        /// 物理防御加成。
        /// </summary>
        PhysicalDfsIncreaseRate = 8,

        /// <summary>
        /// 法术防御加成。
        /// </summary>
        MagicDfsIncreaseRate = 9,

        /// <summary>
        /// 血量上限加成。
        /// </summary>
        MaxHPIncreaseRate = 10,

        /// <summary>
        /// 怒气上涨速率。
        /// </summary>
        AngerIncreaseRate = 11,

        /// <summary>
        /// 真实伤害。
        /// </summary>
        AdditionalDamage = 12,

        /// <summary>
        /// 伤害减免百分比。
        /// </summary>
        DamageReductionRate = 13,

        /// <summary>
        /// 冷却时间缩减百分比。
        /// </summary>
        ReducedSkillCoolDown = 14,

        /// <summary>
        /// 暴击率。
        /// </summary>
        CriticalHitProb = 15,

        /// <summary>
        /// 暴击伤害。
        /// </summary>
        CriticalHitRate = 16,

        /// <summary>
        /// 物理反伤。
        /// </summary>
        PhysicalAtkReflectRate = 17,

        /// <summary>
        /// 法术反击。
        /// </summary>
        MagicAtkReflectRate = 18,

        /// <summary>
        /// 物理穿透。
        /// </summary>
        OppPhysicalDfsReduceRate = 19,

        /// <summary>
        /// 法术穿透。
        /// </summary>
        OppMagicDfsReduceRate = 20,

        /// <summary>
        /// 免爆率。
        /// </summary>
        AntiCriticalHitProb = 21,

        /// <summary>
        /// 物理吸血。
        /// </summary>
        PhysicalAtkHPAbsorbRate = 22,

        /// <summary>
        /// 法术吸血。
        /// </summary>
        MagicAtkHPAbsorbRate = 23,

        /// <summary>
        /// 待战回血。
        /// </summary>
        RecoverHP = 24,

        /// <summary>
        /// 待战时间缩减。
        /// </summary>
        ReducedHeroSwitchCoolDown = 25,

        /// <summary>
        /// 移动速度。
        /// </summary>
        Speed = 26,
    }
}
