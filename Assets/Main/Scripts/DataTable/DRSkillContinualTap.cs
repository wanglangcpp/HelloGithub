using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 技能连续点击配置表。
    /// </summary>
    public class DRSkillContinualTap : IDataRow
    {
        /// <summary>
        /// 技能编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 技能时长。
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// 连续点击开始时间。
        /// </summary>
        public float ContinualTapStartTime { get; private set; }

        /// <summary>
        /// 连续点击持续时间。
        /// </summary>
        public float ContinualTapDuration { get; private set; }

        /// <summary>
        /// 点击区间数。
        /// </summary>
        public int IntervalCount { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            Duration = float.Parse(text[index++]);
            ContinualTapStartTime = float.Parse(text[index++]);
            ContinualTapDuration = float.Parse(text[index++]);
            IntervalCount = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSkillContinualTap>();
        }
    }
}
