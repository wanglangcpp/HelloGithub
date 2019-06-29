namespace Genesis.GameClient
{
    public interface IImpactDataProvider
    {
        /// <summary>
        /// 用于伤害计算的状态。
        /// </summary>
        StateForImpactCalc StateForImpactCalc
        {
            get;
        }

        /// <summary>
        /// 最大生命值。
        /// </summary>
        int MaxHP
        {
            get;
        }

        /// <summary>
        /// 物理攻击。
        /// </summary>
        int PhysicalAttack
        {
            get;
        }

        /// <summary>
        /// 物理防御。
        /// </summary>
        int PhysicalDefense
        {
            get;
        }

        /// <summary>
        /// 法术攻击。
        /// </summary>
        int MagicAttack
        {
            get;
        }

        /// <summary>
        /// 法术防御。
        /// </summary>
        int MagicDefense
        {
            get;
        }

        /// <summary>
        /// 降低对方物理防御百分比。
        /// </summary>
        float OppPhysicalDfsReduceRate
        {
            get;
        }

        /// <summary>
        /// 降低对方法术防御百分比。
        /// </summary>
        float OppMagicDfsReduceRate
        {
            get;
        }

        /// <summary>
        /// 物理伤害吸血率。
        /// </summary>
        float PhysicalAtkHPAbsorbRate
        {
            get;
        }

        /// <summary>
        /// 法术伤害吸血率。
        /// </summary>
        float MagicAtkHPAbsorbRate
        {
            get;
        }

        /// <summary>
        /// 物理伤害反击率。
        /// </summary>
        float PhysicalAtkReflectRate
        {
            get;
        }

        /// <summary>
        /// 法术伤害反击率。
        /// </summary>
        float MagicAtkReflectRate
        {
            get;
        }

        /// <summary>
        /// 受击伤害减免率。
        /// </summary>
        float DamageReductionRate
        {
            get;
        }

        /// <summary>
        /// 暴击率。
        /// </summary>
        float CriticalHitProb
        {
            get;
        }

        /// <summary>
        /// 暴击伤害倍数。
        /// </summary>
        float CriticalHitRate
        {
            get;
        }

        /// <summary>
        /// 免除暴击率。
        /// </summary>
        float AntiCriticalHitProb
        {
            get;
        }

        /// <summary>
        /// 伤害浮动率。
        /// </summary>
        float DamageRandomRate
        {
            get;
        }

        /// <summary>
        /// 附加伤害。
        /// </summary>
        int AdditionalDamage
        {
            get;
        }

        /// <summary>
        /// 克制属性编号。
        /// </summary>
        int ElementId
        {
            get;
        }
    }
}
