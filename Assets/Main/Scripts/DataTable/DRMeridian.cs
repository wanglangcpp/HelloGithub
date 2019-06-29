using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 星图配置表。
    /// </summary>
    public class DRMeridian : IDataRow
    {
        /// <summary>
        /// 战魂编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 消耗星能数量。
        /// </summary>
        public int CostMeridianEnergy
        {
            get;
            private set;
        }

        /// <summary>
        /// 消耗金币数量。
        /// </summary>
        public int CostCoin
        {
            get;
            private set;
        }

        /// <summary>
        /// 生命力增加累计值。
        /// </summary>
        public int MaxHP
        {
            get;
            private set;
        }

        /// <summary>
        /// 攻击力增加累计值。
        /// </summary>
        public int Attack
        {
            get;
            private set;
        }

        /// <summary>
        /// 防御力增加累计值。
        /// </summary>
        public int Defense
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
            CostMeridianEnergy = int.Parse(text[index++]);
            CostCoin = int.Parse(text[index++]);
            MaxHP = int.Parse(text[index++]);
            Attack = int.Parse(text[index++]);
            Defense = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRMeridian>();
        }
    }
}
