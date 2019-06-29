namespace Genesis.GameClient
{
    /// <summary>
    /// 伤害附加数据。
    /// </summary>
    public class ImpactAuxData
    {
        /// <summary>
        /// 加诸目标的 Buff 编号列表。
        /// </summary>
        public int[] BuffIdsToAddToTarget;

        /// <summary>
        /// 产生伤害的 Buff 编号。
        /// </summary>
        public int? CausingBuffId;

        /// <summary>
        /// 当前时间轴触发的技能吸血次数。
        /// </summary>
        public int SkillRecoverHPCount;

        /// <summary>
        /// 产生伤害的 Buff 数据。
        /// </summary>
        public BuffData CausingBuffData;
    }
}
