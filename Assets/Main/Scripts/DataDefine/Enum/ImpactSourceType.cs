namespace Genesis.GameClient
{
    /// <summary>
    /// 伤害来源类型。
    /// </summary>
    public enum ImpactSourceType
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 技能。
        /// </summary>
        Skill,

        /// <summary>
        /// Buff。
        /// </summary>
        Buff,

        /// <summary>
        /// 副本逻辑。
        /// </summary>
        InstanceLogic,
    }
}
