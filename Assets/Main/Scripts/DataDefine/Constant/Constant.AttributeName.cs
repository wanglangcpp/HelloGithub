namespace Genesis.GameClient
{
    public static partial class Constant
    {
        /// <summary>
        /// 属性名称常量。
        /// </summary>
        public static class AttributeName
        {
            public const string PhysicalAttackKey = "UI_TEXT_GEAR_PHYSICAL_DAMAGE";
            public const string MagicAttackKey = "UI_TEXT_GEAR_MAGIC_DAMAGE";
            public const string PhysicalDefenseKey = "UI_TEXT_GEAR_PHYSICAL_ARMOR";
            public const string MagicDefenseKey = "UI_TEXT_GEAR_MAGIC_ARMOR";
            public const string PhysicalAtkHPAbsorbRateKey = "UI_TEXT_GEAR_PHYSICAL_VAMPIRE";
            public const string PhysicalAtkReflectRateKey = "UI_TEXT_GEAR_PHYSICAL_COUNTERATTACK";
            public const string OppPhysicalDfsReduceRateKey = "UI_TEXT_GEAR_PHYSICAL_ARMOR_REDUCED_PCT";
            public const string MagicAtkHPAbsorbRateKey = "UI_TEXT_GEAR_VAMPIRE_SPELL";
            public const string MagicAtkReflectRateKey = "UI_TEXT_GEAR_MAGIC_COUNTERATTACK";
            public const string OppMagicDfsReduceRateKey = "UI_TEXT_GEAR_MAGIC_ARMOR_REDUCED_PCT";
            public const string DamageReductionRateKey = "UI_TEXT_GEAR_DAMAGE_DEDUCTION";
            public const string AdditionalDamageKey = "UI_TEXT_GEAR_DAMAGE_ALL";
            public const string CriticalHitRateKey = "UI_TEXT_GEAR_CRITICAL_STRIKE_DAMAGE_PCT";
            public const string CriticalHitProbKey = "UI_TEXT_GEAR_CRITICAL_STRIKE_PCT";
            public const string AntiCriticalHitProbKey = "UI_TEXT_GEAR_CRITICAL_STRIKE_AVOID_PCT";
            public const string ReducedSkillCoolDownRateKey = "UI_TEXT_GEAR_CD_REDUCED_PCT";
            public const string BackgroundHPRecoverKey = "UI_TEXT_GEAR_BACK_TO_THE_BLOOD";
            public const string MaxHPKey = "UI_TEXT_GEAR_MAX_HP";
            public const string PhysicalAtkIncreaseRateKey = "UI_TEXT_GEAR_PHYSICAL_DAMAGE_INCREASED_PCT";
            public const string MagicAtkIncreaseRateKey = "UI_TEXT_GEAR_MAGIC_DAMAGE_INCREASED_PCT";
            public const string PhysicalDfsIncreaseRateKey = "UI_TEXT_GEAR_PHYSICAL_ARMOR_INCREASED_PCT";
            public const string MagicDfsIncreaseRateKey = "UI_TEXT_GEAR_MAGIC_ARMOR_INCREASED_PCT";
            public const string MaxHPIncreaseRateKey = "UI_TEXT_GEAR_HP_INCREASED_PCT";
            public const string AngerIncreaseKey = "UI_TEXT_GEAR_RAGE_INCREASED_PCT";
            public const string ReducedHeroSwitchCDRateKey = "UI_TEXT_GEAR_HERO_SWITCH_COOL_DOWN_PCT";
            public const string SpeedKey = "UI_TEXT_GEAR_SPEED";

            public static readonly string[] AttributeNameDics = new string[]
            {
                "",
                MaxHPKey,
                PhysicalAttackKey,
                MagicAttackKey,
                PhysicalDefenseKey,
                MagicDefenseKey,
                PhysicalAtkIncreaseRateKey,
                MagicAtkIncreaseRateKey,
                PhysicalDfsIncreaseRateKey,
                MagicDfsIncreaseRateKey,
                MaxHPIncreaseRateKey,
                AngerIncreaseKey,
                AdditionalDamageKey,
                DamageReductionRateKey,
                ReducedSkillCoolDownRateKey,
                CriticalHitProbKey,
                CriticalHitRateKey,
                PhysicalAtkReflectRateKey,
                MagicAtkReflectRateKey,
                OppPhysicalDfsReduceRateKey,
                OppMagicDfsReduceRateKey,
                AntiCriticalHitProbKey,
                PhysicalAtkHPAbsorbRateKey,
                MagicAtkHPAbsorbRateKey,
                BackgroundHPRecoverKey,
                ReducedHeroSwitchCDRateKey,
                SpeedKey,
            };

            public static readonly string[] HeroElementSpriteNames = new string[]
            {
                "",
                "icon_element_thunder",
                "icon_element_ice",
                "icon_element_fire",
                "icon_element_light",
                "icon_element_dark",
            };
        }
    }
}
