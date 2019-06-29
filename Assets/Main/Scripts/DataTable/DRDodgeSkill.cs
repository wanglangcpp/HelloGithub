using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 闪避技能配置表。
    /// </summary>
    public class DRDodgeSkill : IDataRow
    {
        /// <summary>
        /// 闪避技能编号。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 能量条上限。
        /// </summary>
        public float MaxEnergy { get; private set; }

        /// <summary>
        /// 能量条开启值。
        /// </summary>
        public float ValidEnergy { get; private set; }

        /// <summary>
        /// 每次闪避消耗。
        /// </summary>
        public float CostPerDodge { get; private set; }

        /// <summary>
        /// 能量回复速度。
        /// </summary>
        public float RecoverySpeed { get; private set; }

        /// <summary>
        /// 能量回复停滞时间。
        /// </summary>
        public float WaitTimeBeforeRecovery { get; private set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            MaxEnergy = float.Parse(text[index++]);
            ValidEnergy = float.Parse(text[index++]);
            CostPerDodge = float.Parse(text[index++]);
            RecoverySpeed = float.Parse(text[index++]);
            WaitTimeBeforeRecovery = float.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRDodgeSkill>();
        }
    }
}
