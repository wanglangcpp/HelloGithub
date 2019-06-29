using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 玩家基础属性配置表。
    /// </summary>
    public class DRPlayer : IDataRow
    {
        /// <summary>
        /// 玩家等级编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 升级所需经验。
        /// </summary>
        public int LevelUpExp
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
            LevelUpExp = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRPlayer>();
        }
    }
}
