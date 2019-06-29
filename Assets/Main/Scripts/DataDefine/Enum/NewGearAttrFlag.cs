using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备属性的获取标记。
    /// </summary>
    [Flags]
    public enum NewGearAttrFlag
    {
        None = 0x0,

        /// <summary>
        /// 下一（强化）等级。
        /// </summary>
        NextStrengthenLevel = 0x1,

        /// <summary>
        /// 下一总品阶。
        /// </summary>
        NextTotalQualityLevel = 0x2,
    }
}
