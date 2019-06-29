namespace Genesis.GameClient
{
    public static partial class Constant
    {
        public static class Hero
        {
            public const int ProfessionIdMinVal = 1;
            public const int ProfessionIdMaxVal = 5;

            public const float MaxHPFactorMinVal = 1f;
            public const float MaxHPFactorMaxVal = 1.5f;

            public const float PhysicalAttackFactorMinVal = .8f;
            public const float PhysicalAttackFactorMaxVal = 1.5f;

            public const float MagicAttackFactorMinVal = .8f;
            public const float MagicAttackFactorMaxVal = 1.6f;

            public const float PhysicalDefenseFactorMinVal = .8f;
            public const float PhysicalDefenseFactorMaxVal = 1.4f;

            public const float MagicDefenseFactorMinVal = .8f;
            public const float MagicDefenseFactorMaxVal = 1.2f;

            /// <summary>
            /// 最大暴击率。
            /// </summary>
            public const float CriticalHitProbMaxVal = .3f;

            /// <summary>
            /// 最大暴击伤害倍数。
            /// </summary>
            public const float CriticalHitRateMaxVal = 2f;

            /// <summary>
            /// 最大物理伤害反击率。
            /// </summary>
            public const float PhysicalAtkReflectRateMaxVal = .3f;

            /// <summary>
            /// 最大法术伤害反击率。
            /// </summary>
            public const float MagicAtkReflectRateMaxVal = .3f;

            /// <summary>
            /// 最大降低对方物理防御百分比。
            /// </summary>
            public const float OppPhysicalDfsReduceRateMaxVal = .4f;

            /// <summary>
            /// 最大降低对方法术防御百分比。
            /// </summary>
            public const float OppMagicDfsReduceRateMaxVal = .4f;

            /// <summary>
            /// 最大免除暴击率。
            /// </summary>
            public const float AntiCriticalHitProbMaxVal = .3f;

            /// <summary>
            /// 最大物理伤害吸血率。
            /// </summary>
            public const float PhysicalAtkHPAbsorbRateMaxVal = .3f;

            /// <summary>
            /// 最大法术伤害吸血率。
            /// </summary>
            public const float MagicAtkHPAbsorbRateMaxVal = .3f;

            /// <summary>
            /// 最大受击伤害减免率。
            /// </summary>
            public const float DamageReductionRateMaxVal = .5f;

            /// <summary>
            /// 最大技能冷却时间缩减百分比。
            /// </summary>
            public const float ReducedSkillCoolDownRateMaxVal = .3f;

            /// <summary>
            /// 最大英雄切换后的冷却时间缩减百分比。
            /// </summary>
            public const float HeroSwitchCoolDownRateMaxVal = .5f;

            /// <summary>
            /// 每个技能通用徽章槽的最大个数。
            /// </summary>
            public const int MaxBadgeSlotCountPerSkill = 4;
        }
    }
}
