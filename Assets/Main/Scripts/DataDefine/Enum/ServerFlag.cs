using System;

namespace Genesis.GameClient
{
    [Flags]
    public enum ServerFlag
    {
        /// <summary>
        /// 无标记。
        /// </summary>
        None = 0,

        /// <summary>
        /// 维护。
        /// </summary>
        Maintenance = 1 << 0,

        /// <summary>
        /// 推荐。
        /// </summary>
        Recommended = 1 << 1,

        /// <summary>
        /// 新服。
        /// </summary>
        New = 1 << 2,
    }
}
