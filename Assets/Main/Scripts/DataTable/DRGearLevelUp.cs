using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 装备升级配置表。
    /// </summary>
    public class DRGearLevelUp : IDataRow
    {
        /// <summary>
        /// 装备编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        public List<int> LevelUpCostCoin
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
            LevelUpCostCoin = new List<int>();
            for (; index < text.Length; index++)
            {
                LevelUpCostCoin.Add(int.Parse(text[index]));
            }
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRGearLevelUp>();
        }
    }
}
