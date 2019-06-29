using GameFramework.DataTable;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 战魂配置表。
    /// </summary>
    public class DRSoul : IDataRow
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
        /// 战魂名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 战魂描述。
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 战魂分类。
        /// </summary>
        public int Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 战魂品质。
        /// </summary>
        public int Quality
        {
            get;
            private set;
        }

        /// <summary>
        /// 升级后的战魂编号。
        /// </summary>
        public int UpgradedId
        {
            get;
            private set;
        }

        /// <summary>
        /// 图标编号。
        /// </summary>
        public int IconId
        {
            get;
            private set;
        }

        /// <summary>
        /// 使用等级下限。
        /// </summary>
        public int MinLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 卖店价格。
        /// </summary>
        public int Price
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否广播。
        /// </summary>
        public bool Broadcast
        {
            get;
            private set;
        }

        /// <summary>
        /// 战魂属性加成。
        /// </summary>
        public float AddValue
        {
            get;
            private set;
        }

        public int StrengthenItemId
        {
            get;
            private set;
        }

        public int StrengthenItemCount
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
            Name = text[index++];
            Description = text[index++];
            Type = int.Parse(text[index++]);
            Quality = int.Parse(text[index++]);
            UpgradedId = int.Parse(text[index++]);
            IconId = int.Parse(text[index++]);
            MinLevel = int.Parse(text[index++]);
            Price = int.Parse(text[index++]);
            Broadcast = bool.Parse(text[index++]);
            AddValue = float.Parse(text[index++]);
            StrengthenItemId = int.Parse(text[index++]);
            StrengthenItemCount = int.Parse(text[index++]);
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRSoul>();
        }
    }
}
