namespace Genesis.GameClient
{
    public enum ServerLoad
    {
        /// <summary>
        /// 失去连接。
        /// </summary>
        OutOfService = 0,

        /// <summary>
        /// 流畅。
        /// </summary>
        Good,

        /// <summary>
        /// 爆满。
        /// </summary>
        Full,
    }
}
