namespace Genesis.GameClient
{
    /// <summary>
    /// 实体时间轴事件。
    /// </summary>
    public enum EntityTimeLineEvent
    {
        /// <summary>
        /// 移动输入。
        /// </summary>
        Move,

        /// <summary>
        /// 技能输入。
        /// </summary>
        Skill,

        /// <summary>
        /// 受击。
        /// </summary>
        Impact,

        /// <summary>
        /// 受到状态伤害。
        /// </summary>
        StateImpact,

        /// <summary>
        /// 连续点击技能输入。
        /// </summary>
        ContinualTapSkill,

        /// <summary>
        /// 释放蓄力技能。
        /// </summary>
        PerformChargeSkill,
    }
}
