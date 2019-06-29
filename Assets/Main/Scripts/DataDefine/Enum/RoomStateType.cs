namespace Genesis.GameClient
{
    /// <summary>
    /// 房间状态类型。
    /// </summary>
    public enum RoomStateType
    {
        /// <summary>
        /// 等待中。
        /// </summary>
        Waiting = 0,

        /// <summary>
        /// 副本中。
        /// </summary>
        Running,

        /// <summary>
        /// 结算中。
        /// </summary>
        Finish,
    }
}
