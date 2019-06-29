namespace Genesis.GameClient
{
    /// <summary>
    /// 坐标参考类型
    /// </summary>
    public enum TransformType
    {
        /// <summary>
        /// 默认。
        /// </summary>
        Default = 0,

        /// <summary>
        /// 相对于所有者。
        /// </summary>
        RelativeToOwner = 1,

        /// <summary>
        /// 相对于目标。
        /// </summary>
        RelativeToTarget = 2,
    }
}
