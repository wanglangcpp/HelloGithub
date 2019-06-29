using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 技能升级配置表。
    /// </summary>
    public class DRSkillLevelUp : IDataRow
    {
        /// <summary>
        /// 技能等级编号
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        public int CostCoinCount
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            CostCoinCount = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSkillLevelUp>();
        }
    }
}
