namespace Genesis.GameClient
{
    /// <summary>
    /// 技能终止原因。
    /// </summary>
    public enum SkillEndReasonType
    {
        /// <summary>
        /// 技能正常结束。
        /// </summary>
        Finish = 0,

        /// <summary>
        /// 技能被移动打断。
        /// </summary>
        BreakByMove,

        /// <summary>
        /// 技能被移动打断。
        /// </summary>
        BreakBySkill,

        /// <summary>
        /// 技能被移动打断。
        /// </summary>
        BreakByImpact,

        /// <summary>
        /// 其他原因。
        /// </summary>
        Other,
    }
}
