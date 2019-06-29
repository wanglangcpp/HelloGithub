namespace Genesis.GameClient
{
    /// <summary>
    /// 请求释放技能操作的状态。
    /// </summary>
    public enum PerformSkillOperationState
    {
        /// <summary>
        /// 等待。
        /// </summary>
        Waiting,

        /// <summary>
        /// 正在释放技能。
        /// </summary>
        Performing,

        /// <summary>
        /// 释放失败。
        /// </summary>
        PerformFailure,

        /// <summary>
        /// 释放完成。
        /// </summary>
        PerformEnd,
    }
}
