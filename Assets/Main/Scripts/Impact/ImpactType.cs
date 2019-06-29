namespace Genesis.GameClient
{
    /// <summary>
    /// 伤害类型。
    /// </summary>
    public enum ImpactType
    {
        /// <summary>
        /// 数值伤害保留值。
        /// </summary>
        ValueImpactReserved = 0,

        /// <summary>
        /// HP 伤害。
        /// </summary>
        HPDamage = 1,

        /// <summary>
        /// HP 恢复。
        /// </summary>
        HPRecover = 2,

        /// <summary>
        /// 霸体值伤害。
        /// </summary>
        SteadyDamage = 3,

        /// <summary>
        /// 状态伤害保留值。
        /// </summary>
        StateImpactReserved = 100,

        /// <summary>
        /// 硬直,普通击打。
        /// </summary>
        Stiffness = 101,

        /// <summary>
        /// 浮空。
        /// </summary>
        Float = 102,

        /// <summary>
        /// 击飞。
        /// </summary>
        BlownAway = 103,

        /// <summary>
        /// 眩晕。
        /// </summary>
        Stun = 104,

        /// <summary>
        /// 冰冻。
        /// </summary>
        Freeze = 105,

        /// <summary>
        /// 强力击打。
        /// </summary>
        HardHit = 106,

        /// <summary>
        /// 其他伤害保留值。
        /// </summary>
        OtherImpactReserved = 200,

        /// <summary>
        /// 声音和特效。
        /// </summary>
        SoundAndEffect = 201,
    }
}
